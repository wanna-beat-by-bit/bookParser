
using System;
using System.IO;
using Newtonsoft.Json;

namespace bookParser.configManager{
    class Config //includes all fields to manage database connection
    {
        public string? username { get; set; }
        public string? host { get; set; }
        public string? port { get; set; }
        public string? dbname { get; set; }
        public string? server { get; set; }
        public string? password { get; set; }

    }

    class ConfigReader
    {
        public Config ReadConfig(string filePath)
        {
            string json = File.ReadAllText(filePath);
            Config? config = JsonConvert.DeserializeObject<Config>(json);
            if(config != null){
                return config;
            }
            else{
                Console.WriteLine("Bad database config file!");
                Environment.Exit(1);     
                return null;
            }
        }
    }
}