using System;
using System.Xml;
using System.Windows.Forms;

namespace Toolbox
{
	/// <summary>
	/// ToolboxXmlManager - Reads an XML file and populates the toolbox.
	/// </summary>
	internal class ToolboxXmlManager
	{
		Toolbox m_toolbox = null;
		public ToolboxXmlManager(Toolbox toolbox)
		{
			m_toolbox = toolbox;
		}

		public ToolboxTabCollection PopulateToolboxInfo()
		{
			try
			{
				return null;

				//XmlDocument xmlDocument = new XmlDocument();
				//xmlDocument.Load(Toolbox.FilePath);
				//return PopulateToolboxTabs(xmlDocument);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error occured in reading Toolbox.xml file.\n" + ex.ToString());
				return null;
			}
		}

		private Toolbox Toolbox
		{
			get
			{
				return m_toolbox;
			}
		}
		public void AddTab(string tabName, Type[] itemTypes)
		{
			
		}
		private ToolboxTabCollection PopulateToolboxTabs(XmlDocument xmlDocument)
		{
			if(xmlDocument==null)
				return null;

			XmlNode toolboxNode = xmlDocument.FirstChild;
			if(toolboxNode==null)
				return null;

			XmlNode tabCollectionNode = toolboxNode.FirstChild;
			if(tabCollectionNode==null)
				return null;

			XmlNodeList tabsNodeList = tabCollectionNode.ChildNodes;
			if(tabsNodeList==null)
				return null;

			ToolboxTabCollection toolboxTabs = new ToolboxTabCollection();

			foreach(XmlNode tabNode in tabsNodeList)
			{
				if(tabNode==null)
					continue;

				XmlNode propertiesNode = tabNode.FirstChild;
				if(propertiesNode==null)
					continue;

				XmlNode nameNode = propertiesNode[Strings.Name];
				if(nameNode==null)
					continue;

				ToolboxTab toolboxTab = new ToolboxTab();
				toolboxTab.Name = nameNode.InnerXml.ToString();
				PopulateToolboxItems(tabNode, toolboxTab);
				toolboxTabs.Add(toolboxTab);
			}
			if(toolboxTabs.Count==0)
				return null;

			return toolboxTabs;
		}

		private void PopulateToolboxItems(XmlNode tabNode, ToolboxTab toolboxTab)
		{
			if(tabNode==null)
				return;

			XmlNode toolboxItemCollectionNode = tabNode[Strings.ToolboxItemCollection];
			if(toolboxItemCollectionNode==null)
				return;

			XmlNodeList toolboxItemNodeList = toolboxItemCollectionNode.ChildNodes;
			if(toolboxItemNodeList==null)
				return;

			ToolboxItemCollection toolboxItems = new ToolboxItemCollection();

			foreach(XmlNode toolboxItemNode in toolboxItemNodeList)
			{
				if(toolboxItemNode==null)
					continue;

				XmlNode typeNode = toolboxItemNode[Strings.Type];
				if(typeNode==null)
					continue;

				bool found = false;
				System.Reflection.Assembly[] loadedAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();
				for(int i=0; i<loadedAssemblies.Length && !found;i++)
				{
					System.Reflection.Assembly assembly = loadedAssemblies[i];
					System.Type[] types = assembly.GetTypes();
					for(int j=0;j<types.Length && !found;j++)
					{
						System.Type type = types[j];
						if(type.FullName == typeNode.InnerXml.ToString()) 
						{
							ToolboxItem toolboxItem = new ToolboxItem();
							toolboxItem.Type = type;
							toolboxItems.Add(toolboxItem);
							found = true;
						}
					}
				}
			}
			toolboxTab.ToolboxItems = toolboxItems;
			return;
		}

		private class Strings
		{
			public const string Toolbox = "Toolbox";
			public const string TabCollection = "TabCollection";
			public const string Tab = "Tab";
			public const string Properties = "Properties";
			public const string Name = "Name";
			public const string ToolboxItemCollection = "ToolboxItemCollection";
			public const string ToolboxItem = "ToolboxItem";
			public const string Type = "Type";
			public const string WindowsForms = "Windows Forms";
			public const string Components = "Components";
			public const string Data = "Data";
			public const string UserControls = "User Controls";
		}

	}// class
}// namespace
