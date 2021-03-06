﻿using System;
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
using System.Diagnostics;

namespace OutilDevis
{
    public abstract class OuvrageWrapPanel : WrapPanel
    {

        protected Dictionary<string, float> priceList;

        public OuvrageWrapPanel(Dictionary<string, float> _priceList)
        {
            this.Orientation = Orientation.Horizontal;
            this.priceList = _priceList;
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
        Dictionary<string, float> priceList;

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

            // Load the price list
            string priceListFileName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "\\listePrix.txt");
            priceList = File.ReadAllLines(priceListFileName)
              .Select(l => l.Split(new[] { '=' }))
              .ToDictionary(s => s[0].Trim(), s => float.Parse(s[1].Trim()));
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
            _ = ouvrageComboBox.Items.Add("Ouverture");
            _ = ouvrageComboBox.Items.Add("Echafaudage");
            _ = ouvrageComboBox.Items.Add("Corps d'enduit");
            _ = ouvrageComboBox.Items.Add("Piquage des enduits existants");
            _ = ouvrageComboBox.Items.Add("Préparation des murs");
            _ = ouvrageComboBox.Items.Add("Sondage de la façade");
            _ = ouvrageComboBox.Items.Add("Renformis");
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

            LibreWrapPanel librePanel = new LibreWrapPanel(priceList);
            OuvertureWrapPanel ouverturePanel = new OuvertureWrapPanel(priceList);
            EchafaudageWrapPanel echafaudagePanel = new EchafaudageWrapPanel(priceList);
            CorpsDenduitWrapPanel corpsDenduitPanel = new CorpsDenduitWrapPanel(priceList);
            PiquageDesEnduitsExistantsWrapPanel piquageDesEnduitsExistantsPanel = new PiquageDesEnduitsExistantsWrapPanel(priceList);
            PreparationDesMursWrapPanel preparationMursPanel = new PreparationDesMursWrapPanel(priceList);
            SondageWrapPanel sondagePanel = new SondageWrapPanel(priceList);
            RenformisWrapPanel renformisPanel = new RenformisWrapPanel(priceList);
            FinitionWrapPanel finitionPanel = new FinitionWrapPanel(priceList);
            DecaissementWrapPanel decaissementPanel = new DecaissementWrapPanel(priceList);
            HerissonWrapPanel herissonPanel = new HerissonWrapPanel(priceList);
            LiegeWrapPanel liegePanel = new LiegeWrapPanel(priceList);
            DalleWrapPanel dallePanel = new DalleWrapPanel(priceList);

            WrapPanel currentPanel = null;

            ///////////////////////////////////////
            // When the selection changes in the ouvrage box, just set the correct options panel as child to line panel
            ouvrageComboBox.SelectionChanged += delegate
            {
                if (currentPanel != null) linePanel.Children.Remove(currentPanel);

                if (ouvrageComboBox.SelectedItem.ToString() == "Ouvrage libre") currentPanel = librePanel;
                if (ouvrageComboBox.SelectedItem.ToString() == "Ouverture") currentPanel = ouverturePanel;
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
            // Build output filename from client name and date
            string outputFileName = nomClientTextBox.Text;
            outputFileName = string.Concat(outputFileName, "_");
            outputFileName = string.Concat(outputFileName, DateTime.Today.ToString("yyyyMMdd"));
            outputFileName = string.Concat(outputFileName, ".xlsx");
            outputFileName = string.Concat("\\", outputFileName);
            outputFileName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), outputFileName);

            // Open the DevisVierge workbook
            string inputFileName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "\\DevisVierge.xlsx");
            var fi = new FileInfo(inputFileName);
            var p = new ExcelPackage(fi);

            //Get the only worksheet of the document
            var ws = p.Workbook.Worksheets["Feuille1"];

            // Write devis number and current date
            string devisNumberString = "Devis n° ";
            devisNumberString = string.Concat(devisNumberString, DateTime.Today.ToString("yyyyMMdd"));
            devisNumberString = string.Concat(devisNumberString, ", édité le ");
            devisNumberString = string.Concat(devisNumberString, DateTime.Today.ToString("dd/MM/yyyy"));
            devisNumberString = string.Concat(devisNumberString, ", valable 30 jours");
            ws.Cells[10, 1].Value = devisNumberString;

            // Initialize row counter
            int rowNumber = 20;

            // Set the cell values using row and column.
            foreach (DataRow row in table.Rows)
            {
                // Get fields by column index.
                string designation = row.Field<string>(0);
                int quantite = row.Field<int>(1);
                float prixUnitaire = row.Field<float>(2);

                // Fill in the xlsx document 
                ws.Cells[rowNumber, 1].Value = designation;
                ws.Cells[rowNumber, 5].Value = quantite;
                ws.Cells[rowNumber, 6].Value = prixUnitaire;

                // Increment row number
                rowNumber++;
            }

            //Set some properties of the Excel document
            p.Workbook.Properties.Author = "SARL Franck Charreton";
            p.Workbook.Properties.Title = "Devis";
            p.Workbook.Properties.Subject = "Devis subject";
            p.Workbook.Properties.Created = DateTime.Now;

            //Save your file
            FileInfo ofi = new FileInfo(outputFileName);
            p.SaveAs(ofi);
        }
    }
}
