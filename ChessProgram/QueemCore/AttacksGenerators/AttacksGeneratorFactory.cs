using System;

namespace QueemCore.AttacksGenerators
{
	public class AttacksGeneratorFactory
	{
		public static AttacksGenerator CreateGenerator(Figure figureType, PlayerPosition position)
		{
			switch (figureType)
			{			
			case Figure.Knight:
				return new KnightAttacksGenerator(); 
			case Figure.Bishop:
				return new BishopAttacksGenerator();
			case Figure.Rook:
				return new RookAttacksGenerator();
			case Figure.Queen:
				return new QueenAttacksGenerator();
			case Figure.King:
				return new KingAttacksGenerator();
			case Figure.Pawn:
				return new PawnAttacksGenerator(position);
			default:
				return null;
			}
		}
	}
}

