using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BeesInservicePlanner.BeeColony;
using BeesInservicePlanner.UnitData;
using BeesInservicePlanner.AppointmentGenerator;

namespace BeesInservicePlannerTests
{
    [TestClass]
    public class InserviceHiveTests
    {
        private TimeSpan appointmentTime = new TimeSpan(0, 30, 0);
        private DateTime startDate = new DateTime(2015, 2, 15);
        private DateTime endDate = new DateTime(2015, 2, 22);
        private AppointmentGenerator ag;
        private List<Bee> bees;
        private int maxNumberCycles = 1000, maxNumberVisits = 100;

        [TestMethod]
        public void GenerateRandomMemoryMatrixReturningResults()
        {
            //arrange
            //some done in Setup
            Hive hive = new Hive(new List<Bee>(), this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            List<UnitAppointment> appts = hive.GenerateRandomMemoryMatrix();

            Assert.IsTrue(appts.Count > 0);
        }

        [TestMethod]
        public void MeasureOfQualityReturningCorrectCalculation()
        {
            Hive hive = new Hive(new List<Bee>(), this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            throw new NotImplementedException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InserviceHiveConstructorThrowsOnNullArgument()
        {
            Hive hive = new Hive(null, 1000, 100, new Random(), this.ag);
        }

        [TestInitialize()]
        public void Initialize()
        {
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 2, 1);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            List<UnitAppointment> lockedAppointments = new List<UnitAppointment>() { lockedAppointment1, lockedAppointment2 };
            List<Unit> units = new List<Unit>() { unit1, unit2 };

            ag = new AppointmentGenerator(units, lockedAppointments, startDate, endDate, this.appointmentTime);

            bees = new List<Bee>() {
            new Bee(BeeStatus.Active, new List<UnitAppointment>()),
            new Bee(BeeStatus.Active, new List<UnitAppointment>()),
            new Bee(BeeStatus.Active, new List<UnitAppointment>()),
            new Bee(BeeStatus.Active, new List<UnitAppointment>()),
            new Bee(BeeStatus.Active, new List<UnitAppointment>()),
            new Bee(BeeStatus.Active, new List<UnitAppointment>()),
            new Bee(BeeStatus.Active, new List<UnitAppointment>()),
            new Bee(BeeStatus.Scout, new List<UnitAppointment>()),
            new Bee(BeeStatus.Inactive, new List<UnitAppointment>())
            };
        }
    }
}
