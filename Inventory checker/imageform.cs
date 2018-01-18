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
    public partial class imageform : Form
    {
        public MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        public MySqlConnection con = null;
        public MySqlDataAdapter mad = null;
        public string id="";
        public imageform(string id)
        {
            InitializeComponent();
            this.id = id;
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        private void imageform_Load(object sender, EventArgs e)
        {
            conn_string.Server = "localhost";
            conn_string.UserID = "root";
            conn_string.Password = "";
            conn_string.Database = "projf";
            conn_string.ConvertZeroDateTime = true;

            con = new MySqlConnection(conn_string.ToString());
            con.Open();
            string sql = "select * from orderi where id='" + id + "'";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                string YYY = reader[19].ToString();
                //byte[] array = Encoding.ASCII.GetBytes();
                try
                {
                    pictureBox1.Image = byteArrayToImage((Byte[])reader[19]);
                }
                catch (Exception vv)
                {
                    pictureBox1.Image = null;
                
                }

            }
            reader.Close();
        }
    }
}
