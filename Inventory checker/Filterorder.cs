using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Inventory_checker
{
    public partial class Filterorder : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        public MySqlConnection con = null;
        public MySqlDataAdapter mad = null;
        public Filterorder()
        {
            InitializeComponent();
            conn_string.Server = "localhost";
            conn_string.UserID = "root";
            conn_string.Password = "";
            conn_string.Database = "projf";
            conn_string.ConvertZeroDateTime = true;
            con = new MySqlConnection(conn_string.ToString());
            con.Open();
        }
        private static void DoRowDoubleClick(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                string colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
                MessageBox.Show(string.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption));
            }
        }

        public void getc(string g, ComboBox x)
        {

            string sql = "Select Distinct(" + g + ") from orderi ";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                x.Items.Add(reader[0].ToString());



            }

            reader.Close();

        }
        private void Filterorder_Load(object sender, EventArgs e)
        {


            cname.Items.Add("ALL");
            sname.Items.Add("ALL");
            id.Items.Add("ALL");
            darea.Items.Add("ALL");
            getc("customername", cname);
            getc("storename", sname);
            getc("deliveryarea", darea);
            getc("id", id);
            getall("select * from orderi");


            sname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            sname.AutoCompleteSource = AutoCompleteSource.ListItems;
            darea.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            darea.AutoCompleteSource = AutoCompleteSource.ListItems;
            id.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            id.AutoCompleteSource = AutoCompleteSource.ListItems;
            cname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cname.AutoCompleteSource = AutoCompleteSource.ListItems;


      


        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public void getall(string sql)
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("ID ", Type.GetType("System.String"));
            dt.Columns.Add("Date", Type.GetType("System.DateTime"));
            dt.Columns.Add("Store Name ", Type.GetType("System.String"));
            dt.Columns.Add("Customer Name ", Type.GetType("System.String"));
            dt.Columns.Add("Delivery  Area", Type.GetType("System.String"));
            dt.Columns.Add("Item cost ", Type.GetType("System.Double"));
            dt.Columns.Add("Amount paid to store", Type.GetType("System.Double"));
            dt.Columns.Add("Tax paid", Type.GetType("System.Double"));
            dt.Columns.Add("Tax after discount", Type.GetType("System.Double"));
            dt.Columns.Add("Payment type online", Type.GetType("System.String"));
            dt.Columns.Add("Extra money", Type.GetType("System.Double"));
            dt.Columns.Add("Refund amount", Type.GetType("System.Double"));
            dt.Columns.Add("Delivery dispatcher", Type.GetType("System.String"));
            dt.Columns.Add("Bill Number", Type.GetType("System.Int32"));
            dt.Columns.Add("Comemnt", Type.GetType("System.String"));
            dt.Columns.Add("Delivery Coordinator", Type.GetType("System.String"));
            dt.Columns.Add("Delivery Date", Type.GetType("System.DateTime"));
            dt.Columns.Add("Delivery Distance", Type.GetType("System.Double"));
            dt.Columns.Add("Amount collected from customer", Type.GetType("System.Double"));
            dt.Columns.Add("Bill image", typeof(Image));

            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {

                //  dataGridView1.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), dateget(reader[3].ToString()), reader[4].ToString());
                DataRow dr = dt.NewRow();
                for (int i = 0; i < 19; i++)
                    dr[i] = reader[i].ToString();

                try
                {
                    dr[19] = byteArrayToImage((Byte[])reader[19]);
                }

                catch (Exception ex)
                {

                    dr[19] = null;
                }
                dt.Rows.Add(dr);
            }


        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {




        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                gridControl1.ExportToPdf(folderName);
            }



        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {

                var riPicEdit = new RepositoryItemPictureEdit();
                riPicEdit.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
                gridView1.Columns["Bill image"].ColumnEdit = riPicEdit;
                gridView1.Columns["Bill image"].Width = 450;
                gridView1.RowHeight = 450;
                gridView1.Columns["Date"].Visible = false;
               

                string folderName = folderBrowserDialog1.SelectedPath;
               // MessageBox.Show(folderName);
                gridControl1.ExportToXls(folderName + "\\Order.Xls");
                gridView1.Columns["Bill image"].Width = 20;
                gridView1.RowHeight = 20;

                gridView1.Columns["Date"].Visible =true;
            }

        }

        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                gridControl1.ExportToPdf(folderName + "\\Order.pdf");
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                gridControl1.ExportToRtf(folderName + "\\Order.RTF");
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                gridControl1.ExportToCsv(folderName + "\\Order.Csv");
            }

        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {


            DateTime dt;
            DateTime dt2;


            int xv = 0;
            string ss = "select * from orderi ";

            if (cname.Text != "ALL")
            {
                if (xv == 0)
                {
                    xv++;
                    ss = ss + " where customername='" + cname.Text + "'";

                }

                else
                {
                    ss = ss + "  and customername='" + cname.Text + "'";
                    xv++;

                }
            }
            if (sname.Text != "ALL")
            {
                if (xv == 0)
                {
                    xv++;
                    ss = ss + "where storename='" + sname.Text + "'";

                }
                else
                {
                    xv++;
                    ss = ss + "and storename='" + sname.Text + "'";
                }

            }

            if (id.Text != "ALL")
            {
                if (xv == 0)
                {
                    xv++;
                    ss = ss + "where id='" + id.Text + "'";
                }
                else
                {
                    xv++;
                    ss = ss + "and id='" + id.Text + "'";
                }
            }
            if (darea.Text != "ALL")
            {
                if (xv == 0)
                {
                    xv++;
                    ss = ss + "where deliveryarea='" + darea.Text + "'";

                }
                else
                {
                    xv++;
                    ss = ss + "and deliveryarea='" + darea.Text + "'";

                }
            }
            if (dateEdit1.Text != "")
            {
                dt = DateTime.Parse(dateEdit1.Text);
                if (xv == 0)
                {
                    xv++;
                    ss = ss + "where date >='" + dt.Date.ToString("yyyy-MM-dd") + "'";

                }
                else
                {
                    xv++;
                    ss = ss + "and date >='" + dt.Date.ToString("yyyy-MM-dd") + "'";

                }
            }

            if (dateEdit2.Text != "")
            {
                dt2 = DateTime.Parse(dateEdit2.Text);
                if (xv == 0)
                {
                    xv++;
                    ss = ss + "where date <= '" + dt2.Date.ToString("yyyy-MM-dd") + "'";

                }
                else
                {
                    xv++;
                    ss = ss + "and date <= '" + dt2.Date.ToString("yyyy-MM-dd") + "'";

                }
            }

            getall(ss);
        }


        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void gridControl1_Click_1(object sender, EventArgs e)
        {

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {  Image img=null;
            Point pos = gridControl1.PointToClient(MousePosition);
            GridHitInfo ghi = gridView1.CalcHitInfo(pos);
            if (ghi.InRow)
            {

                DataRow dr;
                dr = gridView1.GetDataRow(ghi.RowHandle);
                    Image y = (Image) dr[19];
                    Form2 ord = new Form2(y);
                   ord.ShowDialog();
            }


        }
        public void dispaly(string id)
        {


        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {

        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {





            /*System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
             Image y = (Image) row[19];
             Form2 ord = new Form2(y);
            ord.ShowDialog();*/

            /*
          
                imageform im = new imageform(y);
                im.Show();*/
        }
  

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void darea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
