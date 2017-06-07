using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyTreeview
{
	public delegate void TreeNodeChangedDelegate(object sender, EventArgs args);
	/// <summary>
	/// 多选树形控件
	/// </summary>
	public class MultiSelectTreeView : TreeView
	{
		public event TreeNodeChangedDelegate TreeNodeChanged;
		
		#region property
		private TreeNode _currentNode;
		public TreeNode CurrentNode
		{
			get { return _currentNode; }
		}
		private readonly List<TreeNode> _selectedNodes = new List<TreeNode>();
		public List<TreeNode> SelectedNodes
		{
			get { return _selectedNodes; }
		}
		#endregion

		#region private function
		///   <summary>   
		///   cancel the light   
		///   </summary>   
		///   <param   name="node"></param>   
		private void LowlightNode(TreeNode node)
		{
			node.BackColor = BackColor;
			node.ForeColor = SystemColors.ControlText;
		}
		///   <summary>   
		///   set the light   
		///   </summary>   
		///   <param   name="node"></param>   
		private void HighlightNode(TreeNode node)
		{
			node.BackColor = SystemColors.Highlight;
			node.ForeColor = SystemColors.HighlightText;
		}
		private void AddNode(TreeNode node)
		{
			_selectedNodes.Add(node);
			HighlightNode(node);
		}
		private void RemoveNode(TreeNode node)
		{
			_selectedNodes.Remove(node);
			LowlightNode(node);
		}
		private void ClearSelectedNodes()
		{
			foreach (TreeNode nd in SelectedNodes)
			{
				LowlightNode(nd);
			}
			_selectedNodes.Clear();
		}
		private void SetCurrentNode(TreeNode node)
		{
			_currentNode = node;
			if (_currentNode == null)
				return;

			if (!_selectedNodes.Contains(_currentNode))
				AddNode(_currentNode);

			if (TreeNodeChanged != null)
				TreeNodeChanged(_currentNode, EventArgs.Empty);
		}
		///   <summary>   
		///   single select   
		///   </summary>   
		///   <param   name="node"></param>   
		private void SingleSelectNode(TreeNode node)
		{
			ClearSelectedNodes();
			SetCurrentNode(node);
		}
		/// <summary>
		/// CtrlMulSelectNode
		/// </summary>
		/// <param name="node"></param>
		private void MulSelectNode(TreeNode node)
		{
			if (_selectedNodes.Contains(node))
			{
				RemoveNode(node);

				TreeNode current = null;
				if (_selectedNodes.Count > 0)
					current = _selectedNodes[_selectedNodes.Count - 1];

				SetCurrentNode(current);
			}
			else
				SetCurrentNode(node);
		}
		/// <summary>
		/// ShiftMulSelectNode
		/// </summary>
		/// <param name="node"></param>
		private void ShiftMulSelectNode(TreeNode node)
		{
			TreeNode root = Nodes[0];
			if (root == node || root.Nodes.Count == 0)
				return;

			ClearSelectedNodes();
			if (_currentNode == null || _currentNode == root)
				_currentNode = root.Nodes[0];

			bool isFind = false;
			for (int i = 0; i < root.Nodes.Count; i++)
			{
				TreeNode n = root.Nodes[i];
				if (isFind)
				{
					AddNode(n);

					if (n == _currentNode || n == node)
					{
						break;
					}
				}
				else
				{
					if (n == _currentNode || n == node)
					{
						isFind = true;
						AddNode(n);

						//currentNode和选中节点是同一个，只选中一个
						if (_currentNode == node)
							break;
					}
				}
			}
		}
		/// <summary>
		/// 根据限制条件重新设置树
		/// </summary>
		/// <param name="node"></param>
		private void ResetTree(TreeNode node)
		{
			TreeNode root = Nodes[0];
			if (root == null)
				return;

			//点中根节点，所有子节点取消选中
			if (node.Level == 0)
			{
				SingleSelectNode(node);
			}
				//选中子节点，取消根节点
			else if (node.Level == 1)
			{
				if (_selectedNodes.Contains(root))
				{
					RemoveNode(root);	
				}
			}

			//默认选中根节点
			if (_selectedNodes.Count == 0)
			{
				SetCurrentNode(root);
			}
		}
		#endregion

		#region virtual function
		protected override void OnMouseDown(MouseEventArgs e)
		{
			TreeNode node = GetNodeAt(e.X, e.Y);
			if (node == null)
				return;

			if ((ModifierKeys & Keys.Control) != 0)
				MulSelectNode(node);
			else if ((ModifierKeys & Keys.Shift) != 0)
				ShiftMulSelectNode(node);
			else
				SingleSelectNode(node);

			ResetTree(node);
		}
		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			SelectedNode = null;
		}
		#endregion

	}
}