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
        public void TestMethod1()
        {
            Expression<Func<int, int>> Square = x => x * x;
            Expression<Func<int, int>> SquSquare = Wax.Wax.Unwrap<int, int>(
                x => Square.Expand(Square.Expand(x)));
            Expression<Func<int, int>> Square2 = x => ((x * x) * (x * x));
            string s1 = Square2.ToString();
            string s2 = SquSquare.ToString();
            Assert.IsTrue(Expression.Equals(s1, s2));
        }

        [Test]
        public void TestMethod2()
        {
            Expression<Func<int, int>> SquSquare = x => ((x * x) * (x * x));
            Expression <Func<int, int>> Cube = Wax.Wax.Unwrap<int, int>( x => SquSquare.Expand(x) / x);
            Expression<Func<int, int>> Square2 = x => (((x * x) * (x * x)) / x);
            string c = Cube.ToString();
            string s = Square2.ToString();
            Assert.IsTrue(Expression.Equals(c,s));
        }

        [Test]
        public void TestMethod3()
        {
            Expression<Func<int, int>> Square = x => x * x;
            Expression<Func<int, int>> Cube = x => (((x * x) * (x * x)) / x);
            Expression <Func<int, int>> Foo = Wax.Wax.Unwrap<int, int>(
                    x => Cube.Expand(x + 1) * Square.Expand(x - 1));

            Expression<Func<int, int>> Square2 = x => (((((x + 1) * (x + 1)) * ((x + 1) * (x + 1))) / (x + 1)) * ((x - 1) * (x - 1)));
            string f = Foo.ToString();
            string s = Square2.ToString();
            Assert.IsTrue(Expression.Equals(f, s));
        }

        [Test]
        public void TestMethod4()
        {

            Assert.IsTrue(true);
        }
    }
}
