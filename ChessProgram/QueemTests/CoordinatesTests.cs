using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BasicChessClasses;

namespace QueemTests
{
    [TestFixture]
    public class CoordinatesTests
    {
        [Test]
        public void CreateItemFromString()
        {
            string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
            FieldLetter[] fieldLetters = { FieldLetter.A, FieldLetter.B,
                                         FieldLetter.C, FieldLetter.D, 
                                         FieldLetter.E, FieldLetter.F,
                                         FieldLetter.G, FieldLetter.H };
            for (int i = 0; i < 8; ++i)
            {
                string line = letters[i] + i;
                
                Coordinates coords = new Coordinates(fieldLetters[i], i);
                Coordinates strCoords = new Coordinates(line);

                Assert.AreEqual(coords, strCoords);
            }
        }

        [Test]
        public void CompareWithOperator()
        {
            string line = "e4";
            int x = 4;
            int y = 4;
            Coordinates coords = new Coordinates(FieldLetter.E, 4);
            Coordinates strCoords = new Coordinates(line);
            Coordinates intCoords = new Coordinates(x, y);
            Coordinates otherCoords = new Coordinates(FieldLetter.H, 4);

            Assert.True(coords == strCoords);
            Assert.True(coords.Equals(strCoords));
            Assert.True(coords == intCoords);
            Assert.True(intCoords == strCoords);
            Assert.False(otherCoords == coords);

            Assert.False(otherCoords.GetHashCode() == coords.GetHashCode());
            Assert.True(coords.GetHashCode() == strCoords.GetHashCode());
        }
    }
}
