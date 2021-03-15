using System;
using System.Collections.Generic;
using System.Linq;

namespace AsyncStreams
{
    class Program
    {
        public static void Main()
        {
            var records = ReadLines("transactions.csv");
            
            var grps = from r in records
                where r.Length > 0
                group r by r[0] into g
                select g;

            foreach (var grp in grps)
            {
                Console.WriteLine($"Key: {grp.Key} Count: {grp.Count()}");
            }
        }

        public static IEnumerable<string[]> ReadLines(string path)
        {
            var lines = System.IO.File.ReadAllLines(path);
            foreach (var line in lines)
            {
                yield return line.Split(',');
            }
        }
    }
}
