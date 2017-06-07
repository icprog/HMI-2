//by hdp 2013.11.29
//注意：如果操作文件路径是目录，最后不能带'\',否则部分代码会报异常

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace NetSCADA6.HMI.NSHMIFramework
{
	/// <summary>
	/// 节点操作类，用于图形窗体管理
	/// </summary>
	internal class FileNode
	{
		#region initialize
		private static readonly string[] NodeKeys;
		private static readonly Icon[] NodeImages;
		static FileNode()
		{
			NodeImages = new[] { Rs.iRoot, Rs.iDir, Rs.iFile };
			NodeKeys = new[] { "HMI.Root", "HMI.Dir", "HMI.File" };
		}

		public FileNode()
		{
			InitMenu();
		}
		/// <summary>
		/// 根据root和framework生成一个文件树
		/// </summary>
		/// <param name="root"></param>
		/// <param name="framework"></param>
		public void InitNodes(TreeNode root, HMIFramework framework)
		{
			Debug.Assert(root != null && root.TreeView != null);

			InitTreeView(root);
			InitRoot(root, framework);

			//file
			string project = framework.HMIPath;
			if (!Directory.Exists(project))
				Directory.CreateDirectory(project);
			_treeView.BeginUpdate();
			GetNodes(project, "*" + Rs.sExtension, root);
			_treeView.EndUpdate();
		}

		private void InitTreeView(TreeNode root)
		{
			if (_treeView != null)
				return;

			_treeView = root.TreeView;
			//bind event
			_treeView.DoubleClick += TreeViewDoubleClick;
			_treeView.AfterLabelEdit += TreeViewAfterLabelEdit;
			_treeView.NodeMouseClick += TreeViewNodeMouseClick;
			_treeView.KeyDown += TreeViewKeyDown;
			//ImageList
			for (int i = 0; i < NodeKeys.Length; i++)
				_treeView.ImageList.Images.Add(NodeKeys[i], NodeImages[i]);
		}
		private void InitRoot(TreeNode root, HMIFramework framework)
		{
			root.Nodes.Clear();
			root.Text = Rs.sRootName;
			root.Tag = framework;
			root.ContextMenuStrip = _menuStripNode;
			root.ImageKey = NodeKeys[0];
			root.SelectedImageKey = NodeKeys[0];
		}
		#region init menu
		private readonly ContextMenuStrip _menuStripNode = new ContextMenuStrip();
		private readonly ToolStripMenuItem _menuItemAddFile = new ToolStripMenuItem();
		private readonly ToolStripMenuItem _menuItemAddDir = new ToolStripMenuItem();
		private readonly ToolStripSeparator _menuItemSeparator = new ToolStripSeparator();
		private readonly ToolStripMenuItem _menuItemCopy = new ToolStripMenuItem();
		private readonly ToolStripMenuItem _menuItemPaste = new ToolStripMenuItem();
		private readonly ToolStripMenuItem _menuItemDel = new ToolStripMenuItem();
		private readonly ToolStripMenuItem _menuItemRename = new ToolStripMenuItem();

		private void InitMenu()
		{
			// MenuStripNode
			_menuStripNode.Items.AddRange(new ToolStripItem[] {
            _menuItemAddFile,
            _menuItemAddDir,
            _menuItemSeparator,
            _menuItemCopy,
            _menuItemPaste,
            _menuItemDel,
            _menuItemRename});
			_menuStripNode.Name = "MenuStripNode";
			_menuStripNode.Size = new Size(136, 142);
			_menuStripNode.Opening += MenuStripNodeOpening;
			// MenuItemAddFile
			_menuItemAddFile.Image = Rs.iAddFile.ToBitmap();
			_menuItemAddFile.Size = new Size(135, 22);
			_menuItemAddFile.Text = Rs.sAddFile;
			_menuItemAddFile.Click += MenuItemAddFileClick;
			// MenuItemAddDir
			_menuItemAddDir.Image = Rs.iAddDir.ToBitmap();
			_menuItemAddDir.Size = new Size(135, 22);
			_menuItemAddDir.Text = Rs.sAddDirectory;
			_menuItemAddDir.Click += MenuItemAddDirClick;
			// toolStripMenuItem1
			_menuItemSeparator.Size = new Size(132, 6);
			// MenuItemCopy
			_menuItemCopy.Image = Rs.iCopy.ToBitmap();
			_menuItemCopy.ShortcutKeys = Keys.Control | Keys.C;
			_menuItemCopy.Size = new Size(135, 22);
			_menuItemCopy.Text = Rs.sCopy;
			_menuItemCopy.Click += MenuItemCopyClick;
			// MenuItemPaste
			_menuItemPaste.Image = Rs.iPaste.ToBitmap();
			_menuItemPaste.ShortcutKeys = Keys.Control | Keys.V;
			_menuItemPaste.Size = new Size(135, 22);
			_menuItemPaste.Text = Rs.sPaste;
			_menuItemPaste.Click += MenuItemPasteClick;
			// MenuItemDel
			_menuItemDel.Image = Rs.iDelete.ToBitmap();
			_menuItemDel.ShortcutKeys = Keys.Delete;
			_menuItemDel.Size = new Size(135, 22);
			_menuItemDel.Text = Rs.sDelete;
			_menuItemDel.Click += MenuItemDelClick;
			// MenuItemRename
			_menuItemRename.Size = new Size(135, 22);
			_menuItemRename.Text = Rs.sRename;
			_menuItemRename.Click += MenuItemRenameClick;
		}
		#endregion
		#endregion

		#region enum
		private enum FileType { Dir, File }
		private enum NodeType { Root, Dir, File }
		private enum NodeOperate { AddFile, AddDir, Rename }
		#endregion

		#region field
		private TreeView _treeView;
		#endregion

		#region property
		protected TreeNode SelectNode { get { return _treeView.SelectedNode; } }
		#endregion

		#region node operate
		private static TreeNode GetRoot(TreeNode node)
		{
			while (node != null)
			{
				if (node.Tag is HMIFramework)
					return node;
				node = node.Parent;
			}

			return null;
		}
		private static bool IsRoot(TreeNode node)
		{
			return (node.Tag is HMIFramework);
		}
		private static bool IsHmiNode(TreeNode node)
		{
			return ((node.Tag as string) == Rs.sNodeSign
				|| IsRoot(node));
		}
		private TreeNode CreateNode(string fullPath, FileType type)
		{
			//get name
			string name = fullPath;
			int index = fullPath.LastIndexOf(@"\");
			if (index != -1 && ((index + 1) < fullPath.Length))
				name = fullPath.Substring(index + 1);

			//create
			TreeNode node = new TreeNode(name) { Tag = Rs.sNodeSign, ContextMenuStrip = _menuStripNode };
			string key = (type == FileType.Dir) ? NodeKeys[1] : NodeKeys[2];
			node.ImageKey = key;
			node.SelectedImageKey = key;
			return node;
		}
		private TreeNode CloneNode(TreeNode source)
		{
			TreeNode node = new TreeNode(source.Text) { Tag = Rs.sNodeSign, ContextMenuStrip = _menuStripNode };
			string key = (GetFileType(source) == FileType.Dir) ? NodeKeys[1] : NodeKeys[2];
			node.ImageKey = key;
			node.SelectedImageKey = key;
			return node;
		}
		private static void InsertSortedNode(TreeNode parant, TreeNode child)
		{
			FileType type = GetFileType(child);
			int index = 0;
			for (; index < parant.Nodes.Count; index++)
			{
				if (GetFileType(parant.Nodes[index]) != type)
				{
					if (type == FileType.Dir)
						break;
					if (type == FileType.File)
						continue;
				}

				if (string.Compare(parant.Nodes[index].Text, child.Text, true) > 0)
					break;
			}

			parant.Nodes.Insert(index, child);
		}
		private void GetNodes(string path, string pattern, TreeNode parant)
		{
			//dir
			string[] dirs = Directory.GetDirectories(path);
			Array.Sort(dirs);
			foreach (string dir in dirs)
			{
				TreeNode node = CreateNode(dir, FileType.Dir);
				parant.Nodes.Add(node);
				GetNodes(dir, pattern, node);
			}

			//file
			string[] files = Directory.GetFiles(path, pattern);
			Array.Sort(files);
			foreach (string file in files)
			{
				parant.Nodes.Add(CreateNode(file, FileType.File));
			}
		}
		private void CopyChildNodes(TreeNode source, TreeNode target)
		{
			foreach (TreeNode node in source.Nodes)
			{
				TreeNode cloneNode = CloneNode(node);
				target.Nodes.Add(cloneNode);
				CopyChildNodes(node, cloneNode);
			}
		}
		private static HMIFramework GetFramework(TreeNode node)
		{
			TreeNode root = GetRoot(node);
			if (root != null)
				return (HMIFramework)root.Tag;

			return null;
		}
		#endregion

		#region file operate
		private static FileType GetFileType(TreeNode node)
		{
			if (IsRoot(node)
				|| string.Compare(node.ImageKey, NodeKeys[1], true) == 0)
				return FileType.Dir;

			return FileType.File;
		}
		private static string GetFilePath(TreeNode node)
		{
			string project = ((HMIFramework)(GetRoot(node).Tag)).HMIPath;
			StringBuilder path = new StringBuilder();
			if (GetFileType(node) == FileType.Dir)
				path.Insert(0, @"\");
			while (!IsRoot(node))
			{
				path.Insert(0, node.Text);
				path.Insert(0, @"\");
				node = node.Parent;
			}
			path.Insert(0, project);

			if (path[path.Length - 1] == '\\')
				path.Remove(path.Length - 1, 1);

			return path.ToString();
		}
		private static void CopyFolder(string source, string target)
		{
			if (!Directory.Exists(target))
				Directory.CreateDirectory(target);

			DirectoryInfo direcInfo = new DirectoryInfo(source);
			FileInfo[] files = direcInfo.GetFiles();
			foreach (FileInfo file in files)
				file.CopyTo(Path.Combine(target, file.Name), true);

			DirectoryInfo[] direcInfoArr = direcInfo.GetDirectories();
			foreach (DirectoryInfo dir in direcInfoArr)
				CopyFolder(Path.Combine(source, dir.Name), Path.Combine(target, dir.Name));
		}
		#endregion

		#region menu
		private static TreeNode _copyNode;
		private NodeOperate _operateType;

		private void SetMenuState(NodeType type)
		{
			_menuItemCopy.Visible = true;
			_menuItemRename.Visible = true;
			_menuItemDel.Visible = true;
			_menuItemAddFile.Visible = true;
			_menuItemAddDir.Visible = true;
			_menuItemPaste.Visible = true;
			
			_menuItemPaste.Enabled = IsValidCopy(_copyNode);
			
			switch (type)
			{
				case NodeType.Root:
					_menuItemCopy.Visible = false;
					_menuItemRename.Visible = false;
					_menuItemDel.Visible = false;
					break;
				case NodeType.Dir:
					break;
				case NodeType.File:
					_menuItemAddFile.Visible = false;
					_menuItemAddDir.Visible = false;
					_menuItemPaste.Visible = false;
					break;
			}
		}
		private static bool IsValidCopy(TreeNode copyNode)
		{
			return copyNode != null && copyNode.TreeView != null;
		}
		private static string CreateNewName(TreeNode parentNode, FileType type)
		{
			int index = 1;
			bool hasRepeat = true;
			StringBuilder name = new StringBuilder();
			while (hasRepeat)
			{
				name.Clear();
				if (type ==FileType.File)
					name.Append(Rs.sNewFileName + index + Rs.sExtension);
				else
					name.Append(Rs.sNewDirName + index);
				index++;

				hasRepeat = parentNode.Nodes.Cast<TreeNode>().Any(node => string.Compare(name.ToString(), node.Text, true) == 0);
			}

			return name.ToString();
		}
		private static void CreateCopyName(TreeNode parentNode, TreeNode newNode)
		{
			string name = newNode.Text;
			StringBuilder sb = new StringBuilder(newNode.Text);
			int removeCount = 0;
			int index = 1;
			//字符串符合如下格式"复件(2)"
			if (name.Length >= 5 && string.Compare(name.Substring(0, 3), Rs.sCopyName + "(") == 0)
			{
				int right = name.IndexOf(")");
				if (right != -1)
				{
					string num = name.Substring(3, right - 3);
					int result;
					bool success = int.TryParse(num, out result);
					if (success)
					{
						removeCount = right + 1;
						index = 2;
					}
				}
			}

			bool hasRepeat = true;
			while (hasRepeat)
			{
				hasRepeat = parentNode.Nodes.Cast<TreeNode>().Any(node => string.Compare(node.Text, sb.ToString(), true) == 0);

				if (!hasRepeat) continue;
				sb.Clear();
				if (index == 1)
					sb.Append(Rs.sCopyName + name);
				else
				{
					sb.Append(name);
					if (removeCount > 0)
						sb.Remove(0, removeCount);
					sb.Insert(0, Rs.sCopyName + "(" + index + ")");
				}
				index++;
			}
			newNode.Text = sb.ToString();
		}
		private static void NodeAfterLabelEditEvent(NodeOperate operate, TreeNode node, string oldFullName)
		{
			string fullName = GetFilePath(node);
			switch (operate)
			{
				case NodeOperate.AddFile:
						CreateForm(node);
					break;
				case NodeOperate.AddDir:
					{
						if (!Directory.Exists(fullName))
							Directory.CreateDirectory(fullName);
					}
					break;
				case NodeOperate.Rename:
					{
						if (string.Compare(fullName, oldFullName, true) == 0)	//no change
							return;
						
						RenameForms(node, oldFullName);
						if (GetFileType(node) == FileType.File)
						{
							if (File.Exists(fullName))
								return;
							if (File.Exists(oldFullName))
								File.Move(oldFullName, fullName);
						}
						else
						{
							if (Directory.Exists(fullName))
								return;
							if (Directory.Exists(oldFullName))
								Directory.Move(oldFullName, fullName);
						}
					}
					break;
				default:
					break;
			}
		}
		private static bool IsValidName(TreeNode node, ref string label, ref string message)
		{
			if (string.IsNullOrWhiteSpace(label))
			{
				message = Rs.sNullHint;
				return false;
			}

			//check extension
			if (GetFileType(node) == FileType.File)
			{
				if (label.Length <= 4)
					label += Rs.sExtension;
				else
				{
					string ext = label.Substring(label.Length - 4);
					if (string.Compare(ext, Rs.sExtension, true) != 0)
						label += Rs.sExtension;
				}
			}

			//check repeat
			foreach (TreeNode child in node.Parent.Nodes)
			{
				if (child != node && string.Compare(child.Text, label) == 0)
				{
					message = Rs.sRepeatHint;
					return false;
				}
			}

			return true;
		}
		private void AddEvent(FileType type)
		{
			TreeNode node = SelectNode;
			string fullName = GetFilePath(node) + @"\" +  CreateNewName(node, type);
			TreeNode createNode = CreateNode(fullName, type);
			InsertSortedNode(node, createNode);
			_treeView.SelectedNode = createNode;
			node.Expand();

			_operateType = (type == FileType.File) ? NodeOperate.AddFile : NodeOperate.AddDir;
			_treeView.LabelEdit = true;
			createNode.BeginEdit();
		}

		#region event
		private void MenuStripNodeOpening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			TreeNode node = SelectNode;
			NodeType type;
			if (IsRoot(node))
				type = NodeType.Root;
			else
				type = GetFileType(node) == FileType.Dir ? NodeType.Dir : NodeType.File;

			SetMenuState(type);
		}
		private void MenuItemAddFileClick(object sender, EventArgs e)
		{
			AddEvent(FileType.File);
		}
		private void MenuItemAddDirClick(object sender, EventArgs e)
		{
			AddEvent(FileType.Dir);
		}
		private void MenuItemCopyClick(object sender, EventArgs e)
		{
			_copyNode = SelectNode;
		}
		private void MenuItemPasteClick(object sender, EventArgs e)
		{
			TreeNode parent = SelectNode;

			//防止目标目录和源目录相同
			if (parent == _copyNode)
				parent = parent.Parent;
			//防止父目录拷贝到子目录中
			TreeNode node = parent;
			while (!IsRoot(node))
			{
				if (node.Parent == _copyNode)
				{
					MessageBox.Show(Rs.sSubToParentHint);
					return;
				}
				node = node.Parent;
			}

			//node
			string sourceName = GetFilePath(_copyNode);
			FileType type = GetFileType(_copyNode);
			TreeNode newNode = CreateNode(sourceName, type);
			CreateCopyName(parent, newNode);
			CopyChildNodes(_copyNode, newNode);
			InsertSortedNode(parent, newNode);
			parent.Expand();

			//file
			string destName = GetFilePath(newNode);
			if (type == FileType.File)
			{
				if (File.Exists(sourceName) && !File.Exists(destName))
					File.Copy(sourceName, destName);
			}
			else
			{
				if (Directory.Exists(sourceName))
					CopyFolder(sourceName, destName);
			}
		}
		private void MenuItemDelClick(object sender, EventArgs e)
		{
			TreeNode node = SelectNode;
			string fullName = GetFilePath(node);
			FileType type = GetFileType(node);
			TreeNode nextNode = (node.NextNode ?? node.PrevNode) ?? node.Parent;

			if (type == FileType.File)
			{
				if (!File.Exists(fullName))
					return;

				FileSystem.DeleteFile(fullName,
					UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
				if (!File.Exists(fullName))		//delete success
				{
					//要在Remove(node)之前
					CloseForms(node);
					node.Parent.Nodes.Remove(node);
					_treeView.SelectedNode = nextNode;
				}
			}
			else
			{
				if (!Directory.Exists(fullName))
					return;

				FileSystem.DeleteDirectory(fullName, 
					UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
				if (!Directory.Exists(fullName))		//delete success
				{
					CloseForms(node);
					node.Parent.Nodes.Remove(node);
					_treeView.SelectedNode = nextNode;
					
				}
			}
		}
		private void MenuItemRenameClick(object sender, EventArgs e)
		{
			TreeNode node = SelectNode;
			_operateType = NodeOperate.Rename;
			_treeView.LabelEdit = true;
			node.BeginEdit();
		}
		#endregion
		#endregion

		#region treeview event
		private void TreeViewDoubleClick(object sender, EventArgs e)
		{
			TreeNode node = SelectNode;
			if (node == null || !IsHmiNode(node))
				return;

			OpenForm(node);
		}
		private void TreeViewAfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (!IsHmiNode(e.Node))
				return;

			TreeNode node = e.Node;
			string message = string.Empty;
			string label = e.Label ?? node.Text;

			if (!IsValidName(node, ref label, ref message))
			{
				MessageBox.Show(message, "", MessageBoxButtons.OK);
				e.CancelEdit = true;
				node.BeginEdit();
				return;
			}
			string oldFullName = string.Empty;
			if (_operateType == NodeOperate.Rename)
				oldFullName = GetFilePath(node);

			e.CancelEdit = true;
			node.Text = label;
			_treeView.LabelEdit = false;
			//sort
			TreeNode parent = node.Parent;
			parent.Nodes.Remove(node);
			InsertSortedNode(parent, node);
			_treeView.SelectedNode = node;

			NodeAfterLabelEditEvent(_operateType, node, oldFullName);
		}
		private void TreeViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (!IsHmiNode(e.Node))
				return;
			
			_treeView.SelectedNode = e.Node;
		}
		private void TreeViewKeyDown(object sender, KeyEventArgs e)
		{
			//key事件中一定要判断节点是否存在和有效
			TreeNode node = SelectNode;
			if (node == null || !IsHmiNode(node))
				return;
			
			switch (e.KeyCode)
			{
				case Keys.Delete:
					if (!IsRoot(node))
						MenuItemDelClick(null, null);
					break;
				case Keys.C:		//copy
					if (e.Control)
					{
						if (!IsRoot(node))
							MenuItemCopyClick(null, null);
					}
					break;
				case Keys.V:		//paste
					if (e.Control)
					{
						if (IsValidCopy(_copyNode))
						{
							_treeView.BeginUpdate();
							TreeNode old = node; 
							if (GetFileType(node) == FileType.File)
								node = node.Parent;

							_treeView.SelectedNode = node;
							MenuItemPasteClick(null, null);
							_treeView.SelectedNode = old;
							_treeView.EndUpdate();
						}
					}
					break;
				default:
					break;

			}
		}
		#endregion

		#region form
		private static void CreateForm(TreeNode node)
		{
			HMIFramework framework = GetFramework(node);
			if (framework == null)
				return;
			string fullName = GetFilePath(node);

			framework.Forms.Create(fullName);
		}
		private static void OpenForm(TreeNode node)
		{
			if (GetFileType(node) != FileType.File)
				return;
			
			HMIFramework framework = GetFramework(node);
			if (framework == null)
				return;
			string fullName = GetFilePath(node);

			framework.Forms.Open(fullName);
		}
		private static void RenameForm(HMIFramework framework, TreeNode node, string oldFullName)
		{
			if (GetFileType(node) == FileType.File)
				framework.Forms.Rename(GetFilePath(node), oldFullName);
			else
			{
				foreach (TreeNode n in node.Nodes)
				{
					RenameForm(framework, n, oldFullName + @"\" + n.Text);		
				}
			}
		}
		private static void RenameForms(TreeNode node, string oldFullName)
		{
			HMIFramework framework = GetFramework(node);
			if (framework == null)
				return;

			RenameForm(framework, node, oldFullName);
		}
		private static void CloseForm(HMIFramework framework, TreeNode node)
		{
			if (GetFileType(node) == FileType.File)
				framework.Forms.Close(GetFilePath(node));
			else
			{
				foreach (TreeNode n in node.Nodes)
				{
					CloseForm(framework, n);	
				}
			}
				
		}
		private static void CloseForms(TreeNode node)
		{
			HMIFramework framework = GetFramework(node);
			if (framework == null)
				return;

			CloseForm(framework, node);
		}
		#endregion
	}
}
