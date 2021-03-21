var OpenWindowPlugin = {
    openWindow: function(link)
    {
    	
		url = Pointer_stringify(link)
        	window.open(url);
        	
        
    }
};

mergeInto(LibraryManager.library, OpenWindowPlugin);