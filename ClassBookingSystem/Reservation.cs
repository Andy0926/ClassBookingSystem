using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClassBookingSystem
{
    public class Reservation 
    {
        private string name;
        public string Name
        {
            get { return name; }

        }
      
        private string purpose;
        public string Purpose
        {
            get { return purpose; }

        }

        private string venue;
        public string Venue
        {
            get { return venue; }

        }

        private string date;
        public string Date
        {
            get { return date; }

        }

        public Reservation(string theName, string thePurpose, string theDate, string theVenue)
        {          
                name = theName;
                purpose = thePurpose; //midterm,lecture,tutorial etc
                venue = theVenue;
                date = theDate;          
        }
        public Reservation():this("","","","")
        {

        }
    }
}
