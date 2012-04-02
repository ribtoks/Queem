using System;
using Queem.Core.Extensions;
using System.Collections.Generic;

namespace Queem.Core
{
	public class Move
	{
        private static Dictionary<char, MoveType> promotionDict =
            new Dictionary<char, MoveType>() 
            {
                {'r', MoveType.RookPromotion},
                {'q', MoveType.QueenPromotion},
                {'b', MoveType.BishopPromotion},
                {'k', MoveType.KnightPromotion}
            };

		public Move (Square start, Square end)
			:this(start, end, MoveType.Quiet)
		{
		}
		
		public Move (Square start, Square end, MoveType type)
		{
			this.From = start;
			this.To = end;
			this.Type = MoveType.Quiet;
		}

        public Move(string str)
        {
            // TODO write some cheks & exceptions
            var parts = str.Substring(0, 5).Split('-');
            this.From = (Square)Enum.Parse(typeof(Square), parts[0].ToUpper());
            this.To = (Square)Enum.Parse(typeof(Square), parts[1].ToUpper());

            if (str.Length == 6)
                this.Type = promotionDict[str[5]];
        }

        public Move(Move from)
        {
            this.From = from.From;
            this.To = from.To;
            this.Type = from.Type;
        }
		
		public Square From
		{
			get; set;
		}
		
		public Square To
		{
			get; set;
		}
		
		public MoveType Type 
		{ 
			get; set;
		}

        public override string ToString()
        {
            string promotionPart = string.Empty;
            if ((int)this.Type >= (int)MoveType.Promotion)
                promotionPart = this.Type.GetPromotionFigure().ToString()[0].ToString();

            return string.Format("{0}-{1}{2}", this.From, this.To, promotionPart);
        }
	}
}

