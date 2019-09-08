using System;
using System.Collections;
using System.Collections.Generic;

namespace SPP.L6 {
    public class DynamicList<T> : IEnumerable<T> {
        private T[] _items;
        private int _size;

        public DynamicList(int capacity = 0) {
            _size = 0;
            _items = new T[(uint) capacity];
        }

        public int Count => (int) _size;

        public int Capacity {
            get => _items.Length;
            set {
                if (value < _size) {
                    Console.WriteLine("Cannot set capacity");
                    return;
                }
                if (value > _items.Length) {
                    var newItems = new T[value];
                    Array.Copy(_items, newItems, _size);
                    _items = newItems;
                }
                else
                    _items = new T[0];
            }
        }

        public T[] Items => _items;

        private void RefineCapacity(int minLength) {
            if (_items.Length < minLength) {
                int newCapacity = _items.Length == 0 ? 4 : _items.Length * 2;
                newCapacity = Math.Max(newCapacity, minLength);
                Capacity = newCapacity;
            }
        }

        public void Add(T item) {
            if (_size == _items.Length)
                RefineCapacity(_size + 1);
            _items[_size++] = item;
        }

        public int IndexOf(T item) {
            return Array.IndexOf(_items, item);
        }

        public bool Remove(T item) {
            var index = IndexOf(item);
            if (index < 0) return false;
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index) {
            if ((uint) index >= _size) return;
            _size--;
            if (index < _size)
                Array.Copy(_items, index + 1, _items, index, _size - index);
        }

        public void Clear() {
            if (_size == 0) return;
            Array.Clear(_items, 0, _size);
            _size = 0;
        }

        class Enumerator : IEnumerator<T> {
            private DynamicList<T> _list;
            private int _index;
            private T _current;

            public Enumerator(DynamicList<T> list) {
                _list = list;
                _index = 0;
            }

            public bool MoveNext() {
                if (_index == _list.Count) return false;
                _current = _list._items[_index];
                _index++;
                return true;
            }

            public void Reset() {
                _index = 0;
            }

            public T Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose() { }
        }

        public IEnumerator<T> GetEnumerator() {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return new Enumerator(this);
        }
    }
}