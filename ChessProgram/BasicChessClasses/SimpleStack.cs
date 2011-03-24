using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicChessClasses
{
    public class SimpleStack<T> where T : class, new()
    {
        protected List<T> elements;
        protected int size;

        public SimpleStack()
        {
            int max_count = 20;
            elements = new List<T>(max_count);
            
            for (int i = 0; i < max_count; ++i)
            {
                // some kind of indian code here
                elements.Add(null);
            }
            
            size = 0;
        }

        public void Push(T item)
        {
            // quick and dirty stack
            elements[size] = item;
            ++size;
        }

        public T Pop()
        {
#if DEBUG
            if (size == 0)
                throw new IndexOutOfRangeException();
#endif
            T item = elements[size - 1];
            elements[size - 1] = null;
            --size;

            return item;
        }

        public T[] ToArray()
        {
            return elements.GetRange(0, size).ToArray();
        }
    }
}
