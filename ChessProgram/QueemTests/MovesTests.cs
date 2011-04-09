using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BasicChessClasses;

namespace QueemTests
{
    [TestFixture]
    public class MovesTests
    {
        [Test]
        public void StringTest()
        {
            string line = "c6-c7b";
            PromotionMove pm = new PromotionMove(line);

            Assert.AreEqual(line, pm.ToString());
        }
    }
}
