using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace ClassBookingSystem
{
    public partial class cbsForm : Form
    {
        Time obTime;
        Admin adminClass = new Admin(" "," ");
        public cbsForm()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            dateTimePicker1.MinDate = DateTime.Now.Date; //cannot pick the date which is less that today's date
            groupBoxTime.Enabled = false;
            this.comboBoxStartTime.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxEndTime.DropDownStyle = ComboBoxStyle.DropDownList;

            string name, purpose, venue, date;
            int startTime, endTime;
            Time objReserveDetail;
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

                    objReserveDetail = new Time(startTime, endTime, name, purpose, date, venue);
                    adminClass.RecordAClass(objReserveDetail);
                }
                reader.Close();
                ///////////////////////////////////////
                ///
                displayAll();
                available();
            }
            catch (IOException exc)
            {
                Console.WriteLine("There is no file. New file will be generated" + exc.Message);
            }

        }
        private void cbsForm_Load(object sender, EventArgs e)
        {
           
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can only book from 8am to 8pm\nMinimum 1 hr and Maximum 3hr");
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            int eName, ePurpose, eVenue;
            //check if it contains only string
            if (!Regex.IsMatch(tbxName.Text, @"^[\p{L}]+$"))
            {
                eName = 1;
            }
            else
            {
                eName = 0;               
            }
            string venue;
            venue = tbxVenue.Text.ToUpper();
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
                                eVenue = 0;
                            }
                            else
                                eVenue = 1;
                        }
                        else
                            eVenue = 1;
                    }
                    else
                        eVenue = 1;
                }
                else
                    eVenue = 1;

                if (!Regex.IsMatch(tbxPurpose.Text, @"^[\p{L}]+$"))
                {
                    ePurpose = 1;
                }
                else
                {
                    ePurpose = 0;
                }

                if (eName == 1 || eVenue == 1 || ePurpose == 1)
                {
                    string errorMessage = "";
                    if (eName == 1)
                        errorMessage += "Invalid Name Format(Do not space)\n";
                    else if (eVenue == 1)
                        errorMessage += "Invalid Venue Format\n";
                    else if (ePurpose == 1)
                        errorMessage += "Invalid Purpose Format(Do not space)\n";
                    MessageBox.Show(errorMessage);
                }
                else
                {
                    groupBoxTime.Enabled = true;
                    groupBoxDetails.Enabled = false;

                    Time[] classTemp = adminClass.GetReservationList();
                    Time temp;

                    for (int i = 0; i < adminClass.GetNumberOfClass(); i++)
                    {
                        temp = classTemp[i];
                        if (dateTimePicker1.Text == temp.Date)
                        {
                            int hour = temp.EndTime - temp.StartTime;
                            chgStatus(temp.StartTime, hour);
                            
                        }
                    }
                }
            }

            catch
            {
                MessageBox.Show("exception.....");
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Text;
            string venue = tbxVenue.Text;
            string purpose = tbxPurpose.Text;
            string name = tbxName.Text;
            int tError;
            int startTime = Convert.ToInt32(comboBoxStartTime.Text);
            int endTime = Convert.ToInt32(comboBoxEndTime.Text);

            if (endTime - startTime <= 0)
            {
                MessageBox.Show("End Time cannot less than Start Time");
            }
            else if (endTime - startTime >= 400)
            {
                MessageBox.Show("Duration cannot more than 3 hours");
            }
            else
            {

                tError = adminClass.CheckReservation(startTime, endTime, date, venue);

                if (tError >= 0)
                {
                    MessageBox.Show("The current reservation is conflict. Please enter another time");
                }
                else
                {
                    obTime = new Time(startTime, endTime, name, purpose, date, venue);
                    adminClass.RecordAClass(obTime);
                    MessageBox.Show("Book Success");
                    groupBoxTime.Enabled = false;
                    groupBoxDetails.Enabled = true;
                    displayAll();
                    available();
                }
            }
        }

        private void cbsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to quit?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Time[] classTemp = adminClass.GetReservationList();
                Time temp;

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
                Application.Exit();
                
            }
            else if(dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }

        }

        private void btnProceedCancel_Click(object sender, EventArgs e)
        {
            try
            {
                int cancelChoice = Convert.ToInt32(tbxNo.Text);

                if (cancelChoice <= 0 || cancelChoice > adminClass.GetNumberOfClass())
                {
                    MessageBox.Show("Wrong Choice");
                }

                else
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel?", "Warning", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Time tempCancelStatus = adminClass.CancelReservation(cancelChoice);
                        displayAll();
                        int hour = tempCancelStatus.EndTime - tempCancelStatus.StartTime;

                        chgStatusAvailable(tempCancelStatus.StartTime, hour);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do nothing
                    }
                }
            }
            catch
            {
                MessageBox.Show("Wrong Input");
            }
        }

        public void displayAll()
        {
            int i = 1;
            listViewReservation.Items.Clear();
            var displayReservation = adminClass.GetReservationList();
            foreach (var display in displayReservation)
            {

                var row = new string[] { Convert.ToString(i++), display.Name, display.Date, display.Purpose, display.Venue,
                    Convert.ToString(display.StartTime),Convert.ToString(display.EndTime) };
                var lvr = new ListViewItem(row);

                lvr.Tag = display;
                listViewReservation.Items.Add(lvr);
            }
        }

        public void chgStatus(int startTime,int hr)
        {
            if(startTime == 800)
            {
                if (hr == 100)
                {
                    pbx8to9.Image = Properties.Resources.red;
                }
                else if(hr == 200)
                {
                    pbx8to9.Image = Properties.Resources.red;
                    pbx9to10.Image = Properties.Resources.red;
                }
                else
                {
                    pbx8to9.Image = Properties.Resources.red;
                    pbx9to10.Image = Properties.Resources.red;
                    pbx10to11.Image = Properties.Resources.red;
                }
            }
            if (startTime == 900)
            {
                if (hr == 100)
                {
                    pbx9to10.Image = Properties.Resources.red;
                }
                else if (hr == 200)
                {
                    pbx9to10.Image = Properties.Resources.red;
                    pbx10to11.Image = Properties.Resources.red;
                }
                else
                {
                    pbx9to10.Image = Properties.Resources.red;
                    pbx10to11.Image = Properties.Resources.red;
                    pbx11to12.Image = Properties.Resources.red;

                }
            }
            if (startTime == 1000)
            {
                if (hr == 100)
                {
                    pbx10to11.Image = Properties.Resources.red;
                }
                else if (hr == 200)
                {
                    pbx10to11.Image = Properties.Resources.red;
                    pbx11to12.Image = Properties.Resources.red;
                }
                else
                {
                    pbx10to11.Image = Properties.Resources.red;
                    pbx11to12.Image = Properties.Resources.red;
                    pbx12to1.Image = Properties.Resources.red;
                }
            }
            if (startTime == 1100)
            {
                if (hr == 100)
                {
                    pbx11to12.Image = Properties.Resources.red;
                }
                else if (hr == 200)
                {
                    pbx11to12.Image = Properties.Resources.red;
                    pbx12to1.Image = Properties.Resources.red;
                }
                else
                {
                    pbx11to12.Image = Properties.Resources.red;
                    pbx12to1.Image = Properties.Resources.red;
                    pbx1to2.Image = Properties.Resources.red;
                }
            }
            if (startTime == 1200)
            {
                if (hr == 100)
                {
                    pbx12to1.Image = Properties.Resources.red;
                }
                else if (hr == 200)
                {
                    pbx12to1.Image = Properties.Resources.red;
                    pbx1to2.Image = Properties.Resources.red;
                }
                else
                {
                    pbx12to1.Image = Properties.Resources.red;
                    pbx1to2.Image = Properties.Resources.red;
                    pbx2to3.Image = Properties.Resources.red;
                }
            }
            if (startTime == 1300)
            {
                if (hr == 100)
                {
                    pbx1to2.Image = Properties.Resources.red;
                }
                else if (hr == 200)
                {
                    pbx1to2.Image = Properties.Resources.red;
                    pbx2to3.Image = Properties.Resources.red;
                }
                else
                {
                    pbx1to2.Image = Properties.Resources.red;
                    pbx2to3.Image = Properties.Resources.red;
                    pbx3to4.Image = Properties.Resources.red;
                }
            }
            if (startTime == 1400)
            {
                if (hr == 100)
                {
                    pbx2to3.Image = Properties.Resources.red;
                }
                else if (hr == 200)
                {
                    pbx2to3.Image = Properties.Resources.red;
                    pbx3to4.Image = Properties.Resources.red;
                }
                else
                {
                    pbx2to3.Image = Properties.Resources.red;
                    pbx3to4.Image = Properties.Resources.red;
                    pbx4to5.Image = Properties.Resources.red;
                }
            }
            if (startTime == 1500)
            {
                if (hr == 100)
                {
                    pbx3to4.Image = Properties.Resources.red;
                }
                else if (hr == 200)
                {
                    pbx3to4.Image = Properties.Resources.red;
                    pbx4to5.Image = Properties.Resources.red;
                }
                else
                {
                    pbx3to4.Image = Properties.Resources.red;
                    pbx4to5.Image = Properties.Resources.red;
                    pbx5to6.Image = Properties.Resources.red;
                }

            }
            if (startTime == 1600)
            {
                if (hr == 100)
                {
                    pbx4to5.Image = Properties.Resources.red;
                }
                else if (hr == 200)
                {
                    pbx4to5.Image = Properties.Resources.red;
                    pbx5to6.Image = Properties.Resources.red;
                }
                else
                {
                    pbx4to5.Image = Properties.Resources.red;
                    pbx5to6.Image = Properties.Resources.red;
                    pbx6to7.Image = Properties.Resources.red;
                }

            }
            if (startTime == 1700)
            {
                if (hr == 100)
                {
                    pbx5to6.Image = Properties.Resources.red;
                }
                else if (hr == 200)
                {
                    pbx5to6.Image = Properties.Resources.red;
                    pbx6to7.Image = Properties.Resources.red;
                }
                else
                {
                    pbx5to6.Image = Properties.Resources.red;
                    pbx6to7.Image = Properties.Resources.red;
                    pbx7to8.Image = Properties.Resources.red;
                }

            }
            if (startTime == 1800)
            {
                if (hr == 100)
                {
                    pbx6to7.Image = Properties.Resources.red;
                }
                else
                {
                    pbx6to7.Image = Properties.Resources.red;
                    pbx7to8.Image = Properties.Resources.red;
                }

            }
            if (startTime == 1900)
            {

                pbx7to8.Image = Properties.Resources.red;
            }

        }

        public void chgStatusAvailable(int startTime, int hr)
        {
            if (startTime == 800)
            {
                if (hr == 100)
                {
                    pbx8to9.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx8to9.Image = Properties.Resources.green;
                    pbx9to10.Image = Properties.Resources.green;
                }
                else
                {
                    pbx8to9.Image = Properties.Resources.green;
                    pbx9to10.Image = Properties.Resources.green;
                    pbx10to11.Image = Properties.Resources.green;
                }
            }
            if (startTime == 900)
            {
                if (hr == 100)
                {
                    pbx9to10.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx9to10.Image = Properties.Resources.green;
                    pbx10to11.Image = Properties.Resources.green;
                }
                else
                {
                    pbx9to10.Image = Properties.Resources.green;
                    pbx10to11.Image = Properties.Resources.green;
                    pbx11to12.Image = Properties.Resources.green;

                }
            }
            if (startTime == 1000)
            {
                if (hr == 100)
                {
                    pbx10to11.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx10to11.Image = Properties.Resources.green;
                    pbx11to12.Image = Properties.Resources.green;
                }
                else
                {
                    pbx10to11.Image = Properties.Resources.green;
                    pbx11to12.Image = Properties.Resources.green;
                    pbx12to1.Image = Properties.Resources.green;
                }
            }
            if (startTime == 1100)
            {
                if (hr == 100)
                {
                    pbx11to12.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx11to12.Image = Properties.Resources.green;
                    pbx12to1.Image = Properties.Resources.green;
                }
                else
                {
                    pbx11to12.Image = Properties.Resources.green;
                    pbx12to1.Image = Properties.Resources.green;
                    pbx1to2.Image = Properties.Resources.green;
                }
            }
            if (startTime == 1200)
            {
                if (hr == 100)
                {
                    pbx12to1.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx12to1.Image = Properties.Resources.green;
                    pbx1to2.Image = Properties.Resources.green;
                }
                else
                {
                    pbx12to1.Image = Properties.Resources.green;
                    pbx1to2.Image = Properties.Resources.green;
                    pbx2to3.Image = Properties.Resources.green;
                }
            }
            if (startTime == 1300)
            {
                if (hr == 100)
                {
                    pbx1to2.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx1to2.Image = Properties.Resources.green;
                    pbx2to3.Image = Properties.Resources.green;
                }
                else
                {
                    pbx1to2.Image = Properties.Resources.green;
                    pbx2to3.Image = Properties.Resources.green;
                    pbx3to4.Image = Properties.Resources.green;
                }
            }
            if (startTime == 1400)
            {
                if (hr == 100)
                {
                    pbx2to3.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx2to3.Image = Properties.Resources.green;
                    pbx3to4.Image = Properties.Resources.green;
                }
                else
                {
                    pbx2to3.Image = Properties.Resources.green;
                    pbx3to4.Image = Properties.Resources.green;
                    pbx4to5.Image = Properties.Resources.green;
                }
            }
            if (startTime == 1500)
            {
                if (hr == 100)
                {
                    pbx3to4.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx3to4.Image = Properties.Resources.green;
                    pbx4to5.Image = Properties.Resources.green;
                }
                else
                {
                    pbx3to4.Image = Properties.Resources.green;
                    pbx4to5.Image = Properties.Resources.green;
                    pbx5to6.Image = Properties.Resources.green;
                }

            }
            if (startTime == 1600)
            {
                if (hr == 100)
                {
                    pbx4to5.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx4to5.Image = Properties.Resources.green;
                    pbx5to6.Image = Properties.Resources.green;
                }
                else
                {
                    pbx4to5.Image = Properties.Resources.green;
                    pbx5to6.Image = Properties.Resources.green;
                    pbx6to7.Image = Properties.Resources.green;
                }

            }
            if (startTime == 1700)
            {
                if (hr == 100)
                {
                    pbx5to6.Image = Properties.Resources.green;
                }
                else if (hr == 200)
                {
                    pbx5to6.Image = Properties.Resources.green;
                    pbx6to7.Image = Properties.Resources.green;
                }
                else
                {
                    pbx5to6.Image = Properties.Resources.green;
                    pbx6to7.Image = Properties.Resources.green;
                    pbx7to8.Image = Properties.Resources.green;
                }

            }
            if (startTime == 1800)
            {
                if (hr == 100)
                {
                    pbx6to7.Image = Properties.Resources.green;
                }
                else
                {
                    pbx6to7.Image = Properties.Resources.green;
                    pbx7to8.Image = Properties.Resources.green;
                }

            }
            if (startTime == 1900)
            {

                pbx7to8.Image = Properties.Resources.green;
            }

        }
        public void available()
        {
            
            pbx8to9.Image = Properties.Resources.green;
            pbx9to10.Image = Properties.Resources.green;
            pbx10to11.Image = Properties.Resources.green;
            pbx11to12.Image = Properties.Resources.green;
            pbx12to1.Image = Properties.Resources.green;
            pbx1to2.Image = Properties.Resources.green;
            pbx2to3.Image = Properties.Resources.green;
            pbx3to4.Image = Properties.Resources.green;
            pbx4to5.Image = Properties.Resources.green;
            pbx5to6.Image = Properties.Resources.green;
            pbx6to7.Image = Properties.Resources.green;
            pbx7to8.Image = Properties.Resources.green;

        }
    }
}
