using System.Collections.Generic;
using System.Linq;
using Artifacts;
using Characters;
using Gui;
using Gui.Filters;
using Infrastructure;
using Scripts.Filters;
using Services.Api.DTO;
using Utils;

namespace Services.Api
{
    public class ApiService : IService, IApiEventReceiver
    {
        private readonly Database _database;
        private readonly Dictionary<ArtifactType, List<int>> _rowsByArtifact;
        private readonly List<ArtifactData> _artifactDataList = new();
        private readonly HeroPreset _heroPreset;
        private readonly ArtifactPreset _artifactPreset;
        private readonly MainCanvas _canvas;
        private readonly SaveLoadHelper _saveLoadHelper;
        
        private FilterModel _filterModel;
        private bool _combineResults = true;
        
        public ApiService(
            Database database, 
            HeroPreset heroPreset,
            ArtifactPreset artifactPreset,
            MainCanvas canvas)
        {
            _database = database;
            _heroPreset = heroPreset;
            _canvas = canvas;
            _artifactPreset = artifactPreset;
            _rowsByArtifact = new Dictionary<ArtifactType, List<int>>();
            _saveLoadHelper = new SaveLoadHelper(_canvas.JsonWindow);
            
            _canvas.TeamTable.SetDeleteButtonAction(DeleteTeamData);
            canvas.Init(this, _artifactPreset, _heroPreset);
            _database.OnDataChanged += OnDatabaseChanged;
            _filterModel = new FilterModel();
            _filterModel.AddFilterSource(_canvas.TeamTable);
            _filterModel.AddFilterSource(_canvas.ArtifactTable);
            _filterModel.OnFilterChanged += OnFilterChanged;
            
            _canvas.TeamTable.SetFilterEventReceiver(_filterModel);
            _canvas.ArtifactTable.SetFilterEventReceiver(_filterModel);
            _canvas.FilterTable.OnFilterRemoved += _filterModel.RemoveFilter;
        }

        private void OnFilterChanged()
        {
            var filters = _filterModel.GetFilters();
            _canvas.FilterTable.SetElements(filters);
            InitUi();
        }

        public void Init(string jsonData)
        {
            _database.SetData(jsonData);
        }

        private void DeleteTeamData(TeamData teamData)
        {
            var databaseRow = new DatabaseRow
            {
                Artifact = teamData.Artifact.Artifact.ArtifactType,
                AvgPlace = teamData.AvgPlace,
                StarCount = teamData.Artifact.StarCount,
                TeamComp = teamData.ToBitMask()
            };
            _database.RemoveRow(databaseRow);
        }

        private void OnDatabaseChanged()
        {
            RecalculateCache();
            InitUi();
        }

        private void RecalculateCache()
        {
            InitArtifactRowsMap();
            RecalculateArtifactRankingsCache();
            RecalculateTeamRankingsCache();
        }

        private void RecalculateArtifactRankingsCache()
        {
            _artifactDataList.Clear();

            // Aggregating artifact places by each artifact
            var artifactPlaces = new Dictionary<ArtifactType, List<float>>();
            foreach (var row in _database.Rows)
            {
                if (!artifactPlaces.ContainsKey(row.Artifact))
                    artifactPlaces[row.Artifact] = new List<float>();
                artifactPlaces[row.Artifact].Add(row.AvgPlace);
            }

            // Calculating average place for each artifact
            foreach (var artifactType in artifactPlaces.Keys)
            {
                var avgPlace = 0f;
                foreach (var place in artifactPlaces[artifactType])
                    avgPlace += place;
                avgPlace /= artifactPlaces[artifactType].Count;

                var artifactData = new ArtifactData
                {
                    Artifact = _artifactPreset.GetArtifact(artifactType),
                    AvgPlace = avgPlace
                };
                _artifactDataList.Add(artifactData);
            }
        }

        private void RecalculateTeamRankingsCache()
        {
            
        }

        private void InitUi()
        {
            _canvas.TeamTable.HideAll();
            _canvas.TeamTable.SetElements(GetAllTeamRankings());
            _canvas.TeamTable.ShowDeleteButtons(!_combineResults);

            var artifactDataList = GetArtifactRankings();
            _canvas.ArtifactTable.SetElements(artifactDataList);
        }

        private List<ArtifactData> GetArtifactRankings()
        {
            var artifactDataList = new List<ArtifactData>();
            foreach (var artifact in _artifactPreset.Artifacts)
            {
                var teamRankings = GetTeamRankingsByArtifact(artifact.ArtifactType);
                var avgPlace = 0f;
                foreach (var teamData in teamRankings)
                    avgPlace += teamData.AvgPlace;
                avgPlace /= teamRankings.Count;
                
                var artifactData = new ArtifactData
                {
                    Artifact = artifact,
                    AvgPlace = avgPlace
                };
                if (!float.IsNaN(artifactData.AvgPlace))
                    artifactDataList.Add(artifactData);
            }
            artifactDataList.Sort((x,y) => y.AvgPlace.CompareTo(x.AvgPlace));
            return artifactDataList;
        }

        private void InitArtifactRowsMap()
        {
            _rowsByArtifact.Clear();
            
            var rows = _database.Rows;
            for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var row = rows[rowIndex];
                if (!_rowsByArtifact.ContainsKey(row.Artifact))
                    _rowsByArtifact[row.Artifact] = new List<int>();
                _rowsByArtifact[row.Artifact].Add(rowIndex);
            }
        }
        
        public List<TeamData> GetAllTeamRankings()
        {
            var teamRankings = new List<TeamData>();
            foreach (var row in _database.Rows)
            {
                if (!MatchesFilter(row))
                    continue;
                
                var teamData = new TeamData
                {
                    AvgPlace = row.AvgPlace,
                    Heroes = _heroPreset.GetHeroes(row.TeamComp),
                    Artifact = new ArtifactData
                    {
                        Artifact = _artifactPreset.GetArtifact(row.Artifact),
                        AvgPlace = row.AvgPlace,
                        StarCount = row.StarCount,
                    }
                };
                teamRankings.Add(teamData);
            }
            teamRankings.Sort((x,y) => y.AvgPlace.CompareTo(x.AvgPlace));
            if (_combineResults)
                teamRankings = CombineByComp(teamRankings);
            return teamRankings;
        }
        
        public List<TeamData> GetTeamRankingsByArtifact(ArtifactType artifact, int? artifactStars = null)
        { 
            var teamRankings = new List<TeamData>();
            if (!_rowsByArtifact.ContainsKey(artifact)) 
                return teamRankings;
            
            foreach (var rowIndex in _rowsByArtifact[artifact])
            {
                var row = _database.Rows[rowIndex];
                
                // Check stars
                if (artifactStars.HasValue && row.StarCount != artifactStars)
                    continue;
                
                // Check filter
                if (!MatchesFilter(row))
                    continue;

                var teamData = new TeamData
                {
                    AvgPlace = row.AvgPlace,
                    Heroes = _heroPreset.GetHeroes(row.TeamComp),
                    Artifact = new ArtifactData
                    {
                        Artifact = _artifactPreset.GetArtifact(row.Artifact),
                        AvgPlace = row.AvgPlace
                    }
                };
                teamRankings.Add(teamData);
            }
            teamRankings.Sort((x,y) => y.AvgPlace.CompareTo(x.AvgPlace));
            if (_combineResults)
                teamRankings = CombineByComp(teamRankings);
            return teamRankings;
        }

        private List<TeamData> CombineByComp(List<TeamData> rankings)
        {
            var combinedRankings = new List<TeamData>();
            var compToRankings = new Dictionary<long, List<TeamData>>();
            foreach (var teamData in rankings)
            {
                var comp = teamData.ToBitMask();
                if (!compToRankings.ContainsKey(comp))
                    compToRankings[comp] = new List<TeamData>();
                compToRankings[comp].Add(teamData);
            }

            foreach (var comp in compToRankings.Keys)
            {
                var avgPlace = 0f;
                foreach (var teamData in compToRankings[comp])
                    avgPlace += teamData.AvgPlace;
                avgPlace /= compToRankings[comp].Count;
                
                var combinedTeamData = new TeamData
                {
                    AvgPlace = avgPlace,
                    Heroes = compToRankings[comp][0].Heroes,
                    Artifact = compToRankings[comp][0].Artifact
                };
                combinedRankings.Add(combinedTeamData);
            }
            combinedRankings.Sort((x,y) => y.AvgPlace.CompareTo(x.AvgPlace));
            return combinedRankings;
        }

        public void OnLoadDataButtonClicked(string json) => 
            Init(json);

        public void OnAddResultButtonClicked(DatabaseRow databaseRow) => 
            _database.AddRow(databaseRow);
        
        public void ExportData() => 
            _saveLoadHelper.ExportData(_database.ToJson());

        public void ImportData() => 
            _saveLoadHelper.ImportData(_database.SetData);

        public void SetCombining(bool isOn)
        {
            _combineResults = isOn;
            InitUi();
        }
        
        public void OnDeleteAllDataButtonClicked() => 
            _database.Clear();

        private bool MatchesFilter(DatabaseRow row)
        {
            if (_filterModel.ArtifactFilter != null && !_filterModel.ArtifactFilter.Check(row.Artifact))
                return false;
            
            var team = _heroPreset.GetHeroes(row.TeamComp);
            if (_filterModel.HeroFilterBitmask != 0 
                && _filterModel.HeroFilters.Any(heroFilter => !team.Contains(heroFilter.Filter)))
                return false;

            return true;
        }
    }
}