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

            var toCheck = data.Items
                                .Where(o => o.Login != null && !string.IsNullOrWhiteSpace(o.Login.Password))
                                .Select(o => o.Login.Password)
                                .ToList();
            Console.WriteLine($"Checking {toCheck.Count} passwords");
            Console.WriteLine();

            var unsafePasswords = Checker.Check(toCheck).Result;
            if (unsafePasswords.Count == 0)
            {
                Console.WriteLine("No unsafe password found");
            }
            else
            {
                Console.WriteLine("Unsafe passwords found:");
                Console.WriteLine();
                foreach (var pass in unsafePasswords)
                {
                    Console.WriteLine(pass);
                }
            }            
        }
    }
}