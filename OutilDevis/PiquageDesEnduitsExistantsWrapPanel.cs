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

        public PiquageDesEnduitsExistantsWrapPanel()
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

            // Add them as children to the panel
            addLabeledElementToPanel(existantInput, existantLabel, "Enduit existant");
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
        }
        public override decimal GetPrixUnitaire()
        {
            if (existantInput.SelectedItem.ToString() == "Chaux") return (Convert.ToDecimal(15));
            if (existantInput.SelectedItem.ToString() == "Ciment") return (Convert.ToDecimal(30));
            if (existantInput.SelectedItem.ToString() == "Ciment grillagé") return (Convert.ToDecimal(35));
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
        public override decimal GetVolumeGravats()
        {
            return (Convert.ToDecimal(this.surfaceInput.Value * 0.03));
        }
    }
}
