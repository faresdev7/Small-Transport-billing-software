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
    public partial class Login : Form
    {
        public MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        public MySqlConnection con = null;
        public MySqlDataAdapter mad = null;
        public Login()
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

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = "select * from login where UPPER(username)='" + textEdit1.Text.ToUpper() + "' and password='" + textEdit2.Text + "'";
             

                 MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {

                order pr = new order(textEdit1.Text,0);
                this.Hide();
                pr.ShowDialog();
                reader.Close();

            }
            else
            { 
            
            
            MessageBox.Show("Check your username and password ");
            reader.Close();
            }
        
        
        
        
        }
    }
}
