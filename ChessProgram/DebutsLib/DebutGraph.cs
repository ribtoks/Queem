using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Queem.Core;

namespace DebutsLib
{
    /// <summary>
    /// Simple part of Debutes Tree class
    /// </summary>
    public class MoveNode
    {
        //current root on this level
        Move currentMove;

        //moves-replies on currentMove
        Dictionary<MoveNode, MoveNode> replies;
        
        public Move Move
        {
            get { return currentMove; }
            set 
            {
                currentMove.To = value.To;
                currentMove.From = value.From;
            }
        }

        public Dictionary<MoveNode, MoveNode> Replies
        {
            get { return replies; }
        }

        #region Constructors
        
        public MoveNode(Move from)
        {
            currentMove = new Move(from);
            replies = new Dictionary<MoveNode, MoveNode>();
        }

        public MoveNode(MoveNode from)
        {
            if ((object)from == null)
            {
                currentMove = null;
                replies = new Dictionary<MoveNode, MoveNode>();
            }
            else
            {
                currentMove = new Move(from.currentMove);
                replies = new Dictionary<MoveNode, MoveNode>(from.replies);
            }
        }

        public MoveNode(string from, PlayerPosition whitePos)
        {
            currentMove = new Move(from);
            replies = new Dictionary<MoveNode, MoveNode>();
        }

        #endregion

        /// <summary>
        /// Adds reply to certain move in moves map
        /// </summary>
        /// <param name="moveReply">Move-reply to add</param>
        /// <returns>Move node, that points to added move</returns>
        public MoveNode Add(Move moveReply)
        {
            MoveNode temp = new MoveNode(moveReply);

            if (!replies.ContainsKey(temp))
                replies.Add(temp, temp);

            return replies[temp];
        }

        /// <summary>
        /// Adds reply to certain move in moves map
        /// </summary>
        /// <param name="moveReply">Move, represented by System.String</param>
        /// <param name="whitePos">Position of White figures on board</param>
        /// <returns>Move node, that points to added move</returns>
        public MoveNode Add(string moveReply, PlayerPosition whitePos)
        {
            MoveNode tempNode = new MoveNode(moveReply, whitePos);
            if (!replies.ContainsKey(tempNode))
                replies.Add(tempNode, tempNode);
         
            return replies[tempNode];
        }

        /// <summary>
        /// Gets random reply on last move
        /// </summary>
        /// <returns>MoveNode, that contains reply, or null, if doesn't</returns>
        public MoveNode GetRandomNode()
        {
            int index = DebutGraph.MoveIterator.rand.Next(int.MaxValue) % replies.Count;

            int i = 0;
            foreach (KeyValuePair<MoveNode, MoveNode> pair in replies)
            {
                if (i == index)
                    return pair.Value;
                ++i;
            }

            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            MoveNode move = (MoveNode)obj;
            if ((object)move == null)
                return false;

            if (move.currentMove != this.currentMove)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return currentMove.GetHashCode();
        }

        public static bool operator ==(MoveNode node1, MoveNode node2)
        {
            if (node1.currentMove.Equals((object)node2.currentMove))
                return true;
            return false;
        }

        public static bool operator !=(MoveNode node1, MoveNode node2)
        {
            return !(node1 == node2);
        }


        public override string ToString()
        {
            return currentMove.ToString() + " - " + replies.Count + " replies";
        }
    }

    /// <summary>
    /// Class, that represents a simple map for moves
    /// </summary>
    public class DebutGraph
    {
        /// <summary>
        /// Class, that represents a pointer on DebutGraph Nodes
        /// </summary>
        public class MoveIterator
        {
            MoveNode iter = null;

            MoveIterator prev = MoveIterator.End;

            public MoveNode CurrentNode
            {
                get { return iter; }
                set { iter = value; }
            }

            public MoveIterator PrevIterator
            {
                get { return prev; }
                set { prev = value; }
            }

            public static Random rand = new Random(DateTime.Now.Millisecond);

            #region Constructors

            public MoveIterator(MoveNode node)
            {
                iter = node;
            }

            public MoveIterator(MoveIterator from)
            {
                if (from == MoveIterator.End)
                    return;

                iter = new MoveNode(from.iter);
                prev = new MoveIterator(from.prev);
            }

            public MoveIterator(MoveNode node, MoveIterator prevIter)
            {
                iter = node;
                prev = prevIter;
            }

            #endregion

            public static MoveIterator End = null;

            //moves down on MoveNodes tree
            public static MoveIterator operator ++(MoveIterator oper)
            {
                if (oper.iter.Replies.Count == 0)
                {
                    oper = MoveIterator.End;
                    return oper;
                }
                oper.prev = new MoveIterator(oper);
                oper.iter = oper.iter.GetRandomNode();

                return oper;
            }

            //moves up on MoveNodes tree
            public static MoveIterator operator --(MoveIterator oper)
            {
                oper = oper.prev;
                return oper;
            }

            public override string ToString()
            {
                return "i" + iter.ToString();
            }
        }

        //moves from tree root...
        //used to contain other MoveNodes
        private Dictionary<MoveNode, MoveNode> rootMoves;

        /// <summary>
        /// Looks for beginning of move search
        /// </summary>
        /// <param name="firstMove"></param>
        /// <returns>MoveIterator, that points to reply on 
        /// specified MoveNode in RootMoves</returns>
        public MoveIterator GetIterator(MoveNode firstMove)
        {
            if (rootMoves.ContainsKey(firstMove))
                return new MoveIterator(rootMoves[firstMove]);
            else
                return MoveIterator.End;
        }

        //simple constructor
        public DebutGraph()
        {
            rootMoves = new Dictionary<MoveNode, MoveNode>();
        }

        /// <summary>
        /// Adds all moves to Tree
        /// </summary>
        /// <param name="whitePos">Position of white figures on board</param>
        /// <param name="moves">Pairs of moves and replies e.g. ("e2-e4", "e7-e5")</param>
        public void AddMoves(PlayerPosition whitePos, params string[] moves)
        {
            Move temp = new Move(moves[0]);
            MoveNode tempNode = new MoveNode(temp);

            //iterator, that will add move to tree
            MoveIterator pushingIterator = null;

            MoveNode next = null;

            if (!rootMoves.ContainsKey(tempNode))
            {
                rootMoves.Add(tempNode, tempNode);
                next = rootMoves[tempNode].Add(new Move(moves[1]));
            }
            else
            {
                next = rootMoves[tempNode].Add(new Move(moves[1]));
            }

            pushingIterator = GetIterator(tempNode);
            //next answer
            pushingIterator.CurrentNode = next;

            int i = 2;
            while (i < moves.Length)
            {
                pushingIterator.CurrentNode = pushingIterator.CurrentNode.Add(moves[i], whitePos);
                ++i;
            }
        }

        /// <summary>
        /// Checks if we have debut answer for moves
        /// </summary>
        /// <param name="moves">List of all moves</param>
        /// <returns>MoveIterator that points on certain node, or End iterator if there no answer</returns>
        public MoveIterator CheckMoves(List<Move> moves)
        {
            //there no so long combinations in tree :(
            if (moves.Count > 10)
                return MoveIterator.End;

            MoveNode temp = new MoveNode(moves[0]);

            if (!rootMoves.ContainsKey(temp))
                return MoveIterator.End;

            MoveIterator iter = new MoveIterator(rootMoves[temp]);

            for (int i = 1; i < moves.Count; ++i)
            {
                if (iter == MoveIterator.End)
                    break;

                temp.Move = moves[i];

                if (!iter.CurrentNode.Replies.ContainsKey(temp))
                    return MoveIterator.End;

                iter.CurrentNode = iter.CurrentNode.Replies[temp];
            }

            return iter;
        }
    }
}
