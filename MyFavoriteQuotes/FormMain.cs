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
    private const string Space = " ";
    private string lastSaveLocation = string.Empty;
    private readonly Quotes AllQuotes = new Quotes();

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
      if (!AllQuotes.QuoteFileSaved)
      {
        string msg1 = GetTranslatedString("QuoteAdded");
        string msg2 = GetTranslatedString("QuoteAdded");
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
          result = languageDicoEn[index];
          break;
        case "french":
          result = languageDicoFr[index];
          break;
      }

      return result;
    }

    private string GetTranslatedString(string index)
    {
      string result = string.Empty;
      string language = frenchToolStripMenuItem.Checked ? "french" : "english";

      switch (language.ToLower())
      {
        case "english":
          result = languageDicoEn[index];
          break;
        case "french":
          result = languageDicoFr[index];
          break;
      }

      return result;
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
      GeControlParametersValue();
      LoadLanguages();
      SetLanguage(Settings.Default.LastLanguageUsed);
      LoadQuotes();
      DisplayAllQuotes();
      EnableDisableMenu();
    }

    private void EnableDisableMenu()
    {
      saveToolStripMenuItem.Enabled = !AllQuotes.QuoteFileSaved;
      saveasToolStripMenuItem.Enabled = !AllQuotes.QuoteFileSaved;
    }

    private void DisplayAllQuotes()
    {
      checkBoxListAll.Checked = true;
      textBoxListQuotes.Text = string.Empty;
      foreach (var item in AllQuotes.ToList())
      {
        textBoxListQuotes.Text += item.Sentence + " - " + item.Author + Environment.NewLine;
      }

      textBoxListQuotes.Select(0, 0);
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

      AllQuotes.QuoteFileSaved = true;
    }

    private void CreateQuotesFile()
    {
      List<string> minimumVersion = new List<string>
      {
        "<?xml version=\"1.0\" encoding=\"utf-8\" ?>",
        "<Document>",
        "<DocumentVersion>",
        "<version> 1.0 </version>",
        "</DocumentVersion>",
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
      "</terms>",
    "</Document>"
      };
      StreamWriter sw = new StreamWriter(Settings.Default.QuoteFileName);
      foreach (string item in minimumVersion)
      {
        sw.WriteLine(item);
      }

      sw.Close();
      AllQuotes.QuoteFileSaved = true;
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
        "<?xml version=\"1.0\" encoding=\"utf-8\" ?>",
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
      Settings.Default.LastSaveLocation = lastSaveLocation;
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
          tabPageList.Text = languageDicoEn["ListAllQuotes"];
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
          buttonListDelete.Text = languageDicoEn["Delete"];
          groupBoxListAuthor.Text = languageDicoEn["Author"];
          labelListAuthor.Text = languageDicoEn["Choose"];
          checkBoxAdddisplayAfterAdding.Text = languageDicoEn["DisplayAfterAdding"];
          break;
        case "French":
          checkBoxAdddisplayAfterAdding.Text = languageDicoFr["DisplayAfterAdding"];
          labelListAuthor.Text = languageDicoFr["Choose"];
          groupBoxListAuthor.Text = languageDicoFr["Author"];
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
          tabPageList.Text = languageDicoFr["ListAllQuotes"];
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
          buttonListDelete.Text = languageDicoFr["Delete"];
          break;
      }
    }

    private void ButtonAddClick(object sender, EventArgs e)
    {
      if (textBoxAddAuthor.Text.Trim() == string.Empty)
      {
        textBoxAddAuthor.Text = "unknown author";
      }

      if (textBoxAddQuote.Text == string.Empty)
      {
        DisplayMessageOk(GetTranslatedString("EmptyQuote"), GetTranslatedString("EmptyQuoteShort"), MessageBoxButtons.OK);
        return;
      }

      // check if the quote is not already in
      // TODO code

      AllQuotes.Add(new Quote(textBoxAddAuthor.Text, radioButtonAddLanguageEnglish.Checked ? "English" : "French", textBoxAddQuote.Text));
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

      // return result.Select(i => i.quoteValue).ToList();
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

      if (checkBoxSearchQuote.Checked && checkBoxSearchAuthor.Checked)
      {
        checkBoxSearchAll.Checked = true;
      }
    }

    private void checkBoxSearchQuote_CheckedChanged(object sender, EventArgs e)
    {
      searchQuote = checkBoxSearchQuote.Checked;
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

      if (checkBoxLanguageFrench.Checked && checkBoxLanguageEnglish.Checked)
      {
        checkBoxLanguageAll.Checked = true;
      }
    }

    private void checkBoxLanguageFrench_CheckedChanged(object sender, EventArgs e)
    {
      languageFrench = checkBoxLanguageFrench.Checked;
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
      listlanguageAll = checkBoxListAll.Checked;
      if (checkBoxListAll.Checked)
      {
        checkBoxListEnglish.Checked = true;
        checkBoxListFrench.Checked = true;
        DisplayAllQuotes();
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
        DisplayAllQuotes();
      }

      DisplayQuotes(checkBoxListEnglish.Checked, checkBoxListFrench.Checked);
    }

    private void DisplayQuotes(string author)
    {
      IEnumerable<Quote> result3;
      if (author.ToLower() == "all")
      {
        result3 = from node in AllQuotes.ToList()
                  select node;
      }
      else
      {
        result3 = from node in AllQuotes.ToList()
                  where node.Author == author
                  select node;
      }

      textBoxListQuotes.Text = string.Empty;
      foreach (var quote in result3)
      {
        textBoxListQuotes.Text += quote.Sentence + " - " + quote.Author + Environment.NewLine;
      }
    }

    private void DisplayQuotes(bool englishChecked, bool frenchChecked)
    {
      IEnumerable<Quote> result3 = from node in AllQuotes.ToList() select node;
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
        result3 = from node in AllQuotes.ToList().Where(n => n.Sentence == "")
                  select node;
      }

      textBoxListQuotes.Text = string.Empty;
      comboBoxListAuthor.Items.Clear();
      comboBoxListAuthor.Items.Add("All");
      foreach (var quote in result3)
      {
        textBoxListQuotes.Text += quote.Sentence + " - " + quote.Author + Environment.NewLine;
        if (!comboBoxListAuthor.Items.Contains(quote.Author))
        {
          comboBoxListAuthor.Items.Add(quote.Author);
        }
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

      AllQuotes.QuoteFileSaved = true;
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
        Filter = "Xml files (*.xml)|*.xml"
      };

      if (sfd.ShowDialog() != DialogResult.OK) return;
      if (File.Exists(sfd.FileName))
      {
        File.Delete(sfd.FileName);
      }

      SaveAllQuotesToXmlFile(sfd.FileName);
    }

    private void SaveAllQuotesToXmlFile(string fileName)
    {
      XmlDocument xmlDoc = new XmlDocument();
      XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
      XmlElement xmlRoot = xmlDoc.DocumentElement;
      xmlDoc.InsertBefore(xmlDeclaration, xmlRoot);
      XmlElement rootQuotes = xmlDoc.CreateElement(string.Empty, "Quotes", string.Empty);
      xmlDoc.AppendChild(rootQuotes);

      foreach (var item in AllQuotes.ToList())
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

      AllQuotes.QuoteFileSaved = true;
      EnableDisableMenu();
      lastSaveLocation = fileName;
    }

    private void buttonListDelete_Click(object sender, EventArgs e)
    {
      if (textBoxListQuotes.Text == string.Empty)
      {
        DisplayMessageOk(GetTranslatedString("EmptyText"), GetTranslatedString("EmptyTextShort"), MessageBoxButtons.OK);
        return;
      }

      if (textBoxListQuotes.SelectedText.Length <= 0)
      {
        DisplayMessageOk(GetTranslatedString("NoSelection"), GetTranslatedString("NoSelectionShort"), MessageBoxButtons.OK);
        return;
      }

      // delete the quote
      if (AllQuotes.Remove(SeparateQuote(textBoxListQuotes.SelectedText)[0], SeparateQuote(textBoxListQuotes.SelectedText)[1]))
      {
        DisplayMessageOk(GetTranslatedString("QuoteDeleted"), GetTranslatedString("QuoteDeletedShort"), MessageBoxButtons.OK);
        EnableDisableMenu();
        // refresh and reload list
        DisplayQuotes(checkBoxListEnglish.Checked, checkBoxListFrench.Checked);
      }
      else
      {
        DisplayMessageOk(GetTranslatedString("NoQuoteDeleted") +
          Environment.NewLine +
          GetTranslatedString("TheSentence") +
          Environment.NewLine +
          textBoxListQuotes.SelectedText +
          Environment.NewLine +
          GetTranslatedString("HasNotBeenfound"),
          GetTranslatedString("NoQuoteDeletedShort"), MessageBoxButtons.OK);
      }
    }

    public static string[] SeparateQuote(string wholeQuote)
    {
      string[] result = new string[2];
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
      DisplayQuotes(comboBoxListAuthor.SelectedItem.ToString());
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

    private void CutToClipboard(TextBox tb, string errorMessage = "nothing")
    {
      if (tb != ActiveControl) return;
      if (tb.Text == string.Empty)
      {
        DisplayMessageOk(GetTranslatedString("ThereIs") + Space +
          GetTranslatedString(errorMessage) + Space +
          GetTranslatedString("ToCut") + Space, GetTranslatedString(errorMessage),
          MessageBoxButtons.OK);
        return;
      }

      if (tb.SelectedText == string.Empty)
      {
        DisplayMessageOk(GetTranslatedString("NoTextHasBeenSelected"),
          GetTranslatedString(errorMessage), MessageBoxButtons.OK);
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
        DisplayMessageOk(GetTranslatedString("ThereIsNothingToCopy") + Space,
          GetTranslatedString(message), MessageBoxButtons.OK);
        return;
      }

      if (tb.SelectedText == string.Empty)
      {
        DisplayMessageOk(GetTranslatedString("NoTextHasBeenSelected"),
          GetTranslatedString(message), MessageBoxButtons.OK);
        return;
      }

      Clipboard.SetText(tb.SelectedText);
    }

    private void PasteFromClipboard(TextBoxBase tb)
    {
      if (tb != ActiveControl) return;
      var selectionIndex = tb.SelectionStart;
      tb.Text = tb.Text.Insert(selectionIndex, Clipboard.GetText());
      tb.SelectionStart = selectionIndex + Clipboard.GetText().Length;
    }

    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Control focusedControl = FindFocusedControl(tabControlMain);
      // if (focusedControl is TextBox)
      TextBox box = focusedControl as TextBox;
      if (box != null)
      {
        box.SelectAll();
      }
    }
  }
}