﻿using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;

namespace OutilDevis
{
    class LiegeWrapPanel : OuvrageWrapPanel
    {
        // Controls
        IntegerUpDown surfaceInput;
        ComboBox epaisseurInput;

        // Labels
        Label surfaceLabel;
        Label epaisseurLabel;

        public LiegeWrapPanel()
        {
            // Initialize all controls and their labels
            surfaceInput = new IntegerUpDown();
            epaisseurInput = new ComboBox();

            surfaceLabel = new Label();
            epaisseurLabel = new Label();

            // Set the controls that need it
            epaisseurInput.Items.Add("10 cm");
            epaisseurInput.Items.Add("14 cm");

            // Set defaults
            surfaceInput.Value = 0;
            epaisseurInput.SelectedItem = "10 cm";

            // Add them as children to the panel
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
            addLabeledElementToPanel(epaisseurInput, epaisseurLabel, "Epaisseur");
        }
        public override Single GetPrixUnitaire()
        {
            if (epaisseurInput.SelectedItem.ToString() == "10 cm") return (Convert.ToSingle(52));
            if (epaisseurInput.SelectedItem.ToString() == "14 cm") return (Convert.ToSingle(73));
            return (0);
        }
        public override string GetDesignation()
        {
            string designation = "Isolation liège ";
            designation = string.Concat(designation, epaisseurInput.SelectedItem.ToString());
            designation = string.Concat(designation, ", en m²");
            return (designation);
        }
        public override int GetQuantite()
        {
            return ((int)surfaceInput.Value);
        }
    }
}
