using System;
using Microsoft.Extensions.Hosting;

using bookParser.startup;
using bookParser.Controllers;
using bookParser.repository;
using bookParser.configManager;

namespace bookParser{
    class Program{
        static void Main(string[] args){
            ConfigReader configReader = new ConfigReader(); //custom configuration file
            Config dbConfig = configReader.ReadConfig("config.json");

            string connectionConfig = $@"   
                Server={dbConfig.server};
                Port={dbConfig.port};
                UserId={dbConfig.username};
                Password={dbConfig.password};
                Database={dbConfig.dbname};";


            Repository dbNpgsql = new Repository(connectionConfig);

            if(dbNpgsql.TestConnection()){
                Console.WriteLine("Server is ready");
            }
            else{
                Console.WriteLine("Server not reached");
                Environment.Exit(1);
            }

            dbNpgsql.OpenConnection();

            TestController myController = new TestController(dbNpgsql);


            Startup test = new Startup();
            test.CreateHostBuilder(args).Build().Run();

            dbNpgsql.CloseConnection();
        }
    }
}
