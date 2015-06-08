using System.Collections.Generic;

namespace MyFavoriteQuotes
{
  internal class Quotes
  {
    public List<Quote> ListOfQuotes { get; set; }
    public bool QuoteHasBeenAdded { get; set; }
    public bool QuoteFileSaved { get; set; }

    public Quotes()
    {
      ListOfQuotes = new List<Quote>();
      QuoteHasBeenAdded = false;
      QuoteFileSaved = true;
    }

    public void Add(Quote quote)
    {
      ListOfQuotes.Add(quote);
      QuoteHasBeenAdded = true;
      QuoteFileSaved = false;
    }

    public IEnumerable<Quote> ToList()
    {
      //IEnumerable<Quote> result = new List<Quote>();
      //foreach (var item in ListOfQuotes)
      //{
      //  result.Add(item);
      //}
      //return result;
      return ListOfQuotes;
    }
  }
}