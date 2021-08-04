using System;
using System.Collections.Generic;
using System.IO;
using NUglify;
using NUglify.JavaScript;

namespace WebApp.MinifyCLI
{
    public class Minifier
    {
        private readonly Config _config;
        private readonly FilesInventary _filesInventary;
        private readonly ConsoleHelper _consoleHelper;
        public Minifier(ConsoleHelper consoleHelper, Config config, FilesInventary filesInventary)
        {
            this._consoleHelper = consoleHelper;
            this._filesInventary = filesInventary;
            this._config = config;
        }

        public void Minify()
        {
            string jsRootTargetPath = Path.Combine(Environment.CurrentDirectory,
                    _config.JsRootTarget.Replace('/', Path.DirectorySeparatorChar));
            string cssRootTargetPath = Path.Combine(Environment.CurrentDirectory,
                    _config.CssRootTarget.Replace('/', Path.DirectorySeparatorChar));

            _Minify(_filesInventary.JsSourceFiles, _filesInventary.JsRootSourcePath, jsRootTargetPath);
            _Minify(_filesInventary.CssSourceFiles, _filesInventary.CssRootSourcePath, cssRootTargetPath);
        }

        private void _Minify(IReadOnlyCollection<string> listOfFiles, string rootSourcePath, string rootTargetPath)
        {
            CodeSettings codeSettings = new CodeSettings();
            codeSettings.PreserveFunctionNames = true;

            foreach (var fileName in listOfFiles)
            {
                string sourceCode = File.ReadAllText(fileName);

                UglifyResult result = new UglifyResult();
                if (fileName.EndsWith(".js"))
                    result = Uglify.Js(sourceCode, codeSettings);
                else if (fileName.EndsWith(".css"))
                    result = Uglify.Css(sourceCode);

                if (result.HasErrors == false)
                {
                    string newFilePath = GetNewFilePath(fileName, rootSourcePath, rootTargetPath);

                    Directory.CreateDirectory(Path.GetDirectoryName(newFilePath));
                    File.WriteAllText(newFilePath, result.Code);

                    _consoleHelper.WriteSuccess(fileName.Replace(rootSourcePath, ""));
                }
                else
                {
                    _consoleHelper.WriteError(result.Errors.ToString());
                }

            }
        }

        private string GetNewFilePath(string fileName, string rootSourcePath, string rootTargetPath)
        {
            string newFilePath = fileName.Replace(rootSourcePath, rootTargetPath);
            
            if (newFilePath.EndsWith(".js") && _config.JsInsertMinInfix)
                newFilePath = newFilePath.Insert(newFilePath.Length -3, ".min");
            
            if (newFilePath.EndsWith(".css") && _config.CssInsertMinInfix)
                newFilePath = newFilePath.Insert(newFilePath.Length -4, ".min");

            return newFilePath;
        }

    }
}