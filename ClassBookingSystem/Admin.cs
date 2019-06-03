using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace ClassBookingSystem
{
    public class Admin : AdminDetail
    {
       
        protected ArrayList classList;

        public Admin(string theAdminId, string theAdminPass) : base(theAdminId, theAdminPass)
        {
            classList = new ArrayList();
        }


        public void RecordAClass(Time reserveDetail)
        {
            classList.Add(reserveDetail);
        }

        public int GetNumberOfClass()
        {
            return classList.Count;
        }
        public Time[] GetReservationList()
        {
            Time[] reserveArray = new Time[classList.Count];
            
            for (int i = 0; i < classList.Count; i++)
            {
                reserveArray[i] = (Time)classList[i];
            }
            return reserveArray;
        }

        //duplicate reservation method checking
        public int CheckReservation(int aStartTime, int aEndTime, string aDate, string aVenue)
        {
            Time temp;
            for (int i = 0; i < classList.Count; i++)
            {
                temp = (Time)classList[i];

                if (aVenue == temp.Venue)
                {
                    if (aDate == temp.Date)
                    {
                        if (aStartTime >= temp.StartTime && aStartTime < temp.EndTime)
                        {
                            return i;
                        }

                    }
                }                                                                 
            }
            return -1;
        }

        public int CheckDateStatus(string aDate,string aVenue)
        {
            Time temp;
            for (int i = 0; i < classList.Count; i++)
            {
                temp = (Time)classList[i];

                if (aVenue == temp.Venue)
                {
                    if (aDate == temp.Date)
                    {

                        Console.WriteLine(temp.StartTime);
                    }
                }
            }
            return -1;
        }
        public Time CancelReservation(int numberCancel)
        {
            Time temp;
            temp = (Time)classList[numberCancel - 1];
            classList.RemoveAt(numberCancel-1);
            return temp;
        }
    }
}
