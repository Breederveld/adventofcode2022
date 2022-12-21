using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle_2022_19_2
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

            var search = new Regex(@"Each (?<robot>\w+) robot costs ((?<cost>\d+ \w+)( and )?)+");
            var blueprints = strings
                .Take(3)
                .Select(s =>
                {
                    var robots = s.Split(". ")
                        .Select(sp =>
                        {
                            var match = search.Match(sp);
                            var dict = match.Groups["cost"].Captures
                                .Select(cap =>
                                {
                                    var capsp = cap.Value.Split();
                                    return (value: int.Parse(capsp[0]), resource: capsp[1]);
                                })
                                .ToDictionary(t => Enum.Parse<Resource>(t.resource, true), t => t.value);
                            var cost = Enum.GetValues<Resource>().Select(r => dict.ContainsKey(r) ? dict[r] : 0).ToArray();
                            return (resource: Enum.Parse<Resource>(match.Groups["robot"].Value, true), cost);
                        })
                        .ToDictionary(t => t.resource, t => new Robot(t.resource, t.cost));
                    return robots;
                })
                .ToArray();
            var result = 1;
            for (var i = 0; i < blueprints.Length; i++)
            {
                var blueprint = blueprints[i];
                var maxGeodes = GetMaxGeodes(blueprint);
                Console.WriteLine($"{sw.Elapsed} : {maxGeodes}");
                result *= maxGeodes;
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private static int GetMaxGeodes(Dictionary<Resource, Robot> robots)
        {
            var initialState = new State(
                new[] { 0, 0, 0, 1 },
                new[] { 0, 0, 0, 0 },
                32);
            return GetBestMove(robots, initialState);
        }

        private static int GetBestMove(Dictionary<Resource, Robot> robots, State state)
        {
            var maxCosts = Enumerable.Range(0, robots.Count)
                .ToDictionary(r => (Resource)r, r => robots.Max(robot => robot.Value.Cost[r]));

            var max = 0;
            var stack = new Stack<(Resource, State)>();
            stack.Push((0, state));
            while (stack.Count > 0)
            {
                (var resource, state) = stack.Peek();
                var robot = robots[resource];
                var currState = state;
                if (resource == Resource.Geode || state.Robots[(int)resource] < maxCosts[resource])
                {
                    var found = false;
                    do
                    {
                        if (currState.TryAddRobot(robot, out currState))
                        {
                            found = true;
                        }
                    }
                    while (!found && currState.TryAddMinutes(1, out currState));
                    var geodes = currState.Resources[(int)Resource.Geode];
                    var geodesRobots = currState.Robots[(int)Resource.Geode];
                    if (geodes > 0)
                    {
                        if (geodes > max)
                        {
                            max = geodes;
                        }
                    }
                    if (found)
                    {
                        stack.Push((0, currState));
                        continue;
                    }
                }
                stack.Pop();
                resource++;
                currState = state;
                while ((int)resource == 4 && stack.Count > 0)
                {
                    (resource, currState) = stack.Pop();
                    resource++;
                }
                if ((int)resource < 4)
                {
                    stack.Push((resource, currState));
                }
            }
            return max;
        }

        private enum Resource
        {
            Ore = 3,
            Clay = 2,
            Obsidian = 1,
            Geode = 0,
        }

        private record Robot(
            Resource Resource,
            int[] Cost);

        private record State(
            int[] Robots,
            int[] Resources,
            int Remaining)
        {
            public bool TryAddRobot(Robot robot, out State newState)
            {
                newState = ForceAddRobot(robot);
                if (newState.Resources.Any(v => v < 0))
                {
                    newState = this;
                    return false;
                }
                if (!TryAddMinutes(1, out newState))
                {
                    newState = this;
                    return false;
                }
                newState = newState.ForceAddRobot(robot);
                return true;
            }

            public State ForceAddRobot(Robot robot)
            {
                return new State(
                    Robots.Select((v, idx) => v + (idx == (int)robot.Resource ? 1 : 0)).ToArray(),
                    Resources.Select((v, idx) => v - robot.Cost[idx]).ToArray(),
                    Remaining);
            }

            public bool TryAddMinutes(int minutes, out State newState)
            {
                if (Remaining < minutes)
                {
                    newState = this;
                    return false;
                }
                newState = ForceAddMinutes(minutes);
                return true;
            }

            public State ForceAddMinutes(int minutes)
            {
                return new State(
                    Robots,
                    Resources.Select((v, idx) => v + Robots[idx] * minutes).ToArray(),
                    Remaining - minutes);
            }
        }
    }
}
