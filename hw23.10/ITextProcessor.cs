
using System;
using System.Text;

namespace hw23._10
{
    public interface ITextProcessor
    {
        string ProcessText();
        int CountWords();
        bool ContainsWord(string word);
    }

    public record DocumentStatistics(
        int WordCount,
        int CharacterCount,
        string LongestWord,
        int UniqueWordCount
    );

    public record DocumentSummary(
        string Title,
        string Author,
        string DocumentType,
        int WordCount
    );

    public abstract class Document : ITextProcessor
    {
        private string _title;
        private string _author;
        private string _text;
        private DateTime _createdAt;

        public string Title 
        {
            get => _title; 
            set => _title = value; 
        }
        public string Author 
        {
            get => _author;
            set => _author = value; 
        }
        public string Text 
        {
            get => _text;
            set => _text = value; 
        }
        public DateTime CreatedAt 
        {
            get => _createdAt;
            protected set => _createdAt = value; 
        }

        public Document(string title, string author, string text)
        {
            _title = title;
            _author = author;
            _text = text;
            _createdAt = DateTime.Now;
        }

        public virtual string GetInfo()
        {
            return "Назва: " + Title + "\nАвтор: " + Author + "\nДата створення: " + CreatedAt.ToString("dd.MM.yyyy HH:mm");
        }

        public virtual string ProcessText()
        {
            return Text.Trim();
        }

      
        protected string[] GetCleanWords()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return new string[0];
            }

            char[] delimiters = new char[] { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
            return Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        public int CountWords()
        {
            string[] words = GetCleanWords();
            return words.Length;
        }

        public bool ContainsWord(string word)
        {
            string[] words = GetCleanWords();
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Equals(word, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public DocumentStatistics GetStatistics()
        {
            string[] words = GetCleanWords();
            int wordCount = words.Length;
            int characterCount = Text.Length;

         
            string longestWord = "";
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > longestWord.Length)
                {
                    longestWord = words[i];
                }
            }

           
            int uniqueWordCount = 0;
            string[] seenWords = new string[words.Length];

            for (int i = 0; i < words.Length; i++)
            {
                string current = words[i].ToLower();
                bool alreadyExists = false;

                for (int j = 0; j < uniqueWordCount; j++)
                {
                    if (seenWords[j] == current)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (!alreadyExists)
                {
                    seenWords[uniqueWordCount] = current;
                    uniqueWordCount++;
                }
            }

            return new DocumentStatistics(wordCount, characterCount, longestWord, uniqueWordCount);
        }

        public DocumentSummary GetSummary()
        {
            return new DocumentSummary(Title, Author, GetType().Name, CountWords());
        }
    }

    public class Article : Document
    {
        public string Category { get; set; }
        public int ReadTime { get; set; }

        public Article(string title, string author, string text, string category, int readTime): base(title, author, text)
        {
            Category = category;
            ReadTime = readTime;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + "\nТип: Стаття\nКатегорія: " + Category + "\nЧас читання: " + ReadTime + " хв.";
        }
    }

    public class Report : Document
    {
        public string Department 
        {
            get;
            set; 
        }
        public int PageCount 
        {
            get;
            set; 
        }

        public Report(string title, string author, string text, string department, int pageCount): base(title, author, text)
        {
            Department = department;
            PageCount = pageCount;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + "\nТип: Звіт\nВідділ: " + Department + "\nКількість сторінок: " + PageCount;
        }
    }

    public class ReportGenerator
    {
        public static string GenerateFullReport(Document[] docs, int count)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("========================================");
            sb.AppendLine("         ЗАГАЛЬНИЙ ЗВІТ ДОКУМЕНТІВ      ");
            sb.AppendLine("========================================");
            sb.AppendLine("Дата формування: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            sb.AppendLine("Всього документів у системі: " + count);
            sb.AppendLine("----------------------------------------");

            int totalWords = 0;

            for (int i = 0; i < count; i++)
            {
                Document doc = docs[i];
                DocumentStatistics stats = doc.GetStatistics();
                totalWords = totalWords + stats.WordCount;

                sb.AppendLine("Документ #" + (i + 1));
                sb.AppendLine(doc.GetInfo());
                sb.AppendLine("Статистика тексту:");
                sb.AppendLine(" - Кількість слів: " + stats.WordCount);
                sb.AppendLine(" - Унікальних слів: " + stats.UniqueWordCount);
                sb.AppendLine(" - Найдовше слово: " + (stats.LongestWord == "" ? "-" : stats.LongestWord));
                sb.AppendLine(" - Кількість символів: " + stats.CharacterCount);
                sb.AppendLine("----------------------------------------");
            }

            sb.AppendLine("========================================");
            sb.AppendLine("                ПІДСУМОК               ");
            sb.AppendLine("========================================");
            sb.AppendLine("Загальна кількість слів у всіх документах: " + totalWords);
            if (count > 0)
            {
                sb.AppendLine("Середня кількість слів у документі: " + ((double)totalWords / count));
            }
            sb.AppendLine("========================================");

            return sb.ToString();
        }
    }
}
