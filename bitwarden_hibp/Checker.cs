using bitwarden_hibp.Model;
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
        public async static Task<List<LoginItem>> Check(Dictionary<SplitHash, List<LoginItem>> hashItems)
        {
            var foundList = new List<LoginItem>();
            var groupedByPrefix = hashItems.GroupBy(o => o.Key.HashPrefix);

            var done = 0;
            foreach (var item in groupedByPrefix)
            {
                done++;
                Console.Write($"\r{done}/{hashItems.Count}".PadRight(10));

                var badHashes = await GetBadHashes(item.Key);

                foreach (var badHash in badHashes)
                {
                    var found = item.FirstOrDefault(o => string.Equals(o.Key.HashSuffix, badHash.hash, StringComparison.OrdinalIgnoreCase));
                    if (found.Key != null)
                    {
                        foundList.AddRange(found.Value);
                    }
                }
                Console.Write($"Found: {foundList.Count}".PadRight(10));
            }
            return foundList;
        }

        private static async Task<IEnumerable<(string hash, int count)>> GetBadHashes(string prefix) {
            var client = RestService.For<IHaveIBeenPawnedApi>("https://api.pwnedpasswords.com");
            var response = await client.SearchByRange(prefix);
            var hashCount = ParseResponse(response).ToList();
            return hashCount;
        }

        private static IEnumerable<(string hash, int count)> ParseResponse(string response)
        {
            var lines = response.Split(Environment.NewLine);
            foreach (var line in lines)
            {
                var split = line.Split(':');
                yield return (split[0], int.Parse(split[1]));
            }
        }
    }
}
