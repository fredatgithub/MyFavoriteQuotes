﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      Ce code a été généré par un générateur de test codé de l'interface utilisateur.
//      Version : 14.0.0.0 
//
//      Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//      le code est régénéré.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace CodedUITestMyFavoriteQuotes
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public partial class UIMap
    {
        
        /// <summary>
        /// RecordedMethod1 - Utilisez 'RecordedMethod1Params' pour passer les paramètres dans cette méthode.
        /// </summary>
        public void RecordedMethod1()
        {
            #region Variable Declarations
            WinTabPage uIAjouterTabPage = this.UIMyfavoritequotesV100Window.UITabControlMainWindow.UIAjouterTabPage;
            WinEdit uITextBoxAddQuoteEdit = this.UIMyfavoritequotesV100Window.UITextBoxAddQuoteWindow.UITextBoxAddQuoteEdit;
            WinButton uIAjouterButton = this.UIMyfavoritequotesV100Window.UIAjouterWindow.UIAjouterButton;
            WinTabPage uIListertouteslescitatTabPage = this.UIMyfavoritequotesV100Window.UITabControlMainWindow.UIListertouteslescitatTabPage;
            WinCheckBox uIAnglaisCheckBox = this.UIMyfavoritequotesV100Window.UIAnglaisWindow.UIAnglaisCheckBox;
            WinEdit uITextBoxListQuotesEdit = this.UIMyfavoritequotesV100Window.UITextBoxListQuotesWindow.UITextBoxListQuotesEdit;
            WinWindow uITextBoxListQuotesWindow = this.UIMyfavoritequotesV100Window.UIListertouteslescitatClient.UITextBoxListQuotesWindow;
            WinButton uISupprimerButton = this.UIMyfavoritequotesV100Window.UISupprimerWindow.UISupprimerButton;
            WinButton uIOKButton = this.UICitationSuppriméeWindow.UIOKWindow.UIOKButton;
            WinMenuItem uIQuitterMenuItem = this.UIMyfavoritequotesV100Window.UIMenuStrip1MenuBar.UIFichierMenuItem.UIQuitterMenuItem;
            WinButton uINonButton = this.UIUneouplusieurscitatiWindow.UINonWindow.UINonButton;
            #endregion

            // Clic 'Ajouter' onglet
            Mouse.Click(uIAjouterTabPage, new Point(21, 10));

            // Taper 'test' dans 'textBoxAddQuote' zone de texte
            uITextBoxAddQuoteEdit.Text = this.RecordedMethod1Params.UITextBoxAddQuoteEditText;

            // Clic 'Ajouter' bouton
            Mouse.Click(uIAjouterButton, new Point(37, 9));

            // Clic 'Lister toutes les citations' onglet
            Mouse.Click(uIListertouteslescitatTabPage, new Point(65, 13));

            // Sélectionner 'Anglais' case à cocher
            uIAnglaisCheckBox.Checked = this.RecordedMethod1Params.UIAnglaisCheckBoxChecked;

            // Déplacer 'textBoxListQuotes' zone de texte vers 'textBoxListQuotes' fenêtre
            uITextBoxListQuotesWindow.EnsureClickable(new Point(0, 62));
            Mouse.StartDragging(uITextBoxListQuotesEdit, new Point(129, 60));
            Mouse.StopDragging(uITextBoxListQuotesWindow, new Point(0, 62));

            // Clic 'Supprimer' bouton
            Mouse.Click(uISupprimerButton, new Point(59, 12));

            // Clic 'OK' bouton
            Mouse.Click(uIOKButton, new Point(34, 19));

            // Sélectionner 'Anglais' case à cocher
            uIAnglaisCheckBox.Checked = this.RecordedMethod1Params.UIAnglaisCheckBoxChecked1;

            // Clic 'Fichier' -> 'Quitter' élément de menu
            Mouse.Click(uIQuitterMenuItem, new Point(89, 8));

            // Clic '&Non' bouton
            Mouse.Click(uINonButton, new Point(30, 16));
        }
        
        #region Properties
        public virtual RecordedMethod1Params RecordedMethod1Params
        {
            get
            {
                if ((this.mRecordedMethod1Params == null))
                {
                    this.mRecordedMethod1Params = new RecordedMethod1Params();
                }
                return this.mRecordedMethod1Params;
            }
        }
        
        public UIMyfavoritequotesV100Window UIMyfavoritequotesV100Window
        {
            get
            {
                if ((this.mUIMyfavoritequotesV100Window == null))
                {
                    this.mUIMyfavoritequotesV100Window = new UIMyfavoritequotesV100Window();
                }
                return this.mUIMyfavoritequotesV100Window;
            }
        }
        
        public UICitationSuppriméeWindow UICitationSuppriméeWindow
        {
            get
            {
                if ((this.mUICitationSuppriméeWindow == null))
                {
                    this.mUICitationSuppriméeWindow = new UICitationSuppriméeWindow();
                }
                return this.mUICitationSuppriméeWindow;
            }
        }
        
        public UIUneouplusieurscitatiWindow UIUneouplusieurscitatiWindow
        {
            get
            {
                if ((this.mUIUneouplusieurscitatiWindow == null))
                {
                    this.mUIUneouplusieurscitatiWindow = new UIUneouplusieurscitatiWindow();
                }
                return this.mUIUneouplusieurscitatiWindow;
            }
        }
        #endregion
        
        #region Fields
        private RecordedMethod1Params mRecordedMethod1Params;
        
        private UIMyfavoritequotesV100Window mUIMyfavoritequotesV100Window;
        
        private UICitationSuppriméeWindow mUICitationSuppriméeWindow;
        
        private UIUneouplusieurscitatiWindow mUIUneouplusieurscitatiWindow;
        #endregion
    }
    
    /// <summary>
    /// Paramètres à passer dans 'RecordedMethod1'
    /// </summary>
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class RecordedMethod1Params
    {
        
        #region Fields
        /// <summary>
        /// Taper 'test' dans 'textBoxAddQuote' zone de texte
        /// </summary>
        public string UITextBoxAddQuoteEditText = "test";
        
        /// <summary>
        /// Sélectionner 'Anglais' case à cocher
        /// </summary>
        public bool UIAnglaisCheckBoxChecked = true;
        
        /// <summary>
        /// Sélectionner 'Anglais' case à cocher
        /// </summary>
        public bool UIAnglaisCheckBoxChecked1 = true;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UIMyfavoritequotesV100Window : WinWindow
    {
        
        public UIMyfavoritequotesV100Window()
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.Name] = "My favorite quotes V1.0.0.0";
            this.SearchProperties.Add(new PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.Window", PropertyExpressionOperator.Contains));
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public UITabControlMainWindow UITabControlMainWindow
        {
            get
            {
                if ((this.mUITabControlMainWindow == null))
                {
                    this.mUITabControlMainWindow = new UITabControlMainWindow(this);
                }
                return this.mUITabControlMainWindow;
            }
        }
        
        public UITextBoxAddQuoteWindow UITextBoxAddQuoteWindow
        {
            get
            {
                if ((this.mUITextBoxAddQuoteWindow == null))
                {
                    this.mUITextBoxAddQuoteWindow = new UITextBoxAddQuoteWindow(this);
                }
                return this.mUITextBoxAddQuoteWindow;
            }
        }
        
        public UIAjouterWindow UIAjouterWindow
        {
            get
            {
                if ((this.mUIAjouterWindow == null))
                {
                    this.mUIAjouterWindow = new UIAjouterWindow(this);
                }
                return this.mUIAjouterWindow;
            }
        }
        
        public UIAnglaisWindow UIAnglaisWindow
        {
            get
            {
                if ((this.mUIAnglaisWindow == null))
                {
                    this.mUIAnglaisWindow = new UIAnglaisWindow(this);
                }
                return this.mUIAnglaisWindow;
            }
        }
        
        public UITextBoxListQuotesWindow UITextBoxListQuotesWindow
        {
            get
            {
                if ((this.mUITextBoxListQuotesWindow == null))
                {
                    this.mUITextBoxListQuotesWindow = new UITextBoxListQuotesWindow(this);
                }
                return this.mUITextBoxListQuotesWindow;
            }
        }
        
        public UIListertouteslescitatClient UIListertouteslescitatClient
        {
            get
            {
                if ((this.mUIListertouteslescitatClient == null))
                {
                    this.mUIListertouteslescitatClient = new UIListertouteslescitatClient(this);
                }
                return this.mUIListertouteslescitatClient;
            }
        }
        
        public UISupprimerWindow UISupprimerWindow
        {
            get
            {
                if ((this.mUISupprimerWindow == null))
                {
                    this.mUISupprimerWindow = new UISupprimerWindow(this);
                }
                return this.mUISupprimerWindow;
            }
        }
        
        public UIMenuStrip1MenuBar UIMenuStrip1MenuBar
        {
            get
            {
                if ((this.mUIMenuStrip1MenuBar == null))
                {
                    this.mUIMenuStrip1MenuBar = new UIMenuStrip1MenuBar(this);
                }
                return this.mUIMenuStrip1MenuBar;
            }
        }
        #endregion
        
        #region Fields
        private UITabControlMainWindow mUITabControlMainWindow;
        
        private UITextBoxAddQuoteWindow mUITextBoxAddQuoteWindow;
        
        private UIAjouterWindow mUIAjouterWindow;
        
        private UIAnglaisWindow mUIAnglaisWindow;
        
        private UITextBoxListQuotesWindow mUITextBoxListQuotesWindow;
        
        private UIListertouteslescitatClient mUIListertouteslescitatClient;
        
        private UISupprimerWindow mUISupprimerWindow;
        
        private UIMenuStrip1MenuBar mUIMenuStrip1MenuBar;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UITabControlMainWindow : WinWindow
    {
        
        public UITabControlMainWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "tabControlMain";
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public WinTabPage UIAjouterTabPage
        {
            get
            {
                if ((this.mUIAjouterTabPage == null))
                {
                    this.mUIAjouterTabPage = new WinTabPage(this);
                    #region Critères de recherche
                    this.mUIAjouterTabPage.SearchProperties[WinTabPage.PropertyNames.Name] = "Ajouter";
                    this.mUIAjouterTabPage.WindowTitles.Add("My favorite quotes V1.0.0.0");
                    #endregion
                }
                return this.mUIAjouterTabPage;
            }
        }
        
        public WinTabPage UIListertouteslescitatTabPage
        {
            get
            {
                if ((this.mUIListertouteslescitatTabPage == null))
                {
                    this.mUIListertouteslescitatTabPage = new WinTabPage(this);
                    #region Critères de recherche
                    this.mUIListertouteslescitatTabPage.SearchProperties[WinTabPage.PropertyNames.Name] = "Lister toutes les citations";
                    this.mUIListertouteslescitatTabPage.WindowTitles.Add("My favorite quotes V1.0.0.0");
                    #endregion
                }
                return this.mUIListertouteslescitatTabPage;
            }
        }
        #endregion
        
        #region Fields
        private WinTabPage mUIAjouterTabPage;
        
        private WinTabPage mUIListertouteslescitatTabPage;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UITextBoxAddQuoteWindow : WinWindow
    {
        
        public UITextBoxAddQuoteWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "textBoxAddQuote";
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public WinEdit UITextBoxAddQuoteEdit
        {
            get
            {
                if ((this.mUITextBoxAddQuoteEdit == null))
                {
                    this.mUITextBoxAddQuoteEdit = new WinEdit(this);
                    #region Critères de recherche
                    this.mUITextBoxAddQuoteEdit.SearchProperties[WinEdit.PropertyNames.Name] = "Auteur";
                    this.mUITextBoxAddQuoteEdit.WindowTitles.Add("My favorite quotes V1.0.0.0");
                    #endregion
                }
                return this.mUITextBoxAddQuoteEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUITextBoxAddQuoteEdit;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UIAjouterWindow : WinWindow
    {
        
        public UIAjouterWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "buttonAdd";
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public WinButton UIAjouterButton
        {
            get
            {
                if ((this.mUIAjouterButton == null))
                {
                    this.mUIAjouterButton = new WinButton(this);
                    #region Critères de recherche
                    this.mUIAjouterButton.SearchProperties[WinButton.PropertyNames.Name] = "Ajouter";
                    this.mUIAjouterButton.WindowTitles.Add("My favorite quotes V1.0.0.0");
                    #endregion
                }
                return this.mUIAjouterButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUIAjouterButton;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UIAnglaisWindow : WinWindow
    {
        
        public UIAnglaisWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "checkBoxListEnglish";
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public WinCheckBox UIAnglaisCheckBox
        {
            get
            {
                if ((this.mUIAnglaisCheckBox == null))
                {
                    this.mUIAnglaisCheckBox = new WinCheckBox(this);
                    #region Critères de recherche
                    this.mUIAnglaisCheckBox.SearchProperties[WinCheckBox.PropertyNames.Name] = "Anglais";
                    this.mUIAnglaisCheckBox.WindowTitles.Add("My favorite quotes V1.0.0.0");
                    #endregion
                }
                return this.mUIAnglaisCheckBox;
            }
        }
        #endregion
        
        #region Fields
        private WinCheckBox mUIAnglaisCheckBox;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UITextBoxListQuotesWindow : WinWindow
    {
        
        public UITextBoxListQuotesWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "textBoxListQuotes";
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public WinEdit UITextBoxListQuotesEdit
        {
            get
            {
                if ((this.mUITextBoxListQuotesEdit == null))
                {
                    this.mUITextBoxListQuotesEdit = new WinEdit(this);
                    #region Critères de recherche
                    this.mUITextBoxListQuotesEdit.SearchProperties[WinEdit.PropertyNames.Name] = "Langage";
                    this.mUITextBoxListQuotesEdit.WindowTitles.Add("My favorite quotes V1.0.0.0");
                    #endregion
                }
                return this.mUITextBoxListQuotesEdit;
            }
        }
        #endregion
        
        #region Fields
        private WinEdit mUITextBoxListQuotesEdit;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UIListertouteslescitatClient : WinClient
    {
        
        public UIListertouteslescitatClient(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinControl.PropertyNames.Name] = "Lister toutes les citations";
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public WinWindow UITextBoxListQuotesWindow
        {
            get
            {
                if ((this.mUITextBoxListQuotesWindow == null))
                {
                    this.mUITextBoxListQuotesWindow = new WinWindow(this);
                    #region Critères de recherche
                    this.mUITextBoxListQuotesWindow.SearchProperties[WinWindow.PropertyNames.AccessibleName] = "Langage";
                    this.mUITextBoxListQuotesWindow.SearchProperties.Add(new PropertyExpression(WinWindow.PropertyNames.ClassName, "WindowsForms10.EDIT", PropertyExpressionOperator.Contains));
                    this.mUITextBoxListQuotesWindow.WindowTitles.Add("My favorite quotes V1.0.0.0");
                    #endregion
                }
                return this.mUITextBoxListQuotesWindow;
            }
        }
        #endregion
        
        #region Fields
        private WinWindow mUITextBoxListQuotesWindow;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UISupprimerWindow : WinWindow
    {
        
        public UISupprimerWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.ControlName] = "buttonListDelete";
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public WinButton UISupprimerButton
        {
            get
            {
                if ((this.mUISupprimerButton == null))
                {
                    this.mUISupprimerButton = new WinButton(this);
                    #region Critères de recherche
                    this.mUISupprimerButton.SearchProperties[WinButton.PropertyNames.Name] = "Supprimer";
                    this.mUISupprimerButton.WindowTitles.Add("My favorite quotes V1.0.0.0");
                    #endregion
                }
                return this.mUISupprimerButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUISupprimerButton;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UIMenuStrip1MenuBar : WinMenuBar
    {
        
        public UIMenuStrip1MenuBar(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinMenu.PropertyNames.Name] = "menuStrip1";
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public UIFichierMenuItem UIFichierMenuItem
        {
            get
            {
                if ((this.mUIFichierMenuItem == null))
                {
                    this.mUIFichierMenuItem = new UIFichierMenuItem(this);
                }
                return this.mUIFichierMenuItem;
            }
        }
        #endregion
        
        #region Fields
        private UIFichierMenuItem mUIFichierMenuItem;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UIFichierMenuItem : WinMenuItem
    {
        
        public UIFichierMenuItem(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinMenuItem.PropertyNames.Name] = "Fichier";
            this.WindowTitles.Add("My favorite quotes V1.0.0.0");
            #endregion
        }
        
        #region Properties
        public WinMenuItem UIQuitterMenuItem
        {
            get
            {
                if ((this.mUIQuitterMenuItem == null))
                {
                    this.mUIQuitterMenuItem = new WinMenuItem(this);
                    #region Critères de recherche
                    this.mUIQuitterMenuItem.SearchProperties[WinMenuItem.PropertyNames.Name] = "Quitter";
                    this.mUIQuitterMenuItem.SearchConfigurations.Add(SearchConfiguration.ExpandWhileSearching);
                    this.mUIQuitterMenuItem.WindowTitles.Add("My favorite quotes V1.0.0.0");
                    #endregion
                }
                return this.mUIQuitterMenuItem;
            }
        }
        #endregion
        
        #region Fields
        private WinMenuItem mUIQuitterMenuItem;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UICitationSuppriméeWindow : WinWindow
    {
        
        public UICitationSuppriméeWindow()
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.Name] = "Citation Supprimée";
            this.SearchProperties[WinWindow.PropertyNames.ClassName] = "#32770";
            this.WindowTitles.Add("Citation Supprimée");
            #endregion
        }
        
        #region Properties
        public UIOKWindow UIOKWindow
        {
            get
            {
                if ((this.mUIOKWindow == null))
                {
                    this.mUIOKWindow = new UIOKWindow(this);
                }
                return this.mUIOKWindow;
            }
        }
        #endregion
        
        #region Fields
        private UIOKWindow mUIOKWindow;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UIOKWindow : WinWindow
    {
        
        public UIOKWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.ControlId] = "2";
            this.WindowTitles.Add("Citation Supprimée");
            #endregion
        }
        
        #region Properties
        public WinButton UIOKButton
        {
            get
            {
                if ((this.mUIOKButton == null))
                {
                    this.mUIOKButton = new WinButton(this);
                    #region Critères de recherche
                    this.mUIOKButton.SearchProperties[WinButton.PropertyNames.Name] = "OK";
                    this.mUIOKButton.WindowTitles.Add("Citation Supprimée");
                    #endregion
                }
                return this.mUIOKButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUIOKButton;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UIUneouplusieurscitatiWindow : WinWindow
    {
        
        public UIUneouplusieurscitatiWindow()
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.Name] = "Une ou plusieurs citations ont été ajoutés sans être sauvegardées. Voulez-vous le" +
                "s enregistrer mainteant ?";
            this.SearchProperties[WinWindow.PropertyNames.ClassName] = "#32770";
            this.WindowTitles.Add("Une ou plusieurs citations ont été ajoutés sans être sauvegardées. Voulez-vous le" +
                    "s enregistrer mainteant ?");
            #endregion
        }
        
        #region Properties
        public UINonWindow UINonWindow
        {
            get
            {
                if ((this.mUINonWindow == null))
                {
                    this.mUINonWindow = new UINonWindow(this);
                }
                return this.mUINonWindow;
            }
        }
        #endregion
        
        #region Fields
        private UINonWindow mUINonWindow;
        #endregion
    }
    
    [GeneratedCode("Générateur de test codé de l\'interface utilisateur", "14.0.22823.1")]
    public class UINonWindow : WinWindow
    {
        
        public UINonWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Critères de recherche
            this.SearchProperties[WinWindow.PropertyNames.ControlId] = "7";
            this.WindowTitles.Add("Une ou plusieurs citations ont été ajoutés sans être sauvegardées. Voulez-vous le" +
                    "s enregistrer mainteant ?");
            #endregion
        }
        
        #region Properties
        public WinButton UINonButton
        {
            get
            {
                if ((this.mUINonButton == null))
                {
                    this.mUINonButton = new WinButton(this);
                    #region Critères de recherche
                    this.mUINonButton.SearchProperties[WinButton.PropertyNames.Name] = "Non";
                    this.mUINonButton.WindowTitles.Add("Une ou plusieurs citations ont été ajoutés sans être sauvegardées. Voulez-vous le" +
                            "s enregistrer mainteant ?");
                    #endregion
                }
                return this.mUINonButton;
            }
        }
        #endregion
        
        #region Fields
        private WinButton mUINonButton;
        #endregion
    }
}
