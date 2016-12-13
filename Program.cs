using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace ResetNotForThisSite {
    class Program {
        static void Main(string[] args) {
            while (true) {
                SortedDictionary<String, PasswordCredential> entries = new SortedDictionary<string, PasswordCredential>();
                int y = 0;
                foreach (var entry in new Windows.Security.Credentials.PasswordVault().RetrieveAll()
                    .Where(entry => entry.Properties.Any(kv => kv.Key.Equals("hidden") && kv.Value is bool && (bool)kv.Value))
                ) {
                    ++y;
                    entries[y.ToString()] = entry;
                    Console.WriteLine("{0,3} {1} [{2}]", y, entry.Resource, entry.UserName);
                }

                {
                    Console.WriteLine();
                    Console.Write("Delete which? ");
                    String key = Console.ReadLine().Trim();
                    PasswordCredential entry;
                    if (entries.TryGetValue(key, out entry)) {
                        new Windows.Security.Credentials.PasswordVault().Remove(entry);
                        Console.WriteLine("Deleted.");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
