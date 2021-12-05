
using OpenSourceCurrencyApi.Structures.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenSourceCurrencyApi.Structures
{
    public sealed class CircularLinkedList<T> : ICollection<T>
    {

        Node<T> head = null;
        Node<T> tail = null;
        int count = 0;
        readonly IEqualityComparer<T> comparer;

        public int Count { get { return count; } }
        public bool IsReadOnly { get { return false; } }
        
        public CircularLinkedList()
        : this(null, EqualityComparer<T>.Default)
        {
        }

        public CircularLinkedList(IEnumerable<T> collection)
        : this(collection, EqualityComparer<T>.Default)
        {
        }

        public CircularLinkedList(IEqualityComparer<T> comparer)
    : this(null, comparer)
        {
        }

        public CircularLinkedList(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            this.comparer = comparer;
            if (collection != null)
            {
                foreach (T item in collection)
                    this.AddLast(item);
            }
        }

        /// <summary>
        /// Gets Tail node. Returns NULL if no tail node found
        /// </summary>
        public Node<T> Tail { get { return tail; } }

        /// <summary>
        /// Gets the head node. Returns NULL if no node found
        /// </summary>
        public Node<T> Head { get { return head; } }

        /// <summary>
        /// Gets the item at the current index
        /// </summary>
        /// <param name="index">Zero-based index</param>
        /// <exception cref="ArgumentOutOfRangeException">index is out of range</exception>
        public Node<T> this[int index]
        {
            get
            {
                if (index >= count || index < 0)
                    throw new ArgumentOutOfRangeException("index");
                else
                {
                    Node<T> node = this.head;
                    for (int i = 0; i < index; i++)
                        node = node.Next;
                    return node;
                }
            }
        }


        public void Add(T item)
        {
            this.AddLast(item);
        }

        /// <summary>
        /// Add a new item to the end of the list
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void AddLast(T item)
        {
            // if head is null, then this will be the first item
            if (head == null)
                this.AddFirstItem(item);
            else
            {
                Node<T> newNode = new Node<T>(item);
                tail.Next = newNode;
                newNode.Next = head;
                newNode.Previous = tail;
                tail = newNode;
                head.Previous = tail;
            }
            ++count;
        }

        void AddFirstItem(T item)
        {
            head = new Node<T>(item);
            tail = head;
            head.Next = tail;
            head.Previous = tail;
        }

        /// <summary>
        /// Clears the list
        /// </summary>
        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        /// <summary>
        /// Determines whether a value is in the list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>TRUE if item exist, else FALSE</returns>
        public bool Contains(T item)
        {
            return Find(item) != null;
        }

        /// <summary>
        /// Finds the supplied item and returns a node which contains item. Returns NULL if item not found
        /// </summary>
        /// <param name="item">Item to search</param>
        /// <returns><see cref="Node"/> instance or NULL</returns>
        public Node<T> Find(T item)
        {
            Node<T> node = FindNode(head, item);
            return node;
        }

        Node<T> FindNode(Node<T> node, T valueToCompare)
        {
            Node<T> result = null;
            if (comparer.Equals(node.Value, valueToCompare))
                result = node;
            else if (result == null && node.Next != head)
                result = FindNode(node.Next, valueToCompare);
            return result;
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0 || arrayIndex > array.Length)
                throw new ArgumentOutOfRangeException("arrayIndex");

            Node<T> node = this.head;
            do
            {
                array[arrayIndex++] = node.Value;
                node = node.Next;
            } while (node != head);
        }


        /// <summary>
        /// Gets a forward enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> current = head;
            if (current != null)
            {
                do
                {
                    yield return current.Value;
                    current = current.Next;
                } while (current != head);
            }
        }
        public bool Remove(T item)
        {
            // finding the first occurance of this item
            Node<T> nodeToRemove = this.Find(item);
            if (nodeToRemove != null)
                return this.RemoveNode(nodeToRemove);
            return false;
        }

        bool RemoveNode(Node<T> nodeToRemove)
        {
            Node<T> previous = nodeToRemove.Previous;
            previous.Next = nodeToRemove.Next;
            nodeToRemove.Next.Previous = nodeToRemove.Previous;

            // if this is head, we need to update the head reference
            if (head == nodeToRemove)
                head = nodeToRemove.Next;
            else if (tail == nodeToRemove)
                tail = tail.Previous;

            --count;
            return true;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Bubble sorts the list 
        /// </summary>
        /// <param name="comparer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void BubbleSort(IComparer<T> comparer = null)
        {
            comparer ??= Comparer<T>.Default;

            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            if (head is null)
                return; 

            bool agenda = true;

            while (agenda)
            {
                agenda = false;

                for (Node<T> node = head; !ReferenceEquals(node.Next, head); node = node.Next)
                    if (comparer.Compare(node.Value, node.Next.Value) > 0)
                    {
                        agenda = true;

                        var help = node.Value;
                        node.Value = node.Next.Value;
                        node.Next.Value = help;
                    }
            }
        }
    }
}


