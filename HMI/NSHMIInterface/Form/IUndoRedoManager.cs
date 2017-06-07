using System.Collections.Generic;

namespace UndoMethods
{
	public delegate void UndoRedoOperation<T>(T undoData);
	public delegate void OnStackStatusChanged(bool hasItems);

	/// <summary>
	/// Undo/Redo
	/// </summary>
	public interface IUndoRedoManager
	{
		/// <summary>
		/// Is fired when the undo stack status is changed
		/// </summary>
		event OnStackStatusChanged UndoStackStatusChanged;

		/// <summary>
		/// Is fired when the redo stack status is changed
		/// </summary>
		event OnStackStatusChanged RedoStackStatusChanged;

		/// <summary>
		/// Sets/gets maximum items to be stored in the stack. Note that the change takes effect the next time an item is added to the undo/redo stack
		/// </summary>
		int MaxItems { get; set; }
		/// <summary>
		/// 终止入栈
		/// </summary>
		bool IsStopPush { set; get; }

		int UndoOperationCount { get; }
		int RedoOperationCount { get; }
		bool HasUndoOperations { get; }
		bool HasRedoOperations { get; }


		/// <summary>
		/// Pushes an item onto the undo/redo stack. 
		/// 1) If this is called outside the context of a undo/redo operation, the item is added to the undo stack.
		/// 2) If this is called in the context of an undo operation, the item is added to redo stack.
		/// 3) If this is called in context of an redo operation, item is added to undo stack.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="undoOperation"></param>
		/// <param name="undoData"></param>
		/// <param name="description"></param>
		void Push<T>(UndoRedoOperation<T> undoOperation, T undoData, string description = "");

		/// <summary>
		/// Performs an undo operation
		/// </summary>
		void Undo();

		/// <summary>
		/// Performs a redo operation
		/// </summary>
		void Redo();

		/// <summary>
		/// Clears all undo/redo operations from the stack
		/// </summary>
		void Clear();

		/// <summary>
		/// Returns a list containing description of all undo stack records
		/// </summary>
		/// <returns></returns>
		IList<string> GetUndoStackInformation();

		/// <summary>
		/// Returns a list containing description of all redo stack records
		/// </summary>
		/// <returns></returns>
		IList<string> GetRedoStackInformation();

		/// <summary>
		/// 开启入栈集合
		/// </summary>
		/// <param name="name"></param>
		void StartTransaction(string name);
		/// <summary>
		/// 关闭入栈集合
		/// </summary>
		void EndTransaction();
	}
}