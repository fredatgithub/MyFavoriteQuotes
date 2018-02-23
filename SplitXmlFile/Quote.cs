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
  }
}