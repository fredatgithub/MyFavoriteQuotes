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
      StringBuilder xmlFile = new StringBuilder();
      xmlFile.Append(@"<?xml version=""1.0"" encoding=""utf-8"" ?>");
      xmlFile.Append("<Quotes>");
      xmlFile.Append("<Quote>");

      //try
      //{
      //  using (TextReader reader = new StreamReader(fileName))
      //  {
      //    XmlSerializer serializer = new XmlSerializer(typeof(Quotes));
      //    var list = (Quotes)serializer.Deserialize(reader);
      //  }
      //}
      //catch (Exception exception)
      //{
      //  Console.WriteLine(exception);
      //}

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


      display("Press any key to exit:");
      Console.ReadKey();
    }
  }
}