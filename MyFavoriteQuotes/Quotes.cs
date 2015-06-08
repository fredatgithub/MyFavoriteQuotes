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

    public void Remove(Quote quote)
    {
      ListOfQuotes.Remove(quote);
      QuoteFileSaved = false;
    }

    public bool Remove(string sentence)
    {
      bool result = false;
      foreach (Quote quote in ListOfQuotes)
      {
        if (quote.Sentence == sentence)
        {
          ListOfQuotes.Remove(quote);
          QuoteFileSaved = false;
          result = true;
          break;
        }
      }

      return result;
    }

    public bool Remove(string sentence, string author)
    {
      bool result = false;
      foreach (Quote quote in ListOfQuotes)
      {
        if (quote.Sentence == sentence && quote.Author == author)
        {
          ListOfQuotes.Remove(quote);
          QuoteFileSaved = false;
          result = true;
          break;
        }
      }

      return result;
    }

    public IEnumerable<Quote> ToList()
    {
      return ListOfQuotes;
    }
  }
}