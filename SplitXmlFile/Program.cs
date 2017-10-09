using System;
using System.Collections.Generic;
using System.Linq;
using MFQ =  MyFavoriteQuotes.Properties;


namespace SplitXmlFile
{
  internal static class Program
  {
    private static void Main()
    {
      Action<string> display = Console.WriteLine;
      display("séparation d'un gros fichier xml en plusieurs petits");
      string fileName = "Quote_files\\quote1.xml";
      int startNumber = 3;
      int numberOfQuotePerFile = 250;
      string xmlHeader = @"<?xml version=""1.0"" encoding=""utf-8"" ?>";


      display("Press any key to exit:");
      Console.ReadKey();
    }
  }
}