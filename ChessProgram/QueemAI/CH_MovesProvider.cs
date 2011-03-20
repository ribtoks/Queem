using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    class CH_MovesProvider : MovesProvider
    {
        public CH_MovesProvider(FigureColor myColor, FigureStartPosition myStartPos)
            : base(myColor, myStartPos)
        {
        }
    }
}
