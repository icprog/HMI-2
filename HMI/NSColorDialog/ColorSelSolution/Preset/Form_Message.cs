using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetSCADA6.Common.NSColorManger
{
    internal partial class Form_Message : Form
    {
        public Form_Message()
        {
            InitializeComponent();
        }
       public  string Label = "";

       protected override void OnLoad(EventArgs e)
       {
           base.OnLoad(e);
           textBox1.Text = Label;
       }

        private void button1_Click(object sender, EventArgs e)
        {
            Label = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

    }
}
