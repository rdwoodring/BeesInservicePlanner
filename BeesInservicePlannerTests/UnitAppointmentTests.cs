using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BeesInservicePlanner.UnitData;

namespace BeesInservicePlannerTests
{
    [TestClass]
    public class UnitAppointmentTests
    {
        [TestMethod]
        public void UnitAppointmentCopyConstructorCreatesNewInstance()
        {
            UnitAppointment uaOrig = new UnitAppointment(new Unit(1, 1, 1), new DateTime(2015, 1, 1, 1, 0, 0));
            UnitAppointment uaNewCopy = new UnitAppointment(uaOrig);

            Assert.AreNotEqual(uaOrig, uaNewCopy);
        }
    }
}
