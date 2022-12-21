using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Puzzle_2022_19_1
{
    class Program
    {
        private static List<string> _resources;

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
            _resources = new List<string>();
            var blueprints = strings
                .Select(s =>
                {
                    var robots = s.Split(". ")
                        .Select(sp =>
                        {
                            var match = search.Match(sp);
                            _resources.Add(match.Groups["robot"].Value);
                            var cost = match.Groups["cost"].Captures
                                .Select(cap =>
                                {
                                    var capsp = cap.Value.Split();
                                    return (value: int.Parse(capsp[0]), resource: capsp[1]);
                                })
                                .ToDictionary(t => t.resource, t => t.value);
                            return (resource: match.Groups["robot"].Value, cost);
                        })
                        .ToDictionary(t => t.resource, t => t.cost);
                    return robots;
                })
                .ToArray();
            _resources = _resources.Distinct().ToList();
            blueprints = blueprints
                .Select(b => b.ToDictionary(kv => kv.Key, kv => _resources.ToDictionary(r => r, r => kv.Value.ContainsKey(r) ? kv.Value[r] : 0)))
                .ToArray();
            var result = 0;
            for (var i = 0; i < blueprints.Length; i++)
            {
                var blueprint = blueprints[i];
                var maxGeodes = GetMaxGeodes(blueprint);
                result += maxGeodes * (i + 1);
            }

            // > 983
            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private static int GetMaxGeodes(Dictionary<string, Dictionary<string, int>> robots)
        {
            var state = new State(
                _resources.ToDictionary(r => r, r => r == "ore" ? 1 : 0),
                _resources.ToDictionary(r => r, r => 0),
                24);

            state = GetBestMove(robots, state);

            return state.Resources["geode"];
        }

        static string[]_robotPrio = new[] { "geode", "obsidian", "clay", "ore" };
        private static State GetBestMove(Dictionary<string, Dictionary<string, int>> robots, State state)
        {
            var max = state;
            var minDepth = 0;
            var stack = new Stack<(int, State)>();
            stack.Push((0, state));
            while (stack.Count > 0)
            {
                (var robotIdx, state) = stack.Peek();
                var robotKey = _robotPrio[robotIdx];
                var currState = state;
                if (stack.Count == 5)
                {
                    Console.WriteLine(new string(stack.Select(t => (char)(t.Item1 + '0')).ToArray()));
                }
                var found = false;
                do
                {
                    if (TryAddRobot(currState, robots, robotKey, out currState))
                    {
                        found = true;
                    }
                }
                while (!found && TryAddMinute(currState, robots, out currState));
                if (currState.Resources["geode"] > 0)
                {
                    if (minDepth > currState.Remaining)
                    {
                        minDepth = currState.Remaining;
                    }
                    if (currState.Resources["geode"] > max.Resources["geode"])
                    {
                        max = currState;
                    }
                }
                else if (currState.Remaining <= minDepth)
                {
                    found = false;
                }
                if (found)
                {
                    stack.Push((0, currState));
                    continue;
                }
                stack.Pop();
                robotIdx++;
                currState = state;
                while (robotIdx == 4 && stack.Count > 0)
                {
                    (robotIdx, currState) = stack.Pop();
                    robotIdx++;
                }
                if (robotIdx < 4)
                {
                    stack.Push((robotIdx, currState));
                }
            }
            return max;
        }

        //private static State GetBestMove(Dictionary<string, Dictionary<string, int>> robots, State state)
        //{
        //    var max = state;
        //    var stack = new Stack<(int, State)>();
        //    stack.Push((0, state));
        //    while (stack.Count > 0)
        //    {
        //        (var robotIdx, state) = stack.Peek();
        //        var robotKey = _robotPrio[robotIdx];
        //        var currState = state;
        //        if (stack.Count == 5)
        //        {
        //            Console.WriteLine(new string(stack.Select(t => (char)(t.Item1 + '0')).ToArray()));
        //        }
        //        var found = false;
        //        do
        //        {
        //            if (TryAddRobot(currState, robots, robotKey, out currState))
        //            {
        //                found = true;
        //            }
        //        }
        //        while (!found && TryAddMinute(currState, robots, out currState));
        //        if (currState.Resources["geode"] > max.Resources["geode"])
        //        {
        //            max = currState;
        //        }
        //        if (found)
        //        {
        //            stack.Push((0, currState));
        //            continue;
        //        }
        //        stack.Pop();
        //        robotIdx++;
        //        currState = state;
        //        while (robotIdx == 4 && stack.Count > 0)
        //        {
        //            (robotIdx, currState) = stack.Pop();
        //            robotIdx++;
        //        }
        //        if (robotIdx < 4)
        //        {
        //            stack.Push((robotIdx, currState));
        //        }
        //    }
        //    return max;
        //}

        private static bool TryAddRobot(State state, Dictionary<string, Dictionary<string, int>> robots, string robotKey, out State newState)
        {
            newState = AddRobot(state, robots, robotKey);
            if (newState.Resources.Any(kv => kv.Value < 0))
            {
                newState = state;
                return false;
            }
            if (!TryAddMinute(state, robots, out newState))
            {
                return false;
            }
            newState = AddRobot(newState, robots, robotKey);
            return true;
        }

        private static State AddRobot(State state, Dictionary<string, Dictionary<string, int>> robots, string robotKey)
        {
            var robot = robots[robotKey];
            return new State(
                state.Robots.ToDictionary(kv => kv.Key, kv => kv.Value + (kv.Key == robotKey ? 1 : 0)),
                state.Resources.ToDictionary(kv => kv.Key, kv => kv.Value - robot[kv.Key]),
                state.Remaining);
        }

        private static bool TryAddMinute(State state, Dictionary<string, Dictionary<string, int>> robots, out State newState)
        {
            if (state.Remaining == 0)
            {
                newState = state;
                return false;
            }
            newState = new State(
                state.Robots.ToDictionary(kv => kv.Key, kv => kv.Value),
                state.Resources.ToDictionary(kv => kv.Key, kv => kv.Value + state.Robots[kv.Key]),
                state.Remaining - 1);
            return true;
        }

        private record State(
            Dictionary<string, int> Robots,
            Dictionary<string, int> Resources,
            int Remaining);
    }
}
