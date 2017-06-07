using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetSCADA6.HMI.NSColorDialog
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent(); 
            
        }
        BrushData brushData = new BrushData(); 
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }

        //手动测试
        private void button1_Click(object sender, EventArgs e)
        {
            BrushDialog dlg = new BrushDialog(brushData); 
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                brushData = dlg.BrushData;
            }
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Brush br = brushData.CreateBrush(panel1.ClientRectangle, null);
            if (br != null)
            {
                e.Graphics.FillRectangle(br, ClientRectangle);
                br.Dispose();
            }

        }
        PenData pd = new PenData();
        private void button2_Click(object sender, EventArgs e)
        {
            PenDialog dlg = new PenDialog(pd);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pd = dlg.penData;
            }
        } 

    }
}