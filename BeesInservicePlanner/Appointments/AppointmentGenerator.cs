using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BeesInservicePlanner.UnitData;

namespace BeesInservicePlanner.AppointmentGenerator
{
    public class AppointmentGenerator
    {
        static Random random = new Random();
        
        public List<Unit> Units { get; private set; }
        public List<UnitAppointment> LockedInAppointments { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public TimeSpan AppointmentLength { get; set; }

        public AppointmentGenerator(List<Unit> units, List<UnitAppointment> lockedInAppointments, DateTime startDate, DateTime endDate, TimeSpan appointmentLength)
        {            
            this.LockedInAppointments = new List<UnitAppointment>(lockedInAppointments);
            this.Units = new List<Unit>(units);

            this.StartDate = startDate;
            this.EndDate = endDate;

            this.AppointmentLength = appointmentLength;

            if (this.LockedInAppointments.Count + this.Units.Count > 0)
            {
                this.ValidateTotalAppointmentTimes();
            }

            if (this.LockedInAppointments.Count > 0)
            {              
                this.ValidateLockedInAppointments();                
            }
        }        

        public List<UnitAppointment> GenerateAppointments()
        {
            List<UnitAppointment> appointments = new List<UnitAppointment>();
            appointments.AddRange(this.LockedInAppointments);

            //for each unit in the list, we want to select a time and date, round it to the nearest 5 minutes,and then make sure it's not within appointmentLength of any other appointment
            int totalTimeSpan = Convert.ToInt32(Math.Round((this.EndDate - this.StartDate).TotalMinutes));
            foreach (Unit unit in this.Units)
            {
                bool isDuplicate = true;
                UnitAppointment tempAppointment;
                do
                {
                    int minutesFromStart = random.Next(0, totalTimeSpan);
                    DateTime selectedDateTime = this.StartDate.AddMinutes(minutesFromStart);
                    tempAppointment = new UnitAppointment(unit, selectedDateTime);
                    isDuplicate = this.CheckForDuplicates(tempAppointment, appointments);
                } while (isDuplicate);

                appointments.Add(tempAppointment);
            }

            appointments.Sort((x, y) => DateTime.Compare(x.TimeSlot, y.TimeSlot));

            return appointments;
        }

        private bool CheckForDuplicates(UnitAppointment tempAppointment, List<UnitAppointment> appointments)
        {
            bool isDuplicate = true;
            
            DateTime lowerBound = tempAppointment.TimeSlot;
            DateTime upperBound = tempAppointment.TimeSlot.Add(this.AppointmentLength);

            UnitAppointment closeAppt = (from UnitAppointment ua in appointments
                                            where (ua.TimeSlot > lowerBound && ua.TimeSlot < upperBound) && ua != tempAppointment
                                            select ua).FirstOrDefault();
            
            if (closeAppt == null)
            {
                isDuplicate = false;
            }

            return isDuplicate;
        }

        private void ValidateLockedInAppointments()
        {            
            //debug
            //int diff = Math.Abs((this.LockedInAppointments[0].TimeSlot - this.LockedInAppointments[1].TimeSlot).Minutes);

            foreach(UnitAppointment appt in this.LockedInAppointments)
            {
                bool isDuplicate = this.CheckForDuplicates(appt, this.LockedInAppointments);

                if (isDuplicate)
                {
                    throw new Exception("Can't provide locked in appointments with " + this.AppointmentLength.ToString() + " minutes of each other.");
                }
            }
        }

        private void ValidateTotalAppointmentTimes()
        {
            double totalMinutes = (this.EndDate - this.StartDate).TotalMinutes;
            int aggregateApptTimes = (this.LockedInAppointments.Count + this.Units.Count) * this.AppointmentLength.Minutes;

            if (totalMinutes < aggregateApptTimes)
            {
                throw new ArgumentException("The specified date range does not have enough space for all of the requested appointments. Please adjust the date range, appointment length, or number of units.");
            }
        }
    }
}
