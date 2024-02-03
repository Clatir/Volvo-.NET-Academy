﻿using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var (baseDirectory, projectDirectoryInfo) = TextProcessing.InitializeEnvironment();
        if (projectDirectoryInfo == null) return;

        string folderPath = System.IO.Path.Combine(projectDirectoryInfo.FullName, "100-books");
        string resultFolderPath = System.IO.Path.Combine(projectDirectoryInfo.FullName, "100-Books-Results");
        await TextProcessing.ProcessBooks(folderPath, resultFolderPath);
        System.Console.WriteLine("All books have been processed.");
    }
}
