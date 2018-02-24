using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using MyFavoriteQuotes.Properties;
using Tools;

namespace SplitXmlFile
{
  internal static class Program
  {
    private static void Main()
    {
      Action<string> display = Console.WriteLine;
      display("séparation d'un gros fichier xml en plusieurs petits");
      //string fileName = "Quote_files\\quote1.xml";
      string fileName = "quote1.xml";
      int startNumber = 3;
      int numberOfQuotePerFile = 250;
      StringBuilder xmlFileHeader = new StringBuilder();
      xmlFileHeader.Append(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
      xmlFileHeader.Append("<Quotes>");
      //xmlFileHeader.Append("<Quote>");
      
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

      foreach (var q in result)
      {
        if (!_allQuotes.ListOfQuotes.Contains(new Quote(q.authorValue, q.languageValue, q.sentenceValue)) &&
            q.authorValue != string.Empty && q.languageValue != string.Empty && q.sentenceValue != string.Empty)
        {
          _allQuotes.Add(new Quote(q.authorValue, q.languageValue, q.sentenceValue));
        }
      }

      int counter = 0;
      for (int i = 0; i < _allQuotes.ListOfQuotes.Count; i++)
      {
        
      }

      display("Press any key to exit:");
      Console.ReadKey();
    }
  }
}