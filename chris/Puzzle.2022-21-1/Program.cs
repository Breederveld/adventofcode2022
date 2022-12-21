using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_21_1
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

            var monkeys = strings
                .Select(s => s.Split(" "))
                .ToDictionary(s => s[0].Trim(':'), s => (val: s.Length == 2 ? int.Parse(s[1]) : -1, l: s.Length == 2 ? string.Empty : s[1], r: s.Length == 2 ? string.Empty : s[3], op: s.Length == 2 ? string.Empty : s[2]));
            var values = monkeys
                .Where(kv => kv.Value.val >= 0)
                .ToDictionary(kv => kv.Key, kv => (long)kv.Value.val);
            while (values.Count < monkeys.Count)
            {
                foreach (var monkey in monkeys)
                {
                    if (values.ContainsKey(monkey.Key))
                    {
                        continue;
                    }
                    if (!values.ContainsKey( monkey.Value.l) || !values.ContainsKey(monkey.Value.r))
                    {
                        continue;
                    }
                    switch (monkey.Value.op)
                    {
                        case "+":
                            values[monkey.Key] = values[monkey.Value.l] + values[monkey.Value.r];
                            break;
                        case "-":
                            values[monkey.Key] = values[monkey.Value.l] - values[monkey.Value.r];
                            break;
                        case "*":
                            values[monkey.Key] = values[monkey.Value.l] * values[monkey.Value.r];
                            break;
                        case "/":
                            values[monkey.Key] = values[monkey.Value.l] / values[monkey.Value.r];
                            break;
                    }
                }
            }

            var result = values["root"];

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}