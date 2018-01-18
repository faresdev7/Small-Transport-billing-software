using MySql.Data.MySqlClient;
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
    public partial class Dsipatcher_screen : Form
    {
        public MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        public MySqlConnection con = null;
        public MySqlDataAdapter mad = null;
        public Dsipatcher_screen()
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

        private void Dsipatcher_screen_Load(object sender, EventArgs e)
        {
            getc("deliverydispatcher",comboBox1);
            getall("select * from orderi");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DateTime dt;
            DateTime dt2;


            int xv = 0;
            string ss = "select * from orderi ";

            if (comboBox1.Text != "ALL")
            {
                if (xv == 0)
                {
                    xv++;
                    ss = ss + " where 	deliverydispatcher='" + comboBox1.Text + "'";

                }

                else
                {
                    ss = ss + "  and 	deliverydispatcher='" + comboBox1.Text + "'";
                    xv++;

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
        

        public void  getall(string sql)
        {

            DataTable dt = new DataTable();
             

            dt.Columns.Add("ID ", Type.GetType("System.String"));
            dt.Columns.Add("Date ", Type.GetType("System.DateTime"));
            dt.Columns.Add("Store Name ", Type.GetType("System.String"));
            dt.Columns.Add("Delivery Coordinator", Type.GetType("System.String"));
            dt.Columns.Add("Delivery Distance", Type.GetType("System.Double"));

            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();
            double p = 0;
        
            while (reader.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = reader["id"].ToString();
                dr[1] = reader["date"].ToString();
                dr[2] = reader["storename"].ToString();
                dr[3] = reader["deliverycoordinator"].ToString();
                dr[4] = reader["DeliveryDistance"].ToString();
                try
                {
                    p = p + double.Parse(reader["DeliveryDistance"].ToString());
                }
                catch (Exception c) { }
                dt.Rows.Add(dr);
            }

            gridControl1.DataSource = dt;
            textEdit1.Text = p.ToString();
            reader.Close();
        
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
