# MinifierCLI

CLI tool for minify js and css files.
It requires the minify.json file.

```
{
    "JsRootSource": "js/notmin",
    "JsRootTarget": "js/min",
    "JsInsertMinInfix" : true,
    
    "CssRootSource": "css/notmin",
    "CssRootTarget": "css/min",
    "CssInsertMinInfix" : true,

    "InsertTimeStampInAppSettings": true
}
```

The target path can be the same of source path.

When InsertTimeStampInAppSettings is true, it will create an attribute in file appsettings.json with the timestamp of minify execution that can be used for the cache invalidation.

So, type MinifierCLI
