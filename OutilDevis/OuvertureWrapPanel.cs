using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace OutilDevis
{
    class OuvertureWrapPanel : OuvrageWrapPanel
    {
        // Controls
        ComboBox essenceInput;
        IntegerUpDown largeurInput;
        IntegerUpDown hauteurInput;
        CheckListBox optionsInput;

        // Labels
        Label essenceLabel;
        Label largeurLabel;
        Label hauteurLabel;
        Label optionsLabel;

        // Options
        bool Lindage, AppuiBois, AppuiBriques, DansOuvrageExistant, PlotsBeton, Echafaudage;

        public OuvertureWrapPanel(Dictionary<string, float> _priceList) : base(_priceList)
        {
            // Initialize all controls and their labels
            essenceInput = new ComboBox();
            largeurInput = new IntegerUpDown();
            hauteurInput = new IntegerUpDown();
            optionsInput = new CheckListBox();

            essenceLabel = new Label();
            largeurLabel = new Label();
            hauteurLabel = new Label();
            optionsLabel = new Label();

            // Setup the controls that need it
            essenceInput.Items.Add("Douglas");
            essenceInput.Items.Add("Chêne");

            optionsInput.Items.Add("Lindage");
            optionsInput.Items.Add("Appui bois");
            optionsInput.Items.Add("Appui briques");
            optionsInput.Items.Add("Dans ouvrage existant");
            optionsInput.Items.Add("Plots béton");
            optionsInput.Items.Add("Echafaudage");

            // Set the sizes of controls and labels
            essenceInput.MaxHeight = 25;
            essenceLabel.MaxHeight = 30;

            largeurInput.MaxHeight = 25;
            largeurLabel.MaxHeight = 30;

            hauteurInput.MaxHeight = 25;
            hauteurLabel.MaxHeight = 30;

            optionsLabel.MaxHeight = 30;

            // Set the default values of the controls that need it
            essenceInput.SelectedItem = "Douglas";
            largeurInput.Value = 90;
            hauteurInput.Value = 210;

            // Add them as children to the panel
            addLabeledElementToPanel(essenceInput, essenceLabel, "Essence");
            addLabeledElementToPanel(largeurInput, largeurLabel, "Largeur");
            addLabeledElementToPanel(hauteurInput, hauteurLabel, "Hauteur");
            addLabeledElementToPanel(optionsInput, optionsLabel, "Options");
        }

        void retrieveOptions()
        {
            Lindage = optionsInput.SelectedValue.Contains("Lindage");
            AppuiBois = optionsInput.SelectedValue.Contains("Appui bois");
            AppuiBriques = optionsInput.SelectedValue.Contains("Appui briques");
            DansOuvrageExistant = optionsInput.SelectedValue.Contains("Dans ouvrage existant");
            PlotsBeton = optionsInput.SelectedValue.Contains("Plots béton");
            Echafaudage = optionsInput.SelectedValue.Contains("Echafaudage");
        }

        public override Single GetPrixUnitaire()
        {
            retrieveOptions();
            Single joursMainOeuvre = 0;
            Single volumeBois = 0;
            Single largeur = Convert.ToSingle(largeurInput.Value);
            Single hauteur = Convert.ToSingle(hauteurInput.Value);

            // Base en fonction de l'essence et de la largeur
            if (essenceInput.SelectedItem.ToString() == "Douglas") joursMainOeuvre = 3 + largeur / 50;
            if (essenceInput.SelectedItem.ToString() == "Chêne") joursMainOeuvre = 3 + largeur / 40;

            // Supplément pour les très hautes ouvertures
            if (hauteur > 210) joursMainOeuvre += (hauteur - 210) / 75;

            // Remise pour les ouvertures très peu hautes
            if (hauteur < 120) joursMainOeuvre -= Convert.ToSingle(1.3);

            // Suppléments pour les options
            if (Lindage) joursMainOeuvre += 1;
            if (AppuiBois) joursMainOeuvre += Convert.ToSingle(0.5);
            if (AppuiBriques) joursMainOeuvre += Convert.ToSingle(largeur / 200);
            if (DansOuvrageExistant) joursMainOeuvre += 1;
            if (PlotsBeton) joursMainOeuvre += 1;
            if (Echafaudage) joursMainOeuvre += 1;

            // Calcul du volume de bois
            Single epaisseurLinteaux;
            if (largeur > 150) epaisseurLinteaux = 20; else epaisseurLinteaux = Convert.ToSingle(12.5);
            if (Lindage)
                // 4 jambages et 3 linteaux, section des jambages 12.5 * 20
                volumeBois = 4 * Convert.ToSingle(12.5) * 20 * hauteur + 3 * largeur * 20 * epaisseurLinteaux;
            else
                // 6 jambages et 3 linteaux
                volumeBois = 6 * Convert.ToSingle(12.5) * 20 * hauteur + 3 * largeur * 20 * epaisseurLinteaux;

            // Ajouter l'appui bois si nécessaire
            if (AppuiBois) volumeBois += 3 * largeur * 20 * Convert.ToSingle(12.5);

            // Conversion en m3
            volumeBois = volumeBois / 1000000;

            // Calcul final du coût
            Single prixBois = 0;
            if (essenceInput.SelectedItem.ToString() == "Douglas") prixBois = volumeBois * priceList["Charreton_Douglas"];
            if (essenceInput.SelectedItem.ToString() == "Chêne") prixBois = volumeBois * priceList["Charreton_Chene"];

            return Convert.ToSingle(Math.Round(Convert.ToDecimal((joursMainOeuvre * priceList["Charreton_JourneeMainOeuvre"]) + prixBois)));
        }
        public override Single GetVolumeGravats()
        {
            // Take 10cm margin all around, assume 50cm thickness, and convert to m²
            return Convert.ToSingle((largeurInput.Value + 20) * (hauteurInput.Value + 10) * 50 / 1000000);
        }

        // Build the Désignation string from the user's choices
        public override string GetDesignation()
        {
            retrieveOptions();
            string designation = "Ouverture";

            // Essence
            designation = string.Concat(designation, " avec cadre en ");
            designation = string.Concat(designation, essenceInput.Text);

            // Lindage ?
            if (Lindage) designation = string.Concat(designation, ", en lindage");

            // Largeur
            designation = string.Concat(designation, ", largeur = ");
            designation = string.Concat(designation, largeurInput.Text);
            designation = string.Concat(designation, "cm");

            // Hauteur
            designation = string.Concat(designation, ", hauteur = ");
            designation = string.Concat(designation, hauteurInput.Text);
            designation = string.Concat(designation, "cm");

            // Other options
            if (AppuiBriques) designation = string.Concat(designation, ", avec appui en briques");
            if (PlotsBeton) designation = string.Concat(designation, ", sur plots béton");
            return (designation);
        }
        public override Int32 GetQuantite()
        {
            return (1);
        }
    }
}
