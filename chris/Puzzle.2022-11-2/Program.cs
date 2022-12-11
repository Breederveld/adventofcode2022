using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_11_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rootFolder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var input = File.ReadAllText(Path.Combine(rootFolder, "input.txt"));

            var sw = new Stopwatch();
            var strings = input.Trim().Split("\n").Select(s => s.TrimEnd()).ToArray();
            //var groups = input.Trim().Split("\n\n").Select(grp => grp.Split("\n").ToArray()).ToArray();
            //var ints = strings.Where(st => !string.IsNullOrWhiteSpace(st)).Select(st => int.Parse(st)).ToArray();
            sw.Start();

            var monkeys = new List<Monkey>();
            var idx = -1;
            var mod = 1;
            for (int sIdx = 0; sIdx < strings.Length; sIdx++)
            {
                var str = strings[sIdx];
                if (str.Length == 0) continue;
                if (str[0] != ' ')
                {
                    monkeys.Add(new Monkey());
                    idx++;
                }
                else if (str[2] == 'S')
                {
                    monkeys[idx].Items = str.Substring(18).Split(", ").Select(s => long.Parse(s)).ToList();
                }
                else if (str[2] == 'O')
                {
                    var parts = str.Split(' ');
                    var op = parts.Reverse().Skip(1).First();
                    var val = parts.Last();
                    switch (op)
                    {
                        case "*":
                            if (val == "old")
                                monkeys[idx].Op = i => i * i;
                            else
                                monkeys[idx].Op = i => i * int.Parse(val);
                            break;
                        case "+":
                            if (val == "old")
                                monkeys[idx].Op = i => i + i;
                            else
                                monkeys[idx].Op = i => i + int.Parse(val);
                            break;
                    }
                }
                else if (str[2] == 'T')
                {
                    var test = int.Parse(str.Substring(21));
                    sIdx++;
                    var mt = int.Parse(strings[sIdx].Split(' ').Last());
                    sIdx++;
                    var mf = int.Parse(strings[sIdx].Split(' ').Last());
                    monkeys[idx].Test = i => i % test == 0 ? mt : mf;
                    mod *= test;
                }
            }

            for (int round = 0; round < 10000; round++)
            {
                for (int turn = 0; turn < monkeys.Count; turn++)
                {
                    var monkey = monkeys[turn];
                    var items = monkey.Items.ToArray();
                    monkey.Items.Clear();
                    foreach (int item in items)
                    {
                        var worry = monkey.Op(item) % mod;
                        var next = monkey.Test(worry);
                        monkeys[next].Items.Add(worry);
                        monkey.Inspected++;
                    }
                }
            }
            var sub = monkeys.OrderByDescending(m => m.Inspected).Take(2).ToArray();
            var result = sub[0].Inspected * sub[1].Inspected;

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }

    public class Monkey
    {
        public List<long> Items { get; set; }
        public Func<long, long> Op { get; set; }
        public Func<long, int> Test { get; set; }
        public long Inspected { get; set; }
    }
}