using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
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

        public abstract Single GetPrixUnitaire();
        public abstract string GetDesignation();
        public abstract int GetQuantite();

        public virtual Single GetVolumeGravats() { return 0; }

        public void addLabeledElementToPanel(Control element, Label label, string labelName)
        {
            label.Target = element;
            label.Content = labelName;
            label.Margin = new Thickness(5, 0, 0, 0);
            _ = this.Children.Add(label);
            _ = this.Children.Add(element);
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
        public override Single GetPrixUnitaire()
        {
            return Convert.ToSingle(33);
        }
        public override string GetDesignation()
        {
            return ("Enduit");
        }
        public override int GetQuantite()
        {
            return ((int)surfaceInput.Value);
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

            // For each column we want, create new DataColumn, set DataType, 
            // ColumnName and add to DataTable
            column = new System.Data.DataColumn();

            // Create first column.
            column = new System.Data.DataColumn("Désignation", typeof(string));
            column.ReadOnly = false;
            column.Unique = false;
            // Add the Column to the table.
            table.Columns.Add(column);

            // Create second column.
            column = new System.Data.DataColumn("Prix unitaire", typeof(Single));
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Create third column.
            column = new System.Data.DataColumn("Quantité", typeof(int));
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Create fourth column.
            column = new System.Data.DataColumn("Prix", typeof(Single));
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);
        }

    private void updateDataTable(WrapPanel panel)
        {
            // Clear the existing rows from the data table
            table.Rows.Clear();

            // Walk through the UI to find the ouvrage panels
            System.Collections.IEnumerator rowEnumerator = panel.Children.GetEnumerator();
            // The first child is the DataGrid
            _ = rowEnumerator.MoveNext();

            // Declare tableRow, which will be used in the loop
            System.Data.DataRow tableRow;
            Single volumeGravats = 0;
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
                tableRow[0] = ouvrage.GetDesignation();
                tableRow[1] = ouvrage.GetPrixUnitaire();
                tableRow[2] = ouvrage.GetQuantite();
                tableRow[3] = ouvrage.GetPrixUnitaire() * ouvrage.GetQuantite();
                table.Rows.Add(tableRow);

                // Cumul de la quantité de gravats
                volumeGravats += ouvrage.GetVolumeGravats();
            }

            // Dernière ligne : évacuation des gravats
            tableRow = table.NewRow();
            Single prixUnitaireEvacuationGravats = 150;
            tableRow[0] = "Evacuation des gravats";
            tableRow[1] = prixUnitaireEvacuationGravats;
            tableRow[2] = (int)volumeGravats;
            tableRow[3] = prixUnitaireEvacuationGravats * (int)volumeGravats;
            table.Rows.Add(tableRow);

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
            removeLineButton.MaxHeight = 25;
            removeLineButton.MinWidth = 25;
            _ = linePanel.Children.Add(removeLineButton);

            // Create the ouvrage combo box and add it to the new line panel
            var ouvrageComboBox = new ComboBox();
            _ = ouvrageComboBox.Items.Add("Ouverture");
            _ = ouvrageComboBox.Items.Add("Enduit");
            _ = ouvrageComboBox.Items.Add("Libre");
            _ = ouvrageComboBox.Items.Add("Echafaudage");
            _ = ouvrageComboBox.Items.Add("Corps d'enduit");
            _ = ouvrageComboBox.Items.Add("Piquage des enduits existants");
            ouvrageComboBox.MaxHeight = 25;
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

            // Echafaudage
            EchafaudageWrapPanel echafaudagePanel = new EchafaudageWrapPanel();

            // Corps d'enduit
            CorpsDenduitWrapPanel corpsDenduitPanel = new CorpsDenduitWrapPanel();

            // Corps d'enduit
            PiquageDesEnduitsExistantsWrapPanel piquageDesEnduitsExistantsPanel = new PiquageDesEnduitsExistantsWrapPanel();

            WrapPanel currentPanel = null;

            ///////////////////////////////////////
            // When the selection changes in the ouvrage box, just set the correct options panel as child to line panel
            ouvrageComboBox.SelectionChanged += delegate
            {
                if(currentPanel != null) linePanel.Children.Remove(currentPanel);

                if (ouvrageComboBox.SelectedItem.ToString() == "Ouverture") currentPanel = ouverturePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Enduit") currentPanel = enduitPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Libre") currentPanel = librePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Echafaudage") currentPanel = echafaudagePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Corps d'enduit") currentPanel = corpsDenduitPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Piquage des enduits existants") currentPanel = piquageDesEnduitsExistantsPanel;

                linePanel.Children.Add(currentPanel);
            };

        }

        private void displayDevisButton_Click(object sender, RoutedEventArgs e)
        {
            this.updateDataTable(mainWrap);
        }

        private void exportDevisButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in table.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(";", fields));
            }

            // Build filename from client name and date
            string outputFileName = nomClientTextBox.Text;
            outputFileName = string.Concat(outputFileName, "_");
            outputFileName = string.Concat(outputFileName, DateTime.Today.ToString("yyyyMMdd"));
            outputFileName = string.Concat(outputFileName, ".csv");
            outputFileName = string.Concat("\\", outputFileName);
            outputFileName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), outputFileName);

            // Write
            File.WriteAllText(outputFileName, sb.ToString());
        }
    }
}
