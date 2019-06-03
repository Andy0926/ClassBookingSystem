using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace ClassBookingSystem
{
    public class System
    {
        public void test()
        {
            Admin obAdmin;
            string aId, aPassword;
            Console.WriteLine("Welcome to Class booking system");
            Console.WriteLine("Please enter your ID and Password");
            Console.Write("Enter Your ID: ");
            aId = Console.ReadLine();
            Console.Write("Enter Your Password: ");
            aPassword = Console.ReadLine();
            obAdmin = new Admin(aId, aPassword);
            ReadData(obAdmin);


            int choice = -1;
            //choice =1 will loop
            while (choice == -1)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to class reservation system");
                    Console.WriteLine("1. Make a reservation/booking");
                    Console.WriteLine("2. View the booking status of individual of all venues");
                    Console.WriteLine("3. Cancel Reservations");
                    Console.WriteLine("4. Quit and save");

                    choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            Reserve(obAdmin);
                            choice = -1;
                            break;

                        case 2:
                            Display(obAdmin);
                            Console.WriteLine("Press any key to continue");
                            Console.ReadLine();
                            Console.Clear();
                            choice = -1;
                            break;

                        case 3:
                            Cancel(obAdmin);
                            choice = -1;
                            break;

                        case 4:
                            WriteData(obAdmin);
                            Environment.Exit(0);
                            choice = -1;
                            break;

                        default:
                            Console.WriteLine("Wrong Choice please enter again");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadLine();
                            choice = -1;
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("menu CATCH");
                    Console.WriteLine("Wrong Input");
                    Console.WriteLine("Press any key to enter again");
                    Console.ReadLine();
                    choice = -1;
                }
            }
        }

        public static void Reserve(Admin adminClass)
        {
            string name, purpose, venue, date;
            int startTime, endTime;
            int nError, dError, tError, pError;
            char proceed = 'y';
            Reservation objReserveDetail;
            do
            {
                Console.Clear();
                do
                {
                    //Input the name

                    Console.Write("Enter the name(Do not enter space between the name): ");
                    name = Console.ReadLine();
                    name = name.ToUpper();

                    //check if it contains only string
                    if (!Regex.IsMatch(name, @"^[\p{L}]+$"))
                    {
                        Console.WriteLine("Wrong input. Please enter a valid name");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        Console.Clear();
                        nError = 1;
                    }
                    else
                        nError = 0;
                } while (nError == 1); // to validate name error

                do
                {
                    Console.Clear();
                    Console.Write("Enter the Date (dd/mm/yyyy): ");
                    date = Console.ReadLine();

                    //DateTime method
                    DateTime dt;
                    //validate time format see if it is true
                    if (DateTime.TryParseExact(date, "dd/m/yyyy", null, DateTimeStyles.None, out dt) == true)
                    {
                        // check the day month and year accurately
                        try
                        {
                            Console.WriteLine("Date is correct format");
                            //if the date is incorrect will go to catch
                            dt = DateTime.Parse(date);
                            // Just testing the day Can ignore 
                            Console.WriteLine(dt);
                            Console.WriteLine(dt.DayOfWeek);
                            dError = 0;

                        }
                        catch
                        {
                            //Console.WriteLine("catch");
                            Console.WriteLine("Date Enter is incorrect");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadLine();
                            dError = 1;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Date Enter is incorrect");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        dError = 1;
                    }
                } while (dError == 1);

                do
                {
                    //Input the purpose
                    Console.Clear();
                    Console.Write("Enter the purpose(Do not enter space between the purpose): ");
                    purpose = Console.ReadLine();
                    purpose = purpose.ToUpper();

                    //check if it contains only string
                    if (!Regex.IsMatch(purpose, @"^[\p{L}]+$"))
                    {
                        Console.WriteLine("Wrong input. Please enter a valid purpose");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        Console.Clear();
                        pError = 1;
                    }
                    else
                        pError = 0;
                } while (pError == 1); // to validate name error

                int vError;

                do
                {
                    Console.Clear();
                    Console.WriteLine("Enter the venue");
                    Console.WriteLine("Block- KA or KB");
                    Console.WriteLine("Floor- 1 to 8 for ka 1 to 10 for kb");
                    Console.WriteLine("Room- 1 to 20");
                    Console.WriteLine("Example: KA220 or KB301");
                    Console.Write("Venue: ");

                    //do "subtring" validation or "index of" etc

                    venue = Console.ReadLine();
                    venue = venue.ToUpper();

                    string tempBlock;
                    int tempFloor, tempRoom;

                    try
                    {
                        tempBlock = venue.Substring(0, 2);
                        tempFloor = Convert.ToInt32(venue.Substring(2, 1));
                        tempRoom = Convert.ToInt32(venue.Substring(3, 2));

                        if (venue.Length == 5)
                        {
                            if (tempBlock == "KA" || tempBlock == "KB")
                            {

                                if (tempFloor <= 8 && tempFloor >= 1)
                                {

                                    if (tempRoom <= 20 && tempRoom >= 0)
                                    {

                                        vError = 0;
                                    }
                                    else
                                        vError = 1;
                                }
                                else
                                    vError = 1;
                            }
                            else
                                vError = 1;
                        }
                        else
                            vError = 1;
                        if (vError == 1)
                        {
                            Console.WriteLine("Wrong venue format");
                            Console.WriteLine("Press any key to enter again");
                            Console.ReadLine();
                        }
                    }
                    catch
                    {
                        vError = 1;
                        Console.WriteLine("Wrong venue format");
                        Console.WriteLine("Press any key to enter again");
                        Console.ReadLine();
                    }

                } while (vError == 1);

                do
                {
                    tError = 0;
                    try
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter between 8am and 8pm in 24 hour format");
                        Console.WriteLine("Duration cannot more than 3 hours");
                        Console.Write("Enter the starting time: ");
                        startTime = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the ending time: ");
                        endTime = Convert.ToInt32(Console.ReadLine());

                        if (startTime < 800 || endTime > 2000)
                        {
                            Console.WriteLine("Please book between 8am and 8pm");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadLine();
                            tError = 1;
                        }

                        else
                        {
                            if (endTime - startTime <= 0)
                            {
                                Console.WriteLine("End Time cannot less than Start Time");
                                Console.WriteLine("Press any key to enter again");
                                Console.ReadLine();
                                tError = 1;
                            }

                            else if (endTime - startTime >= 400)
                            {
                                Console.WriteLine("Duration cannot more than 3 hrs");
                                Console.WriteLine("Press any key to enter again");
                                Console.ReadLine();
                                tError = 1;
                            }
                            else
                            {
                                //Duplicate reservation checking
                                tError = adminClass.CheckReservation(startTime, endTime, date, venue);
                                if (tError >= 0)
                                {
                                    Reservation[] classTemp = adminClass.GetReservationList();
                                    Reservation temp;
                                    Console.WriteLine("Booked time is ");
                                    for (int i = 0; i < adminClass.GetNumberOfClass(); i++)
                                    {

                                        temp = classTemp[i];
                                        if (date == temp.Date)
                                        {
                                            Console.WriteLine(temp.StartTime + " to " + temp.EndTime);
                                        }
                                    }

                                    Console.WriteLine("The current Reservation is conflicted!!!");
                                    Console.WriteLine("Please Input Again");
                                    Console.WriteLine("Press any key to continue");
                                    Console.ReadLine(); // to pause the code
                                                        //tError = 1; // keep loop
                                }
                                //If no duplicate then store the data
                                else
                                {
                                    objReserveDetail = new Reservation(name, startTime, endTime, purpose, venue, date);
                                    //objReserveDetail = new Reservation(name, day);

                                    adminClass.RecordAClass(objReserveDetail);
                                    Console.WriteLine(adminClass.GetNumberOfClass());
                                    do
                                    {
                                        try
                                        {
                                            Console.Write("Do you want to book another class? (Y/N)>> ");
                                            proceed = Convert.ToChar(Console.ReadLine());
                                            Console.Clear();
                                            tError = 0; //jump out of while loop
                                        }
                                        catch
                                        {
                                            Console.WriteLine("reserve!!!!!");
                                            Console.WriteLine("Wrong input");
                                            Console.WriteLine("Press any key to input again");
                                            Console.ReadLine();
                                            tError = 1;
                                        }
                                    } while (tError == 1);
                                }
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Wrong input please enter again");
                        Console.ReadLine();
                        tError = 1;
                    }


                } while (tError == 1);

            } while (proceed == 'Y' || proceed == 'y');


        }

        public static void Display(Admin adminClass)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Lecturer's Name\tVenue\tDate\tStartTime\tEndTime\tPurpose");

            Reservation[] classTemp = adminClass.GetReservationList();
            Reservation temp;

            for (int i = 0; i < adminClass.GetNumberOfClass(); i++)
            {
                temp = classTemp[i];
                Console.WriteLine(temp.Name + "\t" + temp.Venue + "\t" + temp.Date + "\t" + temp.StartTime + "\t" + temp.EndTime + "\t" + temp.Purpose);
            }

        }

        public static void Cancel(Admin adminClass)
        {
            int cancelChoice = 0;
            int proceed = 0;

            Console.WriteLine(" ");
            do
            {
                try
                {
                    Console.WriteLine("No\tLecturer's Name\tVenue\t\tDate\t\t\tStartTime\tEndTime\t\tPurpose");

                    Reservation[] classTemp = adminClass.GetReservationList();
                    Reservation temp;

                    for (int i = 0; i < adminClass.GetNumberOfClass(); i++)
                    {
                        temp = classTemp[i];
                        Console.WriteLine(i + 1 + "\t" + temp.Name + "\t" + temp.Date + "\t");
                    }
                    Console.WriteLine("");
                    Console.WriteLine("Enter 0 to quit");
                    Console.WriteLine("Select the number of the reservation that you would like to cancel: ");
                    cancelChoice = Convert.ToInt32(Console.ReadLine());

                    if (cancelChoice < 0 || cancelChoice > adminClass.GetNumberOfClass())
                    {
                        Console.WriteLine("Wrong Choice");
                        Console.ReadLine();
                        proceed = 1;
                    }
                    else if (cancelChoice == 0)
                    {
                        proceed = 0;
                    }

                    else
                    {
                        proceed = 0;
                        adminClass.CancelReservation(cancelChoice);
                        Display(adminClass);
                    }
                }
                catch
                {
                    Console.WriteLine("Wrong Input Press any key to Input again");
                    Console.ReadLine();
                    proceed = 1;
                }
            } while (proceed == 1);
        }
        public static void ReadData(Admin adminClass)  // read the data from the file ONCE when the program start
        {
            string name, purpose, venue, date;
            int startTime, endTime;
            Reservation objReserveDetail;
            try
            {
                StreamReader reader = new StreamReader("Venue.txt", true);
                string line = reader.ReadLine();
                int counter = 1;

                while (line != null)
                {
                    counter++;

                    name = line;

                    line = reader.ReadLine();
                    venue = line;

                    line = reader.ReadLine();
                    date = line;


                    line = reader.ReadLine();
                    startTime = Convert.ToInt32(line);


                    line = reader.ReadLine();
                    endTime = Convert.ToInt32(line);


                    line = reader.ReadLine();
                    purpose = line;

                    line = reader.ReadLine();

                    objReserveDetail = new Reservation(name, startTime, endTime, purpose, venue, date);
                    adminClass.RecordAClass(objReserveDetail);
                }
                reader.Close();
            }
            catch (IOException exc)
            {
                Console.WriteLine("There is no file. New file will be generated" + exc.Message);
            }
        }

        public static void WriteData(Admin adminClass) //rewrite all the data or create a new file if no file is exist and save it
        {
            Reservation[] classTemp = adminClass.GetReservationList();
            Reservation temp;

            StreamWriter writer = new StreamWriter("Venue.txt");

            for (int i = 0; i < adminClass.GetNumberOfClass(); i++)
            {
                temp = classTemp[i];
                try
                {

                    writer.WriteLine(temp.Name);
                    writer.WriteLine(temp.Venue);
                    writer.WriteLine(temp.Date);
                    writer.WriteLine(temp.StartTime);
                    writer.WriteLine(temp.EndTime);
                    writer.WriteLine(temp.Purpose);

                }
                catch (IOException exc)
                {
                    Console.WriteLine("File error: " + exc.Message);
                }

            }
            writer.Close();
        }
    }


}

