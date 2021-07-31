using System;
using System.IO;
using Newtonsoft.Json;

namespace WebApp.MinifyCLI
{
    public class ConfigReader
    {
        private readonly ConsoleHelper _consoleHelper;
        public ConfigReader(ConsoleHelper consoleHelper)
        {
            this._consoleHelper = consoleHelper;
        }

        public Config ReadConfig()
        {
            var config = new Config();
            string fileName;

            fileName = Path.Combine(Environment.CurrentDirectory, "minify.json");
            if (!File.Exists(fileName))
                _consoleHelper.WriteError("minify.json does not exist.");
            else
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    config = JsonConvert.DeserializeObject<Config>(json);
                }

                if (config.InsertTimeStampInAppSettings)
                {
                    fileName = Path.Combine(Environment.CurrentDirectory, "appsettings.json");
                    if (!File.Exists(fileName))
                        _consoleHelper.WriteError("appsettings.json does not exist.");
                }

                // js config validation
                if (string.IsNullOrWhiteSpace(config.JsRootSource))
                    _consoleHelper.WriteError(string.Format("{0} property not set.", nameof(config.JsRootSource)));
                else
                {
                    string path = Path.Combine(Environment.CurrentDirectory,
                        config.JsRootSource.Replace('/', Path.DirectorySeparatorChar));

                    if (!Directory.Exists(path))
                        _consoleHelper.WriteError(string.Format("{0} does not exist.", path));
                }

                if (string.IsNullOrWhiteSpace(config.JsRootTarget))
                    _consoleHelper.WriteError(string.Format("{0} property not set.", nameof(config.JsRootTarget)));
                else
                {
                    string path = Path.Combine(Environment.CurrentDirectory,
                        config.JsRootTarget.Replace('/', Path.DirectorySeparatorChar));

                    if (!Directory.Exists(path))
                        _consoleHelper.WriteError(string.Format("{0} does not exist.", path));
                }

                if (config.JsRootSource.Equals(config.JsRootTarget, StringComparison.InvariantCultureIgnoreCase) &&
                    (config.JsInsertMinInfix == false))
                    _consoleHelper.WriteError(string.Format("when {0} is equals {1}, {2} must be true.",
                        nameof(config.JsRootSource), nameof(config.JsRootTarget), nameof(config.JsInsertMinInfix)));

                // css config validation
                if (string.IsNullOrWhiteSpace(config.CssRootSource))
                    _consoleHelper.WriteError(string.Format("{0} property not set.", nameof(config.CssRootSource)));
                else
                {
                    string path = Path.Combine(Environment.CurrentDirectory,
                        config.CssRootSource.Replace('/', Path.DirectorySeparatorChar));

                    if (!Directory.Exists(path))
                        _consoleHelper.WriteError(string.Format("{0} does not exist.", path));
                }

                if (string.IsNullOrWhiteSpace(config.CssRootTarget))
                    _consoleHelper.WriteError(string.Format("{0} property not set.", nameof(config.CssRootTarget)));
                else
                {
                    string path = Path.Combine(Environment.CurrentDirectory,
                        config.CssRootTarget.Replace('/', Path.DirectorySeparatorChar));

                    if (!Directory.Exists(path))
                        _consoleHelper.WriteError(string.Format("{0} does not exist.", path));
                }

                if (config.CssRootSource.Equals(config.CssRootTarget, StringComparison.InvariantCultureIgnoreCase) &&
                    (config.CssInsertMinInfix == false))
                    _consoleHelper.WriteError(string.Format("when {0} is equals {1}, {2} must be true.",
                        nameof(config.CssRootSource), nameof(config.CssRootTarget), nameof(config.CssInsertMinInfix)));
            }

            return config;
        }
    }
}