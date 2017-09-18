using System;
using System.Collections.Generic;
using System.Linq;


namespace SplitXmlFile
{
  internal static class Program
  {
    private static void Main()
    {
      Action<string> display = Console.WriteLine;
      display("séparation d'un gros fichier xml en plusieurs petit");

      display("Press any key to exit:");
      Console.ReadKey();
    }
  }
}