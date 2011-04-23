using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    /// <summary>
    /// Class for holding history heuristics
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CoordsTable<T> where T : new()
    {
        private T[,] arr;

        public CoordsTable()
        {
            arr = new T[8, 8];
            CreateNewEachItem();
        }

        protected void CreateNewEachItem()
        {
            for (int i = 0; i < 8; ++i)
                for (int j = 0; j < 8; ++j)
                {
                    arr[i, j] = new T();
                }
        }

        public T this[int i, int j]
        {
            get { return arr[i, j]; }
            set { arr[i, j] = value; }
        }

        public T this[Coordinates coords]
        {
            get { return arr[coords.Y, coords.X]; }
            set { arr[coords.Y, coords.X] = value; }
        }
    }
}
