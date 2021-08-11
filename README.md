# MinifierCLI

CLI tool for minify js and css files.
It requires the minify.json file.

```
{
    "JsRootSource": "js/notmin",
    "JsRootTarget": "js/min",
    "JsInsertMinInfix" : true,
    "JsUseBasicMinify" : false,
    
    "CssRootSource": "css/notmin",
    "CssRootTarget": "css/min",
    "CssInsertMinInfix" : true,
    "CssUseBasicMinify" : false,

    "InsertTimeStampInAppSettings": true
}
```

The target path can be the same of source path.

When InsertTimeStampInAppSettings is true, it will create an attribute in file appsettings.json with the timestamp of minify execution that can be used for the cache invalidation.

# Instalation

dotnet tool install --global MinifierCLI

So, type MinifierCLI
