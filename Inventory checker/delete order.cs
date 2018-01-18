using MySql.Data.MySqlClient;
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
    public partial class delete_order : Form
    {
        public MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        public MySqlConnection con = null;
        public MySqlDataAdapter mad = null;
        public order or;
        public delete_order(order or)
        {
            InitializeComponent();
            conn_string.Server = "localhost";
            conn_string.UserID = "root";
            conn_string.Password = "";
            conn_string.Database = "projf";
            conn_string.ConvertZeroDateTime = true;

            con = new MySqlConnection(conn_string.ToString());
            con.Open();
            this.or = or;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text == "")
            {
                MessageBox.Show("Empty");


            }
            else
            {
                string h = "";

                string sql = "select * from orderi where id='" + textEdit1.Text + "'";
                MySqlCommand command = new MySqlCommand(sql, con);

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    h = "1";


                }
                reader.Close();

                if (h == "1")
                {
                    sql = "delete  from orderi where id='" +textEdit1.Text + "'";
                    var sqlcmd = con.CreateCommand();
                    sqlcmd.CommandText = sql;
                    sqlcmd.ExecuteNonQuery();
                    sql = "delete  from orderidet where id='" + textEdit1.Text + "'";
                    sqlcmd.CommandText = sql;
                    sqlcmd.ExecuteNonQuery();
                    MessageBox.Show("oder delete with success");
                    or.neworder();
                }
                else
                { MessageBox.Show("ID does not exit"); }
            }
        }

        private void delete_order_Load(object sender, EventArgs e)
        {

        }
    }
}
