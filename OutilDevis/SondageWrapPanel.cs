﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OutilDevis
{
    class SondageWrapPanel : OuvrageWrapPanel
    {
        public SondageWrapPanel(Dictionary<string, float> _priceList) : base(_priceList) { }

        public override Single GetPrixUnitaire()
        {
            return (priceList["Charreton_SondageFacade"]);
        }
        public override string GetDesignation()
        {
            return ("Sondage de la façade");
        }
        public override Single GetQuantite()
        {
            return (1);
        }
    }
}
