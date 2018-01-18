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
    public partial class Form2 : Form
    {
        public Form2(  Image pp)
        {
            InitializeComponent();

            pictureEdit1.Image = pp;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
