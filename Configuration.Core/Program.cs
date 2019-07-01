// Kræver NuGet: Microsoft.Extensions.Configuration.Json
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Configuration.Core
{
    class Program
    {
        static void Main(string[] args)
        {

            var c1 = ConfigSetupWeak();
            Console.WriteLine(c1["ApplicationName"]);
            Console.WriteLine(c1.GetSection("SectionA")["ModuleName"]);

            var c2 = ConfigSetupStrong();
            Console.WriteLine(c2.ApplicationName);
            Console.WriteLine(c2.MyModuleSettings.ModuleName);


        }

        static IConfigurationRoot ConfigSetupWeak() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }

        static MySettings ConfigSetupStrong()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            var settings = new MySettings();
            // Kræver NuGet: Microsoft.Extensions.Configuration.Binder
            configuration.Bind(settings);
            settings.MyModuleSettings = new MyModuleSettings();
            configuration.GetSection("SectionA").Bind(settings.MyModuleSettings);
            return settings;
        }
    }

    public class MySettings {
        public string ApplicationName { get; set; }
        public MyModuleSettings MyModuleSettings { get; set; }
    }

    public class MyModuleSettings
    {
        public string ModuleName { get; set; }
    }
}
