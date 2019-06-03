using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBookingSystem
{
    public class Time : Reservation
    {
        private int startTime;
        public int StartTime
        {
            get { return startTime; }
        }

        private int endTime;
        public int EndTime
        {
            get { return endTime; }
        }

        public Time(int theStartTime, int theEndTime,string theName, string thePurpose, string theDate, string theVenue )
            :base(theName,thePurpose,theDate,theVenue )
        {
            startTime = theStartTime;
            endTime = theEndTime;
        }
    }
}
