using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;

namespace RectangleTests
{
    [TestClass]
    public class RectangleTest
    {
        [TestMethod]
        public void AreaTest()
        {
            var v = new Rectangle(4, 5);
            Assert.AreEqual(20, v.Area);
        }

        [TestMethod]
        public void PerimeterTest()
        { 
            var v = new Rectangle(4, 5);
            Assert.AreEqual(18, v.Perimeter);
        }
    }
}
