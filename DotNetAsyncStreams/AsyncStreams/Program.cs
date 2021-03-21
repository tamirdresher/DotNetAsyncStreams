using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncStreams
{
    class Program
    {
        public static async Task Main()
        {
            var records = ReadLines("transactions.csv");
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.Cancel();
            
            await foreach (var record in records.WithCancellation(cts.Token).ConfigureAwait(false))
            {
                Console.WriteLine($"Transaction from {record[0]} to {record[1]}");
            }

            #region LINQ
            var grps = records
                .Where(r => r.Length > 0)
                .GroupBy(r => r[0])
                .SelectAwait(async g =>
                {
                    await Task.Delay(1);
                    return g;
                });

            await foreach (var grp in grps)
            {
                Console.WriteLine($"Key: {grp.Key} Count: {await grp.CountAsync()}");
            }
            #endregion
        }

        public static async IAsyncEnumerable<string[]> ReadLines(string path, [EnumeratorCancellation] CancellationToken token=default)
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("Cancellation requested");
                yield break; 
            }

            var lines = await System.IO.File.ReadAllLinesAsync(path);
            foreach (var line in lines)
            {
                yield return line.Split(',');
            }
        }
    }
}
