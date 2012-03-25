using System;
using NUnit.Framework;
using Queem.Core.BitBoards;
using Queem.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Queem.Tests
{
	[TestFixture]
	public class SerializationTests
	{
		Random rand;
		
		public SerializationTests()
		{
			this.rand = new Random(DateTime.Now.Millisecond);
		}
	
		public IEnumerable<Square> GetRandomSquares(int n)
		{
			while ((n--) > 0)
				yield return (Square)rand.Next(64);
		}
		
		public List<Square> GetRandomSquaresList(int n)
		{
			var squares = GetRandomSquares(n).Distinct().ToList();
			squares.Sort();
			return squares;
		}
		
		[Test]
		public void Test1ByteSquares ()
		{
			int count = 100000;
			
			while ((count--) > 0)
			{
				var board = new BitBoard();
				var randomSquares = GetRandomSquaresList(rand.Next(64));
				
				foreach (var sq in randomSquares)
					board.SetBit(sq);
	
				var serializedSquares = BitBoardSerializer.GetSquares(board.GetInnerValue()).Distinct().ToList();
				serializedSquares.Sort();
				Assert.IsTrue(
					 Enumerable.SequenceEqual(
					 	serializedSquares,
					 	randomSquares));
			}
		}
		
		[Test]
		public void Test1ByteMoves()
		{
			int count = 100000;
			
			while ((count--) > 0)
			{
				Square start = (Square)rand.Next(64);
			
				var board = new BitBoard();
				var randomSquares = GetRandomSquaresList(rand.Next(64));
				var moves = new List<Move>();
				
				foreach (var sq in randomSquares)
				{
					board.SetBit(sq);
					moves.Add(new Move(start, sq));
				}
	
				var serializedMoves = BitBoardSerializer.GetMoves(start, board.GetInnerValue()).ToList();
				
				bool isOk = true;
				
				if (serializedMoves.Count != moves.Count)
					isOk = false;
				
				if (isOk)
					for (int i = 0; i < serializedMoves.Count; ++i)
						if ((moves[i].From != serializedMoves[i].From) ||
							(moves[i].To != serializedMoves[i].To))
						{
							isOk = false;
							break;
						}
				
				Assert.IsTrue(isOk);
			}				
		}
	}
}

