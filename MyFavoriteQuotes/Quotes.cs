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
  }
}