using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFavoriteQuotes;
namespace UnitTestApplication
{
  [TestClass]
  public class UnitTestApplicationMethods
  {
    #region SeparateQuote
    [TestMethod]
    public void TestMethod_SeparateQuote()
    {
      var source = "";
      var expected = "";
      var result = MyFavoriteQuotes.SeparateQuote(source);
      Assert.AreEqual(result, expected);
    }

    #endregion SeparateQuote
  }
}