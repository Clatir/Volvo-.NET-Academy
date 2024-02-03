using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal static class TextProcessing
{








    private static void UpdateTopResults<T>(ConcurrentDictionary<T, int> source, Action<Dictionary<T, int>> updateAction)
    {
        var sorted = source.OrderByDescending(pair => pair.Value)
                           .Take(10) 
                           .ToDictionary(pair => pair.Key, pair => pair.Value);
        updateAction(sorted);
    }




    class Book
    {
        private string content { get; set; }

        private String path { get; set; }

        private string title { get; set; }

        private List<string> Sentences { get; set; }
        private List<string> Words { get; set; }
        private List<string> Punctuation { get; set; }
        public long BytesProcessed { get; private set; }
        public int SentencesProcessed { get; private set; }
        public int WordsProcessed { get; private set; }

        public Dictionary<string, int> LongestSentences { get; set; }
        public Dictionary<string, int> ShortestSentences { get; set; }
        public Dictionary<string, int> LongestWords { get; set; }
        public Dictionary<string, int> MostCommonLetters { get; set; }
        public Dictionary<string, int> MostCommonWords { get; set; }


        public Book(string path)
        {
            content = "";
            title = "";
            BytesProcessed = 0;
            SentencesProcessed = 0;
            WordsProcessed = 0;
            Sentences = new List<string>();
            Words = new List<string>();
            Punctuation = new List<string>();
            LongestSentences = new Dictionary<string, int>();
            ShortestSentences = new Dictionary<string, int>();
            LongestWords = new Dictionary<string, int>();
            MostCommonLetters = new Dictionary<string, int>();
            MostCommonWords = new Dictionary<string, int>();
            this.path = path;

        }

        private void UpdateProcessingStats()
        {
            BytesProcessed = new FileInfo(path).Length;
            SentencesProcessed = Sentences.Count;
            WordsProcessed = Words.Count;
        }

        private void ParseWords()
        {
            Words.AddRange(Regex.Split(content, @"\W+").Where(word => !string.IsNullOrEmpty(word)));
        }

        private void ParsePunctuation()
        {
            Punctuation.AddRange(Regex.Matches(content, @"[^\w\s]|_")
                                 .Cast<Match>()
                                 .Select(match => match.Value)
                                 .Where(punctuation => !string.IsNullOrEmpty(punctuation)));

        }

        private void AddAndSortLongestSentences()
        {
            foreach (var sentence in Sentences)
            {
                int length = sentence.Length;
                if (!LongestSentences.ContainsKey(sentence))
                {
                    LongestSentences.Add(sentence, length);
                }
            }

            LongestSentences = LongestSentences.OrderByDescending(pair => pair.Value)
                                               .ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        private void AddAndSortShortestSentences()
        {
            Regex sentenceRegex = new Regex(@"^[A-Z].*[\.!?]$");
            foreach (var sentence in Sentences)
            {
                if (sentenceRegex.IsMatch(sentence))
                {
                    if (!Regex.IsMatch(sentence, @"^\p{Lu}\p{Ll}*\s\p{Lu}\.$") &&
                        !Regex.IsMatch(sentence, @"^[A-Z]\.[A-Z]?\.?$") &&
                        sentence.Split(new char[] { ' ', '.', '?', '!', ',' }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    {
                        int wordCount = sentence.Split(new char[] { ' ', '.', '?', '!', ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                        if (!ShortestSentences.Values.Contains(wordCount)) 
                        {
                            ShortestSentences.Add(sentence, wordCount);
                        }
                    }
                }
            }

            ShortestSentences = ShortestSentences.OrderBy(pair => pair.Value)
                                                 .ToDictionary(pair => pair.Key, pair => pair.Value);
        }




        private void AddAndSortLongestWords()
        {

            foreach (var word in Words)
            {
                int length = word.Length;
                if (!LongestWords.ContainsKey(word))
                {
                    LongestWords.Add(word, length);
                }
            }


            LongestWords = LongestWords.OrderByDescending(pair => pair.Value)
                                       .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private void ParseLettersAndSort()
        {

            foreach (char c in content.ToLower()) 
            {
                if (char.IsLetter(c)) 
                {
                    if (MostCommonLetters.ContainsKey(c.ToString()))
                    {
                        MostCommonLetters[c.ToString()]++;
                    }
                    else
                    {
                        MostCommonLetters.Add(c.ToString(), 1);
                    }
                }
            }


            MostCommonLetters = MostCommonLetters.OrderByDescending(pair => pair.Value)
                                                 .ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        private void AddAndSortMostCommonWords()
        {
            foreach (var word in Words)
            {
                string lowerCaseWord = word.ToLower(); 
                if (MostCommonWords.ContainsKey(lowerCaseWord))
                {
                    MostCommonWords[lowerCaseWord]++;
                }
                else
                {
                    MostCommonWords.Add(lowerCaseWord, 1);
                }
            }


            MostCommonWords = MostCommonWords.OrderByDescending(pair => pair.Value)
                                             .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public async Task ParseAsync()
        {
            content = await File.ReadAllTextAsync(path);

  
            ExtractTitle();


            Sentences = Regex.Split(content, @"(?<=[\.!\?])\s+(?=[A-Z])").ToList();


            ParseWords();
            ParsePunctuation();
            ParseLettersAndSort();
            AddAndSortLongestSentences();
            AddAndSortShortestSentences();
            AddAndSortLongestWords();
            AddAndSortMostCommonWords();
            UpdateProcessingStats();
        }
        public string ExtractTitle()
        {
            var titleLine = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                                    .FirstOrDefault(line => line.Contains("The Project Gutenberg eBook of"));

            if (titleLine != null)
            {
                var titleStartIndex = titleLine.IndexOf("The Project Gutenberg eBook of") + "The Project Gutenberg eBook of".Length;
                title = titleLine.Substring(titleStartIndex).Trim(); 
                return title;
            }

            title = "Unknown Title"; 
            return title;
        }




        public void SaveStatisticsToFile(string resultFolderPath)
        {

            string safeTitle = title.Replace(':', '-').Replace('?', '-').Replace('/', '-').Replace('\\', '-')
                                    .Replace('|', '-').Replace('*', '-').Replace('"', '-').Replace('<', '-')
                                    .Replace('>', '-').Replace('\n', '-').Replace('\r', '-').Trim();
            string fileName = $"{safeTitle}.txt";
            string filePath = Path.Combine(resultFolderPath, fileName);

            if (!Directory.Exists(resultFolderPath))
            {
                Directory.CreateDirectory(resultFolderPath);
            }


            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Title: {title}\n");

                writer.WriteLine("\nTop 10 Shortest Sentences:");
                foreach (var item in ShortestSentences.Take(10))
                {
                    writer.WriteLine($"- {item.Key} (Words: {item.Value})");
                }

                writer.WriteLine("\nTop 10 Longest Words:");
                foreach (var item in LongestWords.Take(10))
                {
                    writer.WriteLine($"- {item.Key} (Length: {item.Value} characters)");
                }

                writer.WriteLine("\nTop 10 Most Common Letters:");
                foreach (var item in MostCommonLetters.Take(10))
                {
                    writer.WriteLine($"- {item.Key} (Occurrences: {item.Value})");
                }

                writer.WriteLine("\nTop 10 Most Common Words:");
                foreach (var item in MostCommonWords.Take(10))
                {
                    writer.WriteLine($"- {item.Key} (Occurrences: {item.Value})");
                }

                writer.WriteLine("Top 10 Longest Sentences:");
                foreach (var item in LongestSentences.Take(10))
                {
                    writer.WriteLine($"- {item.Key} (Length: {item.Value} characters)");
                }
            }
        }


    }





    public static (string baseDirectory, DirectoryInfo projectDirectoryInfo) InitializeEnvironment()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectDirectoryInfo = Directory.GetParent(baseDirectory);
        for (int i = 0; i < 3; i++) 
        {
            projectDirectoryInfo = projectDirectoryInfo?.Parent;
        }

        if (projectDirectoryInfo == null)
        {
            Console.WriteLine("The project folder cannot be found.");
        }
        return (baseDirectory, projectDirectoryInfo);
    }
    public static async Task ProcessBooks(string folderPath, string resultFolderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Folder with books does not exist");
            return;
        }
        if (!Directory.Exists(resultFolderPath))
        {
            Directory.CreateDirectory(resultFolderPath);
        }
        string[] filePaths = Directory.GetFiles(folderPath, "*.txt");
        var processingTasks = filePaths.Select(filePath => ProcessBook(filePath, resultFolderPath)).ToArray();
        await Task.WhenAll(processingTasks);
    }

    private static async Task ProcessBook(string filePath, string resultFolderPath)
    {
        Console.WriteLine($"Starting processing: {filePath}");
        Book book = new Book(filePath);
        await book.ParseAsync();
        
        book.SaveStatisticsToFile(resultFolderPath);
        Console.WriteLine($"Completed processing of file:  {filePath}");
    }
}
