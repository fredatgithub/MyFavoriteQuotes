namespace MyFavoriteQuotes
{
  class Quote
  {
    public string Author { get; set; }
    public string Language { get; set; }
    public string Sentence { get; set; }

    public Quote(string author = "unknown author", string language = "English", string sentence = "")
    {
      Author = author;
      Language = language;
      Sentence = sentence;
    }
  }
}