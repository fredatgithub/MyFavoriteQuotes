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
      string source = "A long long time ago in a galaxy far far away - StarWars";
      string[] expected = { "A long long time ago in a galaxy far far away", "StarWars" };
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }

    [TestMethod]
    public void TestMethod_SeparateQuote_empty_string()
    {
      string source = "";
      string[] expected = new string[2];
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }

    [TestMethod]
    public void TestMethod_SeparateQuote_two_dashes()
    {
      string source = "A long-long time ago in a galaxy far far away - StarWars";
      string[] expected = { "A long-long time ago in a galaxy far far away", "StarWars" };
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }


    [TestMethod]
    public void TestMethod_SeparateQuote_three_dashes()
    {
      string source = "A long-long time ago in a galaxy far-far away - StarWars";
      string[] expected = { "A long-long time ago in a galaxy far-far away", "StarWars" };
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }

    [TestMethod]
    public void TestMethod_SeparateQuote_no_dash()
    {
      string source = "A long long time ago in a galaxy far far away StarWars";
      string[] expected = new string[2];
      string[] result = FormMain.SeparateQuote(source);
      Assert.AreEqual(result[0], expected[0]);
      Assert.AreEqual(result[1], expected[1]);
    }

    #endregion SeparateQuote
    #region helper methods
    public bool AssertCollectionsAreEqual<T>(object collectionExpected, object collectionResult) where T : Type
    {
      bool result = false;
      //for (int i = 0; i < collectionExpected.Length; i++)
      //{
      //  Assert.AreEqual(collectionExpected[i], collectionResult[i]);
      //}
            
      return result;
    }
    #endregion helper methods
  }
}