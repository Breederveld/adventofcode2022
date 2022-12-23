using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_23_1
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

            var elves = strings
                .SelectMany((s, y) => s.Select((c, x) => (x, y, c)))
                .Where(t => t.c == '#')
                .Select(t => new Elf { X = t.x, Y = t.y })
                .ToArray();

            var directions = new List<Direction>(new[] { Direction.North, Direction.South, Direction.West, Direction.East });
            for (int i = 0; i < 10; i++)
            {
                var dirs = directions.Skip(i % 4).Concat(directions.Take(i % 4)).ToArray();
                var toMove = elves
                    .Select(elf => (elf, proposed: elf.Propose(elves, dirs)))
                    .GroupBy(t => t.proposed)
                    .Where(grp => grp.Count() == 1)
                    .Select(grp => grp.First())
                    .Where(t => t.proposed != (t.elf.X, t.elf.Y))
                    .ToArray();
                foreach (var tup in toMove)
                {
                    tup.elf.X = tup.proposed.x;
                    tup.elf.Y = tup.proposed.y;
                }
            }
            var minX = elves.Min(e => e.X);
            var maxX = elves.Max(e => e.X);
            var minY = elves.Min(e => e.Y);
            var maxY = elves.Max(e => e.Y);
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    Console.Write(elves.Any(e => e.X == x && e.Y == y) ? '#' : '.');
                }
                Console.WriteLine();
            }
            var result = (maxX - minX + 1) * (maxY - minY + 1) - elves.Length;

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private enum Direction
        {
            None,
            North,
            South,
            West,
            East,
        }

        private class Elf
        {
            public int X;
            public int Y;
            public Direction Proposed;

            public (int x, int y) Propose(ICollection<Elf> elves, IEnumerable<Direction> directions)
            {
                if (!elves.Any(e => (e.X != X || e.Y != Y) && Math.Abs(e.X - X) <= 1 && Math.Abs(e.Y - Y) <= 1))
                {
                    Proposed = Direction.None;
                    return (X, Y);
                }

                foreach (var dir in directions)
                {
                    var possible = false;
                    switch (dir)
                    {
                        case Direction.North:
                            possible = elves.All(e => e.Y != Y - 1 || e.X < X - 1 || e.X > X + 1);
                            break;
                        case Direction.East:
                            possible = elves.All(e => e.X != X + 1 || e.Y < Y - 1 || e.Y > Y + 1);
                            break;
                        case Direction.South:
                            possible = elves.All(e => e.Y != Y + 1 || e.X < X - 1 || e.X > X + 1);
                            break;
                        case Direction.West:
                            possible = elves.All(e => e.X != X - 1 || e.Y < Y - 1 || e.Y > Y + 1);
                            break;
                        default:
                            return (X, Y);
                    }
                    if (possible)
                    {
                        Proposed = dir;
                        return Move(dir);
                    }
                }
                Proposed = Direction.None;
                return (X, Y);
            }

            public (int x, int y) Move(Direction dir)
            {
                switch (dir)
                {
                    case Direction.North:
                        return (X, Y - 1);
                    case Direction.East:
                        return (X + 1, Y);
                    case Direction.South:
                        return (X, Y + 1);
                    case Direction.West:
                        return (X - 1, Y);
                    default:
                        return (X, Y);
                }
            }
        }
    }
}
