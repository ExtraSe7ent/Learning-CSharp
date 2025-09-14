using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TextProcessor
{
    public string OriginalText { get; private set; }
    public string NormalizedText { get; private set; }
    public int TotalWords { get; private set; }
    public int DistinctWords { get; private set; }
    public Dictionary<string, int> WordFrequency { get; private set; }

    public TextProcessor(string inputText)
    {
        OriginalText = inputText;
        WordFrequency = new Dictionary<string, int>();
        NormalizeText();
        AnalyzeText();
    }

    private void NormalizeText()
    {
        string trimmedText = OriginalText.Trim();
        string[] words = trimmedText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string processedText = string.Join(" ", words);

        if (string.IsNullOrEmpty(processedText))
        {
            NormalizedText = "";
            return;
        }

        var stringBuilder = new StringBuilder(processedText.ToLower());
        bool capitalizeNext = true;

        for (int i = 0; i < stringBuilder.Length; i++)
        {
            char c = stringBuilder[i];
            if (capitalizeNext && char.IsLetter(c))
            {
                stringBuilder[i] = char.ToUpper(c);
                capitalizeNext = false;
            }
            else if (c == '.' || c == '!' || c == '?')
            {
                capitalizeNext = true;
            }
        }
        NormalizedText = stringBuilder.ToString();
    }

    private void AnalyzeText()
    {
        if (string.IsNullOrEmpty(NormalizedText)) return;

        char[] delimiters = new char[] { ' ', '.', ',', ';', ':', '?', '!', '\n', '\r', '\t' };
        string[] words = NormalizedText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        TotalWords = words.Length;

        foreach (string word in words)
        {
            string lowerCaseWord = word.ToLower();
            if (WordFrequency.ContainsKey(lowerCaseWord))
            {
                WordFrequency[lowerCaseWord]++;
            }
            else
            {
                WordFrequency.Add(lowerCaseWord, 1);
            }
        }
        DistinctWords = WordFrequency.Count;
    }

    public void DisplayFrequencyReport()
    {
        Console.WriteLine("\n - Bang thong ke tan suat tu:");
        if (WordFrequency.Count == 0)
        {
            Console.WriteLine("Khong co tu de thong ke.");
            return;
        }

        var sortedWords = WordFrequency.OrderByDescending(pair => pair.Value);

        Console.WriteLine("Tu                | Tan suat");
        Console.WriteLine("------------------|-----------");
        
        foreach (var pair in sortedWords)
        {
            Console.WriteLine($"{pair.Key,-17} | {pair.Value}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("\n - Nhap doan van ban (Enter 2 lan de ket thuc): ");

        StringBuilder userInputBuilder = new StringBuilder();
        string line;

        while (true)
        {
            line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                break;
            }
            userInputBuilder.AppendLine(line);
        }

        TextProcessor processor = new TextProcessor(userInputBuilder.ToString());

        Console.WriteLine("\n - Van ban da chuan hoa:");
        Console.WriteLine(processor.NormalizedText);

        Console.WriteLine("\n - Thong ke:");
        Console.WriteLine($"Tong so tu: {processor.TotalWords}");
        Console.WriteLine($"So luong tu khac nhau: {processor.DistinctWords}");

        processor.DisplayFrequencyReport();
    }
}
