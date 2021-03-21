using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncStreams
{
    class Program
    {
        public static async Task Main()
        {
            var first = AsyncEnumerable.Range(1, 10)
                .Do(async x => await Task.Delay(1000))
                .ToObservable();
            var second = AsyncEnumerable.Range(20, 10)
                .Do(async x => await Task.Delay(500))
                .ToObservable();

            var merged =
                first.Merge(second)
                    .ToAsyncEnumerable();

            await foreach (var x in merged)
            {
                Console.WriteLine(x);
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
