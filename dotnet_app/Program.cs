using bitwarden_hibp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            var dataItems = data.Items
                                .Where(o => o.Login != null && !string.IsNullOrWhiteSpace(o.Login.Password))
                                .ToList();

            var hashItems = new Dictionary<SplitHash, List<LoginItem>>();
            foreach (var dataItem in dataItems)
            {
                var splitHash = new SplitHash(dataItem.Login.Password);
                if (!hashItems.ContainsKey(splitHash)) {
                    hashItems.Add(splitHash, new List<LoginItem>());
                }
                hashItems[splitHash].Add(new LoginItem { SplitHash = splitHash, Item = dataItem });
            }

            Console.WriteLine($"Checking {hashItems.Count} passwords");
            Console.WriteLine();

            var unsafeLogins = Checker.Check(hashItems).Result;
            Console.WriteLine();

            if (unsafeLogins.Count == 0)
            {
                Console.WriteLine("No unsafe logins found");
            }
            else
            {
                Console.WriteLine("Unsafe logins found:");
                Console.WriteLine();
                foreach (var pass in unsafeLogins)
                {
                    Console.WriteLine(pass.Item.Id);
                    Console.WriteLine(pass.Item.name);
                    Console.WriteLine(pass.Item.Login.Username);
                    Console.WriteLine(pass.Item.Login.Password);
                    Console.WriteLine();
                }
            }            
        }
    }
}