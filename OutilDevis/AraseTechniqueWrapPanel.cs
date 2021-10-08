using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace OutilDevis
{
    class AraseTechniqueWrapPanel : OuvrageWrapPanel
    {
        // Controls
        IntegerUpDown longueurInput;

        // Labels
        Label longueurLabel;

        public AraseTechniqueWrapPanel(Dictionary<string, float> _priceList) : base(_priceList)
        {
            // Initialize all controls and their labels
            longueurInput = new IntegerUpDown();
            longueurLabel = new Label();
            longueurInput.Value = 0;

            // Add them as children to the panel
            addLabeledElementToPanel(longueurInput, longueurLabel, "Longueur");
        }
        public override Single GetPrixUnitaire()
        {
            return (priceList["Charreton_AraseTechnique"]);
        }
        public override string GetDesignation()
        {
            return ("Arase technique");
        }
        public override Single GetQuantite()
        {
            return ((Single)longueurInput.Value);
        }
    }
}
