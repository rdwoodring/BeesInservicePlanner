using System;
using System.Text;
using System.Collections.Generic;

using BeesInservicePlanner.UnitData;

namespace BeesInservicePlanner.BeeColony
{
    public class Bee
    {
        public BeeStatus Status { get; set; }
        public List<UnitAppointment> MemoryMatrix { get; set; }
        public double MeasureOfQuality { get; set; }
        public int NumberOfVisits { get; set; }

        public Bee(BeeStatus status, List<UnitAppointment> memoryMatrix, double measureOfQuality = 0, int numberOfVisits = 0 )
        {
            this.Status = status;
            this.MemoryMatrix = memoryMatrix;
            this.MeasureOfQuality = measureOfQuality;
            this.NumberOfVisits = numberOfVisits;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.Status.ToString());
            sb.Append(this.MemoryMatrix.ToString());
            sb.Append(this.MeasureOfQuality.ToString());
            sb.Append(this.NumberOfVisits.ToString());

            return sb.ToString();
        }
    }
}
