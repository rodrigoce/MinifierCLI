using System;
using System.Collections.Generic;
using System.IO;

namespace WebApp.MinifyCLI
{
    public class FilesInventary
    {
        private List<string> _jsSourceFiles;
        private List<string> _cssSourceFiles;
        private Config _config;

        public FilesInventary(Config config)
        {
            _config = config;
            _jsSourceFiles = new List<string>();
            _cssSourceFiles = new List<string>();
        }

        public IReadOnlyCollection<string> JsSourceFiles
        {
            get => _jsSourceFiles.AsReadOnly();
        }

        public IReadOnlyCollection<string> CssSourceFiles
        {
            get => _cssSourceFiles.AsReadOnly();
        }

        public string JsRootSourcePath
        {
            get => Path.Combine(Environment.CurrentDirectory,
                      _config.JsRootSource.Replace('/', Path.DirectorySeparatorChar));
        }

        public string CssRootSourcePath
        {
            get => Path.Combine(Environment.CurrentDirectory,
                _config.CssRootSource.Replace('/', Path.DirectorySeparatorChar));
        }
        public void Exec()
        {

            ReadFilesNames(JsRootSourcePath, _jsSourceFiles);
            ReadFilesNames(CssRootSourcePath, _cssSourceFiles);
        }

        private void ReadFilesNames(string rootSourcePath, List<string> listOfNames)
        {
            var files = Directory.GetFiles(rootSourcePath);

            foreach (var file in files) 
            {
                if (file.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase) &&
                    (!file.EndsWith(".min.js", StringComparison.InvariantCultureIgnoreCase)))
                    listOfNames.Add(file);

                if (file.EndsWith(".css", StringComparison.InvariantCultureIgnoreCase) &&
                    (!file.EndsWith(".min.css", StringComparison.InvariantCultureIgnoreCase)))
                    listOfNames.Add(file);

            }

            var directories = Directory.GetDirectories(rootSourcePath);

            foreach (var subDir in directories)
                ReadFilesNames(subDir, listOfNames);
        }
    }
}