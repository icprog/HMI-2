using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using NetSCADA6.NSInterface.HMI.Form;

namespace NetSCADA6.HMI.NSDrawObj
{
    abstract public class DrawWindow : DrawObj
    {
		#region property
		public Control WindowControl { set; get; }
		public override IHMIForm Parant
		{
			set
			{
				base.Parant = value;
				WindowControl.Parent = Parant as Form;
				WindowControl.Show();
			}
		}
		#endregion

		#region public function
        #endregion

        #region protected function
        //protected override void LoadGeneratePathEvent()
        //{
        //    if (WindowControl != null)
        //    {
        //        WindowControl.Location = new Point((int)Rect.X, (int)Rect.Y);
        //        WindowControl.Size = new Size((int)Rect.Width, (int)Rect.Height);
        //    }

        //    base.LoadGeneratePathEvent();;
        //}
        #endregion

        #region serialize
        public override void Serialize(BinaryFormatter bf, Stream s)
        {
            base.Serialize(bf, s);

            const int version = 1;

            bf.Serialize(s, version);
        }
        public override void Deserialize(BinaryFormatter bf, Stream s)
        {
            base.Deserialize(bf, s);

            int version = (int)bf.Deserialize(s);
        }
        #endregion

    }
}
