using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using NUglify;

namespace WebApp.MinifyCLI
{
    class Program 
    {
        static void Main(string[] args)
        {
            var consoleHelper = new ConsoleHelper();
            consoleHelper.WriteInfo("=========== WebAppMinifyCLI Tool ===========");
            

            var configReader = new ConfigReader(consoleHelper);
            var config = configReader.ReadConfig();


            if (consoleHelper.ErrorCount == 0)
            {
                var filesInventary = new FilesInventary(config);
                filesInventary.Exec();
                
                var minifier = new Minifier(consoleHelper, config, filesInventary);
                minifier.Minify();
                
                if (config.InsertTimeStampInAppSettings)
                    WriteTimeStamp();
            }

            consoleHelper.RestoreInitForegroundColor();
        }
        
        static void WriteTimeStamp()
        {
            var fileName = Path.Combine(Environment.CurrentDirectory, "appsettings.json");
            string content = File.ReadAllText(fileName);

            dynamic appsettings;
            appsettings = JsonConvert.DeserializeObject(content);
            appsettings["MinFilesTimeStamp"] = DateTime.Now.ToString("yyyyMMddHHmm");
            content = JsonConvert.SerializeObject(appsettings, Formatting.Indented);

            File.WriteAllText(fileName, content);
        }
    }
}
