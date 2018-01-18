using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_checker
{
public class product
    {
   public string producrname {get;set;}
   public string batchproduct { get; set; }
   public string date { get; set; }
   public string qty { get; set; }
   public string info { get; set; }
   public product(string producrname, string batchproduct, string date, string qty, string info)
   { 
   this.producrname=producrname;
   this.qty = qty;
   this.batchproduct = batchproduct;
   this.date = date;
   this.info = info;
   
   }
    }
}
