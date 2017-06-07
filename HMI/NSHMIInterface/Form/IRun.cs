using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSCADA6.NSInterface.HMI.Form
{
	public interface IRun : IEnvironment
	{
		void OnDataChanged(string name, double value);
		void OnDataChanged(string name, string value);
	}
}
