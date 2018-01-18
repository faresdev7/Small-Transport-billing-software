using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace Inventory_checker
{
    public partial class Main : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Main()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add("Order ID ", Type.GetType("System.String"));

            dt.Columns.Add("Store name ", Type.GetType("System.String"));
            dt.Columns.Add("Order Date ", Type.GetType("   System.DateTime"));
            dt.Columns.Add("Delivery coordinator ", Type.GetType("System.String"));
            dt.Columns.Add("Delivery dispatcher", Type.GetType("System.String"));
         
            gridControl1.DataSource = dt;
            int x = 0;
            while (x < 50)
            {
                x++;
                //  dataGridView1.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), dateget(reader[3].ToString()), reader[4].ToString());
                DataRow dr = dt.NewRow();
                dr[0] = x.ToString();
                dr[1] = "Store " + x.ToString();
                dr[2] = System.DateTime.Now;
                dr[3] = "Delivery coordinator" + x.ToString();
                dr[4] = "Delivery dispatcher" + x.ToString();

                dt.Rows.Add(dr);
            }
            gridControl1.DataSource = dt;
        }

        private void ribbon_Click(object sender, EventArgs e)
        {
           
         
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}