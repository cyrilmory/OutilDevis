using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;

namespace OutilDevis
{
    class PlancherWrapPanel : OuvrageWrapPanel
    {
        // Controls
        IntegerUpDown surfaceInput;
        CheckListBox optionsInput;

        // Options
        bool Porteuse;

        // Labels
        Label surfaceLabel;
        Label optionsLabel;

        public PlancherWrapPanel(Dictionary<string, float> _priceList) : base(_priceList)
        {
            // Initialize all controls and their labels
            surfaceInput = new IntegerUpDown();
            optionsInput = new CheckListBox();

            surfaceLabel = new Label();
            optionsLabel = new Label();

            // Set the controls that need it
            optionsInput.Items.Add("Porteuse en supplément des muraillères");

            // Set defaults
            surfaceInput.Value = 0;

            // Add them as children to the panel
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
            addLabeledElementToPanel(optionsInput, optionsLabel, "Porteuse");
        }
        void retrieveOptions()
        {
            Porteuse = optionsInput.SelectedValue.Contains("Porteuse en supplément des muraillères");
        }
        public override Single GetPrixUnitaire()
        {
            retrieveOptions();
            if (Porteuse)
                return (priceList["Charreton_Plancher_Porteuse"]);
            else
                return (priceList["Charreton_Plancher"]);
        }
        public override string GetDesignation()
        {
            string designation = "Plancher OSB 22mm nu sur solivage, muraillères";
            if (Porteuse)
                designation = string.Concat(designation, " et porteuse");
            designation = string.Concat(designation, ", en m²");
            return (designation);
        }
        public override Single GetQuantite()
        {
            return ((Single)surfaceInput.Value);
        }
    }
}
