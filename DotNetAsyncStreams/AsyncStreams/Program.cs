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
            
            var grps = records
                .Where(r => r.Length > 0)
                .GroupBy(r => r[0])
                .SelectAwait(async g =>
                {
                    await Task.Delay(1);
                    return g;
                });
            var firstGrp = await grps.FirstAsync();

            await foreach (var transactions in firstGrp.Buffer(2))
            {
                Console.WriteLine($"T1: {transactions[0][1]} T2: {transactions[1][1]}");
            }
           
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
