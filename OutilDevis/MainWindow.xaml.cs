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
using OfficeOpenXml;

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
            column = new System.Data.DataColumn("Quantité", typeof(int));
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Create third column.
            column = new System.Data.DataColumn("Prix unitaire", typeof(Single));
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
                ComboBox ouvrageBox = (ComboBox)rowElementsEnumerator.Current;
                if (ouvrageBox.SelectedItem != null)
                    {
                    // The next child is the combo box
                    _ = rowElementsEnumerator.MoveNext();
                    // The last child is the ouvrageWrapPanel
                    OuvrageWrapPanel ouvrage = (OuvrageWrapPanel)rowElementsEnumerator.Current;

                    tableRow = table.NewRow();
                    tableRow[0] = ouvrage.GetDesignation();
                    tableRow[1] = ouvrage.GetQuantite();
                    tableRow[2] = ouvrage.GetPrixUnitaire();
                    tableRow[3] = ouvrage.GetPrixUnitaire() * ouvrage.GetQuantite();
                    table.Rows.Add(tableRow);

                    // Cumul de la quantité de gravats
                    volumeGravats += ouvrage.GetVolumeGravats();
                    }
            }

            // Dernière ligne : évacuation des gravats
            tableRow = table.NewRow();
            Single prixUnitaireEvacuationGravats = 150;
            tableRow[0] = "Evacuation des gravats";
            tableRow[1] = (int)volumeGravats;
            tableRow[2] = prixUnitaireEvacuationGravats;
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
            _ = ouvrageComboBox.Items.Add("Ouvrage libre");
            _ = ouvrageComboBox.Items.Add("Echafaudage");
            _ = ouvrageComboBox.Items.Add("Ouverture");
            _ = ouvrageComboBox.Items.Add("Sondage de la façade");
            _ = ouvrageComboBox.Items.Add("Préparation des murs");
            _ = ouvrageComboBox.Items.Add("Piquage des enduits existants");
            _ = ouvrageComboBox.Items.Add("Renformis");
            _ = ouvrageComboBox.Items.Add("Corps d'enduit");
            _ = ouvrageComboBox.Items.Add("Enduit de finition");
            _ = ouvrageComboBox.Items.Add("Décaissement");
            _ = ouvrageComboBox.Items.Add("Hérisson ventilé");
            _ = ouvrageComboBox.Items.Add("Isolation liège");
            _ = ouvrageComboBox.Items.Add("Dalle chaux");

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

            LibreWrapPanel librePanel = new LibreWrapPanel();
            OuvertureWrapPanel ouverturePanel = new OuvertureWrapPanel();
            EchafaudageWrapPanel echafaudagePanel = new EchafaudageWrapPanel();
            CorpsDenduitWrapPanel corpsDenduitPanel = new CorpsDenduitWrapPanel();
            PiquageDesEnduitsExistantsWrapPanel piquageDesEnduitsExistantsPanel = new PiquageDesEnduitsExistantsWrapPanel();
            PreparationDesMursWrapPanel preparationMursPanel = new PreparationDesMursWrapPanel();
            SondageWrapPanel sondagePanel = new SondageWrapPanel();
            RenformisWrapPanel renformisPanel = new RenformisWrapPanel();
            FinitionWrapPanel finitionPanel = new FinitionWrapPanel();
            DecaissementWrapPanel decaissementPanel = new DecaissementWrapPanel();
            HerissonWrapPanel herissonPanel = new HerissonWrapPanel();
            LiegeWrapPanel liegePanel = new LiegeWrapPanel();
            DalleWrapPanel dallePanel = new DalleWrapPanel();

            WrapPanel currentPanel = null;

            ///////////////////////////////////////
            // When the selection changes in the ouvrage box, just set the correct options panel as child to line panel
            ouvrageComboBox.SelectionChanged += delegate
            {
                if (currentPanel != null) linePanel.Children.Remove(currentPanel);

                if (ouvrageComboBox.SelectedItem.ToString() == "Ouverture") currentPanel = ouverturePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Ouvrage libre") currentPanel = librePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Echafaudage") currentPanel = echafaudagePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Corps d'enduit") currentPanel = corpsDenduitPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Piquage des enduits existants") currentPanel = piquageDesEnduitsExistantsPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Préparation des murs") currentPanel = preparationMursPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Sondage de la façade") currentPanel = sondagePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Renformis") currentPanel = renformisPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Enduit de finition") currentPanel = finitionPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Décaissement") currentPanel = decaissementPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Hérisson ventilé") currentPanel = herissonPanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Isolation liège") currentPanel = liegePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Dalle chaux") currentPanel = dallePanel;

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
            //outputFileName = string.Concat(outputFileName, ".csv");
            outputFileName = string.Concat(outputFileName, ".xlsx");
            outputFileName = string.Concat("\\", outputFileName);
            outputFileName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), outputFileName);

            // Write
            //File.WriteAllText(outputFileName, sb.ToString());

            //Create a new ExcelPackage
            ExcelPackage excelPackage = new ExcelPackage();

            //Set some properties of the Excel document
            excelPackage.Workbook.Properties.Author = "SARL Franck Charreton";
            excelPackage.Workbook.Properties.Title = "Devis";
            excelPackage.Workbook.Properties.Subject = "Devis subject";
            excelPackage.Workbook.Properties.Created = DateTime.Now;

            //Create the WorkSheet
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

            //Add some text to cell A1
            worksheet.Cells["A1"].Value = "My first EPPlus spreadsheet!";
            //You could also use [line, column] notation:
            worksheet.Cells[1, 2].Value = "This is cell B1!";

            //Save your file
            FileInfo fi = new FileInfo(outputFileName);
            excelPackage.SaveAs(fi);
        }
    }
}
