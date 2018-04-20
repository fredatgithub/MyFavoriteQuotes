using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SplitXmlFile
{
  internal static class Program
  {
    private static void Main()
    {
      Action<string> display = Console.WriteLine;
      display("séparation d'un gros fichier xml en plusieurs petits");
      //string fileName = "Quote_files\\quote1.xml";
      const string fileName = "quote1.xml";
      const int startNumber = 3;
      int numberOfQuotePerFile = 250;
      StringBuilder xmlFileHeaderStringBuilder = new StringBuilder();
      xmlFileHeaderStringBuilder.Append(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
      xmlFileHeaderStringBuilder.Append(Environment.NewLine);
      xmlFileHeaderStringBuilder.Append("<Quotes>");
      xmlFileHeaderStringBuilder.Append(Environment.NewLine);
      string xmlFileHeader = xmlFileHeaderStringBuilder.ToString();
      string xmlFileFooter =$"</Quote>{Environment.NewLine}";

      XDocument xmlDoc;
      try
      {
        xmlDoc = XDocument.Load(fileName);
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception.Message);
        throw ;
      }

      var result = from node in xmlDoc.Descendants("Quote")
        where node.HasElements
        let xElementAuthor = node.Element("Author")
        where xElementAuthor != null
        let xElementLanguage = node.Element("Language")
        where xElementLanguage != null
        let xElementQuote = node.Element("QuoteValue")
        where xElementQuote != null
        select new
        {
          authorValue = xElementAuthor.Value,
          languageValue = xElementLanguage.Value,
          sentenceValue = xElementQuote.Value
        };

      Quotes _allQuotes = new Quotes();

      int duplicateQuote = 0;
      foreach (var q in result)
      {
        if (!_allQuotes.ListOfQuotes.Contains(new Quote(q.authorValue, q.languageValue, q.sentenceValue)) &&
            q.authorValue != string.Empty && q.languageValue != string.Empty && q.sentenceValue != string.Empty)
        {
          _allQuotes.Add(new Quote(q.authorValue, q.languageValue, q.sentenceValue));
        }
        else
        {
          duplicateQuote++;
        }
      }

      display($"there is {duplicateQuote} duplicate quote{Plural(duplicateQuote)} found");

      int quoteCounter = 0;
      int FileNumberCounter = startNumber;
      string tempQuotefile = xmlFileHeader;
      foreach (Quote oneQuote in _allQuotes.ToList())
      {
        if (quoteCounter < numberOfQuotePerFile)
        {
          tempQuotefile += oneQuote.ToString();
          quoteCounter++;
        }
        else
        {
          //write tempquote file and empty it
          using (StreamWriter sw = new StreamWriter(ReplaceNumber(fileName, FileNumberCounter)))
          {
            sw.WriteLine($"{tempQuotefile}{xmlFileFooter}");
          }

          FileNumberCounter++;
          tempQuotefile = xmlFileHeader;
          quoteCounter = 0;
        }
      }
      

      display("Press any key to exit:");
      Console.ReadKey();
    }

    private static string ReplaceNumber(string fileName, int fileNumberCounter)
    {
      return fileName.Replace("1", fileNumberCounter.ToString());
    }

    private static string Plural(int duplicateQuote)
    {
      return duplicateQuote > 1 ? "s" : string.Empty;
    }
  }
}