 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_checker
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void select()

        {
             
        }
        private void Form1_Load(object sender, EventArgs e)
        {
         


 
        }

        private void Button1_Click(object sender, EventArgs e)
        { 
          
        }


        public static string dateget(string hh)
        {
          
            string dd;
            string mm;
            string yyyy;

            yyyy = hh.Substring(0, 4);
            dd = hh.Substring(6, 2);
            mm = hh.Substring(4, 2);
            string kk = dd + "/" + mm + "/" + yyyy;

            return kk;
          
        }
        private void batchnumber_KeyPress(object sender, KeyPressEventArgs e)
        {
 
        }

        private void qty_KeyPress(object sender, KeyPressEventArgs e)
        {try{
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)  )
            {
                e.Handled = true;
            }
         }
            catch(Exception ex){}
        }

        private void Button2_Click(object sender, EventArgs e)
        {
        }        
        public void delete()
        { 
        }

        private void Button4_Click(object sender, EventArgs e)
        {
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        { 
        }

        private void Button3_Click(object sender, EventArgs e)
        {
        }

        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
      
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
          
        }

        private void button7_Click(object sender, EventArgs e)
        {
    
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
       
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
           
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
 
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        { 
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
        public static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }private Random gen = new Random();
        DateTime RandomDay()
        {
            DateTime start = new DateTime(2010, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
        private void button1_Click_1(object sender, EventArgs e)
        { 
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
           
        }

        private void qty_TextChanged(object sender, EventArgs e)
        {

        }

        private void productname_TextChanged(object sender, EventArgs e)
        {

        }

        private void expirydate_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
