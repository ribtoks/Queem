using System;
using System.Collections.Generic;

namespace BasicChessClasses
{
	public abstract class MovesProcessor
	{
		protected struct Delta
		{
			public int x;
			public int y;	
		}
		
		protected List<List<Delta>> directions;
		
		public abstract List<Coordinates> GetMoves(Coordinates currCoords, params byte[] values);
	}
	
	public class PawnProcessor : MovesProcessor
	{
		protected FigureStartPosition startPos = FigureStartPosition.Down;
		
		protected List<List<Delta>> upDirections;

		/*
		 * Pawn positions:
		 * 
		 * -------------------------
		 * |       |       |       |
		 * |       |   2   |       |  -2
		 * |       |       |       |
		 * -------------------------
		 * |       |       |       |
		 * |   0   |   1   |   3   |  -1
		 * |       |       |       |
		 * -------------------------
		 * |       |       |       |
		 * |       |   *   |       |  0
		 * |       |       |       |
		 * -------------------------
		 *     -1      0      +1
		 * 
		 * 
		 *  \ * \ - a pawn position
		 *  \ 0,1,2,3 \ - number of cell, where a pawn can go
		 * (they specify # of a bit in index of the hardcoded directions)
		*/
		
		public PawnProcessor (FigureStartPosition startPosition)
		{
			startPos = startPosition;
			
			directions = new List<List<Delta>> ();
			upDirections = new List<List<Delta>> ();
			
			// directions are set for StartPosition.Down			
			directions.Add (new List<Delta>() {});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-2}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-2},new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1},new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1},new Delta(){x=0, y=-2}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1},new Delta(){x=0, y=-2},new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1},new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1},new Delta(){x=0, y=-2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1},new Delta(){x=0, y=-2},new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1},new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1},new Delta(){x=0, y=-1},new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1},new Delta(){x=0, y=-1},new Delta(){x=0, y=-2},});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1},new Delta(){x=0, y=-1},new Delta(){x=0, y=-2},
				new Delta(){x=1, y=-1}});
			
			for (int i = 0; i < directions.Count; ++i)
			{
				upDirections.Add (new List<Delta>());
				for (int j = 0; j < directions[i].Count; ++j)
					upDirections[i].Add(new Delta() {x=directions[i][j].x, y=directions[i][j].y * (-1)});
			}
		}
		
		public override List<Coordinates> GetMoves (Coordinates currCoords, params byte[] values)
		{
			int index = 0;
			
			// input parameter "values" are just
			// bits of some integer value, that is
			// an index in hardcode moves array
			
			// set all bits of index integer
			for (int i = 0; i < 4; ++i)
				index |= values[i] << (3 - i);
			
			List<Delta> preMoves = directions[index];
			
			if (startPos == FigureStartPosition.Up)
				preMoves = upDirections[index];
			
			List<Coordinates> coords = new List<Coordinates> (preMoves.Count);

			int x = currCoords.X;
			int y = currCoords.Y;
			
			for (int i = 0; i < preMoves.Count; ++i)
				coords.Add (new Coordinates (x + preMoves[i].x, y + preMoves[i].y));
			
			return coords;
		}
		
		public FigureStartPosition StartPos 
		{
			get { return this.startPos; }
			set { startPos = value; }
		}	
	}
	
	public class KingProcessor : MovesProcessor
	{
		/*
		 * King positions:
		 * 
		 * -------------------------
		 * |       |       |       |
		 * |   2   |   3   |   4   |  -1
		 * |       |       |       |
		 * -------------------------
		 * |       |       |       |
		 * |   1   |   *   |   5   |  0
		 * |       |       |       |
		 * -------------------------
		 * |       |       |       |
		 * |   0   |   7   |   6   |  +1
		 * |       |       |       |
		 * -------------------------
		 *     -1      0      +1
		 * 
		 * 
		 *  \ * \ - a king position
		 *  \ 0,1,2,3,4,5,6,7 \ - number of cell, where a king can go
		 * (they specify # of a bit in index of the hardcoded directions)
		*/
		
		public KingProcessor ()
		{
            directions = new List<List<Delta>>();

			#region Hardcoded moves
			
			directions.Add (new List<Delta>() {});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=0, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=1}, new Delta(){x=-1, y=0}, new Delta(){x=-1, y=-1}, new Delta(){x=0, y=-1}, new Delta(){x=1, y=-1}, new Delta(){x=1, y=0}, new Delta(){x=1, y=1}, new Delta(){x=0, y=1}});
			
			#endregion
		}
		
		public override List<Coordinates> GetMoves (Coordinates currCoords, params byte[] values)
		{
			int index = 0;
			
			// input parameter "values" are just
			// bits of some integer value, that is
			// an index in hardcode moves array
			
			// set all bits of index integer			
			for (int i = 0; i < 8; ++i)
				index |= values[i] << (7 - i);
			
			List<Delta> preMoves = directions[index];
			
			List<Coordinates> coords = new List<Coordinates> (preMoves.Count);

			int x = currCoords.X;
			int y = currCoords.Y;
			
			for (int i = 0; i < preMoves.Count; ++i)
				coords.Add (new Coordinates (x + preMoves[i].x, y + preMoves[i].y));
			
			return coords;
		}
	}
	
	public class HorseProcessor : MovesProcessor
	{
		/*
		 * Horse positions:
		 * 
		 * -----------------------------------------
		 * |       |       |       |       |       |
		 * |       |   1   |       |   2   |       |
		 * |       |       |       |       |       |
		 * -----------------------------------------
		 * |       |       |       |       |       |
		 * |   0   |       |       |       |   3   |
		 * |       |       |       |       |       |
		 * -----------------------------------------
		 * |       |       |       |       |       |
		 * |       |       |   *   |       |       |
		 * |       |       |       |       |       |
		 * -----------------------------------------
		 * |       |       |       |       |       |
		 * |   7   |       |       |       |   4   |
		 * |       |       |       |       |       |
		 * -----------------------------------------
		 * |       |       |       |       |       |
		 * |       |   6   |       |   5   |       |
		 * |       |       |       |       |       |
		 * -----------------------------------------
		 * 
		 *  \ * \ - a horse position
		 *  \ 0,1,2,3,4,5,6,7 \ - number of cell, where a horse can go
		 * (they specify # of a bit in index of the hardcoded directions)
		*/
		
		public HorseProcessor ()
		{
            directions = new List<List<Delta>>();

			#region Hardcoded moves
			
			directions.Add (new List<Delta>() {});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-2, y=1}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}});
			directions.Add (new List<Delta>() {new Delta(){x=-2, y=-1}, new Delta(){x=-1, y=-2}, new Delta(){x=1, y=-2}, new Delta(){x=2, y=-1}, new Delta(){x=2, y=1}, new Delta(){x=1, y=2}, new Delta(){x=-1, y=2}, new Delta(){x=-2, y=1}});
			
			#endregion
		}
		
		public override List<Coordinates> GetMoves (Coordinates currCoords, params byte[] values)
		{
			int index = 0;
			
			// input parameter "values" are just
			// bits of some integer value, that is
			// an index in hardcode moves array
			
			// set all bits of index integer
			for (int i = 0; i < 8; ++i)
				index |= values[i] << (7 - i);
			
			List<Delta> preMoves = directions[index];
			
			List<Coordinates> coords = new List<Coordinates> (preMoves.Count);

			int x = currCoords.X;
			int y = currCoords.Y;
			
			for (int i = 0; i < preMoves.Count; ++i)
				coords.Add (new Coordinates (x + preMoves[i].x, y + preMoves[i].y));
			
			return coords;
		}
	}
}

