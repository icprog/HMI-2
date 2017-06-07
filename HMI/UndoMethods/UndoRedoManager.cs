using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UndoMethods
{
    /// <summary>
    /// This is a singleton class which stores undo/redo records and executes the undo/redo operations specified in these records
    /// </summary>
    public class UndoRedoManager : IUndoRedoManager
    {
		/// <summary>
		/// 入栈终止
		/// </summary>
		public bool IsStopPush { set; get; }
		/// <summary>
		/// 是在集合出入栈状态
		/// </summary>
    	private bool _hasTransaction;

        
		/// <summary>
        /// Stores undo records
        /// </summary>
        private List<IUndoRedoRecord> _undoStack = new List<IUndoRedoRecord>();
        
        /// <summary>
        /// Stores redo records 
        /// </summary>
        private List<IUndoRedoRecord> _redoStack = new List<IUndoRedoRecord>();

        /// <summary>
        /// This is used to determine if an undo operation is going on
        /// </summary>
        private bool _undoGoingOn = false;

        /// <summary>
        /// This is used to determine if a redo operation is going on
        /// </summary>
        private bool _redoGoingOn = false;

        /// <summary>
        /// Is fired when the undo stack status is changed
        /// </summary>
        public event OnStackStatusChanged UndoStackStatusChanged;

        /// <summary>
        /// Is fired when the redo stack status is changed
        /// </summary>
        public event OnStackStatusChanged RedoStackStatusChanged;

        /// <summary>
        /// Maximum items to store in undo redo stack
        /// </summary>
        protected int _maxItems = 10;

        /// <summary>
        /// Sets/gets maximum items to be stored in the stack. Note that the change takes effect the next time an item is added to the undo/redo stack
        /// </summary>
        public int MaxItems
        {
            get { return _maxItems; }
            set
            {
                if (_maxItems <= 0)
                {
                    throw new ArgumentOutOfRangeException("Max items can't be <= 0");
                }

                _maxItems = value;
            }
        }

        /// <summary>
        /// Starts a transaction under which all undo redo operations take place
        /// </summary>
        /// <param name="tran"></param>
        public void StartTransaction(string name)
        {
            if (!_hasTransaction)
            {
            	_hasTransaction = true;
                ///push an empty undo operation
				_undoStack.Push(new UndoTransaction(name));
				_redoStack.Push(new UndoTransaction(name));
            }
        }

        /// <summary>
        /// Ends the transaction under which all undo/redo operations take place
        /// </summary>
        /// <param name="tran"></param>
        public void EndTransaction()
        {
            if (_hasTransaction)
            {
                _hasTransaction = false;
                ///now we might have had no items added to undo and redo stack as a part of this transaction. Check empty transaction at top and remove them
                if (_undoStack.Count > 0)
                {
                    UndoTransaction t = _undoStack[0] as UndoTransaction;
                    if (t != null && t.OperationsCount == 0)
                    {
                        _undoStack.Pop();
                    }
                }

                if (_redoStack.Count > 0)
                {
                    UndoTransaction t = _redoStack[0] as UndoTransaction;
                    if (t != null && t.OperationsCount == 0)
                    {
                        _redoStack.Pop();
                    }
                }
            }
        }

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
        public void Push<T>(UndoRedoOperation<T> undoOperation, T undoData, string description = "")
        {
            Debug.Assert(null != undoOperation);

			if (IsStopPush)
				return;

            List<IUndoRedoRecord> stack = null;
            Action eventToFire;

            ///Determien the stack to which this operation will be added
            if (_undoGoingOn)
            {
                Trace.TraceInformation("Adding to redo stack {0} with data {1}", undoOperation.Method.Name, undoData);
                stack = _redoStack;
                eventToFire = new Action(FireRedoStackStatusChanged);
            }
            else
            {
                Trace.TraceInformation("Adding to undo stack {0} with data {1}", undoOperation.Method.Name, undoData);
                stack = _undoStack;
                eventToFire = new Action(FireUndoStackStatusChanged);
            }

            if( (!_undoGoingOn) && (!_redoGoingOn))
            {//if someone added an item to undo stack while there are items in redo stack.. clear the redo stack
                _redoStack.Clear();
                FireRedoStackStatusChanged();
            }
            ///If a transaction is going on, add the operation as a entry to the transaction operation
            if (!_hasTransaction)
            {
                stack.Push(new UndoRedoRecord<T>(undoOperation, undoData, description));
            }
            else
            {
                (stack[0] as UndoTransaction).AddUndoRedoOperation(new UndoRedoRecord<T>(undoOperation, undoData,
                                                                                       description));
            }

            //If the stack count exceeds maximum allowed items
            if (stack.Count > MaxItems)
            {
                object o = stack[stack.Count - 1];
                Trace.TraceInformation("Removing item {0}", o);
                stack.RemoveRange(MaxItems-1, stack.Count-MaxItems);
            }
            //Fire event to inform consumers that the stack size has changed
            eventToFire();
        }

        private void FireUndoStackStatusChanged()
        {
            if (null != UndoStackStatusChanged)
                UndoStackStatusChanged(HasUndoOperations);
        }

        private void FireRedoStackStatusChanged()
        {
            if (null != RedoStackStatusChanged)
                RedoStackStatusChanged(HasRedoOperations);
        }

        public int UndoOperationCount
        {
            get { return _undoStack.Count; }
        }

        public int RedoOperationCount
        {
            get { return _redoStack.Count; }
        }

        public bool HasUndoOperations
        {
            get { return _undoStack.Count != 0; }
        }

        public bool HasRedoOperations
        {
            get { return _redoStack.Count != 0; }
        }

        /// <summary>
        /// Performs an undo operation
        /// </summary>
        public void Undo()
        {
            try
            {
				if (_hasTransaction)
					return;

                _undoGoingOn = true;

                if (_undoStack.Count == 0)
                {
                    throw new InvalidOperationException("Nothing in the undo stack");
                }
                object oUndoData = _undoStack.Pop();

                Type undoDataType = oUndoData.GetType();

                ///If the stored operation was a transaction, perform the undo as a transaction too.
                if (typeof (UndoTransaction).Equals(undoDataType))
                {
					StartTransaction(((UndoTransaction)oUndoData).Name);
                }

                undoDataType.InvokeMember("Execute", BindingFlags.InvokeMethod, null, oUndoData, null);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
            finally
            {
                _undoGoingOn = false;

                EndTransaction();

                FireUndoStackStatusChanged();
            }
        }

        /// <summary>
        /// Performs a redo operation
        /// </summary>
        public void Redo()
        {
            try
            {
				if (_hasTransaction)
					return;
				
				_redoGoingOn = true;
                if (_redoStack.Count == 0)
                {
                    throw new InvalidOperationException("Nothing in the redo stack");
                }
                object oUndoData = _redoStack.Pop();

                Type undoDataType = oUndoData.GetType();

                ///If the stored operation was a transaction, perform the redo as a transaction too.
                if (typeof (UndoTransaction).Equals(undoDataType))
                {
					StartTransaction(((UndoTransaction)oUndoData).Name);
                }

                undoDataType.InvokeMember("Execute", BindingFlags.InvokeMethod, null, oUndoData, null);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
            finally
            {
                _redoGoingOn = false;
                EndTransaction();

                FireRedoStackStatusChanged();
            }
        }

        /// <summary>
        /// Clears all undo/redo operations from the stack
        /// </summary>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            FireUndoStackStatusChanged();
            FireRedoStackStatusChanged();
        }

        /// <summary>
        /// Returns a list containing description of all undo stack records
        /// </summary>
        /// <returns></returns>
        public IList<string> GetUndoStackInformation()
        {
            return _undoStack.ConvertAll( (input)=>input.Name==null ? "" : input.Name);
        }

        /// <summary>
        /// Returns a list containing description of all redo stack records
        /// </summary>
        /// <returns></returns>
        public IList<string> GetRedoStackInformation()
        {
            return _redoStack.ConvertAll((input) => input.Name == null ? "" : input.Name);
        }
    }
}