﻿using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;

namespace OutilDevis
{
    class FinitionWrapPanel : OuvrageWrapPanel
    {
        // Controls
        ComboBox recetteInput;
        IntegerUpDown surfaceInput;
        ComboBox renduInput;

        // Labels
        Label recetteLabel;
        Label surfaceLabel;
        Label renduLabel;

        public FinitionWrapPanel()
        {
            // Initialize all controls and their labels
            recetteInput = new ComboBox();
            surfaceInput = new IntegerUpDown();
            renduInput = new ComboBox();

            recetteLabel = new Label();
            surfaceLabel = new Label();
            renduLabel = new Label();

            // Setup the controls that need it
            recetteInput.Items.Add("Chaux-sable");
            recetteInput.Items.Add("Chaux-terre-sable");
            recetteInput.Items.Add("Chaux-sable + pigment");
            recetteInput.Items.Add("Chaux-chanvre Alliance4");

            renduInput.Items.Add("Gratté");
            renduInput.Items.Add("Taloché");
            renduInput.Items.Add("Projeté");
            renduInput.Items.Add("Stuqué");

            // Set the defaults
            recetteInput.SelectedItem = "Chaux-sable";
            surfaceInput.Value = 0;
            renduInput.SelectedItem = "Gratté";

            // Add them as children to the panel
            addLabeledElementToPanel(recetteInput, recetteLabel, "Recette");
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
            addLabeledElementToPanel(renduInput, renduLabel, "Rendu");
        }
        public override Single GetPrixUnitaire()
        {
            if (recetteInput.SelectedItem.ToString() == "Chaux-sable") return (Convert.ToSingle(32));
            if (recetteInput.SelectedItem.ToString() == "Chaux-terre-sable") return (Convert.ToSingle(32));
            if (recetteInput.SelectedItem.ToString() == "Chaux-sable + pigment") return (Convert.ToSingle(38));
            if (recetteInput.SelectedItem.ToString() == "Chaux-chanvre Alliance4") return (Convert.ToSingle(35));
            return (0);
        }
        public override string GetDesignation()
        {
            string designation = "";
            
            if (recetteInput.SelectedItem.ToString() == "Chaux-sable") designation = "Enduit de finition chaux NHL2 - sable 0/3, rendu ";
            if (recetteInput.SelectedItem.ToString() == "Chaux-terre-sable") designation = "Enduit de finition chaux NHL2 - argile de Commelle - sable 0/3, rendu ";
            if (recetteInput.SelectedItem.ToString() == "Chaux-sable + pigment") designation = "Enduit de finition chaux NHL2 - sable 0/3, pigmenté, rendu ";
            if (recetteInput.SelectedItem.ToString() == "Chaux-chanvre Alliance4") designation = "Enduit de finition chaux - chanvre, rendu ";

            designation = string.Concat(designation, renduInput.Text.ToLower());

            return (designation);
        }
        public override int GetQuantite()
        {
            return ((int)this.surfaceInput.Value);
        }
    }
}
