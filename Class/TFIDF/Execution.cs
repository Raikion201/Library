using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Class.TFIDF
{
    public class Execution
    {
        public List<string> ProcessDocuments(string directoryPath)
        {

            int MAX = 100;
            List<string> documents = new List<string>();
            for (int i = 0; i < MAX; i++)
            {
                int id = i + 1;
                string filePath = directoryPath + id + ".txt";
                string fileContent = File.ReadAllText(filePath);

                // SKip first two lines
                var lines = fileContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Skip(2);
                var document = String.Join(Environment.NewLine, lines);
                document = RemoveSpecialCharacters(document);
                document = RemoveStopWords(document);
                document = document.ToLower();
                documents.Add(document);
            }

            return documents;
        }
        static string RemoveSpecialCharacters(string text)
        {
            return text.Replace("\"", "")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Replace("!", "")
                    .Replace("?", "")
                    .Replace("“", "")
                    .Replace("”", "");
        }

        static string RemoveStopWords(string text)
        {
            string[] documentWords = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            documentWords = documentWords.Where(word => !StopWords.stopWordsList.Contains(word.ToLower())).ToArray();
            return string.Join(" ", documentWords);
        }
       
    }
 }