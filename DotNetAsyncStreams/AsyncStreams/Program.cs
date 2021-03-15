using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncStreams
{
    class Program
    {
        public static async Task Main()
        {
            var records = ReadLines("transactions.csv");

            await foreach (var record in records)
            {
                Console.WriteLine($"Transaction from {record[0]} to {record[1]}");
            }

            #region LINQ
            var grps = from r in records
                       where r.Length > 0
                       group r by r[0] into g
                       select g;

            await foreach (var grp in grps)
            {
                Console.WriteLine($"Key: {grp.Key} Count: {await grp.CountAsync()}");
            }
            #endregion
        }

        public static async IAsyncEnumerable<string[]> ReadLines(string path)
        {
            var lines = await System.IO.File.ReadAllLinesAsync(path);
            foreach (var line in lines)
            {
                yield return line.Split(',');
            }
        }
    }
}
