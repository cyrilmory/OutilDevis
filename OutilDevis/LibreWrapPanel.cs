using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace OutilDevis
{
    class LibreWrapPanel : OuvrageWrapPanel
    {
        // Controls
        TextBox designationInput;
        IntegerUpDown prixUnitaireInput;
        IntegerUpDown quantiteInput;

        // Labels
        Label designationLabel;
        Label prixUnitaireLabel;
        Label quantiteLabel;

        public LibreWrapPanel()
        {
            // Initialize all controls and their labels
            designationInput = new TextBox();
            prixUnitaireInput = new IntegerUpDown();
            quantiteInput = new IntegerUpDown();

            designationLabel = new Label();
            prixUnitaireLabel = new Label();
            quantiteLabel = new Label();

            // Set the sizes of controls and labels
            designationInput.MinWidth = 40;

            // Set defaults
            designationInput.Text = "Nom de l'ouvrage";
            prixUnitaireInput.Value = 0;
            quantiteInput.Value = 0;

            // Add them as children to the panel
            addLabeledElementToPanel(designationInput, designationLabel, "Désignation");
            addLabeledElementToPanel(prixUnitaireInput, prixUnitaireLabel, "Prix unitaire");
            addLabeledElementToPanel(quantiteInput, quantiteLabel, "Quantité");
        }
        public override Single GetPrixUnitaire()
        {
            return (Convert.ToSingle(this.prixUnitaireInput.Value));
        }
        public override string GetDesignation()
        {
            return (designationInput.Text);
        }
        public override int GetQuantite()
        {
            return ((int)quantiteInput.Value);
        }
    }
}
