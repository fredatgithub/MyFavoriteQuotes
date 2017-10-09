#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using MyFavoriteQuotes.Enums;
using MyFavoriteQuotes.Properties;
using Tools;

namespace MyFavoriteQuotes
{
  public partial class FormMain : Form
  {
    public FormMain()
    {
      InitializeComponent();
    }

    readonly Dictionary<string, string> _languageDicoEn = new Dictionary<string, string>();
    readonly Dictionary<string, string> _languageDicoFr = new Dictionary<string, string>();
    private string _lastSaveLocation = string.Empty;
    private readonly Quotes _allQuotes = new Quotes();
    private string _currentLanguage = "english";
    private int _numberOfQuoteFiles = 0;
    private List<string> ListOfQuoteFiles = new List<string>();

    private bool _searchAll;
    private bool _searchAuthor;
    private bool _searchQuote;
    private bool _languageAll;
    private bool _languageEnglish;
    private bool _languageFrench;
    private bool _listlanguageAll;
    private bool _listlanguageEnglish;
    private bool _listlanguageFrench;

    private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!_allQuotes.QuoteFileSaved)
      {
        string msg1 = Translate("QuoteAdded");
        string msg2 = Translate("QuoteAdded");
        var result = DisplayMessage(msg1, msg2, MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
          // we save the xml file.
        }
      }

      SaveControlParametersValue();
      Application.Exit();
    }

    private string GetTranslatedString(string index, string language)
    {
      string result = string.Empty;
      switch (language.ToLower())
      {
        case "english":
          result = _languageDicoEn[index];
          break;
        case "french":
          result = _languageDicoFr[index];
          break;
      }

      return result;
    }

    private string Translate(string word)
    {
      string result = string.Empty;
      string language = frenchToolStripMenuItem.Checked ? "french" : "english";

      switch (language.ToLower())
      {
        case "english":
          result = _languageDicoEn[word];
          break;
        case "french":
          result = _languageDicoFr[word];
          break;
      }

      return result;
    }

    private static WordCase GetWordCase(string myString)
    {
      if (myString.ToLower() == myString)
      {
        return WordCase.AllLowerCase;
      }

      if (myString.ToUpper() == myString)
      {
        return WordCase.AllUpperCase;
      }
      // TODO to be completed


      return WordCase.Unknown;
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
      Text += $" V{fvi.FileMajorPart}.{fvi.FileMinorPart}.{fvi.FileBuildPart}.{fvi.FilePrivatePart}";
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
      DisplayTitle();
      GeControlParametersValue();
      LoadLanguages();
      SetLanguage(Settings.Default.LastLanguageUsed);
      CountQuotefiles();
      LoadQuotes();
      DisplayAllQuotes();
      EnableDisableMenu();
    }

    private void EnableDisableMenu()
    {
      saveToolStripMenuItem.Enabled = !_allQuotes.QuoteFileSaved;
      saveasToolStripMenuItem.Enabled = !_allQuotes.QuoteFileSaved;
    }

    private void DisplayAllQuotes()
    {
      //checkBoxListAll.Checked = true;
      textBoxListQuotes.Text = string.Empty;
      string spaceDashSpace = Punctuation.OneSpace + Punctuation.Dash + Punctuation.OneSpace;
      foreach (var item in _allQuotes.ToList())
      {
        textBoxListQuotes.Text += $"{item.Sentence}{spaceDashSpace}{item.Author}{Environment.NewLine}";
        Application.DoEvents();
      }

      textBoxListQuotes.Select(0, 0);
    }

    private static void CountQuotefiles()
    {
      // TODO count the number of files in Settings.Default.QuoteFileDirectoryName 
      // and add them in a list
    }

    private void LoadQuotes()
    {
      // loading all quotes from all the files quoteX.xml in the folder Quote_files
      if (!Directory.Exists(Settings.Default.QuoteFileDirectoryName))
      {
        CreateQuoteDirectory();
      }

      if (!File.Exists(Path.Combine(Settings.Default.QuoteFileDirectoryName, Settings.Default.QuoteFileName)))
      {
        CreateQuotesFile();
      }

      XDocument xmlDoc;
      try
      {
        xmlDoc = XDocument.Load(Path.Combine(Settings.Default.QuoteFileDirectoryName, Settings.Default.QuoteFileName));
      }
      catch (Exception exception)
      {
        MessageBox.Show(Resources.Error_while_loading + Punctuation.OneSpace +
          Settings.Default.QuoteFileName + Punctuation.OneSpace + Resources.XML_file +
          Punctuation.OneSpace + exception.Message);
        CreateQuotesFile();
        return;
      }

      var result = from node in xmlDoc.Descendants("Quote")
                   where node.HasElements
                   let xElementAuthor = node.Element("Author")
                   where xElementAuthor != null
                   let xElementLanguage = node.Element("Language")
                   where xElementLanguage != null
                   let xElementQuote = node.Element("QuoteValue")
                   where xElementQuote != null
                   select new
                   {
                     authorValue = xElementAuthor.Value,
                     languageValue = xElementLanguage.Value,
                     sentenceValue = xElementQuote.Value
                   };

      foreach (var q in result)
      {
        if (!_allQuotes.ListOfQuotes.Contains(new Quote(q.authorValue, q.languageValue, q.sentenceValue)) &&
          q.authorValue != string.Empty && q.languageValue != string.Empty && q.sentenceValue != string.Empty)
        {
          _allQuotes.Add(new Quote(q.authorValue, q.languageValue, q.sentenceValue));
        }
      }

      _allQuotes.QuoteFileSaved = true;
    }

    private void CreateQuoteDirectory()
    {
      try
      {
        Directory.CreateDirectory(Settings.Default.QuoteFileDirectoryName);
      }
      catch (Exception exception)
      {
        DisplayMessageOk($"There was an error while trying to create the directory {Settings.Default.QuoteFileDirectoryName}{Punctuation.Period} The exception is {exception.Message}", Translate("Error"), MessageBoxButtons.OK);
      }
    }

    private void CreateQuotesFile()
    {
      var minimumVersion = new List<string>
      {
        "<?xml version=\"1.0\" encoding=\"utf-8\" ?>",
        "<Quotes>",
         "<Quote>",
           "<Author>Anonyme</Author>",
           "<Language>French</Language>",
           "<QuoteValue>La culture, c'est comme la confiture, moins on en a et plus on l'étale</QuoteValue>",
        "</Quote>",
        "<Quote>",
           "<Author>Freddy Juhel</Author>",
           "<Language>French</Language>",
           "<QuoteValue>La connaissance est certitude et la certitude ne se confirme pas</QuoteValue>",
        "</Quote>",
        "<Quote>",
           "<Author>Anonymous</Author>",
           "<Language>English</Language>",
           "<QuoteValue>It's impossible said pride. It's risky said experience. It's pointless said reason but give it a try whispered the heart</QuoteValue>",
        "</Quote>",
        "<Quote>",
           "<Author>Antoine de St Exupéry</Author>",
           "<Language>French</Language>",
           "<QuoteValue>Pour ce qui est de l'avenir, il ne s'agit pas de le prévoir mais de le rendre possbile</QuoteValue>",
        "</Quote>",
        "<Quote>",
           "<Author>Albert Einstein</Author>",
           "<Language>French</Language>",
           "<QuoteValue>La vie c’est comme une bicyclette, il faut avancer pour ne pas perdre l’équilibre</QuoteValue>",
        "</Quote>",
         "<Quote>",
           "<Author>Charles Baudelaire</Author>",
            "<Language>French</Language>",
            "<QuoteValue>La curiosité peut être considérée comme le point de départ du génie</QuoteValue>",
        "</Quote>",
        "<Quote>",
          "<Author>Charlie Chaplin</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'humour renforce notre instinct de survie et sauvegarde notre santé d'esprit</QuoteValue>",
        "</Quote>",
        "<Quote>",
          "<Author>Georges Bernard Shaw</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Le succès ne consiste pas à ne jamais faire d'erreur mais à ne jamais faire la même erreur deux fois</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Jean-Jacques Rousseau</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Le plus lent à promettre est toujours le plus fidèle à tenir</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Charles Baudelaire</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Le mal se fait sans effort, naturellement, par fatalité. Le bien est toujours le produit d'un art</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Emmanuel Kant</Author>",
          "<Language>French</Language>",
          "<QuoteValue>On mesure l'intelligence d'un individu à la quantité d'incertitudes qu'il est capable de supporter</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Victor Hugo</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Faites les hommes heureux, vous les faites meilleurs</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Jacques Brel</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Pour moi, Dieu, c'est les hommes</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Michèle Laroque</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Si quelqu'un rêve seul, ce n'est qu'un rêve. Si plusieurs personnes rêvent ensemble, c'est le début d'une réalité</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Simone de Beauvoir</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Qu'est-ce qu'un adulte ? Un enfant gonflé d'âge</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Jean Cocteau</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Le cinéma, c'est l'écriture moderne dont l'encre est la lumière</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>André Breton</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'amour, c'est quand on rencontre quelqu'un qui vous donne de vos nouvelles</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>George Addair</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Tout ce que vous avez toujours voulu est juste de l'autre côté de la peur</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Milan Kundera</Author>",
          "<Language>French</Language>",
          "<QuoteValue>On ne veut être maître de l'avenir que pour pouvoir changer le passé</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Friedrich Nietzsche</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'artiste a le pouvoir de réveiller la force d'agir qui sommeille dans d'autres âmes</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Chateaubriand</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Aimer, c'est bien, savoir aimer, c'est tout</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Georges Bernanos</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'espérance est un risque à courir</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Aristote</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'amitié est une bienveillance mutuelle, par laquelle on se veut et l'on se fait du bien l'un à l'autre</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Eleonor Roosevelt</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Vous devez faire les choses que vous vous croyez incapable de faire</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Patrick Modiano</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Pour aller jusqu'à toi, quel drôle de chemin il m'a fallu prendre</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Jaurès</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Il ne faut avoir aucun regret pour le passé,aucun remords pour le présent,et une confiance inébranlable pour l'avenir</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Laure Conan</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Aimer une personne pour son apparence c'est comme aimer un livre pour sa reliure</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anonyme</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Un baiser est une gourmandise qui ne fait pas grossir</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Seneque</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Ce n'est pas parce que les choses sont difficiles que nous n'osons pas, c'est parce que nous n'osons pas qu'elles sont difficiles</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Confucius</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Choisissez un travail que vous aimez et vous n'aurez pas à travailler un seul jour de votre vie</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anne Frank</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Un ami ne peut prendre la place d'une mère. J'ai besoin de ma mère comme d'un exemple à suivre</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Bud Wilkinson</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La confiance est le ciment invisible qui conduit une équipe à gagner</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anonyme</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Ce n’est pas vos aptitudes, mais plutôt votre attitude, qui vous donne de l’altitude</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Beethoven</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Je ne connais pas d'autres marques de supériorité que la bonté</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anonyme</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Ne jamais confondre la gentillesse pour de la faiblesse, le silence pour de l'ignorance,le calme pour de l'acceptation</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Nietzsche</Author>",
          "<Language>French</Language>",
          "<QuoteValue>On ne tue pas par la colère, mais on tue par le rire</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Carmen Sylva</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La bêtise se met au premier rang pour être vue, l'intelligence se met en arrière pour voir</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Bossuet</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Il y a toujours quelque chose en nous que l'âge ne mûrit pas</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Confucius</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Savoir que l'on sait ce que l'on sait et que l'on ne sait pas ce que l'on ne sait pas, voilà le vrai savoir</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Clemenceau</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La vraie amitié sait être lucide quand il faut, aveugle quand elle doit</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Guy de Maupassant</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Le baiser est la plus sûre façon de se taire en disant tout</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Luis Fernandez</Author>",
          "<Language>French</Language>",
          "<QuoteValue>En compétition il y a toujours un premier et un dernier, mais l'important est de ne pas être le second de soi-même</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Gaston Courtois</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La joie s’acquiert. Elle est une attitude de courage. Être joyeux n’est pas une facilité, c’est une volonté</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Jean de La Fontaine</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'adversaire d'une vraie liberté est un désir excessif de sécurité</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anonyme</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Pour réussir dans le monde, retenez bien ces trois maximes - voir, c'est savoir ; vouloir, c'est pouvoir ; oser, c'est avoir</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anonyme</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Il faut savoir se prêter au rêve lorsque le rêve se prête à nous</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Épictète</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Ce qui émeut les hommes, ce ne sont pas les choses, mais leur opinion sur les choses</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Earl Nightingale</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Notre attitude envers la vie détermine l’attitude de la vie envers nous</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Confucius</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Lorsque l'on se cogne la tête contre un pot et que cela sonne creux, ce n'est pas forcément le pot qui est vide</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anonyme</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'argent ou le pouvoir ne sont pas impressionnants . Les qualités humaines le sont</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anthony J. D’Angelo</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Où que vous alliez, quel que soit le temps, apportez toujours votre propre soleil</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anonyme</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Se connaître soi-même et respecter ce que l'on est, c'est se donner une chance de vaincre ses peurs et d'avoir une vie qui nous ressemble</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Proverbe indien</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La chance est le résultat d’une suite d’actions et d’attitudes positives</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Jean Paul Sartre</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Exister c'est être là...simplement</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Voltaire</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Le seul moyen d'obliger les hommes à dire du bien de vous, c'est d'en faire</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anonymous</Author>",
          "<Language>French</Language>",
          "<QuoteValue>C'est impossible, dit la fierté. C'est risqué, dit l'expérience. C'est sans issue,dit la raison. Essayons murmure le coeur</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Henri Matisse</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Il y a des fleurs partout pour qui veut bien les voir</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Voltaire</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Ce qui touche le cœur se grave dans la mémoire</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Jean Louis Etienne</Author>",
          "<Language>French</Language>",
          "<QuoteValue>On ne repousse pas ses limites, on les découvre</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>La Rochefoucauld</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'amour-propre est le plus grand de tous les flatteurs</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Paul Leautaud</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Aimer, c'est préférer un autre à soi-même</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Nietzsche</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Il faut porter du chaos en soi pour accoucher d'une étoile qui danse</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Henri Bergson</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Partout où il y a joie, il y a création</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Albert Einstein</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Tout est relatif. Mais il est une chose absolue dans notre monde. C'est l'humour</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Anonyme</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La tranquillité de deux mondes repose sur ces deux mots - bienveillance envers les amis, tolérance à l'égard des ennemis</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Johann Wolfgang von Goethe</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Si vous avez confiance en vous-même, vous inspirez confiance aux autres</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Emmanuel Kant</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'apparence requiert art et finesse ; la vérité, calme et simplicité</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Lao Tseu</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Il est plus intelligent d'allumer une toute petite lampe que de se plaindre de l'obscurité</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Martin Luther King</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Les ténèbres ne peuvent pas éclaircir les ténèbres. Seule la lumière peut faire cela</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Aldous Huxley</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'expérience, ce n'est pas ce qui arrive à un homme, c'est ce qu'un homme fait avec ce qui lui arrive</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Pablo Picasso</Author>",
          "<Language>French</Language>",
          "<QuoteValue>S'il y avait une seule vérité on ne pourrait pas faire cent toiles sur le même thème</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Henry Ford</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L’enthousiasme est à la base de tout progrès</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Franklin Delano Roosevelt</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Il est dur d'échouer ; mais il est pire de n'avoir jamais tenté de réussir</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Confucius</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Il faut trois ans pour apprendre à parler, et toute une vie pour apprendre à écouter</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Déclaration de droits de l'homme et du citoyen</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La liberté consiste à pouvoir faire tout ce qui ne nuit pas à autrui</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Alfred de Musset</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Le seul vrai langage au monde est un baiser</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Confucius</Author>",
          "<Language>French</Language>",
          "<QuoteValue>L'homme de bien n'a pas une attitude rigide de refus ou d'acceptation. Le juste est sa seule règle</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Oscar Wilde</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Si l'on veut retrouver sa jeunesse, il suffit d'en répéter les erreurs</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Alphonse Allais</Author>",
          "<Language>French</Language>",
          "<QuoteValue>On devrait construire les villes à la campagne car l'air y est plus pur !</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Victor Hugo</Author>",
          "<Language>French</Language>",
          "<QuoteValue>S’il existe une réalité qui dépasse le rêve, c'est ceci - Vivre</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>La Rochefoucauld</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La parfaite valeur est de faire sans témoin ce qu'on serait capable de faire devant tout le monde</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Charlie Chaplin</Author>",
          "<Language>French</Language>",
          "<QuoteValue>C’est ce que nous sommes tous, des amateurs, on ne vit jamais assez longtemps pour être autre chose</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Alexandra David-Néel</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La bravoure est encore la plus sûre des attitudes. Les choses perdent de leur épouvante a être regardées en face</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Mirabeau</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Nous ne devons jamais ni trop admirer ni trop mépriser</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Cioran</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Ne regarde ni en avant ni en arrière,regarde en toi-même,sans peur ni regret</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Charles Baudelaire</Author>",
          "<Language>French</Language>",
          "<QuoteValue>La conscience d'avoir besoin du pardon rend l'homme plus aimable</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Chögyam Trungpa</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Même dans la pire des situations, nous avons le droit d'insuffler de l'élégance dans notre vie</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Sénèque</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Pendant que l'on attend de vivre, la vie passe</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Confucius</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Après une faute, ne pas se corriger est la vraie faute</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Théodore de Banville</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Ceux qui ne font rien ne se trompent jamais</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Martin Luther King</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Celui qui accepte le mal sans lutter contre lui coopère avec lui</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Proverbe Chinois</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Prends soin de ton corps pour que ton âme ait envie de l'habiter</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Paul Éluard</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Vieillir c'est organiser sa jeunesse au cours des ans</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>Fabrice Luchini</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Citer quelqu’un est davantage un acte d’humilité que de vanité</QuoteValue>",
          "</Quote>",
          "<Quote>",
          "<Author>De la Rochefoucauld</Author>",
          "<Language>French</Language>",
          "<QuoteValue>Aimez le chocolat à fond sans complexes ni fausse honte car...sans un grain de folie,il n'est point d'homme raisonnable</QuoteValue>",
          "</Quote>",
      "<Quotes>",
    "</Document>"
      };
      StreamWriter sw = new StreamWriter(Settings.Default.QuoteFileName);
      foreach (string item in minimumVersion)
      {
        sw.WriteLine(item);
      }

      sw.Close();
      _allQuotes.QuoteFileSaved = true;
      _numberOfQuoteFiles = 1;
    }

    private void LoadLanguages()
    {
      if (!File.Exists(Settings.Default.LanguageFileName))
      {
        CreateLanguageFile();
      }

      // read the translation file and feed the language
      XDocument xDoc;
      try
      {
        xDoc = XDocument.Load(Settings.Default.LanguageFileName);
      }
      catch (Exception exception)
      {
        MessageBox.Show(Resources.Error_while_loading_XML_file + Punctuation.OneSpace + exception.Message);
        CreateLanguageFile();
        return;
      }

      var result = from node in xDoc.Descendants("term")
                   where node.HasElements
                   let xElementName = node.Element("name")
                   where xElementName != null
                   let xElementEnglish = node.Element("englishValue")
                   where xElementEnglish != null
                   let xElementFrench = node.Element("frenchValue")
                   where xElementFrench != null
                   select new
                   {
                     name = xElementName.Value,
                     englishValue = xElementEnglish.Value,
                     frenchValue = xElementFrench.Value
                   };
      foreach (var i in result)
      {
        if (!_languageDicoEn.ContainsKey(i.name))
        {
          _languageDicoEn.Add(i.name, i.englishValue);
        }
#if DEBUG
        else
        {
          MessageBox.Show(Resources.Your_XML_file_has_duplicate_like + Punctuation.Colon + Punctuation.OneSpace + i.name);
        }
#endif
        if (!_languageDicoFr.ContainsKey(i.name))
        {
          _languageDicoFr.Add(i.name, i.frenchValue);
        }
#if DEBUG
        else
        {
          MessageBox.Show(Resources.Your_XML_file_has_duplicate_like + Punctuation.Colon + Punctuation.OneSpace + i.name);
        }
#endif
      }
    }

    private static void CreateLanguageFile()
    {
      var minimumVersion = new List<string>
      {
        "<?xml version=\"1.0\" encoding=\"utf-8\" ?>",
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
      "</terms>"
      };
      StreamWriter sw = new StreamWriter(Settings.Default.LanguageFileName);
      foreach (string item in minimumVersion)
      {
        sw.WriteLine(item);
      }

      sw.Close();
    }

    private void GeControlParametersValue()
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
      checkBoxAdddisplayAfterAdding.Checked = Settings.Default.checkBoxAdddisplayAfterAdding;
    }

    private void SaveControlParametersValue()
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
      Settings.Default.LastSaveLocation = _lastSaveLocation;
      Settings.Default.checkBoxAdddisplayAfterAdding = checkBoxAdddisplayAfterAdding.Checked;
      Settings.Default.Save();
    }

    private void FormMainFormClosing(object sender, FormClosingEventArgs e)
    {
      SaveControlParametersValue();
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
          fileToolStripMenuItem.Text = _languageDicoEn["MenuFile"];
          newToolStripMenuItem.Text = _languageDicoEn["MenuFileNew"];
          openToolStripMenuItem.Text = _languageDicoEn["MenuFileOpen"];
          saveToolStripMenuItem.Text = _languageDicoEn["MenuFileSave"];
          saveasToolStripMenuItem.Text = _languageDicoEn["MenuFileSaveAs"];
          printPreviewToolStripMenuItem.Text = _languageDicoEn["MenuFilePrint"];
          printPreviewToolStripMenuItem.Text = _languageDicoEn["MenufilePageSetup"];
          quitToolStripMenuItem.Text = _languageDicoEn["MenufileQuit"];
          editToolStripMenuItem.Text = _languageDicoEn["MenuEdit"];
          cancelToolStripMenuItem.Text = _languageDicoEn["MenuEditCancel"];
          redoToolStripMenuItem.Text = _languageDicoEn["MenuEditRedo"];
          cutToolStripMenuItem.Text = _languageDicoEn["MenuEditCut"];
          copyToolStripMenuItem.Text = _languageDicoEn["MenuEditCopy"];
          pasteToolStripMenuItem.Text = _languageDicoEn["MenuEditPaste"];
          selectAllToolStripMenuItem.Text = _languageDicoEn["MenuEditSelectAll"];
          toolsToolStripMenuItem.Text = _languageDicoEn["MenuTools"];
          personalizeToolStripMenuItem.Text = _languageDicoEn["MenuToolsCustomize"];
          optionsToolStripMenuItem.Text = _languageDicoEn["MenuToolsOptions"];
          languagetoolStripMenuItem.Text = _languageDicoEn["MenuLanguage"];
          englishToolStripMenuItem.Text = _languageDicoEn["MenuLanguageEnglish"];
          radioButtonAddLanguageEnglish.Text = _languageDicoEn["MenuLanguageEnglish"];
          checkBoxListEnglish.Text = _languageDicoEn["MenuLanguageEnglish"];
          frenchToolStripMenuItem.Text = _languageDicoEn["MenuLanguageFrench"];
          radioButtonAddLanguageFrench.Text = _languageDicoEn["MenuLanguageFrench"];
          checkBoxListFrench.Text = _languageDicoEn["MenuLanguageFrench"];
          groupBoxListLanguage.Text = _languageDicoEn["MenuLanguage"];
          helpToolStripMenuItem.Text = _languageDicoEn["MenuHelp"];
          summaryToolStripMenuItem.Text = _languageDicoEn["MenuHelpSummary"];
          indexToolStripMenuItem.Text = _languageDicoEn["MenuHelpIndex"];
          searchToolStripMenuItem.Text = _languageDicoEn["MenuHelpSearch"];
          aboutToolStripMenuItem.Text = _languageDicoEn["MenuHelpAbout"];
          tabPageSearch.Text = _languageDicoEn["Search"];
          buttonSearch.Text = _languageDicoEn["Search"];
          labelSearch.Text = _languageDicoEn["Search"];
          tabPageAdd.Text = _languageDicoEn["Add"];
          tabPageList.Text = _languageDicoEn["ListAllQuotes"];
          buttonAdd.Text = _languageDicoEn["Add"];
          labelAddAuthor.Text = _languageDicoEn["Author"];
          labelAddQuote.Text = _languageDicoEn["Quote"];
          groupBoxSearch.Text = _languageDicoEn["Search"];
          checkBoxSearchAll.Text = _languageDicoEn["SearchAll"];
          checkBoxSearchAuthor.Text = _languageDicoEn["Author"];
          checkBoxSearchQuote.Text = _languageDicoEn["Quote"];
          groupBoxLanguage.Text = _languageDicoEn["Language"];
          labelAddLanguage.Text = _languageDicoEn["Language"];
          checkBoxLanguageAll.Text = _languageDicoEn["LanguageAll"];
          checkBoxListAll.Text = _languageDicoEn["LanguageAll"];
          checkBoxLanguageEnglish.Text = _languageDicoEn["MenuLanguageEnglish"];
          checkBoxLanguageFrench.Text = _languageDicoEn["MenuLanguageFrench"];
          checkBoxCaseSensitive.Text = _languageDicoEn["CaseSensitive"];
          buttonListDelete.Text = _languageDicoEn["Delete"];
          groupBoxListAuthor.Text = _languageDicoEn["Author"];
          labelListAuthor.Text = _languageDicoEn["Choose"];
          checkBoxAdddisplayAfterAdding.Text = _languageDicoEn["DisplayAfterAdding"];
          labelSearchResultFound.Text = _languageDicoEn["Result found"];
          tabPageStatistics.Text = _languageDicoEn["Statistics"];
          buttonStatCount.Text = _languageDicoEn["Count"];
          buttonSearchDuplicate.Text = _languageDicoEn["Search"];
          TabPageDuplicate.Text = _languageDicoEn["Duplicate"];
          _currentLanguage = "english";
          break;
        case "French":
          checkBoxAdddisplayAfterAdding.Text = _languageDicoFr["DisplayAfterAdding"];
          labelListAuthor.Text = _languageDicoFr["Choose"];
          groupBoxListAuthor.Text = _languageDicoFr["Author"];
          frenchToolStripMenuItem.Checked = true;
          englishToolStripMenuItem.Checked = false;
          fileToolStripMenuItem.Text = _languageDicoFr["MenuFile"];
          newToolStripMenuItem.Text = _languageDicoFr["MenuFileNew"];
          openToolStripMenuItem.Text = _languageDicoFr["MenuFileOpen"];
          saveToolStripMenuItem.Text = _languageDicoFr["MenuFileSave"];
          saveasToolStripMenuItem.Text = _languageDicoFr["MenuFileSaveAs"];
          printPreviewToolStripMenuItem.Text = _languageDicoFr["MenuFilePrint"];
          printPreviewToolStripMenuItem.Text = _languageDicoFr["MenufilePageSetup"];
          quitToolStripMenuItem.Text = _languageDicoFr["MenufileQuit"];
          editToolStripMenuItem.Text = _languageDicoFr["MenuEdit"];
          cancelToolStripMenuItem.Text = _languageDicoFr["MenuEditCancel"];
          redoToolStripMenuItem.Text = _languageDicoFr["MenuEditRedo"];
          cutToolStripMenuItem.Text = _languageDicoFr["MenuEditCut"];
          copyToolStripMenuItem.Text = _languageDicoFr["MenuEditCopy"];
          pasteToolStripMenuItem.Text = _languageDicoFr["MenuEditPaste"];
          selectAllToolStripMenuItem.Text = _languageDicoFr["MenuEditSelectAll"];
          toolsToolStripMenuItem.Text = _languageDicoFr["MenuTools"];
          personalizeToolStripMenuItem.Text = _languageDicoFr["MenuToolsCustomize"];
          optionsToolStripMenuItem.Text = _languageDicoFr["MenuToolsOptions"];
          languagetoolStripMenuItem.Text = _languageDicoFr["MenuLanguage"];
          englishToolStripMenuItem.Text = _languageDicoFr["MenuLanguageEnglish"];
          radioButtonAddLanguageEnglish.Text = _languageDicoFr["MenuLanguageEnglish"];
          checkBoxListEnglish.Text = _languageDicoFr["MenuLanguageEnglish"];
          frenchToolStripMenuItem.Text = _languageDicoFr["MenuLanguageFrench"];
          radioButtonAddLanguageFrench.Text = _languageDicoFr["MenuLanguageFrench"];
          checkBoxListFrench.Text = _languageDicoFr["MenuLanguageFrench"];
          groupBoxListLanguage.Text = _languageDicoFr["MenuLanguage"];
          helpToolStripMenuItem.Text = _languageDicoFr["MenuHelp"];
          summaryToolStripMenuItem.Text = _languageDicoFr["MenuHelpSummary"];
          indexToolStripMenuItem.Text = _languageDicoFr["MenuHelpIndex"];
          searchToolStripMenuItem.Text = _languageDicoFr["MenuHelpSearch"];
          aboutToolStripMenuItem.Text = _languageDicoFr["MenuHelpAbout"];
          tabPageSearch.Text = _languageDicoFr["Search"];
          buttonSearch.Text = _languageDicoFr["Search"];
          labelSearch.Text = _languageDicoFr["Search"];
          tabPageAdd.Text = _languageDicoFr["Add"];
          tabPageList.Text = _languageDicoFr["ListAllQuotes"];
          buttonAdd.Text = _languageDicoFr["Add"];
          labelAddAuthor.Text = _languageDicoFr["Author"];
          labelAddQuote.Text = _languageDicoFr["Quote"];
          groupBoxSearch.Text = _languageDicoFr["Search"];
          checkBoxSearchAll.Text = _languageDicoFr["SearchAll"];
          checkBoxSearchAuthor.Text = _languageDicoFr["Author"];
          checkBoxSearchQuote.Text = _languageDicoFr["Quote"];
          groupBoxLanguage.Text = _languageDicoFr["Language"];
          checkBoxLanguageAll.Text = _languageDicoFr["LanguageAll"];
          checkBoxListAll.Text = _languageDicoFr["LanguageAll"];
          checkBoxLanguageEnglish.Text = _languageDicoFr["MenuLanguageEnglish"];
          checkBoxLanguageFrench.Text = _languageDicoFr["MenuLanguageFrench"];
          checkBoxCaseSensitive.Text = _languageDicoFr["CaseSensitive"];
          labelAddLanguage.Text = _languageDicoFr["Language"];
          buttonListDelete.Text = _languageDicoFr["Delete"];
          labelSearchResultFound.Text = _languageDicoFr["Result found"];
          tabPageStatistics.Text = _languageDicoFr["Statistics"];
          buttonStatCount.Text = _languageDicoFr["Count"];
          buttonSearchDuplicate.Text = _languageDicoFr["Search"];
          tabPageList.Text = _languageDicoFr["ListAllQuotes"];
          TabPageDuplicate.Text = _languageDicoFr["Duplicate"];
          _currentLanguage = "french";
          break;
      }
    }

    private void ButtonAddClick(object sender, EventArgs e)
    {
      if (textBoxAddAuthor.Text.Trim() == string.Empty)
      {
        textBoxAddAuthor.Text = Settings.Default.UnknownAuthor;
      }

      if (textBoxAddQuote.Text == string.Empty)
      {
        DisplayMessageOk(Translate("EmptyQuote"), Translate("EmptyQuoteShort"), MessageBoxButtons.OK);
        return;
      }

      // check if the quote is not already in
      // TODO code

      _allQuotes.Add(new Quote(textBoxAddAuthor.Text, radioButtonAddLanguageEnglish.Checked ? "English" : "French", textBoxAddQuote.Text));
      EnableDisableMenu();
      UpdateAfterAddition();
      if (checkBoxAdddisplayAfterAdding.Checked)
      {
        tabControlMain.SelectedIndex = 2;
        textBoxResult.Select(0, 0);
      }
    }

    private void UpdateAfterAddition()
    {
      textBoxAddAuthor.Text = string.Empty;
      textBoxAddQuote.Text = string.Empty;
      DisplayQuotes(checkBoxListEnglish.Checked, checkBoxListFrench.Checked);
    }

    private static string RemoveColon(string input)
    {
      return input.Replace(':', ' ');
    }

    private static string ReplaceSpaceWithUnderScore(string input)
    {
      return input.Replace(' ', '_');
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

      if (!_searchAll && !_searchAuthor && !_searchQuote)
      {
        checkBoxSearchAll.Checked = true;
      }

      if (!_languageAll && !_languageEnglish && !_languageFrench)
      {
        checkBoxLanguageAll.Checked = true;
      }

      //if (Debugger.IsAttached)
      //{
      //  Debugger.Break();
      //}

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

      var searchedResult = new List<string>();
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

      labelNbOfResultFound.Text = $": {searchedResult.Count}";
      searchedResult = null;
    }

    private List<string> SearchInMemory(string searchedString,
      SearchedCriteria author = SearchedCriteria.AuthorAndQuote,
      SearchedLanguage language = SearchedLanguage.FrenchAndEnglish)
    {
      var result2 = new List<string>();
      // First we select them all and then we remove what's not selected
      var result3 = from node in _allQuotes.ToList()
                    select node;
      var result4 = result3;
      bool caseSensitive = checkBoxCaseSensitive.Checked;
      if (author == SearchedCriteria.Author && caseSensitive)
      {
        result3 = from node in result3
                  where node.Author.Contains(searchedString)
                  select node;
      }

      if (author == SearchedCriteria.Author && !caseSensitive)
      {
        result3 = from node in result3
                  where node.Author.ToLower().Contains(searchedString.ToLower())
                  select node;
      }

      if (author == SearchedCriteria.Quote && caseSensitive)
      {
        result3 = from node in result3
                  where node.Sentence.Contains(searchedString)
                  select node;
      }

      if (author == SearchedCriteria.Quote && !caseSensitive)
      {
        result3 = from node in result3
                  where node.Sentence.ToLower().Contains(searchedString.ToLower())
                  select node;
      }

      if (author == SearchedCriteria.AuthorAndQuote && caseSensitive)
      {
        result3 = from node in result4
                  where (node.Sentence.Contains(searchedString)
                  || node.Author.Contains(searchedString))
                  select node;
      }

      if (author == SearchedCriteria.AuthorAndQuote && !caseSensitive)
      {
        result3 = from node in result4
                  where (node.Sentence.ToLower().Contains(searchedString.ToLower())
                  || node.Author.ToLower().Contains(searchedString.ToLower()))
                  select node;
      }

      if (language == SearchedLanguage.English || language == SearchedLanguage.French)
      {
        result3 = from node in result3
                  where node.Language.Contains(language.ToString())
                  select node;
      }
      else if (language == SearchedLanguage.FrenchAndEnglish)
      {
        result3 = from node in result3
                  where (node.Language.Contains(SearchedLanguage.English.ToString())
                  || node.Language.Contains(SearchedLanguage.French.ToString()))
                  select node;
      }

      foreach (var quote in result3)
      {
        if (quote.Author != string.Empty && quote.Language != string.Empty && quote.Sentence != string.Empty)
        {
          result2.Add(quote.Sentence + " - " + quote.Author);
        }
      }

      return result2;
    }

    private List<string> SearchInXmlFor(string filename, string searchedString, string author, string language = "English")
    {
      var result2 = new List<string>();
      XDocument xDoc;
      try
      {
        xDoc = XDocument.Load(Settings.Default.QuoteFileName);
      }
      catch (Exception exception)
      {
        MessageBox.Show(Resources.Error_while_loading_the + Punctuation.OneSpace +
          Settings.Default.QuoteFileName + Punctuation.OneSpace + Resources.XML_file +
          Punctuation.Colon + Punctuation.OneSpace + exception.Message);
        return result2;
      }

      var result = from node in xDoc.Descendants("Quote")
                   where node.HasElements
                   let xElementAuthor = node.Element("Author")
                   where xElementAuthor != null
                   let xElementLanguage = node.Element("Language")
                   where xElementLanguage != null
                   let xElementquote = node.Element("QuoteValue")
                   where xElementquote != null
                   select new
                   {
                     authorValue = xElementAuthor.Value,
                     languageValue = xElementLanguage.Value,
                     quoteValue = xElementquote.Value
                   };

      foreach (var i in result)
      {
        if (i.languageValue == language)
        {
          result2.Add(i.quoteValue + Punctuation.OneSpace + Punctuation.Dash + Punctuation.OneSpace + i.authorValue);
        }
      }

      return result2;
    }

    private List<string> SearchInXmlFor2(string filename, string searchedString, string author, string language = "English")
    {
      var result2 = new List<string>();
      XDocument xDoc;
      try
      {
        xDoc = XDocument.Load(filename);
      }
      catch (Exception exception)
      {
        MessageBox.Show("Error while loading the " + Settings.Default.QuoteFileName +
                        " xml file: " + exception.Message);
        return result2;
      }

      var result = from node in xDoc.Descendants("Quotes")
                   where node.HasElements
                   where node.Name == "Quote"
                   let xElementquote = node.Element("Quote")
                   where xElementquote != null
                   select new
                   {
                     quoteValue = xElementquote.Value,
                     authorValue = xElementquote.Attribute("Author").Value,
                     languageValue = xElementquote.Attribute("Language").Value,
                   };

      foreach (var i in result)
      {
        result2.Add(i.quoteValue);
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
      _searchAll = checkBoxSearchAll.Checked;
      if (checkBoxSearchAll.Checked)
      {
        checkBoxSearchAuthor.Checked = true;
        checkBoxSearchQuote.Checked = true;
      }
    }

    private void checkBoxSearchAuthor_CheckedChanged(object sender, EventArgs e)
    {
      _searchAuthor = checkBoxSearchAuthor.Checked;
      if (checkBoxSearchAll.Checked && !checkBoxSearchAuthor.Checked)
      {
        checkBoxSearchAll.Checked = false;
      }

      if (checkBoxSearchQuote.Checked && checkBoxSearchAuthor.Checked)
      {
        checkBoxSearchAll.Checked = true;
      }
    }

    private void checkBoxSearchQuote_CheckedChanged(object sender, EventArgs e)
    {
      _searchQuote = checkBoxSearchQuote.Checked;
      if (checkBoxSearchAll.Checked && !checkBoxSearchQuote.Checked)
      {
        checkBoxSearchAll.Checked = false;
      }

      if (checkBoxSearchQuote.Checked && checkBoxSearchAuthor.Checked)
      {
        checkBoxSearchAll.Checked = true;
      }
    }

    private void checkBoxLanguageAll_CheckedChanged(object sender, EventArgs e)
    {
      _languageAll = checkBoxLanguageAll.Checked;
      if (checkBoxLanguageAll.Checked)
      {
        checkBoxLanguageEnglish.Checked = true;
        checkBoxLanguageFrench.Checked = true;
      }
    }

    private void checkBoxLanguageEnglish_CheckedChanged(object sender, EventArgs e)
    {
      _languageEnglish = checkBoxLanguageEnglish.Checked;
      if (checkBoxLanguageAll.Checked && !checkBoxLanguageEnglish.Checked)
      {
        checkBoxLanguageAll.Checked = false;
      }

      if (checkBoxLanguageFrench.Checked && checkBoxLanguageEnglish.Checked)
      {
        checkBoxLanguageAll.Checked = true;
      }
    }

    private void checkBoxLanguageFrench_CheckedChanged(object sender, EventArgs e)
    {
      _languageFrench = checkBoxLanguageFrench.Checked;
      if (checkBoxLanguageAll.Checked && !checkBoxLanguageFrench.Checked)
      {
        checkBoxLanguageAll.Checked = false;
      }

      if (checkBoxLanguageFrench.Checked && checkBoxLanguageEnglish.Checked)
      {
        checkBoxLanguageAll.Checked = true;
      }
    }

    private void checkBoxListAll_CheckedChanged(object sender, EventArgs e)
    {
      _listlanguageAll = checkBoxListAll.Checked;
      if (checkBoxListAll.Checked)
      {
        checkBoxListEnglish.Checked = true;
        checkBoxListFrench.Checked = true;
        DisplayAllQuotes();
      }
    }

    private void checkBoxListEnglish_CheckedChanged(object sender, EventArgs e)
    {
      _listlanguageEnglish = checkBoxListEnglish.Checked;
      if (checkBoxListAll.Checked && !checkBoxListEnglish.Checked)
      {
        checkBoxListAll.Checked = false;
      }

      if (checkBoxListFrench.Checked && checkBoxListEnglish.Checked)
      {
        checkBoxListAll.Checked = true;
        DisplayAllQuotes();
      }

      if (comboBoxListAuthor.SelectedIndex == -1)
      {
        DisplayQuotes(checkBoxListEnglish.Checked, checkBoxListFrench.Checked);
      }
      else
      {
        // take into account if comboboxLanguage has something selected
        if (checkBoxListEnglish.Checked && !checkBoxListFrench.Checked)
        {
          DisplayQuotes(comboBoxListAuthor.SelectedItem.ToString(), Language.English);
        }

        if (!checkBoxListEnglish.Checked && checkBoxListFrench.Checked)
        {
          DisplayQuotes(comboBoxListAuthor.SelectedItem.ToString(), Language.French);
        }


        if (checkBoxListFrench.Checked && checkBoxListEnglish.Checked)
        {
          // TODO add code
        }
      }
    }

    private void DisplayQuotes(string author, Language language = Language.English)
    {
      IEnumerable<Quote> result3;
      if (author.ToLower() == "all")
      {
        result3 = from node in _allQuotes.ToList()
                  select node;
      }
      else // language = all has to be coded otherwise it's a bug
      {
        result3 = from node in _allQuotes.ToList()
                  where node.Author == author
                  && node.Language == language.ToString() // to keep only selected languages
                  select node;
      }

      textBoxListQuotes.Text = string.Empty;
      foreach (var quote in result3)
      {
        textBoxListQuotes.Text += quote.Sentence + Punctuation.OneSpace + Punctuation.Dash + Punctuation.OneSpace +
          quote.Author + Environment.NewLine;
      }

      labelNumberOfQuotes.Text = Translate("Number of quote") + Punctuation.SpaceIfFrench(_currentLanguage) +
        Punctuation.Colon + Punctuation.OneSpace + result3.Count();
    }

    private void DisplayQuotes(bool englishChecked, bool frenchChecked)
    {
      IEnumerable<Quote> result3 = from node in _allQuotes.ToList() select node;
      IEnumerable<Quote> result4 = result3;
      if (englishChecked && !frenchChecked)
      {
        result3 = from node in result4
                  where node.Language.Contains(SearchedLanguage.English.ToString())
                  select node;
      }

      if (frenchChecked && !englishChecked)
      {
        result3 = from node in result4
                  where node.Language.Contains(SearchedLanguage.French.ToString())
                  select node;
      }

      if (englishChecked && frenchChecked)
      {
        result3 = result4;
      }

      if (!englishChecked && !frenchChecked) // nothing to display, empty list
      {
        result3 = from node in _allQuotes.ToList().Where(n => n.Sentence == "")
                  select node;
      }

      textBoxListQuotes.Text = string.Empty;
      comboBoxListAuthor.Items.Clear();
      comboBoxListAuthor.Items.Add("All");
      foreach (var quote in result3)
      {
        textBoxListQuotes.Text += quote.Sentence + Punctuation.OneSpace + Punctuation.Dash +
          Punctuation.OneSpace + quote.Author + Environment.NewLine;
        if (!comboBoxListAuthor.Items.Contains(quote.Author))
        {
          comboBoxListAuthor.Items.Add(quote.Author);
        }
      }

      labelNumberOfQuotes.Text = Translate("Number of quote") + Punctuation.SpaceIfFrench(_currentLanguage) +
        Punctuation.Colon + Punctuation.OneSpace + result3.Count();
    }

    private void checkBoxListFrench_CheckedChanged(object sender, EventArgs e)
    {
      _listlanguageFrench = checkBoxListFrench.Checked;
      if (checkBoxListAll.Checked && !checkBoxListFrench.Checked)
      {
        checkBoxListAll.Checked = false;
      }

      if (checkBoxListFrench.Checked && checkBoxListEnglish.Checked)
      {
        checkBoxListAll.Checked = true;
      }

      DisplayQuotes(checkBoxListEnglish.Checked, checkBoxListFrench.Checked);
    }

    private void buttonAddCancel_Click(object sender, EventArgs e)
    {
      textBoxAddQuote.Text = string.Empty;
      textBoxAddAuthor.Text = string.Empty;
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(Settings.Default.QuoteFileName);
      XmlNode root = doc.DocumentElement;
      XmlElement newQuote = doc.CreateElement("Quote");
      XmlElement newAuthor = doc.CreateElement("Author");
      newAuthor.InnerText = textBoxAddAuthor.Text;
      XmlElement newLanguage = doc.CreateElement("Language");
      newLanguage.InnerText = frenchToolStripMenuItem.Checked ? "French" : "English";
      XmlElement newQuoteValue = doc.CreateElement("QuoteValue");
      newQuoteValue.InnerText = RemoveColon(textBoxAddQuote.Text);
      newQuote.AppendChild(newAuthor);
      newQuote.AppendChild(newLanguage);
      newQuote.AppendChild(newQuoteValue);
      root.AppendChild(newQuote);
      doc.Save(Settings.Default.QuoteFileName);

      _allQuotes.QuoteFileSaved = true;
      EnableDisableMenu();
    }

    private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveFileDialog sfd = new SaveFileDialog
      {
        InitialDirectory = Settings.Default.LastSaveLocation == ""
          ? Environment.SpecialFolder.MyDocuments.ToString()
          : Settings.Default.LastSaveLocation,
        CreatePrompt = false,
        OverwritePrompt = true,
        FileName = "NewQuoteFile.xml",
        DefaultExt = "xml",
        Filter = CreateFilterString("xml") // should be "Xml files (*.xml)|*.xml"
      };

      if (sfd.ShowDialog() != DialogResult.OK) return;
      if (File.Exists(sfd.FileName))
      {
        File.Delete(sfd.FileName);
      }

      SaveAllQuotesToXmlFile(sfd.FileName);
    }

    private static string CreateFilterString(string extension)
    {
      var result = new StringBuilder();
      result.Append(Punctuation.OneSpace);
      result.Append(Punctuation.OpenParenthesis);
      result.Append(Punctuation.Multiply);
      result.Append(Punctuation.Period);
      result.Append(extension);
      result.Append(Punctuation.CloseParenthesis);
      result.Append(Punctuation.Pipe);
      result.Append(Punctuation.Multiply);
      result.Append(Punctuation.Period);
      result.Append(extension);
      return result.ToString();
    }

    private void SaveAllQuotesToXmlFile(string fileName)
    {
      XmlDocument xmlDoc = new XmlDocument();
      XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
      XmlElement xmlRoot = xmlDoc.DocumentElement;
      xmlDoc.InsertBefore(xmlDeclaration, xmlRoot);
      XmlElement rootQuotes = xmlDoc.CreateElement(string.Empty, "Quotes", string.Empty);
      xmlDoc.AppendChild(rootQuotes);

      foreach (var item in _allQuotes.ToList())
      {
        XmlElement newQuote = xmlDoc.CreateElement("Quote");
        XmlElement newAuthor = xmlDoc.CreateElement("Author");
        newAuthor.InnerText = item.Author;
        XmlElement newLanguage = xmlDoc.CreateElement("Language");
        newLanguage.InnerText = item.Language;
        XmlElement newQuoteValue = xmlDoc.CreateElement("QuoteValue");
        newQuoteValue.InnerText = RemoveColon(item.Sentence);
        newQuote.AppendChild(newAuthor);
        newQuote.AppendChild(newLanguage);
        newQuote.AppendChild(newQuoteValue);
        rootQuotes.AppendChild(newQuote);
      }

      xmlDoc.Save(fileName);

      _allQuotes.QuoteFileSaved = true;
      EnableDisableMenu();
      _lastSaveLocation = fileName;
    }

    private void buttonListDelete_Click(object sender, EventArgs e)
    {
      if (textBoxListQuotes.Text == string.Empty)
      {
        DisplayMessageOk(Translate("EmptyText"), Translate("EmptyTextShort"), MessageBoxButtons.OK);
        return;
      }

      if (textBoxListQuotes.SelectedText.Length <= 0)
      {
        DisplayMessageOk(Translate("NoSelection"), Translate("NoSelectionShort"), MessageBoxButtons.OK);
        return;
      }

      // delete the quote
      if (_allQuotes.Remove(SeparateQuote(textBoxListQuotes.SelectedText)[0], SeparateQuote(textBoxListQuotes.SelectedText)[1]))
      {
        DisplayMessageOk(Translate("QuoteDeleted"), Translate("QuoteDeletedShort"), MessageBoxButtons.OK);
        EnableDisableMenu();
        // refresh and reload list
        DisplayQuotes(checkBoxListEnglish.Checked, checkBoxListFrench.Checked);
      }
      else
      {
        DisplayMessageOk(Translate("NoQuoteDeleted") +
          Environment.NewLine +
          Translate("TheSentence") +
          Environment.NewLine +
          textBoxListQuotes.SelectedText +
          Environment.NewLine +
          Translate("HasNotBeenfound"),
          Translate("NoQuoteDeletedShort"), MessageBoxButtons.OK);
      }
    }

    public static string[] SeparateQuote(string wholeQuote)
    {
      var result = new string[2];
      if (wholeQuote.Length < 4)
      {
        return result;
      }

      if (!wholeQuote.Contains('-'))
      {
        return result;
      }

      var lastIndex = wholeQuote.LastIndexOf('-');
      result[0] = wholeQuote.Substring(0, lastIndex - 1);
      result[1] = wholeQuote.Substring(lastIndex + 2);
      return result;
    }

    private void comboBoxListAuthor_SelectedIndexChanged(object sender, EventArgs e)
    {
      // display only selected author in all languages
      DisplayQuotes(comboBoxListAuthor.SelectedItem.ToString(), GetSelectedLanguage(checkBoxListAll, checkBoxListEnglish, checkBoxListFrench)); // add language selected in checkboxes
    }

    private static Language GetSelectedLanguage(params CheckBox[] checkBoxList)
    {
      var tmp = checkBoxList.ToList().Where(box => box.Checked);
      foreach (CheckBox box in tmp)
      {
        if (box.Name.Contains("All"))
        {
          return Language.All;
        }

        if (box.Name.Contains("English"))
        {
          return Language.English;
        }

        if (box.Name.Contains("French"))
        {
          return Language.French;
        }
      }

      return Language.All; // if no other language has been selected
    }

    private static Control FindFocusedControl(Control container)
    {
      foreach (Control childControl in container.Controls.Cast<Control>().Where(childControl => childControl.Focused))
      {
        return childControl;
      }

      return (from Control childControl in container.Controls
              select FindFocusedControl(childControl)).FirstOrDefault(maybeFocusedControl => maybeFocusedControl != null);
    }

    private void cutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Control focusedControl = FindFocusedControl(tabControlMain);
      // if (focusedControl is TextBox)
      TextBox tb = focusedControl as TextBox;
      if (tb != null)
      {
        CutToClipboard(tb);
      }
    }

    private TextBox WhatTextBoxHasFocus()
    {
      return Controls.OfType<TextBox>().FirstOrDefault(control => control.Focused);
    }

    private void copyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Control focusedControl = FindFocusedControl(tabControlMain);
      if (focusedControl is TextBox)
      {
        CopyToClipboard((TextBox)focusedControl);
      }
    }

    private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Control focusedControl = FindFocusedControl(tabControlMain);
      // if (focusedControl is TextBox)
      TextBox tb = focusedControl as TextBox;
      if (tb != null)
      {
        PasteFromClipboard(tb);
      }
    }

    private void CutToClipboard(TextBoxBase tb, string errorMessage = "nothing")
    {
      if (tb != ActiveControl) return;
      if (tb.Text == string.Empty)
      {
        DisplayMessageOk(Translate("ThereIs") + Punctuation.OneSpace +
          Translate(errorMessage) + Punctuation.OneSpace +
          Translate("ToCut") + Punctuation.OneSpace, Translate(errorMessage),
          MessageBoxButtons.OK);
        return;
      }

      if (tb.SelectedText == string.Empty)
      {
        DisplayMessageOk(Translate("NoTextHasBeenSelected"),
          Translate(errorMessage), MessageBoxButtons.OK);
        return;
      }

      Clipboard.SetText(tb.SelectedText);
      tb.SelectedText = string.Empty;
    }

    private void CopyToClipboard(TextBoxBase tb, string message = "nothing")
    {
      if (tb != ActiveControl) return;
      if (tb.Text == string.Empty)
      {
        DisplayMessageOk(Translate("ThereIsNothingToCopy") + Punctuation.OneSpace,
          Translate(message), MessageBoxButtons.OK);
        return;
      }

      if (tb.SelectedText == string.Empty)
      {
        DisplayMessageOk(Translate("NoTextHasBeenSelected"),
          Translate(message), MessageBoxButtons.OK);
        return;
      }

      Clipboard.SetText(tb.SelectedText);
    }

    private void PasteFromClipboard(TextBoxBase tb)
    {
      if (tb != ActiveControl) return;
      var selectionIndex = tb.SelectionStart;
      tb.SelectedText = Clipboard.GetText();
      tb.SelectionStart = selectionIndex + Clipboard.GetText().Length;
    }

    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Control focusedControl = FindFocusedControl(tabControlMain);
      TextBox box = focusedControl as TextBox;
      // box?.SelectAll(); // null propagation
      if (box != null)
      {
        box.SelectAll();
      }
    }

    private void buttonStatCount_Click(object sender, EventArgs e)
    {
      textBoxStatQuotes.Text = string.Empty;

      int totalQuotes = _allQuotes.ToList().Count();
      textBoxStatQuotes.Text += $"{totalQuotes}{Punctuation.OneSpace}{Translate("quote")}{Punctuation.Pluralize(totalQuotes)}{Punctuation.OneSpace}{Translate("in total")}{Environment.NewLine}";

      int frenchQuotes = GetQuotesByLanguage(Language.French).Count();
      textBoxStatQuotes.Text += $"{frenchQuotes}{Punctuation.OneSpace}{Translate("quote")}{Punctuation.Pluralize(frenchQuotes)}{Punctuation.OneSpace}{Translate("in French")}{Environment.NewLine}";

      int englishQuotesNumber = GetQuotesByLanguage().Count();
      string tmpQuote = $"{englishQuotesNumber}{Punctuation.OneSpace}{Translate("quote")}{Punctuation.Pluralize(englishQuotesNumber)}{Punctuation.OneSpace}{Translate("in English")}{Environment.NewLine}";
      textBoxStatQuotes.Text += tmpQuote;
      int tmpQuoteLenght = tmpQuote.Length;

      AddLine(CreateDashLine(tmpQuoteLenght));
      Dictionary<string, int> allQuotes = GetQuotesByAuthor(Language.All);
      IEnumerable<Quote> tmpQuotes = GetDuplicateQuotes(_allQuotes.ToList());
      if (tmpQuotes.Count() != 0)
      {
        foreach (Quote quote in tmpQuotes)
        {
          textBoxStatQuotes.Text += $"{Translate("duplicate quote")}{Punctuation.OneSpace}: {quote.Author} - {quote.Sentence}{Environment.NewLine}";
        }
      }
      else
      {
        textBoxStatQuotes.Text += $"{Translate("No duplicate quote found")}{Environment.NewLine}";
      }

      AddLine(CreateDashLine($"{Translate("No duplicate quote found")}".Length));
      textBoxStatQuotes.Text += $"{Translate("List of author sorted by number of quotes descending")}{Environment.NewLine}";
      AddLine(CreateDashLine($"{Translate("List of author sorted by number of quotes descending")}{Environment.NewLine}".Length));
      allQuotes = SortDictionaryWithValue(allQuotes);
      string lastQuote = string.Empty;
      foreach (KeyValuePair<string, int> quote in allQuotes)
      {
        lastQuote =
          $"{quote.Key}{Punctuation.OneSpace}{Translate("has")}{Punctuation.OneSpace}{quote.Value}{Punctuation.OneSpace}{Translate("quote")}{Punctuation.Pluralize(quote.Value)}{Environment.NewLine}";
        textBoxStatQuotes.Text += lastQuote;
      }

      AddLine(CreateDashLine(lastQuote.Length));
      tmpQuote = $"{Translate("List of author sorted in alphabetical order")}{Environment.NewLine}";
      textBoxStatQuotes.Text += tmpQuote;
      tmpQuoteLenght = tmpQuote.Length;
      AddLine(CreateDashLine(tmpQuoteLenght));
      allQuotes = SortDictionaryWithKey(allQuotes, false);
      foreach (KeyValuePair<string, int> quote in allQuotes)
      {
        textBoxStatQuotes.Text +=
          $"{quote.Key}{Punctuation.OneSpace}{Translate("has")}{Punctuation.OneSpace}{quote.Value}{Punctuation.OneSpace}{Translate("quote")}{Punctuation.Pluralize(quote.Value)}{Environment.NewLine}";
      }
    }

    private static string CreateDashLine(int numberOfDash)
    {
      string result = string.Empty;
      for (int i = 0; i < numberOfDash; i++)
      {
        result += "-";
      }

      return result;
    }

    private void AddLine(string dashLine)
    {
      textBoxStatQuotes.Text += $"{dashLine}{Environment.NewLine}";
    }

    private static IEnumerable<Quote> GetDuplicateQuotes(IEnumerable<Quote> allQuotes)
    {
      var tmpDico = new Dictionary<string, int>();
      var result = new List<Quote>();
      foreach (Quote quote in allQuotes)
      {
        if (tmpDico.ContainsKey(quote.Sentence))
        {
          result.Add(quote);
        }
        else
        {
          tmpDico.Add(quote.Sentence, 1);
        }
      }

      return result;
    }

    private IEnumerable<Quote> GetQuotesByLanguage(Language language = Language.English)
    {
      var result = from node in _allQuotes.ToList()
                   where node.Language == language.ToString()
                   select node;
      return result;
    }

    private static Dictionary<string, int> SortDictionaryWithValue(Dictionary<string, int> dico, bool descending = true)
    {
      return @descending ? dico.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value) :
        dico.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
    }

    private static Dictionary<string, int> SortDictionaryWithKey(Dictionary<string, int> dico, bool descending = true)
    {
      return @descending ? dico.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value) :
        dico.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
    }

    private Dictionary<string, int> GetQuotesByAuthor(Language language = Language.English)
    {
      var dico = new Dictionary<string, int>();
      if (language == Language.All)
      {
        var result = from node in _allQuotes.ToList()
                     select node;
        foreach (Quote quote in result)
        {
          if (dico.ContainsKey(quote.Author))
          {
            dico[quote.Author]++;
          }
          else
          {
            dico.Add(quote.Author, 1);
          }
        }
      }
      else
      {
        var result = from node in _allQuotes.ToList()
                     where node.Language == language.ToString()
                     select node;
        foreach (Quote quote in result)
        {
          if (dico.ContainsKey(quote.Author))
          {
            dico[quote.Author]++;
          }
          else
          {
            dico.Add(quote.Author, 1);
          }
        }
      }

      return dico;
    }

    private void buttonDuplicate_Click(object sender, EventArgs e)
    {
      textBoxDuplicate.Text = $"The Quotes.xml file has {_allQuotes.ListOfQuotes.Count} quotes";
      Quotes allQuotesWithDuplicate = new Quotes();
      // loading all quotes from the file quotes.xml
      if (!File.Exists(Settings.Default.QuoteFileName))
      {
        CreateQuotesFile();
      }

      XDocument xmlDoc;
      try
      {
        xmlDoc = XDocument.Load(Settings.Default.QuoteFileName);
      }
      catch (Exception exception)
      {
        MessageBox.Show(Resources.Error_while_loading + Punctuation.OneSpace +
          Settings.Default.QuoteFileName + Punctuation.OneSpace + Resources.XML_file +
          Punctuation.OneSpace + exception.Message);
        CreateQuotesFile();
        return;
      }

      var result = from node in xmlDoc.Descendants("Quote")
                   where node.HasElements
                   let xElementAuthor = node.Element("Author")
                   where xElementAuthor != null
                   let xElementLanguage = node.Element("Language")
                   where xElementLanguage != null
                   let xElementQuote = node.Element("QuoteValue")
                   where xElementQuote != null
                   select new
                   {
                     authorValue = xElementAuthor.Value,
                     languageValue = xElementLanguage.Value,
                     sentenceValue = xElementQuote.Value
                   };

      foreach (var q in result)
      {
        if (!_allQuotes.ListOfQuotes.Contains(new Quote(q.authorValue, q.languageValue, q.sentenceValue)) &&
          q.authorValue != string.Empty && q.languageValue != string.Empty && q.sentenceValue != string.Empty)
        {
          _allQuotes.Add(new Quote(q.authorValue, q.languageValue, q.sentenceValue));
        }
        else
        {
          allQuotesWithDuplicate.Add(new Quote(q.authorValue, q.languageValue, q.sentenceValue));
        }
      }
      //TODO add translate method for the following terms
      textBoxDuplicate.Text += Environment.NewLine;
      textBoxDuplicate.Text += $"There are {allQuotesWithDuplicate.ListOfQuotes.Count} quote{Punctuation.Pluralize(allQuotesWithDuplicate.ListOfQuotes.Count)} which are duplicate";
      textBoxDuplicate.Text += Environment.NewLine;
      if (allQuotesWithDuplicate.ListOfQuotes.Count > 0)
      {
        textBoxDuplicate.Text += "The duplicate are:";
        textBoxDuplicate.Text += Environment.NewLine;
        foreach (Quote item in allQuotesWithDuplicate.ToList())
        {
          textBoxDuplicate.Text += $"{item.Author} - {item.Sentence}";
          textBoxDuplicate.Text += Environment.NewLine;
        }

        DisplayMessageOk($"{Translate("You should clean the file")}{Punctuation.OneSpace}{Settings.Default.QuoteFileName}", Translate("Cleaning needed"), MessageBoxButtons.OK);
      }
    }
  }
}