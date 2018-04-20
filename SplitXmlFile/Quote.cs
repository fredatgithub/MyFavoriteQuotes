using System;
using System.Xml.Serialization;

namespace SplitXmlFile
{
  [Serializable()]
  public class Quote
  {
    [XmlElement()]
    public string Author { get; set; }
    [XmlElement()]
    public string Language { get; set; }
    [XmlElement()]
    public string Sentence { get; set; }

    public Quote(string author = "unknown author", string language = "English", string sentence = "")
    {
      Author = author;
      Language = language;
      Sentence = sentence;
    }

    public override string ToString()
    {
      /*
        <Quote>
         <Author>Anonyme</Author>
         <Language>French</Language>
         <QuoteValue>Faute de vivre, on finit par mourir</QuoteValue>
        </Quote> 
       * */

      return $"<Quote>{Environment.NewLine}<Author>{Author}</Author>{Environment.NewLine}<Language>{Language}</Language>{Environment.NewLine}<QuoteValue>{Sentence}</QuoteValue>{Environment.NewLine}</Quote>{Environment.NewLine}";
    }
  }
}