using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BeesInservicePlanner.BeeColony;
using BeesInservicePlanner.UnitData;
using BeesInservicePlanner.AppointmentGenerator;

namespace BeesInservicePlannerTests
{
    [TestClass]
    public class HiveTests
    {
        private TimeSpan appointmentTime = new TimeSpan(0, 30, 0);
        private DateTime startDate = new DateTime(2015, 2, 15);
        private DateTime endDate = new DateTime(2015, 2, 22);
        private AppointmentGenerator ag;
        private List<Bee> bees;
        private int maxNumberCycles = 1000, maxNumberVisits = 100;

        [TestMethod]
        public void HiveConstructorSettingGlobalBestMeasureOfQuality()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            Hive hive = new Hive(this.bees, this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            Assert.IsTrue(hive.BestMeasureOfQuality > 0);
        }

        [TestMethod]
        public void HiveConstructorGlobalBestMeasureOfQualityIsLowest()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            Hive hive = new Hive(this.bees, this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            List<Bee> tempBees = new List<Bee>(hive.Bees.OrderBy(o => o.MeasureOfQuality).ToList());

            for (int i = 0; i < tempBees.Count - 1; i++)
            {
                Console.WriteLine("{0} <= {1}", tempBees[i].MeasureOfQuality, tempBees[i + 1].MeasureOfQuality);
                Assert.IsTrue(tempBees[i].MeasureOfQuality <= tempBees[i + 1].MeasureOfQuality);
            }

            Assert.AreEqual(tempBees[0].MeasureOfQuality, hive.BestMeasureOfQuality);
        }

        [TestMethod]
        public void HiveConstructorSettingGlobalBestMemoryMatrix()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            Hive hive = new Hive(this.bees, this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            Assert.AreNotEqual(null, hive.BestMemoryMatrix);
        }

        [TestMethod]
        public void GenerateRandomMemoryMatrixReturningResults()
        {
            //arrange
            //some done in Setup
            Hive hive = new Hive(this.bees, this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            List<UnitAppointment> appts = hive.GenerateRandomMemoryMatrix();

            Assert.IsTrue(appts.Count > 0);
        }

        [TestMethod]
        public void GenerateNeighborMemoryMatrixReturnsAppointmentsInTimeOrder()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            Hive hive = new Hive(this.bees, this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            AppointmentGenerator gen = new AppointmentGenerator(units, appts, this.startDate, this.endDate, new TimeSpan(0, 30, 0));

            //act
            List<UnitAppointment> allAppts = gen.GenerateAppointments();
            List<UnitAppointment> swappedAppts = hive.GenerateNeighborMemoryMatrix(allAppts);

            //assert
            for (int i = 0; i < swappedAppts.Count - 1; i++)
            {
                Assert.IsTrue(swappedAppts[i].TimeSlot < swappedAppts[i + 1].TimeSlot);
            }
        }

        [TestMethod]
        public void GenerateNeighborMemoryMatrixReturnsCorrectNumberOfAppointments()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            Hive hive = new Hive(this.bees, this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            AppointmentGenerator gen = new AppointmentGenerator(units, appts, this.startDate, this.endDate, new TimeSpan(0, 30,0));

            //act
            List<UnitAppointment> allAppts = gen.GenerateAppointments();
            List<UnitAppointment> swappedAppts = hive.GenerateNeighborMemoryMatrix(allAppts);

            //assert
            Assert.AreEqual(allAppts.Count, swappedAppts.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerateNeighborMemoryMatrixWontMoveLockedAppointments()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            Hive hive = new Hive(this.bees, this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            //act
            hive.GenerateNeighborMemoryMatrix(appts);
        }

        [TestMethod]
        public void MeasureOfQualityReturningCorrectCalculation()
        {
            //arrange
            Hive hive = new Hive(this.bees, this.maxNumberCycles, this.maxNumberVisits, new Random(), this.ag);

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                new UnitAppointment(new Unit(2, 2, 3),new DateTime(205,2,17,10,30,0)),
                new UnitAppointment(new Unit(2, 1, 3),new DateTime(205,2,17,12,0,0))
            };

            //act
            double score = hive.MeasureOfQuality(appts);

            double xDiff, yDiff, buildingDiff, distance, minutes, correctScore;

            xDiff = Math.Pow(2 - 2, 2);
            yDiff = Math.Pow(2*2 - 2*1, 2);
            buildingDiff = Math.Pow(10*3 - 10*3, 2);

            distance = Math.Sqrt(xDiff + yDiff + buildingDiff);

            DateTime startTime = appts[0].TimeSlot, endTime = appts[appts.Count - 1].TimeSlot;

            minutes = (endTime - startTime).TotalMinutes;

            correctScore = distance + minutes;

            Console.WriteLine("Expected: " + correctScore);
            Console.WriteLine("Actual: " + score);

            //assert
            Assert.AreEqual(correctScore, score);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HiveConstructorThrowsOnNullArgument()
        {
            Hive hive = new Hive(null, 1000, 100, new Random(), this.ag);
        }

        [TestMethod]
        public void SolveScoutBeeGeneratingNewMatrix()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            List<Bee> beeList = new List<Bee>() 
            {
                new Bee(BeeStatus.Scout, new List<UnitAppointment>())
            };

            Hive hive = new Hive(beeList, 1, this.maxNumberVisits, new Random(), this.ag);

            List<UnitAppointment> origMatrix = hive.Bees[0].MemoryMatrix;

            hive.Solve();

            List<UnitAppointment> newMatrix = hive.Bees[0].MemoryMatrix;

            Assert.AreNotEqual(origMatrix, newMatrix);
        }

        [TestMethod]
        public void SolveScoutBeeConvincingInactiveBeeToBecomeActiveAt100PercentPersuasion()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            List<Bee> beeList = new List<Bee>() 
            {
                new Bee(BeeStatus.Scout, new List<UnitAppointment>()),
                new Bee(BeeStatus.Inactive, new List<UnitAppointment>())
            };
            
            Hive hive = new Hive(beeList, 1, this.maxNumberVisits, new Random(), this.ag, 1);

            //act
            hive.BestMeasureOfQuality = double.MaxValue;

            List<Bee> inactiveBee = (from bee in hive.Bees
                                     where bee.Status == BeeStatus.Inactive
                                     select bee).ToList();

            foreach (Bee bee in inactiveBee)
            {
                bee.MeasureOfQuality = double.MaxValue;
            }

            hive.Solve();

            inactiveBee = (from bee in hive.Bees
                           where bee.Status == BeeStatus.Inactive
                           select bee).ToList();
            
            //assert
            Assert.AreEqual(0, inactiveBee.Count);
        }

        [TestMethod]
        public void SolveScoutBeeFailsConvincingInactiveBeeToBecomeActiveAt0PercentPersuasion()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            List<Bee> beeList = new List<Bee>() 
            {
                new Bee(BeeStatus.Scout, new List<UnitAppointment>()),
                new Bee(BeeStatus.Inactive, new List<UnitAppointment>())
            };

            Hive hive = new Hive(beeList, 1, this.maxNumberVisits, new Random(), this.ag, 0);

            //act
            hive.BestMeasureOfQuality = double.MaxValue;

            List<Bee> inactiveBee = (from bee in hive.Bees
                                     where bee.Status == BeeStatus.Inactive
                                     select bee).ToList();

            foreach (Bee bee in inactiveBee)
            {
                bee.MeasureOfQuality = double.MaxValue;
            }

            hive.Solve();

            inactiveBee = (from bee in hive.Bees
                           where bee.Status == BeeStatus.Inactive
                           select bee).ToList();

            //assert
            Assert.AreEqual(1, inactiveBee.Count);
        }

        [TestMethod]
        public void SolveActiveBeeGetsBetterMemoryMatrixAt0PercentMistake()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            List<Bee> beeList = new List<Bee>() 
            {
                new Bee(BeeStatus.Active, new List<UnitAppointment>())
            };

            Hive hive = new Hive(beeList, 1, this.maxNumberVisits, new Random(), this.ag, 1, 0);

            hive.Bees[0].MeasureOfQuality = double.MaxValue;

            List<UnitAppointment> origAppts = hive.Bees[0].MemoryMatrix;

            //act
            hive.Solve();

            List<UnitAppointment> newAppts = hive.Bees[0].MemoryMatrix;


            //assert
            Assert.AreNotEqual(origAppts, newAppts);
            Assert.AreNotEqual(double.MaxValue, hive.Bees[0].MeasureOfQuality);
        }

        [TestMethod]
        public void SolveActiveBeeWorseMemoryMatrixAt100PercentMistake()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            List<Bee> beeList = new List<Bee>() 
            {
                new Bee(BeeStatus.Active, new List<UnitAppointment>())
            };

            Hive hive = new Hive(beeList, 1, this.maxNumberVisits, new Random(), this.ag, 1, 1);

            hive.Bees[0].MeasureOfQuality = 0;

            List<UnitAppointment> origAppts = hive.Bees[0].MemoryMatrix;

            //act
            hive.Solve();

            List<UnitAppointment> newAppts = hive.Bees[0].MemoryMatrix;


            //assert
            Assert.AreNotEqual(origAppts, newAppts);
            Assert.AreNotEqual(0, hive.Bees[0].MeasureOfQuality);
        }

        [TestMethod]
        public void SolveActiveBeeRejectsBetterMemoryMatrixAt100PercentMistake()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            List<Bee> beeList = new List<Bee>() 
            {
                new Bee(BeeStatus.Active, new List<UnitAppointment>())
            };

            Hive hive = new Hive(beeList, 1, this.maxNumberVisits, new Random(), this.ag, 1, 1);

            hive.Bees[0].MeasureOfQuality = double.MaxValue;

            List<UnitAppointment> origAppts = hive.Bees[0].MemoryMatrix;

            //act
            hive.Solve();

            List<UnitAppointment> newAppts = hive.Bees[0].MemoryMatrix;


            //assert
            Assert.AreEqual(origAppts, newAppts);
            Assert.AreEqual(double.MaxValue, hive.Bees[0].MeasureOfQuality);
        }

        [TestMethod]
        public void SolveActiveBeeRejectsWorseMemoryMatrixAt0PercentMistake()
        {
            //arrange
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            UnitAppointment lockedAppointment1 = new UnitAppointment(lockedUnit1, new DateTime(2015, 2, 25, 11, 30, 0), true);
            UnitAppointment lockedAppointment2 = new UnitAppointment(lockedUnit2, new DateTime(2015, 2, 25, 10, 30, 0), true);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);
            Unit unit3 = new Unit(3, 1, 2);
            Unit unit4 = new Unit(4, 3, 3);
            Unit unit5 = new Unit(1, 1, 5);

            List<Unit> units = new List<Unit>() { unit1, unit2, unit3, unit4, unit5 };

            List<UnitAppointment> appts = new List<UnitAppointment>()
            {
                lockedAppointment1,
                lockedAppointment2
            };

            List<Bee> beeList = new List<Bee>() 
            {
                new Bee(BeeStatus.Active, new List<UnitAppointment>())
            };

            Hive hive = new Hive(beeList, 1, this.maxNumberVisits, new Random(), this.ag, 1, 0);

            hive.Bees[0].MeasureOfQuality = 0;

            List<UnitAppointment> origAppts = hive.Bees[0].MemoryMatrix;

            //act
            hive.Solve();

            List<UnitAppointment> newAppts = hive.Bees[0].MemoryMatrix;


            //assert
            Assert.AreEqual(origAppts, newAppts);
            Assert.AreEqual(0, hive.Bees[0].MeasureOfQuality);
        }

        [TestInitialize()]
        public void Initialize()
        {
            Unit lockedUnit1 = new Unit(1, 2, 3);
            Unit lockedUnit2 = new Unit(1, 2, 2);

            Unit unit1 = new Unit(2, 2, 3);
            Unit unit2 = new Unit(2, 1, 3);

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
