using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wax;

namespace NUnit.Tests1
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            Expression<Func<int, int>> Square = x => x * x;
            Expression<Func<int, int>> SquSquare = Wax.Wax.Unwrap<int, int>(
                x => Square.Expand(Square.Expand(x)));

            Expression<Func<int, int>> Square2 = x => ((x * x) * (x * x));

            Assert.IsTrue(Expression.Equals(SquSquare, Square2));
        }
    }
}
