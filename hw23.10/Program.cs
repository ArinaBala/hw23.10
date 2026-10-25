using System;
using System.Text;
using hw23._10;

namespace DocumentProcessingSystem
{
    class Program
    {
        
        static Document[] documents = new Document[100];
        static int docCount = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            bool exit = false;

            while (!exit)
            {
               
                Console.WriteLine("1. Додати статтю");
                Console.WriteLine("2. Додати звіт");
                Console.WriteLine("3. Показати всі документи");
                Console.WriteLine("4. Знайти документи за словом");
                Console.WriteLine("5. Показати статистику документа");
                Console.WriteLine("6. Згенерувати загальний звіт");
                Console.WriteLine("7. Демонстрація роботи з record (with)");
                Console.WriteLine("8. Вийти");
                Console.Write("\nОберіть дію: ");

                string input = Console.ReadLine() ?? "";
                Console.WriteLine();

                if (input == "1")
                {
                    AddArticle();
                }
                else if (input == "2")
                {
                    AddReport();
                }
                else if (input == "3")
                {
                    ShowAllDocuments();
                }
                else if (input == "4")
                {
                    SearchByWord();
                }
                else if (input == "5")
                {
                    ShowDocumentStatistics();
                }
                else if (input == "6")
                {
                    GenerateFullReport();
                }
                else if (input == "7")
                {
                    DemonstrateRecordWith();
                }
                else if (input == "8")
                {
                    exit = true;
                    Console.WriteLine("Завершення роботи програми.");
                }
                else
                {
                    Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                }
            }
        }

        static void AddArticle()
        {
            if (docCount >= documents.Length)
            {
                Console.WriteLine("Масив заповнений! Неможливо додати більше документів.");
                return;
            }

            Console.Write("Введіть назву статті: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Введіть автора: ");
            string author = Console.ReadLine() ?? "";

            Console.Write("Введіть категорію: ");
            string category = Console.ReadLine() ?? "";

            Console.Write("Введіть час читання (у хвилинах): ");
            int.TryParse(Console.ReadLine(), out int readTime);

            Console.WriteLine("Введіть текст:");
            string text = Console.ReadLine() ?? "";

            documents[docCount] = new Article(title, author, text, category, readTime);
            docCount++;

            Console.WriteLine("\nДокумент успішно створено!");
        }

        static void AddReport()
        {
            if (docCount >= documents.Length)
            {
                Console.WriteLine("Масив заповнений!");
                return;
            }

            Console.Write("Введіть назву звіту: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Введіть автора: ");
            string author = Console.ReadLine() ?? "";

            Console.Write("Введіть відділ: ");
            string department = Console.ReadLine() ?? "";

            Console.Write("Введіть кількість сторінок: ");
            int.TryParse(Console.ReadLine(), out int pageCount);

            Console.WriteLine("Введіть текст:");
            string text = Console.ReadLine() ?? "";

            documents[docCount] = new Report(title, author, text, department, pageCount);
            docCount++;

            Console.WriteLine("\nДокумент успішно створено!");
        }

        static void ShowAllDocuments()
        {
            if (docCount == 0)
            {
                Console.WriteLine("Список документів порожній.");
                return;
            }

            Console.WriteLine("--- СПИСОК УСІХ ДОКУМЕНТІВ ---");
            for (int i = 0; i < docCount; i++)
            {
                Console.WriteLine("[" + (i + 1) + "]");
                Console.WriteLine(documents[i].GetInfo());
                Console.WriteLine("-----------------------------");
            }
        }

        static void SearchByWord()
        {
            Console.Write("Введіть слово для пошуку: ");
            string word = Console.ReadLine() ?? "";

            int foundCount = 0;

            for (int i = 0; i < docCount; i++)
            {
                if (documents[i].ContainsWord(word))
                {
                    Console.WriteLine("\nЗнайдено документ:");
                    Console.WriteLine(documents[i].Title);
                    Console.WriteLine("Автор: " + documents[i].Author);
                    Console.WriteLine("Кількість слів: " + documents[i].CountWords());
                    Console.WriteLine("-----------------------------");
                    foundCount++;
                }
            }

            if (foundCount == 0)
            {
                Console.WriteLine("\nДокументів із таким словом не знайдено.");
            }
        }

        static void ShowDocumentStatistics()
        {
            if (docCount == 0)
            {
                Console.WriteLine("Список документів порожній.");
                return;
            }

            ShowAllDocuments();
            Console.Write("Оберіть номер документа для перегляду статистики: ");

            int.TryParse(Console.ReadLine(), out int index);

            if (index > 0 && index <= docCount)
            {
                Document doc = documents[index - 1];
                DocumentStatistics stats = doc.GetStatistics();

                Console.WriteLine("\n--- СТАТИСТИКА ДЛЯ '" + doc.Title + "' ---");
                Console.WriteLine("Кількість слів: " + stats.WordCount);
                Console.WriteLine("Кількість унікальних слів: " + stats.UniqueWordCount);
                Console.WriteLine("Найдовше слово: " + (stats.LongestWord == "" ? "-" : stats.LongestWord));
                Console.WriteLine("Кількість символів: " + stats.CharacterCount);
            }
            else
            {
                Console.WriteLine("Некоректний номер документа.");
            }
        }

        static void GenerateFullReport()
        {
            if (docCount == 0)
            {
                Console.WriteLine("Немає документів для формування звіту.");
                return;
            }

            string reportText = ReportGenerator.GenerateFullReport(documents, docCount);
            Console.WriteLine(reportText);
        }

        static void DemonstrateRecordWith()
        {
            if (docCount == 0)
            {
                Console.WriteLine("Спочатку додайте хоча б один документ.");
                return;
            }

            DocumentSummary summary = documents[0].GetSummary();
            Console.WriteLine("Початковий DocumentSummary:");
            Console.WriteLine(summary);

            DocumentSummary updatedSummary = summary with { Author = "Оновлений Автор", Title = summary.Title + " (Копія)" };

            Console.WriteLine("\nНовий DocumentSummary (створений за допомогою 'with'):");
            Console.WriteLine(updatedSummary);

            Console.WriteLine("\nПеревірка оригінального запису (залишився без змін):");
            Console.WriteLine(summary);
        }
    }
}