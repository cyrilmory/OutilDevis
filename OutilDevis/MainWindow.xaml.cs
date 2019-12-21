using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace OutilDevis
{
    public abstract class OuvrageWrapPanel : WrapPanel
    {

        public OuvrageWrapPanel()
        {
            this.Orientation = Orientation.Horizontal;
        }

        public abstract decimal GetPrixUnitaire();
        public abstract string GetDesignation();

        public void addLabeledElementToPanel(Control element, Label label, string labelName)
        {
            label.Target = element;
            label.Content = labelName;
            label.Margin = new Thickness(5, 0, 0, 0);
            _ = this.Children.Add(label);
            _ = this.Children.Add(element);
        }
    }

    public class LibreWrapPanel : OuvrageWrapPanel
    {
        // Controls
        TextBox designationInput;
        IntegerUpDown prixUnitaireInput;
        IntegerUpDown quantiteInput;

        // Labels
        Label designationLabel;
        Label prixUnitaireLabel;
        Label quantiteLabel;

        public LibreWrapPanel()
        {
            // Initialize all controls and their labels
            designationInput  = new TextBox();
            prixUnitaireInput = new IntegerUpDown();
            quantiteInput = new IntegerUpDown();

            designationLabel = new Label();
            prixUnitaireLabel = new Label();
            quantiteLabel = new Label();

            // Add them as children to the panel
            addLabeledElementToPanel(designationInput, designationLabel, "Désignation");
            addLabeledElementToPanel(prixUnitaireInput, prixUnitaireLabel, "Prix unitaire");
            addLabeledElementToPanel(quantiteInput, quantiteLabel, "Quantité");
        }
        public override decimal GetPrixUnitaire()
        {
            return (Convert.ToDecimal(this.prixUnitaireInput.Value));
        }
        public override string GetDesignation()
        {
            return ("Libre");
        }
    }

    public class EnduitWrapPanel : OuvrageWrapPanel
    {
        // Controls
        IntegerUpDown surfaceInput;
        CheckListBox recetteInput;

        // Labels
        Label surfaceLabel;
        Label recetteLabel;

        public EnduitWrapPanel()
        {
            // Initialize all controls and their labels
            surfaceInput = new IntegerUpDown();
            recetteInput = new CheckListBox();
            
            surfaceLabel = new Label();
            recetteLabel = new Label();

            // Setup the controls that need it
            recetteInput.Items.Add("Chaux");
            recetteInput.Items.Add("Terre");
            recetteInput.Items.Add("Chanvre");

            // Add them as children to the panel
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
            addLabeledElementToPanel(recetteInput, recetteLabel, "Recette");
        }
        public override decimal GetPrixUnitaire()
        {
            return Convert.ToDecimal(33);
        }
        public override string GetDesignation()
        {
            return ("Enduit");
        }
    }

    public class OuvertureWrapPanel : OuvrageWrapPanel
    {
        // Controls
        ComboBox essenceInput;
        IntegerUpDown largeurInput;
        IntegerUpDown hauteurInput;
        CheckListBox optionsInput;

        // Labels
        Label essenceLabel;
        Label largeurLabel;
        Label hauteurLabel;
        Label optionsLabel;

        public OuvertureWrapPanel()
        {
            // Initialize all controls and their labels
            essenceInput = new ComboBox();
            largeurInput = new IntegerUpDown();
            hauteurInput = new IntegerUpDown();
            optionsInput = new CheckListBox();
            
            essenceLabel = new Label();
            largeurLabel = new Label();
            hauteurLabel = new Label();
            optionsLabel = new Label();

            // Setup the controls that need it
            essenceInput.Items.Add("Douglas");
            essenceInput.Items.Add("Chêne");

            optionsInput.Items.Add("Lindage");
            optionsInput.Items.Add("Appui bois");
            optionsInput.Items.Add("Appui briques");
            optionsInput.Items.Add("Plots béton");
            optionsInput.Items.Add("Echafaudage");

            // Add them as children to the panel
            addLabeledElementToPanel(essenceInput, essenceLabel, "Essence");
            addLabeledElementToPanel(largeurInput, largeurLabel, "Largeur");
            addLabeledElementToPanel(hauteurInput, hauteurLabel, "Hauteur");
            addLabeledElementToPanel(optionsInput, optionsLabel, "Options");
        }

        public override decimal GetPrixUnitaire()
        {
            return Convert.ToDecimal(1800);
        }
        public override string GetDesignation()
        {
            return ("Ouverture");
        }
    }

   


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

    // Declare the DataTable (storing values) and the DataGrid (displaying them)
    System.Data.DataTable table;
    DataGrid tableau;
    DataView DevisView;

    public MainWindow()
        {
            InitializeComponent();
            mainWrap.Orientation = Orientation.Vertical;

            // Initialize the data table
            table = new System.Data.DataTable("DevisTable");
            initializeDataTable();

            // Initialize the data grid
            tableau = new DataGrid();
            _ = mainWrap.Children.Add(tableau);
        }

    private void initializeDataTable()
        {
            // Declare variables for DataColumn and DataRow objects.
            System.Data.DataColumn column;

            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new System.Data.DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            column.ReadOnly = true;
            column.Unique = true;
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            // Create second column.
            column = new System.Data.DataColumn("Désignation", typeof(string));
            //column.DataType = System.Type.GetType("System.String");
            //column.ColumnName = "Désignation";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            // Create third column.
            column = new System.Data.DataColumn("Prix unitaire", typeof(decimal));
            //column.DataType = System.Type.GetType("System.Decimal");
            //column.ColumnName = "Prix unitaire";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Make the ID column the primary key column.
            System.Data.DataColumn[] PrimaryKeyColumns = new System.Data.DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns["ID"];
            table.PrimaryKey = PrimaryKeyColumns;
        }

    private void updateDataTable(WrapPanel panel) ///// TO BE CONTINUED
        {
            // Clear the existing rows from the data table
            table.Rows.Clear();

            // Walk through the UI to find the ouvrage panels
            System.Collections.IEnumerator rowEnumerator = panel.Children.GetEnumerator();
            // The first child is the DataGrid
            _ = rowEnumerator.MoveNext();

            // Declare tableRow, which will be used in the loop
            System.Data.DataRow tableRow;
            while (rowEnumerator.MoveNext())
            {
                WrapPanel currentRow = (WrapPanel)rowEnumerator.Current;
                System.Collections.IEnumerator rowElementsEnumerator = currentRow.Children.GetEnumerator();
                // Suprisingly, the first child is something invalid
                _ = rowElementsEnumerator.MoveNext();
                // The next child is the remove button
                _ = rowElementsEnumerator.MoveNext();
                // The next child is the combo box
                _ = rowElementsEnumerator.MoveNext();
                // The last child is the ouvrageWrapPanel
                OuvrageWrapPanel ouvrage = (OuvrageWrapPanel)rowElementsEnumerator.Current;

                tableRow = table.NewRow();
                tableRow[1] = ouvrage.GetDesignation(); ;
                tableRow[2] = ouvrage.GetPrixUnitaire();
                table.Rows.Add(tableRow);
            }

            DevisView = new DataView(table);
            tableau.ItemsSource = DevisView;

        }

            private void addLineButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a line panel that will contain the remove button and the ouvrage box
            var linePanel = new WrapPanel();
            linePanel.Orientation = Orientation.Horizontal;
            linePanel.Margin = new Thickness(0, 5, 0, 5);
            _ = mainWrap.Children.Add(linePanel);

            // Create the remove button and add it to new line panel
            var removeLineButton = new Button();
            removeLineButton.Content = "-";
            removeLineButton.Margin = new Thickness(5, 0, 5, 0);
            _ = linePanel.Children.Add(removeLineButton);

            // Create the ouvrage combo box and add it to the new line panel
            var ouvrageComboBox = new ComboBox();
            _ = ouvrageComboBox.Items.Add("Ouverture");
            _ = ouvrageComboBox.Items.Add("Enduit");
            _ = ouvrageComboBox.Items.Add("Libre");
            _ = linePanel.Children.Add(ouvrageComboBox);

            // When the remove button is clicked, remove the whole line
            removeLineButton.Click += delegate
            {
                mainWrap.Children.Remove(linePanel);
            };

            //////////////////////////////////////
            // Create all possible options panel
            //////////////////////////////////////

            // Ouverture
            OuvertureWrapPanel ouverturePanel = new OuvertureWrapPanel();

            // Enduit
            EnduitWrapPanel enduitPanel = new EnduitWrapPanel();

            // Libre
            LibreWrapPanel librePanel = new LibreWrapPanel();


            WrapPanel currentPanel = null;

            ///////////////////////////////////////
            // When the selection changes in the ouvrage box, just set the correct options panel as child to line panel
            ouvrageComboBox.SelectionChanged += delegate
            {
                if(currentPanel != null) linePanel.Children.Remove(currentPanel);

                if (ouvrageComboBox.SelectedItem.ToString() == "Ouverture") currentPanel = ouverturePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Enduit") currentPanel = enduitPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Libre") currentPanel = librePanel;

                linePanel.Children.Add(currentPanel);
            };

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.updateDataTable(mainWrap);
        }
    }
}
