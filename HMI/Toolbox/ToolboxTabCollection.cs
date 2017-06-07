namespace Toolbox {
    using System;
    using System.Collections;
    
    
    //  Project   : ToolboxLibrary
    //  Class     : ToolboxTabCollection
    // 
    //  Copyright (C) 2002, Microsoft Corporation
    // ------------------------------------------------------------------------------
    //  <summary>
    //  Strongly-typed collection of ToolboxTab objects
    //  </summary>
    //  <remarks></remarks>
    //  <history>
    //      [dineshc] 3/26/2003  Created
    //  </history>
    [Serializable()]
    internal class ToolboxTabCollection : CollectionBase {
        
        ///  <summary>
        ///       Initializes a new instance of <see cref="ToolboxTabCollection"/>.
        ///  </summary>
        ///  <remarks></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public ToolboxTabCollection() {
        }
        
        ///  <summary>
        ///       Initializes a new instance of <see cref="ToolboxTabCollection"/> based on another <see cref="ToolboxTabCollection"/>.
        ///  </summary>
        ///  <param name="value">
        ///       A <see cref="ToolboxTabCollection"/> from which the contents are copied
        ///  </param>
        ///  <remarks></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public ToolboxTabCollection(ToolboxTabCollection value) {
            this.AddRange(value);
        }
        
        ///  <summary>
        ///       Initializes a new instance of <see cref="ToolboxTabCollection"/> containing any array of <see cref="ToolboxTab"/> objects.
        ///  </summary>
        ///  <param name="value">
        ///       A array of <see cref="ToolboxTab"/> objects with which to intialize the collection
        ///  </param>
        ///  <remarks></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public ToolboxTabCollection(ToolboxTab[] value) {
            this.AddRange(value);
        }
        
        ///  <summary>
        ///  Represents the entry at the specified index of the <see cref="ToolboxTab"/>.
        ///  </summary>
        ///  <param name="index">The zero-based index of the entry to locate in the collection.</param>
        ///  <value>
        ///  The entry at the specified index of the collection.
        ///  </value>
        ///  <remarks><exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is outside the valid range of indexes for the collection.</exception></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public ToolboxTab this[int index] {
            get {
                return ((ToolboxTab)(List[index]));
            }
            set {
                List[index] = value;
            }
        }
        
        ///  <summary>
        ///    Adds a <see cref="ToolboxTab"/> with the specified value to the 
        ///    <see cref="ToolboxTabCollection"/> .
        ///  </summary>
        ///  <param name="value">The <see cref="ToolboxTab"/> to add.</param>
        ///  <returns>
        ///    The index at which the new element was inserted.
        ///  </returns>
        ///  <remarks><seealso cref="ToolboxTabCollection.AddRange"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public int Add(ToolboxTab value) {
            return List.Add(value);
        }
        
        ///  <summary>
        ///  Copies the elements of an array to the end of the <see cref="ToolboxTabCollection"/>.
        ///  </summary>
        ///  <param name="value">
        ///    An array of type <see cref="ToolboxTab"/> containing the objects to add to the collection.
        ///  </param>
        ///  <remarks><seealso cref="ToolboxTabCollection.Add"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void AddRange(ToolboxTab[] value) {
            for (int i = 0; (i < value.Length); i = (i + 1)) {
                this.Add(value[i]);
            }
        }
        
        ///  <summary>
        ///     
        ///       Adds the contents of another <see cref="ToolboxTabCollection"/> to the end of the collection.
        ///    
        ///  </summary>
        ///  <param name="value">
        ///    A <see cref="ToolboxTabCollection"/> containing the objects to add to the collection.
        ///  </param>
        ///  <remarks><seealso cref="ToolboxTabCollection.Add"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void AddRange(ToolboxTabCollection value) {
            for (int i = 0; (i < value.Count); i = (i + 1)) {
                this.Add(value[i]);
            }
        }
        
        ///  <summary>
        ///  Gets a value indicating whether the 
        ///    <see cref="ToolboxTabCollection"/> contains the specified <see cref="ToolboxTab"/>.
        ///  </summary>
        ///  <param name="value">The <see cref="ToolboxTab"/> to locate.</param>
        ///  <returns>
        ///  <see langword="true"/> if the <see cref="ToolboxTab"/> is contained in the collection; 
        ///   otherwise, <see langword="false"/>.
        ///  </returns>
        ///  <remarks><seealso cref="ToolboxTabCollection.IndexOf"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public bool Contains(ToolboxTab value) {
            return List.Contains(value);
        }
        
        ///  <summary>
        ///  Copies the <see cref="ToolboxTabCollection"/> values to a one-dimensional <see cref="System.Array"/> instance at the 
        ///    specified index.
        ///  </summary>
        ///  <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the values copied from <see cref="ToolboxTabCollection"/> .</param>
        ///  <param name="index">The index in <paramref name="array"/> where copying begins.</param>
        ///  <remarks><exception cref="System.ArgumentException"><paramref name="array"/> is multidimensional. <para>-or-</para> <para>The number of elements in the <see cref="ToolboxTabCollection"/> is greater than the available space between <paramref name="arrayIndex"/> and the end of <paramref name="array"/>.</para></exception>
        ///  <exception cref="System.ArgumentNullException"><paramref name="array"/> is <see langword="null"/>. </exception>
        ///  <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than <paramref name="array"/>"s lowbound. </exception>
        ///  <seealso cref="System.Array"/>
        ///  </remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void CopyTo(ToolboxTab[] array, int index) {
            List.CopyTo(array, index);
        }
        
        ///  <summary>
        ///    Returns the index of a <see cref="ToolboxTab"/> in 
        ///       the <see cref="ToolboxTabCollection"/> .
        ///  </summary>
        ///  <param name="value">The <see cref="ToolboxTab"/> to locate.</param>
        ///  <returns>
        ///  The index of the <see cref="ToolboxTab"/> of <paramref name="value"/> in the 
        ///  <see cref="ToolboxTabCollection"/>, if found; otherwise, -1.
        ///  </returns>
        ///  <remarks><seealso cref="ToolboxTabCollection.Contains"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public int IndexOf(ToolboxTab value) {
            return List.IndexOf(value);
        }
        
        ///  <summary>
        ///  Inserts a <see cref="ToolboxTab"/> into the <see cref="ToolboxTabCollection"/> at the specified index.
        ///  </summary>
        ///  <param name="index">The zero-based index where <paramref name="value"/> should be inserted.</param>
        ///  <param name=" value">The <see cref="ToolboxTab"/> to insert.</param>
        ///  <remarks><seealso cref="ToolboxTabCollection.Add"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void Insert(int index, ToolboxTab value) {
            List.Insert(index, value);
        }
        
        ///  <summary>
        ///    Returns an enumerator that can iterate through 
        ///       the <see cref="ToolboxTabCollection"/> .
        ///  </summary>
        ///  <returns>An enumerator for the collection</returns>
        ///  <remarks><seealso cref="System.Collections.IEnumerator"/></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public new ToolboxTabEnumerator GetEnumerator() {
            return new ToolboxTabEnumerator(this);
        }
        
        ///  <summary>
        ///     Removes a specific <see cref="ToolboxTab"/> from the 
        ///    <see cref="ToolboxTabCollection"/> .
        ///  </summary>
        ///  <param name="value">The <see cref="ToolboxTab"/> to remove from the <see cref="ToolboxTabCollection"/> .</param>
        ///  <remarks><exception cref="System.ArgumentException"><paramref name="value"/> is not found in the Collection. </exception></remarks>
        ///  <history>
        ///      [dineshc] 3/26/2003  Created
        ///  </history>
        public void Remove(ToolboxTab value) {
            List.Remove(value);
        }
        
        public class ToolboxTabEnumerator : object, IEnumerator {
            
            private IEnumerator baseEnumerator;
            
            private IEnumerable temp;
            
            public ToolboxTabEnumerator(ToolboxTabCollection mappings) {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            
            public ToolboxTab Current {
                get {
                    return ((ToolboxTab)(baseEnumerator.Current));
                }
            }
            
            object IEnumerator.Current {
                get {
                    return baseEnumerator.Current;
                }
            }
            
            public bool MoveNext() {
                return baseEnumerator.MoveNext();
            }
            
            bool IEnumerator.MoveNext() {
                return baseEnumerator.MoveNext();
            }
            
            public void Reset() {
                baseEnumerator.Reset();
            }
            
            void IEnumerator.Reset() {
                baseEnumerator.Reset();
            }
        }
    }
}
