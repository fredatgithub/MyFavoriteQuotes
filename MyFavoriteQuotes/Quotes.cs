using System.Collections.Generic;

namespace MyFavoriteQuotes
{
  class Quotes
  {
    public List<Quote> ListOfQuotes { get; set; }

    public Quotes()
    {
      ListOfQuotes = new List<Quote>();
    }

    public void Add(Quote quote)
    {
      ListOfQuotes.Add(quote);
    }

    public IEnumerable<Quote> ToList()
    {
      //IEnumerable<Quote> result = new List<Quote>();
      //foreach (var item in ListOfQuotes)
      //{
      //  result.Add(item);
      //}
      //return result;
      return (IEnumerable<Quote>)ListOfQuotes;
    }
  }
}