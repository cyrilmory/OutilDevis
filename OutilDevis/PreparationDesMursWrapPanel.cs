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

        public PreparationDesMursWrapPanel(Dictionary<string, float> _priceList) : base(_priceList)
        {
            // Initialize all controls and their labels
            surfaceInput = new IntegerUpDown();
            surfaceLabel = new Label();
            surfaceInput.Value = 0;

            // Add them as children to the panel
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
        }
        public override Single GetPrixUnitaire()
        {
            return (priceList["Charreton_PreparationMurs"]);
        }
        public override string GetDesignation()
        {
            return ("Préparation des murs (protection, démontage DEP, ...)");
        }
        public override Single GetQuantite()
        {
            return ((Single)surfaceInput.Value);
        }
    }
}
