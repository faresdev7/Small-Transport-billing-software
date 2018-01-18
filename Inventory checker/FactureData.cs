using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory_checker
{
  public   class FactureData
    {
     public string code { get; set; }
     public string libelle { get; set; }
     public string quantite { get; set; }
     public string prixunitaire { get; set; }
     public string remise { get; set; }
     public string total { get; set; }
     public string typep { get; set; }
     public string prixht { get; set; }
     public string tva { get; set; }
     public string unit { get; set; }


     public FactureData(string libelle, string code, string quantite, string prixunitaire, string total, string remise, string typep, string prixht, string tva, string unit)
        {
            this.code = code;
            this.libelle = libelle;
            this.quantite = quantite;
            this.prixunitaire = prixunitaire;
            this.remise = remise;
            this.total = total;
            this.prixht = prixht;
            this.tva = tva;
            this.typep = typep;
            this.unit = unit;
        
        }
    }
}
