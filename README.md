# IconExtractor

A nodejs package that returns base64 image data for a path's icon.

This is a simple nodejs wrapper around a .net executable that will extract icon image data from a given path and return it.

Get an instance of the icon extractor with

`var iconExtractor = require('icon-extractor');`

This object contains an event emitter with two events, `icon` and `error`

To get an icon's data you need to call the `getIcon` function which takes two parameters.
The first is a context parameter. This will return with the icon data so you can have some information about what the return
data is for. The second parameter is the path of the file you want the icon for.

Then, you need to listen on the emitter for the icon data like this

`iconExtractor.emitter.on('icon', function(iconData){ /*do stuff here*/ });`

This data comes back as a json object containing three fields, `Context`, `Path` and `Base64ImageData`

Here is an example of it all put together

```
var iconExtractor = require('icon-extractor');

iconExtractor.emitter.on('icon', function(data){
  console.log('Here is my context: ' + data.Context);
  console.log('Here is the path it was for: ' + data.Path);
  console.log('Here is the base64 image: ' + data.Base64ImageData);
});

iconExtractor.getIcon('SomeContextLikeAName','c:\myexecutable.exe');
```
