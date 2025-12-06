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
            // 1. Use UglyToad.PigPdf to extract CAT no's and store in db/csv -> Done 
            // 2. Visit Oxoid Website to download exact description and price if stated. 
            // 3. Connect with sfda website to extract license status. 
            // 4. Update status in db/csv

            Console.WriteLine("Starting extraction...");

            string pdfPath = @"C:\Users\nabeelk\Home\Al-Majharia\oxoid-project\scraper\oxoid-web-scrapring\2025-EU-Microbiology-Catalog_EN.pdf";
            string csvPath = @"C:\Users\nabeelk\Home\Al-Majharia\oxoid-project\scraper\output.csv";

            var extractor = new PdfExtractor();
            extractor.ExtractToCsv(pdfPath, csvPath);

            Console.WriteLine("Done!");
        }
    }
}
