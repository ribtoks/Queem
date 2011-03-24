using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BasicChessClasses;

namespace QueemTests
{
    [TestFixture]
    public class FigureCollectionTest
    {
        [Test]
        public void DoubleKillingTest()
        {
            Coordinates coords1 = new Coordinates("e6");
            Coordinates coords2 = new Coordinates("d5");
            FigureColor myColor = FigureColor.White;

            FigureCollection<Pawn> figures = new FigureCollection<Pawn>();
            figures.AddFigure(coords1, FigureType.Pawn, myColor);
            figures.AddFigure(coords2, FigureType.Pawn, myColor.GetOppositeColor());

            figures.RemoveFigure(coords1);
            figures.RestoreFigure(coords1);
            Assert.NotNull(figures[coords1],
                "Cannot restore figure 1 after one deletion");

            // remove first figure on that coord
            figures.RemoveFigure(coords1);
            // move second figure there
            figures.MoveFigure(coords2, coords1);

            // remove second figure on that coord
            figures.RemoveFigure(coords1);

            // try to restore second figure
            figures.RestoreFigure(coords1);
            Assert.NotNull(figures[coords1], 
                "Cannot restore figure 2 after one deletion");

            // move second figure out of there
            figures.MoveFigure(coords1, coords2);

            Assert.DoesNotThrow(() =>
            {
                // try to restore first figure
                figures.RestoreFigure(coords1);
            });

            Assert.NotNull(figures[coords1]);
            Assert.True(figures[coords1].Type == FigureType.Pawn);
        }
    }
}
