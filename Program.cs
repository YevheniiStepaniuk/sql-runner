using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace SqlRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = 
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var files = configuration.GetSection("Files").GetChildren().Select(x => x.Value).ToArray();
            var sqlConnectionString = configuration["ConnectionString"];

            Console.WriteLine($"Starting connection to: {sqlConnectionString}");

            using (var conn = new SqlConnection(sqlConnectionString))
            {
                var server = new Server(new ServerConnection(conn));
                Console.WriteLine($"Connection success, starting SQL executing, {files.Length}");

                foreach (var file in files)
                {
                    Console.WriteLine($"Starting running: {file} sql files");
                    if (!File.Exists(file))
                    {
                        Console.WriteLine($"File not exist: {file}");
                        continue;
                    }

                    var sql = File.ReadAllText(file);

                    try
                    {
                       var rows = server.ConnectionContext.ExecuteNonQuery(sql, ExecutionTypes.ContinueOnError);
                       Console.WriteLine($"Execution success, affected rows: {rows}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Execution failed: {e.Message}");
                        Console.WriteLine($"Execution failed: {e.StackTrace}");
                    }
                }
            }

            Console.WriteLine("Execution finished, connection was closed");

            Console.WriteLine("Press any key to close...");

            Console.ReadKey();
        }
    }
}
