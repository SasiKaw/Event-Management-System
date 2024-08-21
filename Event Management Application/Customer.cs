using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Event_Management_Application
{
    public partial class Customer : Form
    {
        Database database;

        string cusId;
        string name;
        string nic;
        string address;
        string phoneNum;
        string email;
        public Customer()
        {
            InitializeComponent();
            database = Database.getInstance();
            database.conn = new SqlConnection(database.path);

            Display();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void Customer_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = cusName.Text;
            nic = cusNIC.Text;
            address = cusAddress.Text;
            phoneNum = cusPhone.Text;
            email = cusEmail.Text;

            Regex nameReg = new Regex(@"^[a-zA-Z ]+$");
            Regex nicReg = new Regex(@"^(([1-9])([0-9]{8}[VX])|([1-9])([0-9]{11}))$");
            Regex phoneReg = new Regex(@"^[0-9]{10}$");
            Regex emailReg = new Regex(@"^[a-z0-9]+@[a-z]+.com$");

            bool nameCheck = nameReg.IsMatch(name);
            bool nicCheck = nicReg.IsMatch(nic);
            bool phoneCheck = phoneReg.IsMatch(phoneNum);
            bool emailCheck = emailReg.IsMatch(email);

            if (nameCheck)
            {
                if(nicCheck)
                {
                    if(phoneCheck)
                    {
                        if(emailCheck)
                        {
                            try
                            {
                                database.conn.Open();
                                database.cmd = new SqlCommand("insert into Customers(Name,NIC,Address,PhoneNumber,Email) values ('"+ name + "','"+ nic + "','"+ address + "','" + phoneNum + "','" + email + "')", database.conn);
                                database.cmd.ExecuteNonQuery();
                                database.conn.Close();

                                MessageBox.Show("Insert Sucessful");
                                Reset();
                                Display();

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Enter a valid Email");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Phone number must be 10 digits");
                    }
                }
                else
                {
                    MessageBox.Show("Please check the NIC Number.");
                }
            }
            else
            {
                MessageBox.Show("Name only contain letters.");
            }

        }

        private void cusName_TextChanged(object sender, EventArgs e)
        {

        }

        private void cusNIC_TextChanged(object sender, EventArgs e)
        {

        }

        private void cusAddress_TextChanged(object sender, EventArgs e)
        {

        }

        public void Display()
        {
            try
            {
                database.conn.Open();
                database.cmd = new SqlCommand("select * from Customers", database.conn);
                database.cmd.ExecuteNonQuery();
                database.conn.Close();

                SqlDataAdapter sd = new SqlDataAdapter(database.cmd);
                DataTable dt = new DataTable();
                sd.Fill(dt);
                dataGridView1.DataSource = dt;
                database.conn.Close();
            }
            catch(Exception ex) 
            {
                 MessageBox.Show(ex.Message); 
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                name = cusName.Text;
                nic = cusNIC.Text;
                address = cusAddress.Text;
                phoneNum = cusPhone.Text;
                email = cusEmail.Text;

                Regex nameReg = new Regex(@"^[a-zA-Z ]+$");
                Regex nicReg = new Regex(@"^(([1-9])([0-9]{8}[VX])|([1-9])([0-9]{11}))$");
                Regex phoneReg = new Regex(@"^[0-9]{10}$");
                Regex emailReg = new Regex(@"^[a-z0-9]+@[a-z]+.com$");

                bool nameCheck = nameReg.IsMatch(name);
                bool nicCheck = nicReg.IsMatch(nic);
                bool phoneCheck = phoneReg.IsMatch(phoneNum);
                bool emailCheck = emailReg.IsMatch(email);

                if (nameCheck)
                {
                    if (nicCheck)
                    {
                        if (phoneCheck)
                        {
                            if (emailCheck)
                            {
                                try
                                {
                                    database.conn.Open();
                                    database.cmd = new SqlCommand("update Customers set Name = '" +name+ "', NIC = '" +nic+ "', Address = '"+address+"', PhoneNumber = '"+phoneNum+"', Email = '"+ email +"' where cusId = '"+cusId+"'", database.conn);
                                    database.cmd.ExecuteNonQuery();
                                    database.conn.Close();

                                    MessageBox.Show("Update Sucessful !");
                                    Display();

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Enter a valid Email");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Phone number must be 10 digits");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please check the NIC Number.");
                    }
                }
                else
                {
                    MessageBox.Show("Name only contain letters.");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            cusId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            cusName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            cusNIC.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            cusAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            cusPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            cusEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            cusName.Text = string.Empty;
            cusNIC.Text = string.Empty;
            cusAddress.Text = string.Empty;
            cusPhone.Text = string.Empty;
            cusEmail.Text = string.Empty;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                database.conn.Open();
                database.cmd = new SqlCommand("delete from Customers where cusId = '"+ cusId +"'",database.conn);
                database.cmd.ExecuteNonQuery();
                database.conn.Close();
                Reset();
                Display();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
