using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;


using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

namespace oxoid_web_scrapring
{
    internal class Program
    {
        static void Main(string[] args)
        {



            // Goal of this project: 
                // 1. Use UglyToad.PigPdf to extract CAT no's and store in db/csv
                // 2. Connect with sfda website to extract license status.
                // 3. Update status in db/csv
            Console.WriteLine("hello this is my first line of code in a million years.");

            string csvPath = @"C:\Users\nabeelk\Home\Al-Majharia\oxoid-project\scraper\output.csv";
            var sb = new StringBuilder();
            // Header
            sb.AppendLine("page_no,line");


            using (PdfDocument document = PdfDocument.Open(@"C:\Users\nabeelk\Home\Al-Majharia\oxoid-project\scraper\oxoid-web-scrapring\2025-EU-Microbiology-Catalog_EN.pdf"))
            {
                foreach (Page page in document.GetPages())
                {
                    string text = ContentOrderTextExtractor.GetText(page);
                    IEnumerable<Word> words = page.GetWords(NearestNeighbourWordExtractor.Instance);

                    //Console.WriteLine($"####Page Number{page.Number}###");
                    //Console.WriteLine($"{text}");

                    // Split text by \n into lines
                    string[] lines = text
                        .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(l => l.Trim())
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .ToArray();

                    Console.WriteLine($"#### Page {page.Number} ####");

                    // Print the lines
                    Console.WriteLine("[");

                    foreach (var line in lines)
                    {
                        // Escape quotes for CSV safety
                        string safeLine = line.Replace("\"", "\"\"");
                        Console.WriteLine($"{page.Number},\"{safeLine}\"");


                        
                        sb.AppendLine($"{page.Number},\"{safeLine}\"");
                    }

                    Console.WriteLine("]");
                    Console.WriteLine(); // blank line between pages

                }


            }

            // Write CSV to file
            File.WriteAllText(csvPath, sb.ToString(), Encoding.UTF8);

            Console.WriteLine("CSV file generated successfully!");





        }
    }
}
