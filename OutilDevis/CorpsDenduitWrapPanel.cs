using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;

namespace OutilDevis
{
    class CorpsDenduitWrapPanel : OuvrageWrapPanel
    {
        // Controls
        ComboBox recetteInput;
        IntegerUpDown surfaceInput;

        // Labels
        Label recetteLabel;
        Label surfaceLabel;

        public CorpsDenduitWrapPanel()
        {
            // Initialize all controls and their labels
            recetteInput = new ComboBox();
            surfaceInput = new IntegerUpDown();

            recetteLabel = new Label();
            surfaceLabel = new Label();

            // Setup the controls that need it
            recetteInput.Items.Add("Chaux-sable");
            recetteInput.Items.Add("Chaux-chanvre Alliance4");

            // Add them as children to the panel
            addLabeledElementToPanel(recetteInput, recetteLabel, "Recette");
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
        }
        public override decimal GetPrixUnitaire()
        {
            if (recetteInput.SelectedItem.ToString() == "Chaux-sable") return (Convert.ToDecimal(38));
            if (recetteInput.SelectedItem.ToString() == "Chaux-chanvre Alliance4") return (Convert.ToDecimal(42));
            return (0);
        }
        public override string GetDesignation()
        {
            if (recetteInput.SelectedItem.ToString() == "Chaux-sable") return ("Corps d’enduit chaux NHL2 - sable 0/4, en m²");
            if (recetteInput.SelectedItem.ToString() == "Chaux-chanvre Alliance4") return ("Corps d’enduit chaux - chanvre, épaisseur 3cm, en m²");
            return ("");
        }
        public override int GetQuantite()
        {
            return ((int)this.surfaceInput.Value);
        }
    }
}
