namespace A12;

public static class Solution
{
    public class Plot
    {
        public char Plant { get; init; }
        public int Region { get; set; }
        public (int X, int Y) Point { get; set; }
    }

    public class Region
    {
        public int Id { get; set; }
        public char Plant { get; init; }
        public int Perimeter { get; set; }
        public int Area { get; set; }
        public List<Plot> Plots { get; set; } = new();
    }
    
    public class Garden
    {
        public Dictionary<(int X, int Y), Plot> Plots { get; set; } = new();
        public int RegionId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
    
    public static int Solve(string dataPath)
    {
        var garden = ToGarden(File.ReadAllText(dataPath));
        var regions = GetRegions(garden);

        return CalculateCost(regions);
    }

    public static int CalculateCost(List<Region> regions)
    {
        var solution = 0;

        foreach (var region in regions)
        {
            solution += (region.Area * region.Perimeter);
        }

        return solution;
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
            
            p.Region = region.Id;
            region.Area++;
            region.Plots.Add(p);
            
            var (x, y) = p.Point;
            TryVisit(p, (x, y - 1));
            TryVisit(p, (x - 1, y));
            TryVisit(p, (x, y + 1));
            TryVisit(p, (x + 1, y));
        }

        return region;

        void TryVisit(Plot from, (int x, int y) point)
        {
            if (garden.Plots.TryGetValue(point, out var to) && from.Plant == to.Plant)
            {
                plots.Push(to);
            } else {
                region.Perimeter++;
            }
        }
    }
}