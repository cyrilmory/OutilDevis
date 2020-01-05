﻿using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;

namespace OutilDevis
{
    class DalleWrapPanel : OuvrageWrapPanel
    {
        // Controls
        ComboBox recetteInput;
        IntegerUpDown surfaceInput;

        // Labels
        Label recetteLabel;
        Label surfaceLabel;

        public DalleWrapPanel()
        {
            // Initialize all controls and their labels
            recetteInput = new ComboBox();
            surfaceInput = new IntegerUpDown();

            recetteLabel = new Label();
            surfaceLabel = new Label();

            // Set the controls that need it
            recetteInput.Items.Add("béton de chaux NHL 3.5");
            recetteInput.Items.Add("béton de chaux NHL 3.5, pouzzolane, sable de pierre ponce");

            // Set defaults
            surfaceInput.Value = 0;
            recetteInput.SelectedItem = "béton de chaux NHL 3.5";

            // Add them as children to the panel
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
            addLabeledElementToPanel(recetteInput, recetteLabel, "Recette");
        }
        public override Single GetPrixUnitaire()
        {
            if (recetteInput.SelectedItem.ToString() == "béton de chaux NHL 3.5") return (Convert.ToSingle(64));
            if (recetteInput.SelectedItem.ToString() == "béton de chaux NHL 3.5, pouzzolane, sable de pierre ponce") return (Convert.ToSingle(70));
            return (0);
        }
        public override string GetDesignation()
        {
            string designation = "Dalle de ";
            designation = string.Concat(designation, recetteInput.SelectedItem.ToString());
            designation = string.Concat(designation, ", épaisseur 10 cm, en m²");
            return (designation);
        }
        public override int GetQuantite()
        {
            return ((int)surfaceInput.Value);
        }
    }
}
