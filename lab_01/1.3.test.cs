using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp2;



namespace FigureTests
{
    [TestClass]
    public class FigureTests
    {
        [TestMethod]
        public void LengthSideTest()
        {
            var a = new Point(4, 1);
            var b = new Point(1, 5);
            var c = new Point(1, 2);
            var f = new Figure(a, b, c, "TEST1");
            Assert.AreEqual(5, f.LengthSide(a, b));
        }

        [TestMethod]
        public void PerimeterCalculatorTest()
        {
            var a = new Point(0, 0);
            var b = new Point(0, 3);
            var c = new Point(4, 0);
            var f = new Figure(a, b, c, "TEST2");
            Assert.AreEqual(12, f.PerimeterCalculator());
        }


    }
}
