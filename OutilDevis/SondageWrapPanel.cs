using System;
using System.Collections.Generic;
using System.Text;

namespace OutilDevis
{
    class SondageWrapPanel : OuvrageWrapPanel
    {
        public override Single GetPrixUnitaire()
        {
            return (200);
        }
        public override string GetDesignation()
        {
            return ("Sondage de la façade");
        }
        public override int GetQuantite()
        {
            return (1);
        }
    }
}
