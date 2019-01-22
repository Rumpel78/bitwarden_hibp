using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bitwarden_hibp
{
    public static class Checker
    {
        public async static Task<List<string>> Check(IEnumerable<string> passwords)
        {
            var client = RestService.For<IHaveIBeenPawnedApi>("https://api.pwnedpasswords.com");
            var sha1List = new List<(string hash, string pass)>();

            foreach (var password in passwords)
            {
                var hash = GetHashString(password);
                sha1List.Add((hash, password));
            }

            var prefixes = new Dictionary<string, List<(string hash, string pass)>>();
            foreach (var item in sha1List)
            {
                var prefix = item.hash.Substring(0, 5);
                if (!prefixes.ContainsKey(prefix))
                {
                    prefixes.Add(prefix, new List<(string, string)>());
                }
                prefixes[prefix].Add(item);
            }

            var done = 0;
            var foundList = new List<(string pass, int count)>();
            foreach (var kv in prefixes)
            {
                var prefix = kv.Key;
                var response = await client.SearchByRange(prefix);
                var hashCount = Parse(response).ToList();
                done++;
                Console.Write($"\r{done}/{prefixes.Count}".PadRight(10));

                foreach (var hc in hashCount)
                {
                    var found = kv.Value.FirstOrDefault(item => item.hash.Substring(5).Equals(hc.hash, StringComparison.OrdinalIgnoreCase));
                    if (found.hash != null)
                    {
                        foundList.Add((found.pass, hc.count));
                    }
                }
                Console.Write($"Found: {foundList.Count}".PadRight(10));
            }
            return foundList.Select(found => found.pass).ToList();
        }

        private static IEnumerable<(string hash, int count)> Parse(string response)
        {
            var lines = response.Split(Environment.NewLine);
            foreach (var line in lines)
            {
                var split = line.Split(':');
                yield return (split[0], int.Parse(split[1]));
            }
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA1.Create();  // SHA1.Create()
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
