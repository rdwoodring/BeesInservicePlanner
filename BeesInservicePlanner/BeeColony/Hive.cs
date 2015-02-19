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
        public List<UnitAppointment> BestMemoryMatrix { get; set; }
        public double BestMeasureOfQuality { get; set; }

        public Hive(List<Bee> bees, int maxNumberCycles, int maxNumberVisits, Random rand, AppointmentGenerator.AppointmentGenerator appointmentGenerator, double persuasionProbability = 0.90, double mistakeProbability = 0.01)
        {
            if (bees == null || rand == null || appointmentGenerator == null)
            {
                throw new ArgumentNullException();
            }

            this.BestMeasureOfQuality = -1;

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

            //set the global best measure of quality
            this.Bees = this.Bees.OrderBy(o => o.MeasureOfQuality).ToList();
            this.BestMeasureOfQuality = this.Bees[0].MeasureOfQuality;
            this.BestMemoryMatrix = new List<UnitAppointment>(this.Bees[0].MemoryMatrix);
        }

        public List<UnitAppointment> GenerateRandomMemoryMatrix()
        {
            return ag.GenerateAppointments();
        }

        public List<UnitAppointment> GenerateNeighborMemoryMatrix(List<UnitAppointment> memoryMatrix)
        {
            //check to see if there is any possibility to even create neighbor matrices
            //if there aren't at least two "unlocked" appointments that are adjacent, 
            //there is no possibility to create a viable neighbor matrix and we'll be stuck in an infinite loop below
            this.CheckForNeighborPotential(memoryMatrix);

            List<UnitAppointment> neighborMemoryMatrix = new List<UnitAppointment>(memoryMatrix);
            bool swapped = false;            

            do
            {
                int index = random.Next(0, memoryMatrix.Count), nextIndex;
                    
                if (index == memoryMatrix.Count - 1)
                {
                    nextIndex = 0;
                }
                else
                {
                    nextIndex = index + 1;
                }

                //if neither of the indices are locked we can swap them
                if (!memoryMatrix[index].Locked && !memoryMatrix[nextIndex].Locked)
                {
                    UnitAppointment newAppt1 = new UnitAppointment(neighborMemoryMatrix[index]), newAppt2 = new UnitAppointment(neighborMemoryMatrix[nextIndex]);

                    //swap the time slots
                    newAppt1.TimeSlot = neighborMemoryMatrix[nextIndex].TimeSlot;
                    newAppt2.TimeSlot = neighborMemoryMatrix[index].TimeSlot;

                    //remove the old appointments
                    UnitAppointment oldAppt1 = neighborMemoryMatrix[index], oldAppt2 = neighborMemoryMatrix[nextIndex];
                    neighborMemoryMatrix.Remove(oldAppt1);
                    neighborMemoryMatrix.Remove(oldAppt2);

                    //insert the new ones
                    neighborMemoryMatrix.Add(newAppt1);
                    neighborMemoryMatrix.Add(newAppt2);

                    //order the list by date
                    neighborMemoryMatrix.Sort((x, y) => DateTime.Compare(x.TimeSlot, y.TimeSlot));

                    //set swapped to true to break the loop
                    swapped = true;
                }

            } while (!swapped);

            return neighborMemoryMatrix;
        }

        private void CheckForNeighborPotential(List<UnitAppointment> memoryMatrix)
        {
            List<UnitAppointment> unlockedAppts = (from UnitAppointment ua in memoryMatrix
                                                   where !ua.Locked
                                                   select ua).ToList();

            bool viable = false;

            foreach(UnitAppointment unlockedAppt in unlockedAppts)
            {
                int index = memoryMatrix.IndexOf(unlockedAppt), nextIndex;

                //if the index is the last in the matrix, we'll check the status of the first item
                if (index == memoryMatrix.Count - 1)
                {
                    nextIndex = 0;
                }
                //otherwise, we'll check the next item
                else
                {
                    nextIndex = index + 1;
                }

                if (!memoryMatrix[nextIndex].Locked)
                {
                    viable = true;
                }
            }

            if (!viable)
            {
                //TODO: consider replacing this with some code to make all bees with this matrix inactive since it is not possible to generate neighbors
                throw new Exception("Can't move a locked appointment.  There are no viable neighbor matrices.");
            }
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
            //throw new NotImplementedException();
            List<UnitAppointment> neighborMatrix = this.GenerateNeighborMemoryMatrix(bee.MemoryMatrix);
            double neighborMeasureOfQuality = this.MeasureOfQuality(neighborMatrix);

            double mistake = random.NextDouble();

            //the new matrix is better than the old
            if (neighborMeasureOfQuality < bee.MeasureOfQuality)
            {
                //if we DON'T make a mistake and reject the better matrix
                if (mistake >= this.MistakeProbability)
                {
                    bee.MeasureOfQuality = neighborMeasureOfQuality;
                    bee.MemoryMatrix = neighborMatrix;

                    //reset visits
                    bee.NumberOfVisits = 0;

                    //also check to see if it's better than the global best
                    if (neighborMeasureOfQuality < this.BestMeasureOfQuality)
                    {
                        this.BestMeasureOfQuality = neighborMeasureOfQuality;
                        this.BestMemoryMatrix = neighborMatrix;
                    }

                    //and do the waggle dance
                    this.DoWaggleDance(bee);
                }
            }
            //the neighbor matrix is NOT better (or is equal) to the current matrix
            else if (neighborMeasureOfQuality >= bee.MeasureOfQuality)
            {
                //if we make a mistake and accept it anyway
                if (mistake < this.MistakeProbability)
                {
                    bee.MeasureOfQuality = neighborMeasureOfQuality;
                    bee.MemoryMatrix = neighborMatrix;

                    //reset visits
                    bee.NumberOfVisits = 0;

                    //and this dumb, mistaken bee will still try the waggle:
                    this.DoWaggleDance(bee);
                }
            }

            //add a visit to the counter
            bee.NumberOfVisits++;

            //the food source is exhausted
            if (bee.NumberOfVisits >= this.MaxNumberVisits)
            {
                bee.Status = BeeStatus.Inactive;
            }
        }

        private void ProcessScoutBee(Bee bee)
        {
            bee.MemoryMatrix = new List<UnitAppointment>(this.GenerateRandomMemoryMatrix());
            bee.MeasureOfQuality = this.MeasureOfQuality(bee.MemoryMatrix);

            if (bee.MeasureOfQuality < this.BestMeasureOfQuality)
            {
                this.DoWaggleDance(bee);
            }
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
                if (inactiveBee.MeasureOfQuality > bee.MeasureOfQuality)
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
