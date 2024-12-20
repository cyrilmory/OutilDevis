﻿using System;
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

        public CorpsDenduitWrapPanel(Dictionary<string, float> _priceList) : base(_priceList)
        {
            // Initialize all controls and their labels
            recetteInput = new ComboBox();
            surfaceInput = new IntegerUpDown();

            recetteLabel = new Label();
            surfaceLabel = new Label();

            // Setup the controls that need it
            recetteInput.Items.Add("Chaux-sable");
            recetteInput.Items.Add("Chaux-chanvre Alliance4");

            // Set the defaults
            recetteInput.SelectedItem = "Chaux-sable";
            surfaceInput.Value = 0;

            // Add them as children to the panel
            addLabeledElementToPanel(recetteInput, recetteLabel, "Recette");
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
        }
        public override Single GetPrixUnitaire()
        {
            if (recetteInput.SelectedItem.ToString() == "Chaux-sable") return (priceList["Charreton_CorpsEnduitChauxSable"]);
            if (recetteInput.SelectedItem.ToString() == "Chaux-chanvre Alliance4") return (priceList["Charreton_CorpsEnduitChauxChanvre"]);
            return (0);
        }
        public override string GetDesignation()
        {
            if (recetteInput.SelectedItem.ToString() == "Chaux-sable") return ("Corps d’enduit chaux NHL2 - sable 0/4, en m²");
            if (recetteInput.SelectedItem.ToString() == "Chaux-chanvre Alliance4") return ("Corps d’enduit chaux - chanvre, épaisseur 3cm, en m²");
            return ("");
        }
        public override Single GetQuantite()
        {
            return ((Single)this.surfaceInput.Value);
        }
    }
}
