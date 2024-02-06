using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

static class TextProcessing
{
   
    private static ConcurrentDictionary<string, int> GlobalLongestSentences = new ConcurrentDictionary<string, int>();
    private static ConcurrentDictionary<string, int> GlobalShortestSentences = new ConcurrentDictionary<string, int>();
    private static ConcurrentDictionary<string, int> GlobalLongestWords = new ConcurrentDictionary<string, int>();
    private static ConcurrentDictionary<string, int> GlobalMostCommonLetters = new ConcurrentDictionary<string, int>();
    private static ConcurrentDictionary<string, int> GlobalMostCommonWords = new ConcurrentDictionary<string, int>();



    private static void UpdateGlobalStats(Book book)
    {
       
        UpdateGlobalLongestSentences(new ConcurrentDictionary<string, int>(book.LongestSentences));
        UpdateGlobalShortestSentences(new ConcurrentDictionary<string, int>(book.ShortestSentences));
        UpdateGlobalLongestWords(new ConcurrentDictionary<string, int>(book.LongestWords));
        UpdateGlobalMostCommonLetters(new ConcurrentDictionary<string, int>(book.MostCommonLetters));
        UpdateGlobalMostCommonWords(new ConcurrentDictionary<string, int>(book.MostCommonWords));
    }


    private static void UpdateGlobalLongestSentences(ConcurrentDictionary<string, int> bookLongestSentences)
    {
        foreach (var sentence in bookLongestSentences)
        {
            GlobalLongestSentences.AddOrUpdate(sentence.Key, sentence.Value,
                (key, existingVal) => Math.Max(existingVal, sentence.Value));
        }
    }

    private static void UpdateGlobalShortestSentences(ConcurrentDictionary<string, int> bookShortestSentences)
    {
        foreach (var sentence in bookShortestSentences)
        {
            GlobalShortestSentences.AddOrUpdate(sentence.Key, sentence.Value,
                (key, existingVal) => Math.Min(existingVal, sentence.Value));
        }
    }

    private static void UpdateGlobalLongestWords(ConcurrentDictionary<string, int> bookLongestWords)
    {
        foreach (var word in bookLongestWords)
        {
            GlobalLongestWords.AddOrUpdate(word.Key, word.Value,
                (key, existingVal) => Math.Max(existingVal, word.Value));
        }
    }

    private static void UpdateGlobalMostCommonLetters(ConcurrentDictionary<string, int> bookMostCommonLetters)
    {
        foreach (var letter in bookMostCommonLetters)
        {
            GlobalMostCommonLetters.AddOrUpdate(letter.Key, letter.Value,
                (key, existingVal) => existingVal + letter.Value);
        }
    }

    private static void UpdateGlobalMostCommonWords(ConcurrentDictionary<string, int> bookMostCommonWords)
    {
        foreach (var word in bookMostCommonWords)
        {
            GlobalMostCommonWords.AddOrUpdate(word.Key, word.Value,
                (key, existingVal) => existingVal + word.Value);
        }
    }

    private static void UpdateTopResults<T>(ConcurrentDictionary<T, int> source, Action<Dictionary<T, int>> updateAction)
    {
        var sorted = source.OrderByDescending(pair => pair.Value)
                           .Take(10) 
                           .ToDictionary(pair => pair.Key, pair => pair.Value);
        updateAction(sorted);
    }

    private static void ProcessGlobalStats()
    {
       
        var sortedLongestWords = GlobalLongestWords.OrderByDescending(pair => pair.Value)
            .Take(10)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        GlobalLongestWords = new ConcurrentDictionary<string, int>(sortedLongestWords);

       
        var sortedMostCommonLetters = GlobalMostCommonLetters.OrderByDescending(pair => pair.Value)
            .Take(10)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        GlobalMostCommonLetters = new ConcurrentDictionary<string, int>(sortedMostCommonLetters);

       
        var sortedMostCommonWords = GlobalMostCommonWords.OrderByDescending(pair => pair.Value)
            .Take(10)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        GlobalMostCommonWords = new ConcurrentDictionary<string, int>(sortedMostCommonWords);

        
        var sortedShortestSentences = GlobalShortestSentences.OrderBy(pair => pair.Value)
            .Take(10)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        GlobalShortestSentences = new ConcurrentDictionary<string, int>(sortedShortestSentences);

       
        var sortedLongestSentences = GlobalLongestSentences.OrderByDescending(pair => pair.Value)
            .Take(10)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        GlobalLongestSentences = new ConcurrentDictionary<string, int>(sortedLongestSentences);
    }


    class Book
    {
        private StringBuilder content { get; set; }


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
            content = new StringBuilder();
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

        public string GetProcessingStats()
        {
            return $"Processed: {BytesProcessed} bytes, {WordsProcessed} words, {SentencesProcessed} sentences.";
        }

        private void ParseWords()
        {
            Words.AddRange(Regex.Split(content.ToString(), @"\W+").Where(word => !string.IsNullOrEmpty(word)));
        }

        private void ParsePunctuation()
        {
            Punctuation.AddRange(Regex.Matches(content.ToString(), @"[^\w\s]|_")
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
           
            foreach (char c in content.ToString().ToLower()) 
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
            var fileContent = await File.ReadAllTextAsync(path);
            content.Clear();
            content.Append(fileContent);



            ExtractTitle(); 

            
            Sentences = Regex.Split(content.ToString(), @"(?<=[\.!\?])\s+(?=[A-Z])").ToList();

           
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
            var titleLine = content.ToString().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
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

    public static async Task SaveGlobalStatisticsToFile()
    {
        
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;               
        var projectDirectoryInfo = Directory.GetParent(baseDirectory); 

        for (int i = 0; i < 3; i++) 
        {
            if (projectDirectoryInfo != null) projectDirectoryInfo = projectDirectoryInfo.Parent;
            else break; 
        }

        if (projectDirectoryInfo == null)
        {
            Console.WriteLine("The project folder cannot be found.");
            return;
        }

        string filePath = Path.Combine(projectDirectoryInfo.FullName, "GlobalStatistics.txt");        
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            await writer.WriteLineAsync("\nGlobal Top 10 Longest Words:");
            foreach (var item in GlobalLongestWords)
            {
                await writer.WriteLineAsync($"- {item.Key} (Length: {item.Value} characters)");
            }

            await writer.WriteLineAsync("\nGlobal Top 10 Most Common Letters:");
            foreach (var item in GlobalMostCommonLetters)
            {
                await writer.WriteLineAsync($"- {item.Key} (Occurrences: {item.Value})");
            }

            await writer.WriteLineAsync("\nGlobal Top 10 Most Common Words:");
            foreach (var item in GlobalMostCommonWords)
            {
                await writer.WriteLineAsync($"- {item.Key} (Occurrences: {item.Value})");
            }

            await writer.WriteLineAsync("\nGlobal Top 10 Shortest Sentences:");
            foreach (var item in GlobalShortestSentences)
            {
                await writer.WriteLineAsync($"- {item.Key} (Words: {item.Value})");
            }

            await writer.WriteLineAsync("Global Top 10 Longest Sentences:");
            foreach (var item in GlobalLongestSentences)
            {
                await writer.WriteLineAsync($"- {item.Key} (Length: {item.Value} characters)");
            }
        }

        Console.WriteLine($"Global statistics saved to {filePath}");
    }




    public static void AggregateGlobalResults()
    {
        
        Func<ConcurrentDictionary<string, int>, IEnumerable<KeyValuePair<string, int>>> sortAndTakeTop10 =
            (source) => source.OrderByDescending(pair => pair.Value).Take(10);

       
        GlobalLongestSentences = new ConcurrentDictionary<string, int>(sortAndTakeTop10(GlobalLongestSentences).ToDictionary(kv => kv.Key, kv => kv.Value));
        GlobalShortestSentences = new ConcurrentDictionary<string, int>(sortAndTakeTop10(GlobalShortestSentences).ToDictionary(kv => kv.Key, kv => kv.Value));
        GlobalLongestWords = new ConcurrentDictionary<string, int>(sortAndTakeTop10(GlobalLongestWords).ToDictionary(kv => kv.Key, kv => kv.Value));
        GlobalMostCommonLetters = new ConcurrentDictionary<string, int>(sortAndTakeTop10(GlobalMostCommonLetters).ToDictionary(kv => kv.Key, kv => kv.Value));
        GlobalMostCommonWords = new ConcurrentDictionary<string, int>(sortAndTakeTop10(GlobalMostCommonWords).ToDictionary(kv => kv.Key, kv => kv.Value));
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
        Console.WriteLine();
        Console.WriteLine($"Starting processing: {filePath}");
        Console.WriteLine();
        Book book = new Book(filePath);
        await book.ParseAsync();

        string stats = book.GetProcessingStats(); 
        string output = $"Completed processing of file: {filePath}\n{stats}";
        
        Console.WriteLine(output);
        Console.WriteLine();

        UpdateGlobalStats(book);
        book.SaveStatisticsToFile(resultFolderPath);
    }





}
