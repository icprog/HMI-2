using System;

namespace Toolbox
{
	/// <summary>
	/// ToolboxItem.
	/// </summary>
	internal class ToolboxItem
	{
		private string m_name = null;
		private Type m_type = null;

		public ToolboxItem()
		{
		}

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public Type Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}

	}
}
