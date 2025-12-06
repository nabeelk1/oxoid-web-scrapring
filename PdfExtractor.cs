using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

namespace oxoid_web_scrapring
{
    public class PdfExtractor
    {
        public void ExtractToCsv(string pdfPath, string csvPath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("page_no,line,SKU");

            using (PdfDocument document = PdfDocument.Open(pdfPath))
            {
                foreach (Page page in document.GetPages())
                {
                    string text = ContentOrderTextExtractor.GetText(page);

                    string[] lines = text
                        .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(l => l.Trim())
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .ToArray();

                    foreach (var line in lines)
                    {
                        string safeLine = line.Replace("\"", "\"\"");
                        string lastWord = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? "";

                        sb.AppendLine($"{page.Number},\"{safeLine}\",\"{lastWord}\"");
                    }
                }
            }

            File.WriteAllText(csvPath, sb.ToString(), Encoding.UTF8);
            Console.WriteLine("CSV file created successfully!");
        }
    }
}
