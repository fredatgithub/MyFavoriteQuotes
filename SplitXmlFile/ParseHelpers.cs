using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Script.Serialization; // Add reference: System.Web.Extensions

namespace SplitXmlFile
{
  public static class ParseHelpers
  {
    private static JavaScriptSerializer _json;
    private static JavaScriptSerializer Json => _json ?? (_json = new JavaScriptSerializer());

    public static Stream ToStream(this string @this)
    {
      var stream = new MemoryStream();
      var writer = new StreamWriter(stream);
      writer.Write(@this);
      writer.Flush();
      stream.Position = 0;
      return stream;
    }

    public static T ParseXml<T>(this string @this) where T : class
    {
      var reader = XmlReader.Create(@this.Trim().ToStream(), new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Document });
      return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
    }

    public static T ParseJson<T>(this string @this) where T : class
    {
      return Json.Deserialize<T>(@this.Trim());
    }
  }
}