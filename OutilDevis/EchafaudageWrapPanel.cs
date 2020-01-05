using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;

namespace OutilDevis
{
    class EchafaudageWrapPanel : OuvrageWrapPanel
    {
        // Controls
        IntegerUpDown surfaceInput;

        // Labels
        Label surfaceLabel;

        public EchafaudageWrapPanel()
        {
            // Initialize all controls and their labels
            surfaceInput = new IntegerUpDown();
            surfaceLabel = new Label();

            // Add them as children to the panel
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
        }
        public override decimal GetPrixUnitaire()
        {
            return (Convert.ToDecimal(12));
        }
        public override string GetDesignation()
        {
            return ("Montage d'échafaudage");
        }
        public override int GetQuantite()
        {
            return ((int)this.surfaceInput.Value);
        }
    }
}
