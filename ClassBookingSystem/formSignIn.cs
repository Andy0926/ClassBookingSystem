using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ClassBookingSystem
{
    public partial class FormSignIn : Form
    {
        Admin obAdmin = new Admin(" ", " ");
        public FormSignIn()
        {
            
            InitializeComponent();
            obAdmin = ReadAdminDetail(obAdmin);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbxUser.Text == obAdmin.AdminId)
                {
                    if (tbxPass.Text == obAdmin.AdminPass)
                    {
                        this.Hide();
                        cbsForm formCbs = new cbsForm();
                        
                        formCbs.ShowDialog();
                        Environment.Exit(0);
                    }
                    else
                    {
                        MessageBox.Show("Wrong Id or Password");
                    }

                }
                else
                {
                    MessageBox.Show("Wrong Id or Password");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error code: " + ex.Message);
            }

        }

        private void FormSignIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        public static Admin ReadAdminDetail(Admin admin)
        {
            try
            {

                string id, pass;
                StreamReader reader = new StreamReader("Admin.txt", true);
                string line = reader.ReadLine();
                id = line;
                line = reader.ReadLine();
                pass = line;

                admin = new Admin(id, pass);
                reader.Close();
                return admin;
            }

            catch (IOException exc)
            {
                Console.WriteLine("There is no file. New file will be generated" + exc.Message);
                return null;
            }

        }
    }
}
