using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 

namespace NetSCADA6.HMI.NSDrawNodes
{
    public partial class StreamDialog : Form
    {
        public StreamDialog(StreamControl lf)
        {
            _lineFlow = lf;
            InitializeComponent();
            UpdateData(true);
        }
        private StreamControl _lineFlow;
        public StreamControl LineFlow
        {
            get
            {
                return _lineFlow;
            }
        }

        public void UpdateData(bool ToControl)
        {
            if (ToControl)
            {
                int i = 0;
                if (_lineFlow.Enable) i = 1; else i = 0;
                textBox1.Text = i.ToString();
                if (_lineFlow.IsForward) i = 1; else i = 0;
                textBox2.Text = i.ToString();
                textBox3.Text = _lineFlow.Interval.ToString();
                HScrollBarUserControl1.Value = _lineFlow.StepLength;
            }
            else
            {
                _lineFlow.IsForward = int.Parse(textBox2.Text) != 0;
                int interval = int.Parse(textBox3.Text);
                if (interval <= 0) interval = 1;
                if (interval > 1000) interval = 1000;
                _lineFlow.Interval = interval;

                _lineFlow.StepLength = (int)HScrollBarUserControl1.Value;
                _lineFlow.Enable = int.Parse(textBox1.Text) != 0;
            }
        }


        private void button_ok_Click(object sender, EventArgs e)
        {
            UpdateData(false);
        }
    }
}
