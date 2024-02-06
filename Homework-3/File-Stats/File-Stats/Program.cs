using System.Threading.Tasks;

class Program
{
    private static async Task Main(string[] args)
    {
        var (baseDirectory, projectDirectoryInfo) = TextProcessing.InitializeEnvironment();
        if (projectDirectoryInfo == null) return;

        string folderPath = Path.Combine(projectDirectoryInfo.FullName, "100-books");
        string resultFolderPath = Path.Combine(projectDirectoryInfo.FullName, "100-Books-Results");
        await TextProcessing.ProcessBooks(folderPath, resultFolderPath);

        TextProcessing.AggregateGlobalResults();
        await TextProcessing.SaveGlobalStatisticsToFile();
        Console.WriteLine("All books have been processed.");
    }
}
