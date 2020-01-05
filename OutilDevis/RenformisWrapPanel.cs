using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace OutilDevis
{
    class RenformisWrapPanel : OuvrageWrapPanel
    {
        // Controls
        IntegerUpDown surfaceInput;

        // Labels
        Label surfaceLabel;

        public RenformisWrapPanel()
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
            return (Convert.ToSingle(10));
        }
        public override string GetDesignation()
        {
            return ("Renformis (prix susceptible d'être modifié après la purge)");
        }
        public override int GetQuantite()
        {
            return ((int)surfaceInput.Value);
        }
    }
}
