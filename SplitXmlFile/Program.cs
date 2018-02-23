using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

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

      try
      {
        using (TextReader reader = new StreamReader(fileName))
        {
          XmlSerializer serializer = new XmlSerializer(typeof(Quotes));
          var list = (Quotes)serializer.Deserialize(reader);
        }
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception);
      }


      display("Press any key to exit:");
      Console.ReadKey();
    }
  }
}