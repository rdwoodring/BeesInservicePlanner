using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BeesInservicePlanner.UnitData;
using BeesInservicePlanner.AppointmentGenerator;

namespace BeesInservicePlannerTests
{
    [TestClass]
    public class AppointmentGeneratorTests
    {
        private TimeSpan appointmentTime = new TimeSpan(0, 30, 0);
        private DateTime startDate = new DateTime(2015, 2, 15);
        private DateTime endDate = new DateTime(2015, 2, 22);

        [TestMethod]
        public void GenerateAppointmentsReturnsRightNumberOfAppointments()
        {
            //arrange
            Unit unit = new Unit(1, 1, 1);
            Unit lockedUnit = new Unit(1, 2, 3);
            UnitAppointment lockedAppointment = new UnitAppointment(lockedUnit, new DateTime(2015, 2, 25, 10, 30, 0), true);

            List<Unit> unitList = new List<Unit>() { unit };
            List<UnitAppointment> lockedAppointments = new List<UnitAppointment>() {lockedAppointment};

            AppointmentGenerator ag = new AppointmentGenerator(unitList, lockedAppointments, startDate, endDate, this.appointmentTime);

            //act
            int numberOfAppointments = ag.GenerateAppointments().Count;

            //assert
            Assert.AreEqual(unitList.Count + lockedAppointments.Count, numberOfAppointments);
        }

        [TestMethod]
        public void GenerateAppointmentsAppointmentsAreOrderedByDateTime()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            List<UnitAppointment> lockedAppointments = new List<UnitAppointment>() { lockedAppointment1, lockedAppointment2 };

            AppointmentGenerator ag = new AppointmentGenerator(new List<Unit>(), lockedAppointments, startDate, endDate, this.appointmentTime);

            //act
            List<UnitAppointment> appointments = ag.GenerateAppointments();

            //assert
            Assert.IsTrue(appointments[0].TimeSlot < appointments[1].TimeSlot);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AppointmentGeneratorConstructorThrowsIfTwoLockedAppointmentsWithinAppointmentTimeMinutesOfEachOther()
        {
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 10, 15, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Console.WriteLine("Time Delta: {0}", Math.Abs((lockedAppointment1.TimeSlot - lockedAppointment2.TimeSlot).Minutes).ToString());

            List<UnitAppointment> lockedAppointments = new List<UnitAppointment>() { lockedAppointment1, lockedAppointment2 };

            //act
            AppointmentGenerator ag = new AppointmentGenerator(new List<Unit>(), lockedAppointments, startDate, endDate, this.appointmentTime);

            
        }

        [TestMethod]
        public void AppointmentGeneratorConstructorAcceptsValidLockedInAppointments()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            List<UnitAppointment> lockedAppointments = new List<UnitAppointment>() { lockedAppointment1, lockedAppointment2 };

            //act
            AppointmentGenerator ag = new AppointmentGenerator(new List<Unit>(), lockedAppointments, startDate, endDate, this.appointmentTime);

            //assert
            Assert.AreEqual(lockedAppointments.Count, ag.LockedInAppointments.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AppointmentGeneratorConstructorThrowsIfNullArguments()
        {
            AppointmentGenerator ag1 = new AppointmentGenerator(null, new List<UnitAppointment>(), startDate, endDate, this.appointmentTime);
            AppointmentGenerator ag2 = new AppointmentGenerator(new List<Unit>(), null, startDate, endDate, this.appointmentTime);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AppointmentGeneratorConstructorThrowsIfDateRangeIsLessThanTotalAppointmentTimes()
        {
            DateTime start = new DateTime(2015, 2, 28, 10, 30, 0);
            DateTime end = start.AddMinutes(30);

            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            List<Unit> units = new List<Unit>() { lockedUnit1, lockedUnit2 };

            //act
            AppointmentGenerator ag = new AppointmentGenerator(units, new List<UnitAppointment>(), start, end, this.appointmentTime);
        }
    }
}
