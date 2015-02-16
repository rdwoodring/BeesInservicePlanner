using System;
using System.Linq;
using System.Collections.Generic;

using BeesInservicePlanner.UnitData;
using BeesInservicePlanner.AppointmentGenerator;

namespace BeesInservicePlanner.BeeColony
{
    public class Hive
    {
        private AppointmentGenerator.AppointmentGenerator ag;

        static Random random = null;

        public int TotalNumberBees { get; private set; }
        public int NumberInactive { get; private set; }
        public int NumberActive { get; private set; }
        public int NumberScout { get; private set; }

        public int MaxNumberCycles { get; set; }
        public int MaxNumberVisits { get; set; }

        public double PersuasionProbability { get; private set; }
        public double MistakeProbability { get; private set; }

        public List<Bee> Bees { get; set; }
        public List<Object> BestMemoryMatrix { get; set; }
        public double BestMeasureOfQuality { get; set; }

        public Hive(List<Bee> bees, int maxNumberCycles, int maxNumberVisits, Random rand, AppointmentGenerator.AppointmentGenerator appointmentGenerator, double persuasionProbability = 0.90, double mistakeProbability = 0.01)
        {
            if (bees == null || rand == null || appointmentGenerator == null)
            {
                throw new ArgumentNullException();
            }

            this.Bees = bees;
            this.MaxNumberCycles = maxNumberCycles;
            this.MaxNumberVisits = maxNumberVisits;

            this.PersuasionProbability = persuasionProbability;
            this.MistakeProbability = mistakeProbability;

            random = rand;

            this.ag = appointmentGenerator;

            this.TotalNumberBees = this.Bees.Count;
            this.NumberInactive = (from Bee bee in this.Bees
                                   where bee.Status == BeeStatus.Inactive
                                   select bee).ToList().Count;
            this.NumberScout = (from Bee bee in this.Bees
                                where bee.Status == BeeStatus.Scout
                                select bee).ToList().Count;
            this.NumberActive = this.TotalNumberBees - this.NumberInactive - this.NumberScout;

            foreach (Bee bee in this.Bees)
            {
                //setting the initial memory matrix with a randomized solution
                bee.MemoryMatrix = this.GenerateRandomMemoryMatrix();

                //measuring the memory matrix's quality and assign it to the bee's measure of quality property
                bee.MeasureOfQuality = this.MeasureOfQuality(bee.MemoryMatrix);
            }
        }

        public List<UnitAppointment> GenerateRandomMemoryMatrix()
        {
            return ag.GenerateAppointments();
        }

        public List<UnitAppointment> GenerateNeighborMemoryMatrix(List<UnitAppointment> memoryMatrix)
        {
            throw new NotImplementedException();
        }

        public double MeasureOfQuality(List<UnitAppointment> memoryMatrix)
        {
            double totalQuality = 0;

            //measure euclidean distance between each unit
            for (int i = 0; i < memoryMatrix.Count - 1; i++)
            {
                double xDiff, yDiff, buildingDiff;

                xDiff = Math.Pow((memoryMatrix[i].Unit.X - memoryMatrix[i + 1].Unit.X), 2);
                yDiff = Math.Pow((memoryMatrix[i].Unit.Y - memoryMatrix[i + 1].Unit.Y), 2);
                buildingDiff = Math.Pow((memoryMatrix[i].Unit.Building - memoryMatrix[i + 1].Unit.Building), 2);

                double distance = Math.Sqrt(xDiff + yDiff + buildingDiff);

                totalQuality += distance;
            }

            //measure total used time for all appointments; add in a penalty for "down time" that's "locked" between the first and last appointment
            DateTime startTime = memoryMatrix[0].TimeSlot;
            DateTime endTime = memoryMatrix[memoryMatrix.Count - 1].TimeSlot;

            double totalMinutes = (endTime - startTime).TotalMinutes;

            totalQuality += totalMinutes;

            return totalQuality;
        }

        public virtual void Solve()
        {
            for (int i = 0; i < this.MaxNumberCycles; i++)
            {
                foreach (Bee bee in this.Bees)
                {
                    if (bee.Status == BeeStatus.Active)
                    {
                        this.ProcessActiveBee(bee);
                    }
                    else if (bee.Status == BeeStatus.Scout)
                    {
                        this.ProcessScoutBee(bee);
                    }
                    else if (bee.Status == BeeStatus.Inactive)
                    {
                        this.ProcessInactiveBee(bee);
                    }
                }
            }
        }

        private void ProcessActiveBee(Bee bee)
        {
            throw new NotImplementedException();
        }

        private void ProcessScoutBee(Bee bee)
        {
            throw new NotImplementedException();
        }

        protected virtual void ProcessInactiveBee(Bee bee)
        {
            return;
        }

        protected virtual void DoWaggleDance(Bee bee)
        {
            List<Bee> inactiveBees = (from Bee inactiveBee in this.Bees
                                         where inactiveBee.Status == BeeStatus.Inactive
                                         select inactiveBee).ToList();

            foreach (Bee inactiveBee in inactiveBees)
            {
                if (inactiveBee.MeasureOfQuality < bee.MeasureOfQuality)
                {
                    double persuasion = random.NextDouble();
                    if (persuasion < this.PersuasionProbability)
                    {
                        inactiveBee.MemoryMatrix = new List<UnitAppointment>(bee.MemoryMatrix);
                        inactiveBee.Status = BeeStatus.Active;
                    }
                }
            }
        }
    }
}
