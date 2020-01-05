using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit;
using System.Windows.Controls;

namespace OutilDevis
{
    class DecaissementWrapPanel : OuvrageWrapPanel
    {
        // Controls
        ComboBox laTerreEstInput;
        IntegerUpDown surfaceInput;
        IntegerUpDown profondeurInput;

        // Labels
        Label laTerreEstLabel;
        Label surfaceLabel;
        Label profondeurLabel;

        public DecaissementWrapPanel()
        {
            // Initialize all controls and their labels
            laTerreEstInput = new ComboBox();
            surfaceInput = new IntegerUpDown();
            profondeurInput = new IntegerUpDown();

            laTerreEstLabel = new Label();
            surfaceLabel = new Label();
            profondeurLabel = new Label();

            // Setup the controls that need it
            laTerreEstInput.Items.Add("écartée sur le terrain");
            laTerreEstInput.Items.Add("évacuée avec les gravats");

            // Setup defaults
            laTerreEstInput.SelectedItem = "écartée sur le terrain";
            surfaceInput.Value = 0;
            profondeurInput.Value = 40;

            // Add them as children to the panel
            addLabeledElementToPanel(surfaceInput, surfaceLabel, "Surface");
            addLabeledElementToPanel(profondeurInput, profondeurLabel, "Profondeur");
            addLabeledElementToPanel(laTerreEstInput, laTerreEstLabel, "La terre est ");
        }
        public override Single GetPrixUnitaire()
        {
            return (42);
        }
        public override string GetDesignation()
        {
            string designation = "Décaissement (après sondage des soubassements). La terre est ";
            designation = string.Concat(designation, laTerreEstInput.SelectedItem.ToString());
            return (designation);
        }
        public override int GetQuantite()
        {
            return ((int)this.surfaceInput.Value);
        }

        // Gravats seulement s'il faut évacuer la terre
        public override Single GetVolumeGravats()
        {
            if (laTerreEstInput.SelectedItem.ToString() == "évacuée avec les gravats") 
                return (Convert.ToSingle(this.surfaceInput.Value * this.profondeurInput.Value /100));
            else
                return (0);
        }
    }
}
