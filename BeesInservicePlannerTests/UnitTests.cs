using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BeesInservicePlanner.UnitData;

namespace BeesInservicePlannerTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void UnitConstructorScalingYAndBuildingProperties()
        {
            //arrange
            int x = 1, y = 2, building = 2;
            
            //act
            Unit unit = new Unit(x, y, building);

            //assert
            Assert.AreEqual(x, unit.X);
            Assert.AreEqual(y * 2, unit.Y);
            Assert.AreEqual(building * 10, unit.Building);
        }
    }
}
