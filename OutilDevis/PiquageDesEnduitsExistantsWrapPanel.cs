using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;

namespace OutilDevis
{
    class PiquageDesEnduitsExistantsWrapPanel : OuvrageWrapPanel
    {
        // Controls
        ComboBox existantInput;
        IntegerUpDown surfaceInput;

        // Labels
        Label existantLabel;
        Label surfaceLabel;

        public PiquageDesEnduitsExistantsWrapPanel(Dictionary<string, float> _priceList) : base(_priceList)
        {
            // Initialize all controls and their labels
            existantInput = new ComboBox();
            surfaceInput = new IntegerUpDown();

            existantLabel = new Label();
            surfaceLabel = new Label();

            // Setup the controls that need it
            existantInput.Items.Add("Chaux");
            existantInput.Items.Add("Ciment");
            existantInput.Items.Add("Ciment grillagé");

            // Setup defaults
            existantInput.SelectedItem = "Ciment";
            surfaceInput.Value = 0;

            // Add them as children to the panel
            addLabeledElementToPanel(existantInput, existantLabel, "Enduit existant");
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
        }
        public override Single GetPrixUnitaire()
        {
            if (existantInput.SelectedItem.ToString() == "Chaux") return (priceList["Charreton_PiquageEnduitChaux"]);
            if (existantInput.SelectedItem.ToString() == "Ciment") return (priceList["Charreton_PiquageEnduitCiment"]);
            if (existantInput.SelectedItem.ToString() == "Ciment grillagé") return (priceList["Charreton_PiquageEnduitCimentGrillage"]);
            return (0);
        }
        public override string GetDesignation()
        {
            string designation = "Piquage de l'enduit existant (";

            // Nature de l'enduit existant
            designation = string.Concat(designation, existantInput.SelectedItem.ToString());
            designation = string.Concat(designation, "), en m²");

            return (designation);
        }
        public override int GetQuantite()
        {
            return ((int)this.surfaceInput.Value);
        }

        // Assume 3cm of thickness
        public override Single GetVolumeGravats()
        {
            return (Convert.ToSingle(this.surfaceInput.Value * 0.03));
        }
    }
}
