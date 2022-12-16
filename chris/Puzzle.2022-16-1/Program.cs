using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2022_16_1
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

            // Read the data.
            var search = new Regex(@"Valve (?<valve>\w+) has flow rate=(?<flow>\d+); tunnels? leads? to valves? (?<tunnels>.*)$");
            var valves = strings
                .Select(s =>
                {
                    var match = search.Match(s);
                    return (valve: match.Groups["valve"].Value, flow: int.Parse(match.Groups["flow"].Value), tunnels: match.Groups["tunnels"].Value.Split(", "));
                })
                .ToArray();
            var byValve = valves.ToDictionary(t => t.valve);

            // Find shortest routes that lead to a valve being turned.
            var routes = valves
                .Where(t => t.flow > 0 || t.valve == "AA")
                .Select(t =>
                {
                    var queue = new Queue<(string, int)>();
                    queue.Enqueue((t.valve, 0));
                    var dists = valves.ToDictionary(v => v.valve, v => 10000);

                    while (queue.Count > 0)
                    {
                        (var next, var dist) = queue.Dequeue();
                        foreach (var tunnel in byValve[next].tunnels)
                        {
                            if (dists[tunnel] >= dist + 1)
                            {
                                dists[tunnel] = dist + 1;
                                queue.Enqueue((tunnel, dist + 1));
                            }
                        }
                    }

                    return (
                        t.valve,
                        dists: valves
                            .Where(t => t.flow > 0)
                            .Select(t => (t.valve, dist: dists[t.valve] + 1))
                            .ToArray());
                })
                .ToDictionary(route => route.valve, route => route.dists);

            // Attempt all paths.
            var paths = new Stack<(string valve, int pressure, int time)[]>();
            paths.Push(new[] { ("AA", 0, 30) });

            var result = 0;
            while (paths.Count > 0)
            {
                var path = paths.Pop();
                var last = path[0];
                foreach ((var tunnel, var distance) in routes[last.valve])
                {
                    if (last.time < distance)
                        continue;
                    if (path.Any(p => p.valve == tunnel))
                        continue;

                    var remains = last.time - distance;
                    var pressure = last.pressure + byValve[tunnel].flow * remains;
                    paths.Push(new[] { (tunnel, pressure, remains) }.Concat(path).ToArray());
                    if (pressure > result)
                    {
                        result = pressure;
                    }
                }
                paths = new Stack<(string valve, int pressure, int time)[]>(paths.OrderBy(p => p[0].pressure));
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}
