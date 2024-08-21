using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Event_Management_Application
{
    public partial class Login : Form
    {
        Database database;

        public Login()
        {
            InitializeComponent();
            database = Database.getInstance();
            database.conn = new SqlConnection(database.path);

            // Set the PasswordChar property to mask the password initially
            password.PasswordChar = '*';

            // Subscribe to the CheckedChanged event
            showPass.CheckedChanged += showPass_CheckedChanged;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            splitContainer1.Panel1MinSize = 60;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = userName.Text;
            string pass = password.Text;
            string uRole = role.Text; 

            try
            {
                database.conn.Open();
                database.cmd = new SqlCommand("select userName,password from Users where Role='"+uRole+"'", database.conn);
                database.cmd.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(database.cmd);
                DataTable dt = new DataTable(); 
                adapter.Fill(dt);
                database.conn.Close();

                if (username == dt.Rows[0]["userName"].ToString())
                {
                    if (pass == dt.Rows[0]["password"].ToString())
                    {
                        MessageBox.Show("Login Successful!");

                        this.Hide();
                        Dashboard d = new Dashboard();
                        d.Show();
                    }

                    else 
                    {
                        MessageBox.Show("Incorrect Password");
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect Username");
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void showPass_CheckedChanged(object sender, EventArgs e)
        {
            if (showPass.Checked)
            {
                password.PasswordChar = '\0';
            }
            else
            {
                password.PasswordChar = '*';
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
