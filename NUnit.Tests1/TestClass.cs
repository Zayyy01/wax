using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wax;
using System.Collections;

namespace NUnit.Tests1
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void UnWrapOne()
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
        public void UnwrapTwo()
        {
            Expression<Func<int, int>> SquSquare = x => ((x * x) * (x * x));
            Expression <Func<int, int>> Cube = Wax.Wax.Unwrap<int, int>( x => SquSquare.Expand(x) / x);
            Expression<Func<int, int>> Square2 = x => (((x * x) * (x * x)) / x);
            string c = Cube.ToString();
            string s = Square2.ToString();
            Assert.IsTrue(Expression.Equals(c,s));
        }

        [Test]
        public void UnwrapThree()
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
        public void TestAnd()
        {
            Expression<Func<int, bool>> equals2 = x => x == 2;
            Expression<Func<int, bool>> equals3 = x => x == 3;
            var s = Wax.Wax.And(equals2, equals3);
            Assert.IsTrue(Expression.Equals(s.ToString(), "x => ((x == 2) AndAlso (x == 3))")); 
        }

        [Test]
        public void TestOr()
        {
            Expression<Func<int, bool>> equals2 = x => x == 2;
            Expression<Func<int, bool>> equals3 = x => x == 3;
            var s = Wax.Wax.Or(equals2, equals3);
            Assert.IsTrue(Expression.Equals(s.ToString(), "x => ((x == 2) OrElse (x == 3))"));
        }

        [Test]
        public void TestAll()
        {
            Expression<Func<int, bool>> equals2 = x => x == 2;
            Expression<Func<int, bool>> equals3 = x => x == 3;
            Expression<Func<int, bool>> equals4 = x => x == 4;
            var s = Wax.Wax.All(equals2, equals3, equals4);
            Assert.IsTrue(Expression.Equals(s.ToString(), "x => (((x == 2) AndAlso (x == 3)) AndAlso (x == 4))"));
        }

        [Test]
        public void TestAny()
        {
            Expression<Func<int, bool>> equals2 = x => x == 2;
            Expression<Func<int, bool>> equals3 = x => x == 3;
            Expression<Func<int, bool>> equals4 = x => x == 4;
            var s = Wax.Wax.Any(equals2, equals3, equals4);
            Assert.IsTrue(Expression.Equals(s.ToString(), "x => (((x == 2) OrElse (x == 3)) OrElse (x == 4))"));
        }

        [Test]
        public void TestInverse()
        {
            Expression<Func<int, bool>> equals2 = x => x == 2;
            var s = Wax.Wax.Inverse(equals2);
            Assert.IsTrue(Expression.Equals(s.ToString(), "x => (x != 2)"));
        }
    }

}
