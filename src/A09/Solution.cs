namespace A09;

public static class Solution
{
    public static long Checksum(string dataPath, bool fullFile)
    {
        long checksum = 0;
        
        var data = File.ReadAllText(dataPath);
        var files = new Dictionary<int, (int Id, int Length)>();
        var freeChunks = new Dictionary<int, int>();

        var id = 0;
        var index = 0;
        for (var i = 0; i < data.Length; i++)
        {
            var length = data[i] - 48;
            if (i % 2 == 0)
            {
                files[index] = (id, length);
                id++;
            }
            else if (length > 0)
            {
                freeChunks[index] = length;
            }

            index += length;
        }

        var allFiles = files.OrderByDescending(kv => kv.Value.Id).ToList();
        foreach (var file in allFiles)
        {
            var fileValue = file.Value;
            while (fileValue.Length > 0)
            {
                var free = freeChunks
                    .Where(kv => kv.Key < file.Key && kv.Value >= (fullFile ? file.Value.Length : 1))
                    .OrderBy(kv => kv.Key)
                    .FirstOrDefault();

                if (free.Value == 0) break;
                
                freeChunks.Remove(free.Key);
                if (free.Value > fileValue.Length)
                {
                    freeChunks[free.Key + fileValue.Length] = free.Value - fileValue.Length;
                }
                
                if (free.Value < fileValue.Length)
                {
                    fileValue = (fileValue.Id, fileValue.Length - free.Value);
                    files[file.Key] = fileValue;
                    files[free.Key] = (fileValue.Id, free.Value);
                }
                else
                {
                    files.Remove(file.Key);
                    files[free.Key] = fileValue;
                    break;
                }
            }
        }

        checksum = CalculateChecksum(files);
        
        return checksum;
    }

    private static long CalculateChecksum(Dictionary<int, (int Id, int Length)> files, bool print = false)
    {
        long checksum = 0;
        var index = 0;
        foreach (var file in files.OrderBy(kv => kv.Key))
        {
            while (index < file.Key)
            {
                if (print)
                {
                    Console.Write(".");
                }

                index++;
            }
            for (var i = 0; i < file.Value.Length; i++)
            {
                if (print)
                {
                    Console.Write($"{file.Value.Id}");
                }
                
                checksum += index * file.Value.Id;
                index++;
            }
        }

        if (print)
        {
            Console.WriteLine();
        }

        return checksum;
    }
}
