﻿using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;

namespace OutilDevis
{
    class HerissonWrapPanel : OuvrageWrapPanel
    {
        // Controls
        IntegerUpDown surfaceInput;

        // Labels
        Label surfaceLabel;

        public HerissonWrapPanel(Dictionary<string, float> _priceList) : base(_priceList)
        {
            // Initialize all controls and their labels
            surfaceInput = new IntegerUpDown();
            surfaceLabel = new Label();

            // Set defaults
            surfaceInput.Value = 0;

            // Add them as children to the panel
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
        }
        public override Single GetPrixUnitaire()
        {
            return (priceList["Charreton_Herisson"]);
        }
        public override string GetDesignation()
        {
            return ("Hérisson ventilé, en m²");
        }
        public override Single GetQuantite()
        {
            return ((Single)surfaceInput.Value);
        }
    }
}
