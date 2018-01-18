using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Inventory_checker
{
    public partial class Delivery_Charge : Form
    {
        public int x = 0;
        public MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        public MySqlConnection con = null;
        public MySqlDataAdapter mad = null;
        public string storelon;
        public string storealt;
        public string dellon;
        public string delalt;
        public string h1 = "";
        public string h2 = "";
        public double ff = 0;
        public string urll = "";
        public Delivery_Charge()
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

        private void Delivery_Charge_Load(object sender, EventArgs e)
        {
            string sql = "select * from store  ORDER by `Storename` ASC";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                storename.Items.Add(reader[0].ToString());
                storelon = reader[1].ToString();
               storealt = reader[2].ToString();
            }
            reader.Close();
            command = null;
            sql = " select * from delivreyarea    ORDER by `area` ASC";
            command = new MySqlCommand(sql, con);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                DeliveryArea.Items.Add(reader[0].ToString());
                dellon=reader[1].ToString();
                delalt=reader[2].ToString();
            }
            reader.Close();
            command = null;
            sql = " select * from pa";
            command = new MySqlCommand(sql, con);
            reader = command.ExecuteReader();

             if(reader.Read())
              x=int.Parse(reader[0].ToString());
 
            reader.Close();
            command = null;
   storename.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
   storename.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        public double GetDrivingDistanceInMiles(string origin, string destination)
        {
            string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + origin + "&destinations=" + destination + "&mode=driving&sensor=false&language=en-EN&units=metric";
            urll = url; try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader sreader = new StreamReader(dataStream);
                string responsereader = sreader.ReadToEnd();
                response.Close();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(responsereader);
                if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
                {
                    XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                    return Convert.ToDouble(distance[0].ChildNodes[0].InnerText.ToString()) / 1000;
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show("Check internet connexion");

            }
            return 0;
        }

        /// <summary>
        /// Get Location based on Latitude and Longitude.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public string GetLocation(double latitude, double longitude)
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + latitude + "," + longitude + "&sensor=false";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);
            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                XmlNodeList location = xmldoc.GetElementsByTagName("distance");
                return xmldoc.GetElementsByTagName("formatted_address")[0].ChildNodes[0].InnerText;
            }

            return "";
        }

        public string getareaorginfromdb(string area)
        {
            string origin = "";
            string destination = "";
            string sql = "select * from delivreyarea where area='" + area + "'";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                origin = reader[2].ToString()+","+reader[1].ToString();


            }
            reader.Close();
            return origin;

        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public string getareaorginfromstore(string area)
        {
            string origin = "";
            string destination = "";
            string sql = "select * from store where storename='" + area + "'";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                origin = reader[2].ToString() + "," + reader[1].ToString();

            } reader.Close();
            return origin;


        }
        public int tonumber( string a)
        {

           
            string b = string.Empty;
            int val=0;

            for (int i = 0; i < a.Length; i++)
            {
                if (Char.IsDigit(a[i]))
                    b += a[i];
            }

            if (b.Length > 0)
                val = int.Parse(b);


            return val;

        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {

            h1 = DeliveryArea.Text;
            h2 = storename.Text;
            Thread thread1 = new Thread(new ThreadStart(A));
         
            thread1.Start();

            thread1.Join();

            label1.Text = ff.ToString();
            textEdit1.Text = urll;
        }
        public void A()
        { 
           double u = GetDrivingDistanceInMiles(getareaorginfromdb(h1), getareaorginfromstore(h2));

           ff = u*2*x ;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
