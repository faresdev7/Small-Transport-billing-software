using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Net;
using System.IO;
using System.Xml;
using DevExpress.XtraReports.UI;
using System.Threading;
using DevExpress.XtraEditors;
namespace Inventory_checker
{
    public partial class order : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public byte[] Contentimg { get; set; }
 
           public  MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
           public   MySqlConnection con  =null;
           public MySqlDataAdapter mad = null;
           public string imagefile = "";
           public int result = 0;
           public double IGST = 0;
           public double CGST = 9;
           public double SGST = 9;
           public string GSTIN ="EEE";
           public double TOTALAMOUNT=0;
           public double DISCOUNT = 0;
           public  double DELIVERYCHARGE=0;
           public double HOTELSERVICE=0;
           public double CHARITYAMOUNT = 0;
           public double TOTALPAYABLE = 0;
           public double MIGST =0;
           public double MCGST = 0;
           public double MSGST =0;
           public double xx = 0;
           public static string user = "admin";
        
        
  
        public order()
        {
           InitializeComponent();
           conn_string.Server = "localhost";
           conn_string.UserID = "root";
           conn_string.Password = "";
           conn_string.Database = "projf";
           conn_string.ConvertZeroDateTime = true;
   
           con = new MySqlConnection(conn_string.ToString());
           con.Open();

           barButtonItem10.Enabled = true;
           barButtonItem11.Enabled = true;
        }
        public order(string g,int i)
        {
            InitializeComponent();
            conn_string.Server = "localhost";
            conn_string.UserID = "root";
            conn_string.Password = "";
            conn_string.Database = "projf";
            conn_string.ConvertZeroDateTime = true;

            con = new MySqlConnection(conn_string.ToString());
            con.Open();

            barButtonItem10.Enabled = true;
            barButtonItem11.Enabled = true;
            user = g;
            deliverycorrdinator.Text = user;
        }
        public order(string h)
        {
            InitializeComponent();
            conn_string.Server = "localhost";
            conn_string.UserID = "root";
            conn_string.Password = "";
            conn_string.Database = "projf";
            conn_string.ConvertZeroDateTime = true;

            con = new MySqlConnection(conn_string.ToString());
            con.Open();
            id.Enabled = false;
            barButtonItem1.Enabled = false;
            barButtonItem3.Enabled = false;
            barButtonItem2.Enabled = false;
            id.Text = h;

            displayi();

        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {

            MemoryStream ms = new MemoryStream();
            try
            { imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            
            }
            catch (Exception tr)
            {
                return null;
            
            }
            return ms.ToArray();
           
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }

            catch (Exception eee)
            {
             return   null;
            }
    
    }

        public double GetDrivingDistanceInMiles(string origin, string destination)
        {
            string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + origin + "&destinations=" + destination + "&mode=driving&sensor=false&language=en-EN&units=metric";

          
                 HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //extEdit1.Text = url;
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
      
            return 0;
        }

        /// <summary>
        /// Get Location based on Latitude and Longitude.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        /// 
 
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

        public string  getareaorginfromdb(string area)
        {
            string origin = "";
            string destination = "";
            string sql = "select * from delivreyarea where area='"+area+"'";
            MySqlCommand command = new MySqlCommand(sql, con);
 
            MySqlDataReader reader = command.ExecuteReader();

            if(reader.Read())
            {
                origin = reader[2].ToString() +","+ reader[1].ToString();
              

            }
            reader.Close();
            return origin;
            
        }
        public string getfixed()
        {
            string origin = "";
            string destination = "";
            string sql = "select * from fixed";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                origin = reader[1].ToString() + "," + reader[0].ToString();


            }
            reader.Close();
            return origin;

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
            }
            
            reader.Close();
            return origin;
    

        }

        public string getid()
        {
            string sql = "SELECT max(id) FROM orderi";
            MySqlCommand command = new MySqlCommand(sql, con);
            string x = "";
            int y = 0;
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                try
                {
                   y = int.Parse(reader[0].ToString()) ;

                }
                catch (Exception ex)
                {
                   y=1;
                }
            }
            reader.Close();
        return (y+1).ToString();
        
        }
        private void order_Load(object sender, EventArgs e)
        {
        
        }
        public void number(object sender, KeyPressEventArgs e)
        {



        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {  string sql = "select * from orderi where id='" + id.Text + "'";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {

                MessageBox.Show("Order id already exists");
                reader.Close();
            
            }else
            {
            reader.Close();
            insert();
            if (result==1)
            MessageBox.Show("Order add with success");
            result = 0;
            }
        }
        public void insert()
        {

            if (charge.Text == "")
            {

                charge.Text = "";
            
            }

            string CUSTOMERNAME = customername.Text.ToString();
            string STORENAME = storename.Text.ToString();
            string DELIVERYAREA = deliveryarea.Text.ToString();
            if (itemcost.Text == "")
                itemcost.Text = "0";
                double ITEMCOST = double.Parse(itemcost.Text.ToString());
                if (amountpaidtostore.Text == "")
                    amountpaidtostore.Text = "0";
            double AMOUNTPAIDTOSTORE = double.Parse(amountpaidtostore.Text.ToString());
            if (taxpaid.Text == "")
                taxpaid.Text = "0";

            double TAXPAID = double.Parse(taxpaid.Text.ToString());
            double TAXAFTERDISCOUNT;
  
 



            if (hotelserviceag.Text == "")
               hotelserviceag.Text = "0";
            else
            HOTELSERVICE = double.Parse(hotelserviceag.Text);



          if (charityamou.Text == "")
              charityamou.Text = "0";
          else

         CHARITYAMOUNT = double.Parse(charityamou.Text);



         if (dicountt.Text == "")
                         dicountt.Text = "0";
         else

       DISCOUNT= double.Parse(dicountt.Text);






            Contentimg = imageToByteArray(pictureBox1.Image);
            int x = 0;


            if (extramoney.Text == "")
                extramoney.Text = "0";
            if (refundamount.Text == "")
            {
                refundamount.Text = "0";
            }
            if (id.Text == "")
            {
                x = 1;
            }
            if (date.Text == "")
            {
                x = 1;
            }
            if (storename.Text == "")
            {
                x = 11;
            }
            if (deliveryarea.Text == "")
            {
                x = 1;
            }
            if (amountpaidtostore.Text == "")
            {
                x = 1;
            }

            if (itemcost.Text == "")
            {
                x = 1;

            }

            if (taxpaid.Text == "")
            {
                x = 1;

            }
            if (taxafterdiscount.Text == "")
            {
                x = 1;

            }
            if (Amountcollectedfromcustomer.Text == "")
            {
                x = 1;

            }

            if (deliverydispatcher.Text == "")
            {
                x = 1;

            }

            if (billnumber.Text == "")
            {
                x = 1;

            }

            if (x == 1)
            { MessageBox.Show("check data entered"); }
            else
            {
                string p1 = "0";
                string p2 = "0";
                string p3 = "0";

                p1 = (GetDrivingDistanceInMiles(getareaorginfromdb(deliveryarea.Text), getareaorginfromstore(storename.Text)).ToString()) + "";
                p2 = (GetDrivingDistanceInMiles(getfixed(), getareaorginfromstore(storename.Text)).ToString()) + "";
                p3 = (GetDrivingDistanceInMiles(getareaorginfromdb(deliveryarea.Text), getfixed()).ToString()) + "";


                distance.Text = (double.Parse(p1) + double.Parse(p2) + double.Parse(p2)).ToString();
          
                double p = 0;
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    p = p + double.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                TOTALAMOUNT = p;
                double bass = 0;
                if (distance.Text == "")
                    distance.Text = "0";
                DELIVERYCHARGE = double.Parse(charge.Text);
                bass = TOTALAMOUNT + -DISCOUNT + DELIVERYCHARGE;
               // charge.Text=DELIVERYCHARGE.ToString();
                MIGST = bass * (IGST / 100);
                MCGST = bass * (CGST / 100);
                MSGST = bass * (SGST / 100);

                TOTALPAYABLE = bass + MIGST + MSGST + MSGST + HOTELSERVICE + CHARITYAMOUNT;

                string ID = id.Text.ToString();
                DateTime DATE = date.DateTime.Date;

                if (taxafterdiscount.Text == "YES")
                    TAXAFTERDISCOUNT = 1;
                else
                    TAXAFTERDISCOUNT = 0;
                string PAYMENTTYPEONLINE = "";
                if (paymenttypeonline.SelectedIndex == 0)
                    PAYMENTTYPEONLINE = "COD";

                if (paymenttypeonline.SelectedIndex == 1)
                    PAYMENTTYPEONLINE = "CARD";
                if (paymenttypeonline.SelectedIndex == 2)
                    PAYMENTTYPEONLINE = "PAYTM";
                if (paymenttypeonline.SelectedIndex == 3)
                    PAYMENTTYPEONLINE = "Other";

                double EXTRAMONEY = double.Parse(extramoney.Text.ToString());
                double REFUNDAMOUNT = double.Parse(refundamount.Text.ToString());
                string DELIVERYDISPATCHER = deliverydispatcher.Text.ToString();
                int BILLNUMBER = int.Parse(billnumber.Text.ToString());
                string COMMENT = comment.Text.ToString();



  




                string sql = "insert into orderi (id,date,customername,storename,deliveryarea,itemcost,amountpaidtostore,taxpaid,taxafterdiscount,paymenttypeonline,extramoney,refundamount,deliverydispatcher,billnumber,comment,image,deliverycharge,hotelserivceagency,charityamount,totalamount,igst,cgst,sgst,migst,mcgst,msgst,gstin,discount,totalg,deliverydistance,amountcollectedfromcustomer,deliverycoordinator) values (@ID,@DATE,@CUSTOMERNAME,@STORENAME,@DELIVERYAREA,@ITEMCOST,@AMOUNTPAIDTOSTORE,@TAXPAID,@TAXAFTERDISCOUNT,@PAYMENTTYPEONLINE,@EXTRAMONEY,@REFUNDAMOUNT,@DELIVERYDISPATCHER,@BILLNUMBER,@COMMENT,@IMAGE,@DELIVERYCHARGE,@HOTELSERIVCEAGENCY,@CHARITYAMOUNT,@TOTALAMOUNT,@IGST,@CGST,@SGST,@MIGST,@MCGST,@MSGST,@GSTIN,@DISCOUNT,@TOTALG,@DELIVERYDISTANCE,@AMOUNTCOLLECTEDFROMCUSTOMER,@DELIVERYCOORDINATOR)";
                
                if(charge.Text=="")
                {

                    charge.Text = "0";


                }
                DELIVERYCHARGE = double.Parse(charge.Text);


                var sqlcmd = con.CreateCommand();
                sqlcmd.CommandText = sql;
                sqlcmd.Parameters.AddWithValue("@ID", ID);
                sqlcmd.Parameters.AddWithValue("@DATE", DATE);
                sqlcmd.Parameters.AddWithValue("@CUSTOMERNAME", CUSTOMERNAME);
                sqlcmd.Parameters.AddWithValue("@STORENAME", STORENAME);
                sqlcmd.Parameters.AddWithValue("@DELIVERYAREA", DELIVERYAREA);
                sqlcmd.Parameters.AddWithValue("@ITEMCOST", ITEMCOST);
                sqlcmd.Parameters.AddWithValue("@AMOUNTPAIDTOSTORE", AMOUNTPAIDTOSTORE);
                sqlcmd.Parameters.AddWithValue("@TAXPAID", TAXPAID);
                sqlcmd.Parameters.AddWithValue("@TAXAFTERDISCOUNT", TAXAFTERDISCOUNT);
                sqlcmd.Parameters.AddWithValue("@PAYMENTTYPEONLINE", PAYMENTTYPEONLINE);
                sqlcmd.Parameters.AddWithValue("@EXTRAMONEY", EXTRAMONEY);
                sqlcmd.Parameters.AddWithValue("@REFUNDAMOUNT", REFUNDAMOUNT);
                sqlcmd.Parameters.AddWithValue("@DELIVERYDISPATCHER", DELIVERYDISPATCHER);
                sqlcmd.Parameters.AddWithValue("@BILLNUMBER", BILLNUMBER);
                sqlcmd.Parameters.AddWithValue("@COMMENT", COMMENT);
           
                sqlcmd.Parameters.AddWithValue("@DELIVERYCHARGE",DELIVERYCHARGE);
              sqlcmd.Parameters.AddWithValue("@HOTELSERIVCEAGENCY",HOTELSERVICE);
              sqlcmd.Parameters.AddWithValue("@CHARITYAMOUNT", CHARITYAMOUNT);
sqlcmd.Parameters.AddWithValue("@TOTALAMOUNT",TOTALAMOUNT);
sqlcmd.Parameters.AddWithValue("@IGST",IGST);
sqlcmd.Parameters.AddWithValue("@CGST",CGST);
sqlcmd.Parameters.AddWithValue("@SGST",SGST);
sqlcmd.Parameters.AddWithValue("@MIGST",MIGST);
sqlcmd.Parameters.AddWithValue("@MCGST", MCGST);
sqlcmd.Parameters.AddWithValue("@MSGST",MSGST);
sqlcmd.Parameters.AddWithValue("@GSTIN",GSTIN);
sqlcmd.Parameters.AddWithValue("@DISCOUNT",DISCOUNT);
sqlcmd.Parameters.AddWithValue("@TOTALG", TOTALPAYABLE);
sqlcmd.Parameters.AddWithValue("@DELIVERYDISTANCE", distance.Text);
sqlcmd.Parameters.AddWithValue("@AMOUNTCOLLECTEDFROMCUSTOMER", Amountcollectedfromcustomer.Text );
sqlcmd.Parameters.AddWithValue("@DELIVERYCOORDINATOR",deliverycorrdinator.Text);             
                byte[] imageBuffer = new byte[76800];
                if (Contentimg==null)
                    Contentimg = imageBuffer;
                sqlcmd.Parameters.AddWithValue("@IMAGE", Contentimg);
                sqlcmd.ExecuteNonQuery();




for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
{
    sql = "insert into orderidet (id,itemname,hsn,unitprice,quant,amount) values (@ID,@ITEMNAME,@HSN,@UNITPRICE,@QUANT,@AMOUNT)";
    string ITEMNAME = dataGridView1.Rows[i].Cells[0].Value.ToString();
    string HSN = dataGridView1.Rows[i].Cells[1].Value.ToString();
    double UNITPRICE = double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
    double QUANT = double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
    double AMOUNT = double.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
    sqlcmd = con.CreateCommand();
    sqlcmd.CommandText = sql;
    sqlcmd.Parameters.AddWithValue("@ID", id.Text);
    sqlcmd.Parameters.AddWithValue("@ITEMNAME", ITEMNAME);
    sqlcmd.Parameters.AddWithValue("@HSN", HSN);
    sqlcmd.Parameters.AddWithValue("@UNITPRICE", UNITPRICE);
    sqlcmd.Parameters.AddWithValue("@QUANT", QUANT);
    sqlcmd.Parameters.AddWithValue("@AMOUNT", AMOUNT);
    sqlcmd.ExecuteNonQuery();
}
               result = 1;
            
            }
        
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
      
           
        }

        private void id_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

        }

        private void itemcost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))  && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;

         
         
        }

        private void amountpaidtostore_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;
        }

        private void taxpaid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;
        }

        private void taxafterdiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;
        }

        private void Amountcollectedfromcustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;
        }

        private void extramoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;
        }

        private void refundamount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                e.Handled = true;
        }

        private void billnumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)))
                e.Handled = true;
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

           
                Filterorder ft = new Filterorder();
                ft.ShowDialog();
            
        }
        private static RotateFlipType GetOrientationToFlipType(int orientationValue)
        {
            RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

            switch (orientationValue)
            {
                case 1:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    rotateFlipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    rotateFlipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    rotateFlipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    rotateFlipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    rotateFlipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    rotateFlipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return rotateFlipType;
        }
        void A()
        {
            Thread.Sleep(2500);
            pictureBox1.Image = Image.FromFile(imagefile);
        }
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {



            
              
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
                if (open.ShowDialog() == DialogResult.OK)
                {
                   imagefile = open.FileName;

                    //    MessageBox.Show(open.FileName);
                   Image img = Image.FromFile(imagefile);

                    foreach (var prop in img.PropertyItems)
                    {
                        if (prop.Id == 0x0112) //value of EXIF
                        {
                            int orientationValue = img.GetPropertyItem(prop.Id).Value[0];
                            RotateFlipType rotateFlipType = GetOrientationToFlipType(orientationValue);
                            img.RotateFlip(rotateFlipType);
                          //   img.Save(System.IO.Path.GetFileName(@"bill" + id.Text));

                            break;

                        }
                        else
                        {
                        //   img.Save(System.IO.Path.GetFileName(@"bill" + id.Text));

                            break;


                        }
                    }




                  //  imagefile = System.IO.Path.GetFileName(@"bill" + id.Text);


                    Thread thread1 = new Thread(new ThreadStart(A));

                    thread1.Start();

                    thread1.Join();

                }
         






            /*
            string PAYMENTTYPEONLINE = "";
            if (paymenttypeonline.SelectedIndex == 0)
                PAYMENTTYPEONLINE = "COD";

            if (paymenttypeonline.SelectedIndex == 1)
                PAYMENTTYPEONLINE = "CARD";
            if (paymenttypeonline.SelectedIndex == 2)
                PAYMENTTYPEONLINE = "PAYTM";
            if (paymenttypeonline.SelectedIndex == 3)
                PAYMENTTYPEONLINE = "Other";
            XtraReport1 xxx = new XtraReport1();

            xxx.Parameters["ID"].Value = id.Text;
            xxx.Parameters["date"].Value = date.Text;
            xxx.Parameters["customername"].Value = customername.Text;
            xxx.Parameters["deliveryarea"].Value = deliveryarea.Text;
            xxx.Parameters["deliverydistance"].Value = deliverydispatcher.Text;
            xxx.Parameters["itemcost"].Value = deliveryarea.Text;
            xxx.Parameters["paymenttype"].Value = PAYMENTTYPEONLINE;
            xxx.Parameters["storename"].Value = storename.Text;
            xxx.Parameters["deliverydispatcher"].Value = deliverydispatcher.Text;

            xxx.Parameters["deliverydispatcher"].Visible = false;
            xxx.Parameters["ID"].Visible = false;
            xxx.Parameters["date"].Visible = false;
            xxx.Parameters["customername"].Visible = false;
            xxx.Parameters["deliveryarea"].Visible = false;
            xxx.Parameters["deliverydistance"].Visible = false;
            xxx.Parameters["itemcost"].Visible = false;
            xxx.Parameters["paymenttype"].Visible = false;
            xxx.Parameters["storename"].Visible = false;
            ReportPrintTool printTool2 = new ReportPrintTool(xxx);
            printTool2.ShowPreview();*/
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Delivery_Charge dc = new Delivery_Charge();
            dc.ShowDialog();
        }
        public void displayi()
        {
            int m = 0;
      
            string sql = "select * from orderi where id='" + id.Text + "'";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                m = 1;
                string YYY = reader[19].ToString();
                date.Text = reader["date"].ToString();
                customername.Text = reader["customername"].ToString();
                storename.Text = reader["storename"].ToString();
                deliveryarea.Text = reader["deliveryarea"].ToString();
                itemcost.Text = reader["itemcost"].ToString();
                Amountcollectedfromcustomer.Text = reader["Amountcollectedfromcustomer"].ToString();
                amountpaidtostore.Text = reader["amountpaidtostore"].ToString();
                taxpaid.Text = reader["taxpaid"].ToString();
                deliverycorrdinator.Text = reader["deliverycoordinator"].ToString();
                if (reader["taxafterdiscount"].ToString()=="1")
                taxafterdiscount.Text ="YES" ;
                else
                    taxafterdiscount.Text = "NO";
                string PAYMENTTYPEONLINE = reader["paymenttypeonline"].ToString();
                if (PAYMENTTYPEONLINE == "COD")

                    paymenttypeonline.SelectedIndex = 0;
                if (PAYMENTTYPEONLINE == "CARD")
                    paymenttypeonline.SelectedIndex = 1;
                if (PAYMENTTYPEONLINE == "PAYTM")
                    paymenttypeonline.SelectedIndex = 2;
                if (PAYMENTTYPEONLINE == "Other")
                    paymenttypeonline.SelectedIndex = 3;

                extramoney.Text = reader["extramoney"].ToString();
                refundamount.Text = reader["refundamount"].ToString();
                deliverydispatcher.Text = reader["deliverydispatcher"].ToString();
                billnumber.Text = reader["billnumber"].ToString();

                comment.Text = reader["comment"].ToString();

                charge.Text = reader["deliverycharge"].ToString();
                hotelserviceag.Text = reader["hotelserivceagency"].ToString();

                charityamou.Text = reader["charityamount"].ToString();
                dicountt.Text = reader["discount"].ToString();


                pictureBox1.Image = byteArrayToImage((Byte[])reader[19]);
                reader.Close();
               
            }
            if(m==1)
            {
               string p1="0";
                string p2="0";
                string p3="0";

  p1= (GetDrivingDistanceInMiles(getareaorginfromdb(deliveryarea.Text), getareaorginfromstore(storename.Text)).ToString()) + "";
  p2 = (GetDrivingDistanceInMiles(getfixed(), getareaorginfromstore(storename.Text)).ToString()) + "";
  p3 = (GetDrivingDistanceInMiles(getareaorginfromdb(deliveryarea.Text), getfixed()).ToString()) + "";
            

                           distance.Text=(double.Parse(p1)+double.Parse(p2)+double.Parse(p2)).ToString();
            }
          sql = "select * from orderidet where id='" + id.Text + "'";
          reader.Close();

            command = new MySqlCommand(sql, con);
            reader = command.ExecuteReader();
            while (reader.Read())
            { 
            
            dataGridView1.Rows.Add(reader[0].ToString(),reader[1].ToString(),reader[2].ToString(),reader[3].ToString(),reader[4].ToString());
            
            }

            reader.Close();

            id.Select();
           
        }
        private void id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                neworder();
                displayi();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string sql = "select * from orderi where id='" + id.Text + "'";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {

                MessageBox.Show("Order id does not exists");
                reader.Close();

            }
            else
            {
                reader.Close();
                 sql = "delete  from orderi where id='" + id.Text + "'";
                var sqlcmd = con.CreateCommand();
                sqlcmd.CommandText = sql;
                sqlcmd.ExecuteNonQuery();
                sql = "delete  from orderidet where id='" + id.Text + "'";
                sqlcmd.CommandText = sql;
                sqlcmd.ExecuteNonQuery();
                insert();
                if (result == 1)
                    MessageBox.Show("Order upadte with success");
                result = 0;
            
            }
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            delete_order dor = new delete_order(this);
            dor.ShowDialog();

        }

        private void id_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void itemcost_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void taxafterdiscount_KeyPress_1(object sender, KeyPressEventArgs e)
        {
               e.Handled = true;
        }

        private void Amountcollectedfromcustomer_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (Amountcollectedfromcustomer.Text.Contains('.'))
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))

                    e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
        }

        private void itemcost_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (itemcost.Text.Contains('.'))
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))

                    e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
        }

        private void amountpaidtostore_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (amountpaidtostore.Text.Contains('.'))
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))

                    e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
        }

        private void taxpaid_KeyPress_1(object sender, KeyPressEventArgs e)
        {

            if (taxpaid.Text.Contains('.'))
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))

                    e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
        }

        private void extramoney_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (extramoney.Text.Contains('.'))
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))

                    e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
        }

        private void refundamount_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (refundamount.Text.Contains('.'))
            { 
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar))  && (e.KeyChar != '-'))
               
                e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
        }

        private void billnumber_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if ( (!char.IsDigit(e.KeyChar)))
               
                e.Handled = true;
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            double p = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                p = p + double.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
            TOTALAMOUNT = p;
            double bass = 0;
            if (distance.Text == "")
                distance.Text = "0";
            DELIVERYCHARGE = double.Parse(distance.Text) * 2 * xx;
            bass = TOTALAMOUNT + DISCOUNT + -DELIVERYCHARGE;

            MIGST = bass * (IGST / 100);
            MCGST = bass * (CGST / 100);
           MSGST = bass * (SGST / 100);

            TOTALPAYABLE = bass + MIGST + MSGST + MSGST + HOTELSERVICE + CHARITYAMOUNT;
            if (dicountt.Text == "")
                DISCOUNT = 0;
            else
                DISCOUNT = double.Parse(dicountt.Text);

            if (dicountt.Text == "")
                DISCOUNT = 0;
            else
                DISCOUNT = double.Parse(dicountt.Text);

            if (hotelserviceag.Text == "")
             HOTELSERVICE = 0;
            else
                HOTELSERVICE = double.Parse(hotelserviceag.Text);

            if (charge.Text == "")
               DELIVERYCHARGE = 0;
            else
                DELIVERYCHARGE = double.Parse(charge.Text);


            if (charityamou.Text == "")
             CHARITYAMOUNT = 0;
            else
                CHARITYAMOUNT = double.Parse(charityamou.Text);









            string PAYMENTTYPEONLINE = "";
            if (paymenttypeonline.SelectedIndex == 0)
                PAYMENTTYPEONLINE = "COD";

            if (paymenttypeonline.SelectedIndex == 1)
                PAYMENTTYPEONLINE = "CARD";
            if (paymenttypeonline.SelectedIndex == 2)
                PAYMENTTYPEONLINE = "PAYTM";
            if (paymenttypeonline.SelectedIndex == 3)
                PAYMENTTYPEONLINE = "Other";

            List<FactureData> Listfacture = new List<FactureData>();
            for (int i = 0; i < dataGridView1.Rows.Count-1;i++ )
                Listfacture.Add(new FactureData(dataGridView1.Rows[i].Cells[0].Value.ToString(), dataGridView1.Rows[i].Cells[1].Value.ToString(), dataGridView1.Rows[i].Cells[2].Value.ToString(), dataGridView1.Rows[i].Cells[3].Value.ToString(), dataGridView1.Rows[i].Cells[4].Value.ToString(), "", "dd", "", "", ""));

            XtraReport1 xxx = new XtraReport1();

          
        
           
            xxx.Parameters["deliveryarea"].Value = deliveryarea.Text;
            xxx.Parameters["deliverydistance"].Value = distance.Text;
     
        
            xxx.Parameters["storename"].Value = storename.Text;
            xxx.Parameters["deliverydispatcher"].Value = deliverydispatcher.Text;


            xxx.Parameters["ID"].Value = id.Text;
            xxx.Parameters["date"].Value = date.Text;
            xxx.Parameters["customername"].Value = customername.Text;
            xxx.Parameters["bill"].Value = id.Text;
            xxx.Parameters["CGST"].Value = "CGST @" + CGST.ToString() + "% :";
            xxx.Parameters["MCGST"].Value = MCGST.ToString() ;
            xxx.Parameters["charityamount"].Value = CHARITYAMOUNT.ToString();
            xxx.Parameters["discount"].Value = DISCOUNT.ToString();
            xxx.Parameters["deliverycharge"].Value = DELIVERYCHARGE.ToString();
            xxx.Parameters["GSTIN"].Value = GSTIN;
            xxx.Parameters["hotelservicecharge"].Value = HOTELSERVICE.ToString();
            xxx.Parameters["IGST"].Value = "IGST @" + IGST.ToString() + "% :";
            xxx.Parameters["MIGST"].Value =  MIGST.ToString();
            xxx.Parameters["itemcost"].Value = "";
           
            xxx.Parameters["paymentmethod"].Value = PAYMENTTYPEONLINE;
            xxx.Parameters["paymenttype"].Value = PAYMENTTYPEONLINE;

            xxx.Parameters["SGST"].Value = "SGST @" + SGST.ToString() + "% :";
            xxx.Parameters["MSGST"].Value =  MSGST.ToString();
            xxx.Parameters["totalamount"].Value = TOTALAMOUNT.ToString();
            xxx.Parameters["totalpayable"].Value = TOTALPAYABLE.ToString();

            xxx.Parameters["deliveryarea"].Visible = false;
            xxx.Parameters["deliverydistance"].Visible = false;


            xxx.Parameters["storename"].Visible = false;
            xxx.Parameters["deliverydispatcher"].Visible = false;
            xxx.Parameters["parameter1"].Visible = false;
            xxx.Parameters["ID"].Visible = false;
            xxx.Parameters["date"].Visible = false;
            xxx.Parameters["customername"].Visible = false;
            xxx.Parameters["bill"].Visible = false;
            xxx.Parameters["CGST"].Visible = false;
            xxx.Parameters["MCGST"].Visible = false;
            xxx.Parameters["charityamount"].Visible = false;
            xxx.Parameters["discount"].Visible = false;
            xxx.Parameters["deliverycharge"].Visible = false;
            xxx.Parameters["GSTIN"].Visible = false;
            xxx.Parameters["hotelservicecharge"].Visible = false;
            xxx.Parameters["IGST"].Visible = false;
            xxx.Parameters["MIGST"].Visible = false;
            xxx.Parameters["itemcost"].Visible = false;

            xxx.Parameters["paymentmethod"].Visible = false;
            xxx.Parameters["paymenttype"].Visible = false;

            xxx.Parameters["SGST"].Visible = false;
            xxx.Parameters["MSGST"].Visible = false;
            xxx.Parameters["totalamount"].Visible = false;
            xxx.Parameters["totalpayable"].Visible = false;
            xxx.bindingSource1.DataSource = Listfacture;
            ReportPrintTool printTool2 = new ReportPrintTool(xxx);
            printTool2.ShowPreview();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            double p = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                p = p + double.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
            TOTALAMOUNT = p;
            dataGridView1.Rows.Add(textEdit2.Text, textEdit4.Text, textEdit5.Text, textEdit3.Text,textEdit6.Text);
    
        
        }

        private void textEdit5_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (textEdit5.Text.Contains('.'))
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))

                    e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
        }

        private void textEdit3_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (textEdit3.Text.Contains('.'))
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))

                    e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (textEdit3.Text == "")
                    textEdit3.Text = "0";
                if (textEdit5.Text == "")
                    textEdit5.Text = "0";
                textEdit6.Text = ((double.Parse(textEdit3.Text)) * (double.Parse(textEdit5.Text))).ToString();
           


            }
        }

        private void textEdit6_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (textEdit6.Text.Contains('.'))
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))

                    e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
        }

        private void ribbonStatusBar_Click(object sender, EventArgs e)
        {

        }

        private void distance_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void deliverycorrdinator_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void charge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textEdit6.Text.Contains('.'))
            {
                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '-'))

                    e.Handled = true;
            }


            else
            {

                if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar != '.') && (e.KeyChar != '-'))

                    e.Handled = true;

            }
        }

        private void w(object sender, EventArgs e)
        {

            string sql = "select * from store  ORDER by `Storename` ASC";
            MySqlCommand command = new MySqlCommand(sql, con);

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                storename.Items.Add(reader[0].ToString());

            }
            reader.Close();
            command = null;
            sql = " select * from delivreyarea  ORDER by `area` ASC";
            command = new MySqlCommand(sql, con);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                deliveryarea.Items.Add(reader[0].ToString());

            }
            reader.Close();
            command = null;
            sql = " select * from deliverydispatcher  ORDER by `Dispatcher` ASC";
            command = new MySqlCommand(sql, con);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                deliverydispatcher.Items.Add(reader[0].ToString());

            }
            reader.Close();
            sql = " select * from pa";
            command = new MySqlCommand(sql, con);
            reader = command.ExecuteReader();

            if (reader.Read())
                xx = int.Parse(reader[0].ToString());

            reader.Close();
            command = null;
            storename.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            storename.AutoCompleteSource = AutoCompleteSource.ListItems;
            deliveryarea.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            deliveryarea.AutoCompleteSource = AutoCompleteSource.ListItems;
            deliverydispatcher.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            deliverydispatcher.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        public void neworder()

        {
        
                date.Text = "";
                customername.Text =  "";
                storename.Text = "";
                deliveryarea.Text =  "";
                itemcost.Text =  "";
                Amountcollectedfromcustomer.Text = "";
                amountpaidtostore.Text =  "";
                taxpaid.Text =  "";
           
 

                extramoney.Text =  "";
                refundamount.Text = "";
                deliverydispatcher.Text =  "";
                billnumber.Text = "";

                comment.Text =  "";

                charge.Text = "";
                hotelserviceag.Text =  "";

                charityamou.Text =  "";
                dicountt.Text =  "";


                deliverycorrdinator.Text = "";
              
                textEdit2.Text = "";
                textEdit3.Text = "";
                textEdit4.Text = "";
                textEdit5.Text = "";
                textEdit6.Text = "";


                distance.Text = "";

                foreach (Control textEdit in  this.Controls)
                {
                    if (textEdit is TextEdit)
                    {
                        (textEdit as TextEdit).ResetText();
                    }
                }
           
            foreach (Control x in this.Controls)
            {
                if (x is TextBox)
                {
                    ((TextBox)x).Text = String.Empty;
                }
            }
            foreach (Control x in this.Controls)
            {
                if (x is System.Windows.Forms.ComboBox)
                {
                    ((System.Windows.Forms.ComboBox)x).Text = String.Empty;
                }
            }
            foreach (Control x in this.Controls)
            {
                if (x is DevExpress.XtraEditors.ComboBoxEdit)
                {
                    ((DevExpress.XtraEditors.ComboBoxEdit)x).Text = String.Empty;
                }
            }
            dataGridView1.Rows.Clear();

            date.Text = DateTime.Now.Date.ToString();

            pictureBox1.Image = null;
        }
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            id.Text = "";

    
            neworder();
            deliverycorrdinator.Text = user;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Form2 fff = new Form2(pictureBox1.Image);
            fff.ShowDialog();
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (user.ToUpper() == "ADMIN")
            {
                Dsipatcher_screen ds = new Dsipatcher_screen();
                ds.ShowDialog();
            }
        }
    }
}
