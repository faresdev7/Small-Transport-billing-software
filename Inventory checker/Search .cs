 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inventory_checker
{
    public partial class Search : DevExpress.XtraBars.Ribbon.RibbonForm
    {
    
        public Search()
        {
            InitializeComponent();
        }

        private void Search_Load(object sender, EventArgs e)
        {
            
          
             
        }

        private void comboBoxEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
             
 
        }

        private void comboBoxEdit1_Validated(object sender, EventArgs e)
        {

        }

        private void comboBoxEdit1_TextChanged(object sender, EventArgs e)
        {
                  
           
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1.Print();
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
