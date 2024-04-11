using System;
using System.Collections;
using System.Collections.Generic;
using Artifacts;
using Characters;
using Gui;
using Infrastructure;
using Services.Api;
using Services.Api.DTO;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    public HeroPreset HeroPreset;
    public ArtifactPreset ArtifactPreset;
    public MainCanvas MainCanvas;
    
    private Dictionary<Type, Func<IService>> _ctors;
    private  Dictionary<Type, IService> _services;
    
    public static ServiceLocator Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        _ctors = new Dictionary<Type, Func<IService>>();
        _services = new Dictionary<Type, IService>();
        
        DontDestroyOnLoad(gameObject);

        RegisterServices();
    }

    private void RegisterServices()
    {
        RegisterApiService();
    }

    private void RegisterApiService()
    {
        var database = new Database();
        Register(CreateApiService, false);
        
        ApiService CreateApiService() 
            => new(database, HeroPreset, ArtifactPreset, MainCanvas);
    }

    public void Register<T>(Func<T> ctor, bool lazy = true) where T : IService
    {
        _ctors[typeof(T)] = () => ctor();
        if (!lazy) Get<T>();
    }

    public T Get<T>() where T : IService
    {
        if (!_services.ContainsKey(typeof(T))) 
            _services[typeof(T)] = _ctors[typeof(T)]();

        return (T) _services[typeof(T)];
    }
}
