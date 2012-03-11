using System;

namespace QueemCore.AttacksGenerators
{
	public class AttacksGeneratorFactory
	{
		public static AttacksGenerator CreateGenerator(Figure figureType)
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
			default:
				return null;
			}
		}
	}
}

