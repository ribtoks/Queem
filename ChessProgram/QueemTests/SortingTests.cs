using System;
using NUnit.Framework;
using Queem.Core.ChessBoard;
using DebutsLib;
using Queem.AI;
using Queem.Core;

namespace Queem.Tests
{
	[TestFixture]
	public class SortingTests
	{
		private GameProvider provider;
	
		public SortingTests ()
		{
			this.provider = new GameProvider();
			string path = "chess.game";
			string[] lines = System.IO.File.ReadAllLines(path);
            Queem.Core.Color color = Queem.Core.Color.White;
            foreach (var line in lines)
            {
                var move = new Move(line);
                
                if (this.provider.PlayerBoards[(int)color].Figures[(int)move.From] == Queem.Core.Figure.King)
                    if (Math.Abs((int)move.From - (int)move.To) == 2)
                        move.Type = MoveType.KingCastle;

                this.provider.ProcessMove(move, color);
                color = (Queem.Core.Color)(1 - (int)color);
            }
		}
		
		[Test]
		public void SortTest()
		{
			var moves = this.provider.PlayerBoard1.GetMoves(
				this.provider.PlayerBoard2, 
				this.provider.History.GetLastMove(), 
				MovesMask.AllMoves);
				
			this.provider.FilterMoves(moves, this.provider.PlayerBoard1.FigureColor);
			
			var copyArray = MovesArray.New();
			copyArray.Size = moves.Size;
			for (int i = 0; i < moves.Size; ++i)
			{
				copyArray.InnerArray[i].From = moves.InnerArray[i].From;
				copyArray.InnerArray[i].To = moves.InnerArray[i].To;
				copyArray.InnerArray[i].Type = moves.InnerArray[i].Type;
				copyArray.InnerArray[i].Value = moves.InnerArray[i].Value;				
			}
			
			MovesSorter.Sort(moves);
			MovesSorter.QuickSort(copyArray);
			
			var arr1 = moves.InnerArray;
			var arr2 = moves.InnerArray;
			
			for (int i = 0; i < moves.Size; ++i)
			{
				Assert.AreEqual(arr1[i].From, arr2[i].From);
				Assert.AreEqual(arr1[i].To, arr2[i].To);				
			}				
		}
	}
}

