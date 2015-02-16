using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeesInservicePlanner.UnitData
{
    public class Unit
    {
        public int X {get; set;}
        public int Y {get; set;} 
        public int Building {get; set;}
        public string Name { get; set; }

        public Unit(int x, int y, int building)
        {
            this.X = x;

            //convert the y and building values to create a "movement penalty" for changing floors or buildings
            this.Y = y * 2;
            this.Building = building * 10;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: " + this.Name);
            sb.Append("X: " + this.X.ToString());
            sb.Append("Y: " + this.Y.ToString());
            sb.Append("Building: " + this.Building.ToString());

            return sb.ToString();
            //return base.ToString();
        }
    }
}
