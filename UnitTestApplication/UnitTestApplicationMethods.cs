using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFavoriteQuotes;
using System;

namespace UnitTestApplication
{
  [TestClass]
  public class UnitTestApplicationMethods
  {
    #region SeparateQuote
    [TestMethod]
    public void TestMethod_SeparateQuote_simple_quote()
    {
      const string source = "A long long time ago in a galaxy far far away - StarWars";
      string[] expected = { "A long long time ago in a galaxy far far away", "StarWars" };
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }

    [TestMethod]
    public void TestMethod_SeparateQuote_empty_string()
    {
      const string source = "";
      string[] expected = new string[2];
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }

    [TestMethod]
    public void TestMethod_SeparateQuote_two_dashes()
    {
      const string source = "A long-long time ago in a galaxy far far away - StarWars";
      string[] expected = { "A long-long time ago in a galaxy far far away", "StarWars" };
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }


    [TestMethod]
    public void TestMethod_SeparateQuote_three_dashes()
    {
      const string source = "A long-long time ago in a galaxy far-far away - StarWars";
      string[] expected = { "A long-long time ago in a galaxy far-far away", "StarWars" };
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }

    [TestMethod]
    public void TestMethod_SeparateQuote_no_dash()
    {
      const string source = "A long long time ago in a galaxy far far away StarWars";
      string[] expected = new string[2];
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }

    #endregion SeparateQuote
    #region helper methods
    public bool AssertCollectionsAreEqual<T>(object collectionExpected, object collectionResult) where T : Type
    {
      const bool result = false;
      //for (int i = 0; i < collectionExpected.Length; i++)
      //{
      //  Assert.AreEqual(collectionExpected[i], collectionResult[i]);
      //}
            
      return result;
    }
    #endregion helper methods

    #region GetQuotesByAuthor
    public void TestMethod_Get_Quotes_By_Author()
    {
      // TODO to be completed
      //var source = new Dictionary<string, int> {{"Jules Verne", 5}};
      //var expected = new Dictionary<string, int> { { "Jules Verne", 5 } };
      //var result = FormMain.GetQuotesByAuthor(source);
      //Assert.AreEqual(result[0], expected[0]);
      //Assert.AreEqual(result[1], expected[1]);
      Assert.IsFalse(false);
    }
    #endregion GetQuotesByAuthor
    #region CountQuotes
    public void TestMethod_Count_Quotes()
    {
      // TODO to be completed
      //var source = new Dictionary<string, int> {{"Jules Verne", 5}};
      //var expected = new Dictionary<string, int> { { "Jules Verne", 5 } };
      //var result = FormMain.CountQuotes(source);
      //Assert.AreEqual(result[0], expected[0]);
      //Assert.AreEqual(result[1], expected[1]);

      Assert.IsFalse(false);
    }
    #endregion CountQuotes
  }
}