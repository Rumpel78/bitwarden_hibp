using bitwarden_hibp.Model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace bitwarden_hibp
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = args.Length > 0 ? args[0] : "bitwarden_export_sample.json";
            var content = File.ReadAllText(filename);
            var data = JsonConvert.DeserializeObject<BitwardenExport>(content);
            Console.WriteLine($"Checking {data.Items.Count} passwords");
            Console.WriteLine();

            var passwords = Checker.Check(data.Items.Select(item => item.Login.Password)).Result;

            if (passwords.Count == 0)
            {
                Console.WriteLine("No unsafe password found");
            }
            else
            {
                Console.WriteLine("Unsafe passwords found:");
                Console.WriteLine();
                foreach (var pass in passwords)
                {
                    Console.WriteLine(pass);
                }
            }
            Console.ReadLine();
        }
    }
}