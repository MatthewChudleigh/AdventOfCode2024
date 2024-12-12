﻿namespace A12;

public static class Solution
{
    public class Plot
    {
        public char Plant { get; init; }
        public int Region { get; set; }
        public (int X, int Y) Point { get; set; }
    }

    public class Side
    {
        public int Id { get; set; }
        public HashSet<Plot> Plots { get; set; } = new();
    }

    public class Region
    {
        public int Id { get; set; }
        public char Plant { get; set; }
        public int Perimeter { get; set; }
        public int Area { get; set; }
        public List<Plot> Plots { get; set; } = new();
        public int SideCount { get; set; }
        public Dictionary<(int X, int Y, Tlbr Tlbr), Side> Sides { get; set; } = new();
    }
    
    public class Garden
    {
        public Dictionary<(int X, int Y), Plot> Plots { get; set; } = new();
        public int RegionId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
    
    public static (int Solution, int SolutionWithSides) Solve(string dataPath)
    {
        var garden = ToGarden(File.ReadAllText(dataPath));
        var regions = GetRegions(garden);

        return CalculateCost(regions);
    }

    public static (int Solution, int SolutionWithSides) CalculateCost(List<Region> regions)
    {
        var solution = 0;
        var solutionWithSides = 0;

        foreach (var region in regions)
        {
            var sides = region.Sides.GroupBy(s => s.Value.Id)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Key));

            solution += (region.Area * region.Perimeter);
            solutionWithSides += (region.Area * sides.Count);
        }

        return (solution, solutionWithSides);
    }

    public static Garden ToGarden(string data)
    {
        var garden = new Garden();
        int width = 0, height = 0;
        foreach (var (x, y, c) in data.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                     .SelectMany((line, y) => line.Select((c, x) => (x, y, c))))
        {
            width = x > width ? x : width;
            height = y > height ? y : height; 
            garden.Plots[(x, y)] = new Plot() { Plant = c, Region = 0, Point = (x,y) };
        }

        garden.Width = width + 1;
        garden.Height = height + 1;
        return garden;
    }
    
    public static List<Region> GetRegions(Garden garden)
    {
        var regions = new List<Region>();
        var visited = new HashSet<Plot>();

        foreach (var plot in garden.Plots)
        {
            if (visited.Contains(plot.Value)) continue;
            var plots = new Stack<Plot>();
            plots.Push(plot.Value);
            regions.Add(VisitPlot(garden, visited, plots));
        }
        
        return regions;
    }

    public enum Tlbr
    {
        Top,
        Left,
        Bottom,
        Right
    };
    private static Region VisitPlot(Garden garden, HashSet<Plot> visited, Stack<Plot> plots)
    {
        garden.RegionId++;
        var region = new Region()
        {
            Id = garden.RegionId
        };

        while (plots.TryPop(out var p))
        {
            if (!visited.Add(p)) continue;
            region.Plant = p.Plant;
            
            p.Region = region.Id;
            region.Area++;
            region.Plots.Add(p);
            
            var (x, y) = p.Point;
            var top = TryVisit(p, (x, y - 1));
            var left = TryVisit(p, (x - 1, y));
            var bottom = TryVisit(p, (x, y + 1));
            var right = TryVisit(p, (x + 1, y));
            
            AddSides(p, region, top, left, bottom, right);
        }

        return region;

        bool TryVisit(Plot from, (int x, int y) point)
        {
            if (garden.Plots.TryGetValue(point, out var to) && from.Plant == to.Plant)
            {
                plots.Push(to);
                return true;
            } else {
                region.Perimeter++;
                return false;
            }
        }
    }

    private static void AddSides(
        Plot p, Region region,
        bool top, bool left, bool bottom, bool right)
    {
        var (x, y) = p.Point;

        if (!top)
        {
            AddSide((1, 0), Tlbr.Top);
        }

        if (!left)
        {
            AddSide((0, 1), Tlbr.Left);
        }

        if (!bottom)
        {
            AddSide((1, 0), Tlbr.Bottom);
        }
        
        if (!right)
        {
            AddSide((0, 1), Tlbr.Right);
        }

        void AddSide((int X, int Y) to, Tlbr dir)
        {
            region.Sides.TryGetValue((p.Point.X + to.X, p.Point.Y + to.Y, dir), out var lhs);
            region.Sides.TryGetValue((p.Point.X - to.X, p.Point.Y - to.Y, dir), out var rhs);
            if (lhs == null && rhs == null)
            {
                region.SideCount++;
                var side = new Side()
                {
                    Id = region.SideCount
                };
                region.Sides[(p.Point.X, p.Point.Y, dir)] = side;
                side.Plots.Add(p);
            } 
            else if (lhs != null)
            {
                if (rhs != null)
                {
                    foreach (var r in rhs.Plots)
                    {
                        lhs.Plots.Add(r);
                        region.Sides[(r.Point.X, r.Point.Y, dir)] = lhs;
                    }
                }

                lhs.Plots.Add(p);
                region.Sides[(p.Point.X, p.Point.Y, dir)] = lhs;
            }
            else if (rhs != null)
            {
                rhs.Plots.Add(p);
                region.Sides[(p.Point.X, p.Point.Y, dir)] = rhs;
            }

        }
    }
}