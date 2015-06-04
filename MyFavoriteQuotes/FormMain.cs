/*
The MIT License(MIT)
Copyright(c) 2015 Freddy Juhel
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using MyFavoriteQuotes.Properties;

namespace MyFavoriteQuotes
{
  public partial class FormMain : Form
  {
    public FormMain()
    {
      InitializeComponent();
    }

    readonly Dictionary<string, string> languageDicoEn = new Dictionary<string, string>();
    readonly Dictionary<string, string> languageDicoFr = new Dictionary<string, string>();
    private Quotes AllQuotes = new Quotes();

    private bool searchAll;
    private bool searchAuthor;
    private bool searchQuote;
    private bool languageAll;
    private bool languageEnglish;
    private bool languageFrench;
    private bool listlanguageAll;
    private bool listlanguageEnglish;
    private bool listlanguageFrench;

    private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveWindowValue();
      Application.Exit();
    }

    private void AboutToolStripMenuItemClick(object sender, EventArgs e)
    {
      AboutBoxApplication aboutBoxApplication = new AboutBoxApplication();
      aboutBoxApplication.ShowDialog();
    }

    private void DisplayTitle()
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
      Text += string.Format(" V{0}.{1}.{2}.{3}", fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart);
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
      DisplayTitle();
      GetWindowValue();
      LoadLanguages();
      SetLanguage(Settings.Default.LastLanguageUsed);
      LoadQuotes();
      DisplayAllQuotes();
    }

    private void DisplayAllQuotes()
    {
      checkBoxListAll.Checked = true;
      textBoxListQuotes.Text = string.Empty;
      foreach (var item in AllQuotes.ToList())
      {
        textBoxListQuotes.Text += item.Sentence + " - " + item.Author + Environment.NewLine;
      }
    }

    private void LoadQuotes()
    {
      // loading all quotes from the file quotes.xml
      if (!File.Exists(Settings.Default.QuoteFileName))
      {
        CreateQuotesFile();
      }

      XDocument xmlDoc = XDocument.Load(Settings.Default.QuoteFileName);
      var result = from node in xmlDoc.Descendants("Quote")
                   where node.HasElements
                   select new
                   {
                     authorValue = node.Element("Author").Value,
                     languageValue = node.Element("Language").Value,
                     sentenceValue = node.Element("QuoteValue").Value
                   };

      foreach (var q in result)
      {
        AllQuotes.Add(new Quote(q.authorValue, q.languageValue, q.sentenceValue));
      }
    }

    private void CreateQuotesFile()
    {
      throw new NotImplementedException();
    }

    private void LoadLanguages()
    {
      if (!File.Exists(Settings.Default.LanguageFileName))
      {
        CreateLanguageFile();
      }

      // read the translation file and feed the language
      XDocument xmlDoc = XDocument.Load(Settings.Default.LanguageFileName);
      var result = from node in xmlDoc.Descendants("term")
                   where node.HasElements
                   select new
                   {
                     quoteValue = node.Element("name").Value,
                     authorValue = node.Element("englishValue").Value,
                     languageValue = node.Element("frenchValue").Value
                   };

      foreach (var i in result)
      {
        languageDicoEn.Add(i.quoteValue, i.authorValue);
        languageDicoFr.Add(i.quoteValue, i.languageValue);
      }
    }

    private static void CreateLanguageFile()
    {
      List<string> minimumVersion = new List<string>
      {
        "<?xml version=\"1.0\" encoding=\"utf - 8\" ?>",
        "<Document>",
        "<DocumentVersion>",
        "<version> 1.0 </version>",
        "</DocumentVersion>",
        "<terms>",
         "<term>",
           "<name>MenuFile</name>",
           "<englishValue>File</englishValue>",
           "<frenchValue>Fichier</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuFileNew</name>",
          "<englishValue>New</englishValue>",
          "<frenchValue>Nouveau</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuFileOpen</name>",
          "<englishValue>Open</englishValue>",
          "<frenchValue>Ouvrir</frenchValue>",
          "</term>",
        "<term>",
          "<name>MenuFileSave</name>",
          "<englishValue>Save</englishValue>",
          "<frenchValue>Enregistrer</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuFileSaveAs</name>",
          "<englishValue>Save as ...</englishValue>",
          "<frenchValue>Enregistrer sous ...</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuFilePrint</name>",
          "<englishValue>Print ...</englishValue>",
          "<frenchValue>Imprimer ...</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenufilePageSetup</name>",
          "<englishValue>Page setup</englishValue>",
          "<frenchValue>Aperçu avant impression</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenufileQuit</name>",
          "<englishValue>Quit</englishValue>",
          "<frenchValue>Quitter</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuEdit</name>",
          "<englishValue>Edit</englishValue>",
          "<frenchValue>Edition</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuEditCancel</name>",
          "<englishValue>Cancel</englishValue>",
          "<frenchValue>Annuler</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuEditRedo</name>",
          "<englishValue>Redo</englishValue>",
          "<frenchValue>Rétablir</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuEditCut</name>",
          "<englishValue>Cut</englishValue>",
          "<frenchValue>Couper</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuEditCopy</name>",
          "<englishValue>Copy</englishValue>",
          "<frenchValue>Copier</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuEditPaste</name>",
          "<englishValue>Paste</englishValue>",
          "<frenchValue>Coller</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuEditSelectAll</name>",
          "<englishValue>Select All</englishValue>",
          "<frenchValue>Sélectionner tout</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuTools</name>",
          "<englishValue>Tools</englishValue>",
          "<frenchValue>Outils</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuToolsCustomize</name>",
          "<englishValue>Customize ...</englishValue>",
          "<frenchValue>Personaliser ...</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuToolsOptions</name>",
          "<englishValue>Options</englishValue>",
          "<frenchValue>Options</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuLanguage</name>",
          "<englishValue>Language</englishValue>",
          "<frenchValue>Langage</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuLanguageEnglish</name>",
          "<englishValue>English</englishValue>",
          "<frenchValue>Anglais</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuLanguageFrench</name>",
          "<englishValue>French</englishValue>",
          "<frenchValue>Français</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuHelp</name>",
          "<englishValue>Help</englishValue>",
          "<frenchValue>Aide</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuHelpSummary</name>",
          "<englishValue>Summary</englishValue>",
          "<frenchValue>Sommaire</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuHelpIndex</name>",
          "<englishValue>Index</englishValue>",
          "<frenchValue>Index</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuHelpSearch</name>",
          "<englishValue>Search</englishValue>",
          "<frenchValue>Rechercher</frenchValue>",
        "</term>",
        "<term>",
          "<name>MenuHelpAbout</name>",
          "<englishValue>About</englishValue>",
          "<frenchValue>A propos de ...</frenchValue>",
        "</term>",
        "<term>",
          "<name>All</name>",
          "<englishValue>All</englishValue>",
          "<frenchValue>Tout</frenchValue>",
        "</term>",
        "<term>",
          "<name>Author</name>",
          "<englishValue>Author</englishValue>",
          "<frenchValue>Auteur</frenchValue>",
        "</term>",
        "<term>",
          "<name>Quote</name>",
          "<englishValue>Quote</englishValue>",
          "<frenchValue>Citation</frenchValue>",
        "</term>",
        "<term>",
          "<name>CaseSensitive</name>",
          "<englishValue>Case sensitive</englishValue>",
          "<frenchValue>Sensible à la casse</frenchValue>",
        "</term>",
      "</terms>",
    "</Document>"
      };
      StreamWriter sw = new StreamWriter(Settings.Default.LanguageFileName);
      foreach (string item in minimumVersion)
      {
        sw.WriteLine(item);
      }

      sw.Close();
    }

    private void GetWindowValue()
    {
      Width = Settings.Default.WindowWidth;
      Height = Settings.Default.WindowHeight;
      Top = Settings.Default.WindowTop < 0 ? 0 : Settings.Default.WindowTop;
      Left = Settings.Default.WindowLeft < 0 ? 0 : Settings.Default.WindowLeft;
      tabControlMain.SelectedIndex = Settings.Default.LastTabUsed;
      checkBoxCaseSensitive.Checked = Settings.Default.SearchCaseSensitive;
      textBoxSearch.Text = Settings.Default.textBoxSearch;
      checkBoxSearchAll.Checked = Settings.Default.checkBoxSearchAll;
      checkBoxSearchAuthor.Checked = Settings.Default.checkBoxSearchAuthor;
      checkBoxSearchQuote.Checked = Settings.Default.checkBoxSearchQuote;
      checkBoxLanguageAll.Checked = Settings.Default.checkBoxLanguageAll;
      checkBoxLanguageEnglish.Checked = Settings.Default.checkBoxLanguageEnglish;
      checkBoxLanguageFrench.Checked = Settings.Default.checkBoxLanguageFrench;
    }

    private void SaveWindowValue()
    {
      Settings.Default.WindowHeight = Height;
      Settings.Default.WindowWidth = Width;
      Settings.Default.WindowLeft = Left;
      Settings.Default.WindowTop = Top;
      Settings.Default.LastLanguageUsed = frenchToolStripMenuItem.Checked ? "French" : "English";
      Settings.Default.LastTabUsed = tabControlMain.SelectedIndex;
      Settings.Default.SearchCaseSensitive = checkBoxCaseSensitive.Checked;
      Settings.Default.textBoxSearch = textBoxSearch.Text;
      Settings.Default.checkBoxSearchAll = checkBoxSearchAll.Checked;
      Settings.Default.checkBoxSearchAuthor = checkBoxSearchAuthor.Checked;
      Settings.Default.checkBoxSearchQuote = checkBoxSearchQuote.Checked;
      Settings.Default.checkBoxLanguageAll = checkBoxLanguageAll.Checked;
      Settings.Default.checkBoxLanguageEnglish = checkBoxLanguageEnglish.Checked;
      Settings.Default.checkBoxLanguageFrench = checkBoxLanguageFrench.Checked;
      Settings.Default.Save();
    }

    private void FormMainFormClosing(object sender, FormClosingEventArgs e)
    {
      SaveWindowValue();
    }

    private void FrenchToolStripMenuItemClick(object sender, EventArgs e)
    {
      SetLanguage(Language.French.ToString());
    }

    private void EnglishToolStripMenuItemClick(object sender, EventArgs e)
    {
      SetLanguage(Language.English.ToString());
    }

    private void SetLanguage(string myLanguage)
    {
      switch (myLanguage)
      {
        case "English":
          frenchToolStripMenuItem.Checked = false;
          englishToolStripMenuItem.Checked = true;
          fileToolStripMenuItem.Text = languageDicoEn["MenuFile"];
          newToolStripMenuItem.Text = languageDicoEn["MenuFileNew"];
          openToolStripMenuItem.Text = languageDicoEn["MenuFileOpen"];
          saveToolStripMenuItem.Text = languageDicoEn["MenuFileSave"];
          saveasToolStripMenuItem.Text = languageDicoEn["MenuFileSaveAs"];
          printPreviewToolStripMenuItem.Text = languageDicoEn["MenuFilePrint"];
          printPreviewToolStripMenuItem.Text = languageDicoEn["MenufilePageSetup"];
          quitToolStripMenuItem.Text = languageDicoEn["MenufileQuit"];
          editToolStripMenuItem.Text = languageDicoEn["MenuEdit"];
          cancelToolStripMenuItem.Text = languageDicoEn["MenuEditCancel"];
          redoToolStripMenuItem.Text = languageDicoEn["MenuEditRedo"];
          cutToolStripMenuItem.Text = languageDicoEn["MenuEditCut"];
          copyToolStripMenuItem.Text = languageDicoEn["MenuEditCopy"];
          pasteToolStripMenuItem.Text = languageDicoEn["MenuEditPaste"];
          selectAllToolStripMenuItem.Text = languageDicoEn["MenuEditSelectAll"];
          toolsToolStripMenuItem.Text = languageDicoEn["MenuTools"];
          personalizeToolStripMenuItem.Text = languageDicoEn["MenuToolsCustomize"];
          optionsToolStripMenuItem.Text = languageDicoEn["MenuToolsOptions"];
          languagetoolStripMenuItem.Text = languageDicoEn["MenuLanguage"];
          englishToolStripMenuItem.Text = languageDicoEn["MenuLanguageEnglish"];
          radioButtonAddLanguageEnglish.Text = languageDicoEn["MenuLanguageEnglish"];
          checkBoxListEnglish.Text = languageDicoEn["MenuLanguageEnglish"];
          frenchToolStripMenuItem.Text = languageDicoEn["MenuLanguageFrench"];
          radioButtonAddLanguageFrench.Text = languageDicoEn["MenuLanguageFrench"];
          checkBoxListFrench.Text = languageDicoEn["MenuLanguageFrench"];
          groupBoxListLanguage.Text = languageDicoEn["MenuLanguage"];
          helpToolStripMenuItem.Text = languageDicoEn["MenuHelp"];
          summaryToolStripMenuItem.Text = languageDicoEn["MenuHelpSummary"];
          indexToolStripMenuItem.Text = languageDicoEn["MenuHelpIndex"];
          searchToolStripMenuItem.Text = languageDicoEn["MenuHelpSearch"];
          aboutToolStripMenuItem.Text = languageDicoEn["MenuHelpAbout"];
          tabPageSearch.Text = languageDicoEn["Search"];
          buttonSearch.Text = languageDicoEn["Search"];
          labelSearch.Text = languageDicoEn["Search"];
          tabPageAdd.Text = languageDicoEn["Add"];
          buttonAdd.Text = languageDicoEn["Add"];
          labelAddAuthor.Text = languageDicoEn["Author"];
          labelAddQuote.Text = languageDicoEn["Quote"];
          groupBoxSearch.Text = languageDicoEn["Search"];
          checkBoxSearchAll.Text = languageDicoEn["SearchAll"];
          checkBoxSearchAuthor.Text = languageDicoEn["Author"];
          checkBoxSearchQuote.Text = languageDicoEn["Quote"];
          groupBoxLanguage.Text = languageDicoEn["Language"];
          labelAddLanguage.Text = languageDicoEn["Language"];
          checkBoxLanguageAll.Text = languageDicoEn["LanguageAll"];
          checkBoxListAll.Text = languageDicoEn["LanguageAll"];
          checkBoxLanguageEnglish.Text = languageDicoEn["MenuLanguageEnglish"];
          checkBoxLanguageFrench.Text = languageDicoEn["MenuLanguageFrench"];
          checkBoxCaseSensitive.Text = languageDicoEn["CaseSensitive"];
          break;
        case "French":
          frenchToolStripMenuItem.Checked = true;
          englishToolStripMenuItem.Checked = false;
          fileToolStripMenuItem.Text = languageDicoFr["MenuFile"];
          newToolStripMenuItem.Text = languageDicoFr["MenuFileNew"];
          openToolStripMenuItem.Text = languageDicoFr["MenuFileOpen"];
          saveToolStripMenuItem.Text = languageDicoFr["MenuFileSave"];
          saveasToolStripMenuItem.Text = languageDicoFr["MenuFileSaveAs"];
          printPreviewToolStripMenuItem.Text = languageDicoFr["MenuFilePrint"];
          printPreviewToolStripMenuItem.Text = languageDicoFr["MenufilePageSetup"];
          quitToolStripMenuItem.Text = languageDicoFr["MenufileQuit"];
          editToolStripMenuItem.Text = languageDicoFr["MenuEdit"];
          cancelToolStripMenuItem.Text = languageDicoFr["MenuEditCancel"];
          redoToolStripMenuItem.Text = languageDicoFr["MenuEditRedo"];
          cutToolStripMenuItem.Text = languageDicoFr["MenuEditCut"];
          copyToolStripMenuItem.Text = languageDicoFr["MenuEditCopy"];
          pasteToolStripMenuItem.Text = languageDicoFr["MenuEditPaste"];
          selectAllToolStripMenuItem.Text = languageDicoFr["MenuEditSelectAll"];
          toolsToolStripMenuItem.Text = languageDicoFr["MenuTools"];
          personalizeToolStripMenuItem.Text = languageDicoFr["MenuToolsCustomize"];
          optionsToolStripMenuItem.Text = languageDicoFr["MenuToolsOptions"];
          languagetoolStripMenuItem.Text = languageDicoFr["MenuLanguage"];
          englishToolStripMenuItem.Text = languageDicoFr["MenuLanguageEnglish"];
          radioButtonAddLanguageEnglish.Text = languageDicoFr["MenuLanguageEnglish"];
          checkBoxListEnglish.Text = languageDicoFr["MenuLanguageEnglish"];
          frenchToolStripMenuItem.Text = languageDicoFr["MenuLanguageFrench"];
          radioButtonAddLanguageFrench.Text = languageDicoFr["MenuLanguageFrench"];
          checkBoxListFrench.Text = languageDicoFr["MenuLanguageFrench"];
          groupBoxListLanguage.Text = languageDicoFr["MenuLanguage"];
          helpToolStripMenuItem.Text = languageDicoFr["MenuHelp"];
          summaryToolStripMenuItem.Text = languageDicoFr["MenuHelpSummary"];
          indexToolStripMenuItem.Text = languageDicoFr["MenuHelpIndex"];
          searchToolStripMenuItem.Text = languageDicoFr["MenuHelpSearch"];
          aboutToolStripMenuItem.Text = languageDicoFr["MenuHelpAbout"];
          tabPageSearch.Text = languageDicoFr["Search"];
          buttonSearch.Text = languageDicoFr["Search"];
          labelSearch.Text = languageDicoFr["Search"];
          tabPageAdd.Text = languageDicoFr["Add"];
          buttonAdd.Text = languageDicoFr["Add"];
          labelAddAuthor.Text = languageDicoFr["Author"];
          labelAddQuote.Text = languageDicoFr["Quote"];
          groupBoxSearch.Text = languageDicoFr["Search"];
          checkBoxSearchAll.Text = languageDicoFr["SearchAll"];
          checkBoxSearchAuthor.Text = languageDicoFr["Author"];
          checkBoxSearchQuote.Text = languageDicoFr["Quote"];
          groupBoxLanguage.Text = languageDicoFr["Language"];
          checkBoxLanguageAll.Text = languageDicoFr["LanguageAll"];
          checkBoxListAll.Text = languageDicoFr["LanguageAll"];
          checkBoxLanguageEnglish.Text = languageDicoFr["MenuLanguageEnglish"];
          checkBoxLanguageFrench.Text = languageDicoFr["MenuLanguageFrench"];
          checkBoxCaseSensitive.Text = languageDicoFr["CaseSensitive"];
          labelAddLanguage.Text = languageDicoFr["Language"];
          break;
      }
    }

    private void ButtonAddClick(object sender, EventArgs e)
    {
      if (textBoxAddAuthor.Text == string.Empty)
      {
        textBoxAddAuthor.Text = "unknown author";
      }

      if (textBoxAddQuote.Text == string.Empty)
      {
        if (frenchToolStripMenuItem.Checked)
        {
          DisplayMessageOk(Settings.Default.TextBoxEmptyFr, "Citation vide", MessageBoxButtons.OK);
          return;
        }

        if (englishToolStripMenuItem.Checked)
        {
          DisplayMessageOk(Settings.Default.TextBoxEmptyEN, "Empty quote", MessageBoxButtons.OK);
          return;
        }
      }

      // check if the quote is not already in
      // TODO code
      // open the quotes.xml file and add the quote

      AllQuotes.Add(new Quote(textBoxAddAuthor.Text,
        radioButtonAddLanguageEnglish.Checked ? "English" : "French"
        , textBoxAddQuote.Text));
    }

    private string RemoveColon(string input)
    {
      return input.Replace(':', ' ');
    }

    private DialogResult DisplayMessage(string message, string title, MessageBoxButtons buttons)
    {
      DialogResult result = MessageBox.Show(this, message, title, buttons);
      return result;
    }

    private void DisplayMessageOk(string message, string title, MessageBoxButtons buttons)
    {
      MessageBox.Show(this, message, title, buttons);
    }

    private void ButtonSearchClick(object sender, EventArgs e)
    {
      textBoxResult.Text = string.Empty;
      if (textBoxSearch.Text == string.Empty)
      {
        DisplayMessageOk(
          frenchToolStripMenuItem.Checked ? Settings.Default.textBoxSearchFr : Settings.Default.textBoxSearchEn,
          frenchToolStripMenuItem.Checked ? Settings.Default.SearchEmptyFr : Settings.Default.SearchEmptyEn,
          MessageBoxButtons.OK);
      }

      if (!searchAll && !searchAuthor && !searchQuote)
      {
        checkBoxSearchAll.Checked = true;
      }

      if (!languageAll && !languageEnglish && !languageFrench)
      {
        checkBoxLanguageAll.Checked = true;
      }

      //if (Debugger.IsAttached)
      //{
      //  Debugger.Break();
      //}

      List<string> searchedResult = new List<string>();
      var criteriaAuthor = SearchedCriteria.NoCriteriaChosen;
      var criteriaLanguage = SearchedLanguage.NoLanguageChosen;
      if (checkBoxSearchAll.Checked)
      {
        criteriaAuthor = SearchedCriteria.AuthorAndQuote;
      }

      if (checkBoxSearchAuthor.Checked && !checkBoxSearchQuote.Checked)
      {
        criteriaAuthor = SearchedCriteria.Author;
      }

      if (!checkBoxSearchAuthor.Checked && checkBoxSearchQuote.Checked)
      {
        criteriaAuthor = SearchedCriteria.Quote;
      }

      if (checkBoxLanguageAll.Checked)
      {
        criteriaLanguage = SearchedLanguage.FrenchAndEnglish;
      }

      if (checkBoxLanguageEnglish.Checked && !checkBoxLanguageFrench.Checked)
      {
        criteriaLanguage = SearchedLanguage.English;
      }

      if (!checkBoxLanguageEnglish.Checked && checkBoxLanguageFrench.Checked)
      {
        criteriaLanguage = SearchedLanguage.French;
      }

      searchedResult = SearchInMemory(textBoxSearch.Text, criteriaAuthor, criteriaLanguage);
      if (searchedResult.Count != 0)
      {
        foreach (string item in searchedResult)
        {
          textBoxResult.Text += item + Environment.NewLine;
        }
      }
      else
      {
        DisplayMessage("No result were found.", "No result", MessageBoxButtons.OK);
      }

      searchedResult = null;
    }

    private List<string> SearchInMemory(string searchedString,
      SearchedCriteria author = SearchedCriteria.AuthorAndQuote,
      SearchedLanguage language = SearchedLanguage.FrenchAndEnglish)
    {
      List<string> result2 = new List<string>();
      IEnumerable<Quote> result3;
      IEnumerable<Quote> result4;
      // First we select them all and then we remove what's not selected
      result3 = from node in AllQuotes.ToList()
                select node;
      result4 = result3;
      bool caseSensitive = checkBoxCaseSensitive.Checked;
      if (author == SearchedCriteria.Author && caseSensitive)
      {
        result3 = from node in result3
                  where node.Author.ToString().Contains(searchedString)
                  select node;
      }

      if (author == SearchedCriteria.Author && !caseSensitive)
      {
        result3 = from node in result3
                  where node.Author.ToString().ToLower().Contains(searchedString.ToLower())
                  select node;
      }

      if (author == SearchedCriteria.Quote && caseSensitive)
      {
        result3 = from node in result3
                  where node.Sentence.ToString().Contains(searchedString)
                  select node;
      }

      if (author == SearchedCriteria.Quote && !caseSensitive)
      {
        result3 = from node in result3
                  where node.Sentence.ToString().ToLower().Contains(searchedString.ToLower())
                  select node;
      }

      if (author == SearchedCriteria.AuthorAndQuote && caseSensitive)
      {
        result3 = from node in result4
                  where (node.Sentence.ToString().Contains(searchedString)
                  || node.Author.ToString().Contains(searchedString))
                  select node;
      }

      if (author == SearchedCriteria.AuthorAndQuote && !caseSensitive)
      {
        result3 = from node in result4
                  where (node.Sentence.ToString().ToLower().Contains(searchedString.ToLower())
                  || node.Author.ToString().ToLower().Contains(searchedString.ToLower()))
                  select node;
      }

      if (language == SearchedLanguage.English || language == SearchedLanguage.French)
      {
        result3 = from node in result3
                  where node.Language.ToString().Contains(language.ToString())
                  select node;
      }
      else if (language == SearchedLanguage.FrenchAndEnglish)
      {
        result3 = from node in result3
                  where (node.Language.ToString().Contains(SearchedLanguage.English.ToString())
                  || node.Language.ToString().Contains(SearchedLanguage.French.ToString()))
                  select node;
      }

      foreach (var quote in result3)
      {
        result2.Add(quote.Sentence + " - " + quote.Author);
      }
      
      return result2;
    }

    private List<string> SearchInXmlFor(string filename, string searchedString, string author, string language = "English")
    {
      List<string> result2 = new List<string>();
      XDocument xDoc = XDocument.Load(Settings.Default.QuoteFileName);
      var result = from node in xDoc.Descendants("Quote")
                   where node.HasElements
                   select new
                   {
                     authorValue = node.Element("Author").Value,
                     languageValue = node.Element("Language").Value,
                     quoteValue = node.Element("QuoteValue").Value
                   };

      foreach (var i in result)
      {
        if (i.languageValue == language)
        {
          result2.Add(i.quoteValue + " - " + i.authorValue);
        }

      }

      return result2;
    }

    private List<string> SearchInXmlFor2(string filename, string searchedString, string author, string language = "English")
    {
      List<string> result2 = new List<string>();
      XDocument xDoc = XDocument.Load(filename);
      var result = from node in xDoc.Descendants("Quotes")
                   where node.HasElements
                   where node.Name == "Quote"
                   //where node.Element("Quote").Attribute("Author") == author
                   //where node.Element("Quote"). == language
                   //where node.Element("Author") == searchString
                   select new
                   {
                     quoteValue = node.Element("Quote").Value,
                     authorValue = node.Element("Quote").Attribute("Author").Value,
                     languageValue = node.Element("Quote").Attribute("Language").Value,
                   };

      foreach (var i in result)
      {
        result2.Add(i.quoteValue);
        //languageDicoEn.Add(i.quoteValue, i.authorValue);
        //languageDicoFr.Add(i.quoteValue, i.languageValue);
      }

      return result2;
    }

    private List<string> SearchXmlFor(string filename, string searchedString, string author, string language = "English")
    {
      List<string> result = new List<string>();
      XmlTextReader reader = new XmlTextReader(filename);
      while (reader.Read())
      {
        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Quote")
        {
          if (reader.GetAttribute("Author").Contains(author))
          {
            result.Add(reader.ReadContentAsString() + Environment.NewLine);
          }
        }
      }

      return result;
    }

    private bool IsAlreadyInXml(string fileName, string searchedItem)
    {
      bool result = false;
      XmlTextReader reader = new XmlTextReader(fileName);
      while (reader.Read())
      {
        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Quote")
        {
          if (reader.GetAttribute("name") == searchedItem)
          {
            result = true;
          }
        }
      }

      return result;
    }

    private void checkBoxSearchAll_CheckedChanged(object sender, EventArgs e)
    {
      searchAll = checkBoxSearchAll.Checked;
      if (checkBoxSearchAll.Checked)
      {
        checkBoxSearchAuthor.Checked = true;
        checkBoxSearchQuote.Checked = true;
      }
    }

    private void checkBoxSearchAuthor_CheckedChanged(object sender, EventArgs e)
    {
      searchAuthor = checkBoxSearchAuthor.Checked;
      if (checkBoxSearchAll.Checked && !checkBoxSearchAuthor.Checked)
      {
        checkBoxSearchAll.Checked = false;
      }
    }

    private void checkBoxSearchQuote_CheckedChanged(object sender, EventArgs e)
    {
      searchQuote = checkBoxSearchQuote.Checked;
      if (checkBoxSearchAll.Checked && !checkBoxSearchQuote.Checked)
      {
        checkBoxSearchAll.Checked = false;
      }
    }

    private void checkBoxLanguageAll_CheckedChanged(object sender, EventArgs e)
    {
      languageAll = checkBoxLanguageAll.Checked;
      if (checkBoxLanguageAll.Checked)
      {
        checkBoxLanguageEnglish.Checked = true;
        checkBoxLanguageFrench.Checked = true;
      }
    }

    private void checkBoxLanguageEnglish_CheckedChanged(object sender, EventArgs e)
    {
      languageEnglish = checkBoxLanguageEnglish.Checked;
      if (checkBoxLanguageAll.Checked && !checkBoxLanguageEnglish.Checked)
      {
        checkBoxLanguageAll.Checked = false;
      }
    }

    private void checkBoxLanguageFrench_CheckedChanged(object sender, EventArgs e)
    {
      languageFrench = checkBoxLanguageFrench.Checked;
      if (checkBoxLanguageAll.Checked && !checkBoxLanguageFrench.Checked)
      {
        checkBoxLanguageAll.Checked = false;
      }
    }

    private void checkBoxListAll_CheckedChanged(object sender, EventArgs e)
    {
      listlanguageAll = checkBoxListAll.Checked;
      if (checkBoxListAll.Checked)
      {
        checkBoxListEnglish.Checked = true;
        checkBoxListFrench.Checked = true;
      }
    }

    private void checkBoxListEnglish_CheckedChanged(object sender, EventArgs e)
    {
      listlanguageEnglish = checkBoxListEnglish.Checked;
      if (checkBoxListAll.Checked && !checkBoxListEnglish.Checked)
      {
        checkBoxListAll.Checked = false;
      }

      if (checkBoxListFrench.Checked && checkBoxListEnglish.Checked)
      {
        checkBoxListAll.Checked = true;
      }
    }

    private void checkBoxListFrench_CheckedChanged(object sender, EventArgs e)
    {
      listlanguageFrench = checkBoxListFrench.Checked;
      if (checkBoxListAll.Checked && !checkBoxListFrench.Checked)
      {
        checkBoxListAll.Checked = false;
      }

      if (checkBoxListFrench.Checked && checkBoxListEnglish.Checked)
      {
        checkBoxListAll.Checked = true;
      }
    }
  }
}