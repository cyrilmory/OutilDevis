using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace OutilDevis
{
    class PreparationDesMursWrapPanel : OuvrageWrapPanel
    {
        // Controls
        IntegerUpDown surfaceInput;

        // Labels
        Label surfaceLabel;

        public PreparationDesMursWrapPanel()
        {
            // Initialize all controls and their labels
            surfaceInput = new IntegerUpDown();
            surfaceLabel = new Label();

            // Add them as children to the panel
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
        }
        public override Single GetPrixUnitaire()
        {
            return (Convert.ToSingle(8));
        }
        public override string GetDesignation()
        {
            return ("Préparation des murs (protection, démontage DEP, ...)");
        }
        public override int GetQuantite()
        {
            return ((int)surfaceInput.Value);
        }
    }
}
