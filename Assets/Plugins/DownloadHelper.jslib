mergeInto(LibraryManager.library, {
	DownloadFile : function(array, size, fileNamePtr)
	{
		var fileName = UTF8ToString(fileNamePtr);
	 
		var bytes = new Uint8Array(size);
		for (var i = 0; i < size; i++)
		{
		   bytes[i] = HEAPU8[array + i];
		}
	 
		var blob = new Blob([bytes]);
		var link = document.createElement('a');
		link.href = window.URL.createObjectURL(blob);
		link.download = fileName;
	 
		var event = document.createEvent("MouseEvents");
		event.initMouseEvent("click");
		link.dispatchEvent(event);
		window.URL.revokeObjectURL(link.href);
	},
	
	
	// https://pixeleuphoria.com/blog/index.php/2020/04/29/unity-webgl-upload-content/
    BrowserTextUpload: function(extFilter, gameObjName, dataSinkFn)
    {
        // If this is the first time being called, create the reusable 
        // input DOM object.
        if(typeof inputLoader == "undefined")
        {
            inputLoader = document.createElement("input");
            // We need it to be of the "file" type to have to popup dialog
            // and file uploading features we need.
            inputLoader.setAttribute("type", "file");
            // When we add it to the body, make sure it doesn't visually
            // affect the page.
            inputLoader.style.display = 'none';
            // We need to add it to the body in order for it to be active.
            document.body.appendChild(inputLoader);

            // Setup the callback
            inputLoader.onchange = 
                function(x)
                {
                    // If empty, nothing was selected.
                    if(this.value == "")
                        return;

                    // In this example, we assume only one file is selected
                    var file = this.files[0];
                    // The file data isn't instantly available at this level.
                    // In order to access the file contents, we need to run
                    // it through the FileReader and process it in its onload
                    // callback (this is callback-ception)
                    var reader = new FileReader();
                    // Clear the value to empty. Because onchange only triggers if
                    // the selection is different. So if the same file is selected again
                    // afterwards for legitimate reasons, it wouldn't trigger an onchange
                    // if we don't clear this.
                    this.value = "";
                    // Giving reader.onload access to this input.
                    var thisInput = this;
                    
                    reader.onload = function(evt) 
                    {
	                    if (evt.target.readyState != 2)
		                    return;

	                    if (evt.target.error) 
	                    {
		                    alert("Error while reading file " + file.name + ": " + loadEvent.target.error);
		                    return;
	                    }
	                    
	                    // The text results are in evt.target.result, so just send that
	                    // back into our app with SendMessage(). Note that we DON'T need
	                    // to call UTF8ToString() on it.
                        SendMessage(
                            inputLoader.gameObjName, 
                            inputLoader.dataSinkFn, 
                            evt.target.result);
                    }
                    reader.readAsText(file);
                }
        }
        // We need to turn these values into strings before the callback, because the
        // memory that these parameter-string-pointers point to will not be valid later.
        inputLoader.gameObjName = UTF8ToString(gameObjName);
        inputLoader.dataSinkFn = UTF8ToString(dataSinkFn);
        // Set the extension filtering for the upload dialog
        inputLoader.setAttribute("accept", UTF8ToString(extFilter))
        // Force the input object to activate and open the upload dialog.
        inputLoader.click();
    },
});