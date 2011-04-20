using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public enum TableValueType { Exact, LowerBound, UpperBound }

    public class TableMoveInfo
    {
        public int Value { get; set; }
        public TableValueType Type { get; set; }
        public int Depth { get; set; }

        public TableMoveInfo()
        {
        }

        public TableMoveInfo(int value, 
            TableValueType type, int depth)
        {
            this.Value = value;
            this.Type = type;
            this.Depth = depth;
        }
    }

    public class TranspositionTable
    {
        protected Dictionary<ulong, TableMoveInfo> table;

        public TranspositionTable()
        {
            this.table = new Dictionary<ulong, TableMoveInfo>();
        }

        public void AddPosition(ulong hash, int value, 
            TableValueType type, int depth)
        {
            // when debug will throw exception 
            // if same board position set twice
//#if DEBUG
            //this.table.Add(hash, new TableMoveInfo(value, type, depth));
//#else
            this.table[hash] = new TableMoveInfo(value, type, depth);
//#endif
        }

        public TableMoveInfo this[ulong hash]
        {
            get 
            {
                TableMoveInfo t = null;
                if (this.table.TryGetValue(hash, out t))
                    return t;
                else
                    return null;
            }
        }

        public void Clear()
        {
            this.table.Clear();
        }
    }
}
