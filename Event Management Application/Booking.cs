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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Event_Management_Application
{
    public partial class Booking : Form
    {
        Database database;
        private const decimal DJCost = 10000m; 
        private const decimal DecorationCost = 15000m;
        private const decimal WeddingCarCost = 20000m;
        private const decimal PoruwaCeremonyCost = 5000m;
        private const decimal CakeCost = 8000m;

        public Booking()
        {
            InitializeComponent();
            database = Database.getInstance();
            database.conn = new SqlConnection(database.path);
            getCustomerID();
            installment.Enabled = false;
            diveAmo.Enabled = false;

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex count = new Regex(@"^[0-9]+$");
            bool personCountCheck = count.IsMatch(textBox2.Text);
            bool plateCost = count.IsMatch(textBox18.Text);

            if(comboBox1.SelectedItem != null)
            {
                if(comboBox2.SelectedItem != null)
                {
                    if (personCountCheck)
                    {
                        if(radioButton1.Checked != false || radioButton2.Checked != false)
                        {
                            if (plateCost)
                            {
                                if(paymentOpt.SelectedItem != null)
                                {
                                    try
                                    {

                                        string customerName = textBox1.Text;
                                        string eventName = comboBox2.SelectedItem.ToString();
                                        DateTime eventDate = dateTimePicker1.Value.Date;
                                        int numberOfPersons = int.Parse(textBox2.Text);
                                        string dishes = textBox3.Text;
                                        decimal platePrice = decimal.Parse(textBox18.Text);
                                        string beverages = string.Join(", ", new string[]
                                        {
                                        checkBox1.Checked ? "Soda" : null,
                                        checkBox6.Checked ? "Wine" : null,
                                        checkBox5.Checked ? "Beer" : null,
                                        checkBox4.Checked ? "Coca Cola" : null,
                                        checkBox3.Checked ? "Juice" : null,

                                        }.Where(x => x != null));

                                        string additionalEvents = string.Join(", ", new string[]
                                        {
                                        checkBox2.Checked ? "DJ" : null,
                                        checkBox7.Checked ? "Decoration" : null,
                                        checkBox8.Checked ? "Wedding Car" : null,
                                        checkBox9.Checked ? "Poruwa Ceremony" : null,
                                        checkBox10.Checked ? "Cake" : null,
                                            // Add more additional events as needed
                                        }.Where(x => x != null));

                                        string paymentSatuts = null;
                                        string paymentOption = paymentOpt.SelectedItem.ToString();
                                        decimal firstInstallment = string.IsNullOrEmpty(installment.Text)? 0 : decimal.Parse(installment.Text);
                                        decimal dueAmount = string.IsNullOrEmpty(diveAmo.Text)? 0 :  decimal.Parse(diveAmo.Text);
                                        decimal netTotal = decimal.Parse(textBox5.Text);
                                        decimal discount = string.IsNullOrEmpty(textBox3.Text) ? 0 : decimal.Parse(textBox3.Text);
                                        decimal grossTotal = decimal.Parse(textBox6.Text);
                                        int customerId = int.Parse(comboBox1.SelectedItem.ToString());

                                        if (paymentOption == "Installment Wise")
                                        {
                                            paymentSatuts = "Pending";
                                            
                                        }
                                        else if (paymentOption == "Direct Payments")
                                        {
                                            paymentSatuts = "Paid";
                                        }

                                        // SQL query to insert the data into the Booking table
                                        string query = @"
                                            INSERT INTO Booking (
                                                CustomerName, 
                                                Event, 
                                                EventDate, 
                                                NoOfPersons, 
                                                Dishes, 
                                                PlatePrice, 
                                                Beverages, 
                                                AdditionalEvents, 
                                                PaymentOption,
                                                PaymentStatus, 
                                                FirstInstallment, 
                                                DueAmount, 
                                                NetTotal, 
                                                Discount, 
                                                GrossTotal, 
                                                CustomerID
                                            ) 
                                            VALUES (
                                                @CustomerName, 
                                                @Event, 
                                                @EventDate, 
                                                @NoOfPersons, 
                                                @Dishes, 
                                                @PlatePrice, 
                                                @Beverages, 
                                                @AdditionalEvents, 
                                                @PaymentOption,
                                                @PaymentStatus, 
                                                @FirstInstallment, 
                                                @DueAmount, 
                                                @NetTotal, 
                                                @Discount, 
                                                @GrossTotal, 
                                                @CustomerID
                                            )";

                                        using (SqlConnection conn = new SqlConnection(database.path))
                                        {
                                            using (SqlCommand cmd = new SqlCommand(query, conn))
                                            {
                                                // Add parameters to prevent SQL injection
                                                cmd.Parameters.AddWithValue("@CustomerName", customerName);
                                                cmd.Parameters.AddWithValue("@Event", eventName);
                                                cmd.Parameters.AddWithValue("@EventDate", eventDate);
                                                cmd.Parameters.AddWithValue("@NoOfPersons", numberOfPersons);
                                                cmd.Parameters.AddWithValue("@Dishes", dishes);
                                                cmd.Parameters.AddWithValue("@PlatePrice", platePrice);
                                                cmd.Parameters.AddWithValue("@Beverages", beverages);
                                                cmd.Parameters.AddWithValue("@AdditionalEvents", additionalEvents);
                                                cmd.Parameters.AddWithValue("@PaymentOption", paymentOption);
                                                cmd.Parameters.AddWithValue("@PaymentStatus", paymentSatuts);
                                                cmd.Parameters.AddWithValue("@FirstInstallment", firstInstallment);
                                                cmd.Parameters.AddWithValue("@DueAmount", dueAmount);
                                                cmd.Parameters.AddWithValue("@NetTotal", netTotal);
                                                cmd.Parameters.AddWithValue("@Discount", discount);
                                                cmd.Parameters.AddWithValue("@GrossTotal", grossTotal);
                                                cmd.Parameters.AddWithValue("@CustomerID", customerId);


                                                conn.Open();
                                                cmd.ExecuteNonQuery();
                                                conn.Close();

                                                MessageBox.Show("Booking information added successfully.");
                                                Reset();
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("An error occurred while adding the booking: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Enter a payment option");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Enter a valid plate price");
                            }
                        }
                        else
                        {

                            MessageBox.Show("Please select one of the menue");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Person Count must be a valid amount");
                    }
                }
                else
                {
                    MessageBox.Show("Select a event");
                }
            }
            else
            {
                MessageBox.Show("Select a customer ID");
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reset();
        }
        public void Reset()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            installment.Text = string.Empty;
            textBox5.Text = string.Empty;
            textBox6.Text = string.Empty;
            textBox7.Text = string.Empty;
            textBox8.Text = string.Empty;
            textBox9.Text = string.Empty;
            textBox10.Text = string.Empty;
            textBox11.Text = string.Empty;
            textBox12.Text = string.Empty;
            textBox13.Text = string.Empty;
            textBox14.Text = string.Empty;
            textBox15.Text = string.Empty;
           
            diveAmo.Text = string.Empty;
            textBox18.Text = string.Empty;

            
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;

            radioButton1.Checked = false;
            radioButton2.Checked = false;




            comboBox1.Items.Clear(); 
            getCustomerID();

            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }

            if (paymentOpt.Items.Count > 0)
            {
                paymentOpt.SelectedIndex = 0;
            }

            
            dateTimePicker1.Value = DateTime.Today;
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cusId = int.Parse(comboBox1.SelectedItem.ToString());
            
            try
            {
                database.conn.Open();
                database.cmd = new SqlCommand(@"select Name from Customers where cusId = '"+cusId+"'", database.conn);

                SqlDataReader reader = database.cmd.ExecuteReader();

                textBox1.Text = string.Empty;

                if (reader.Read())
                {
                    textBox1.Text = reader["Name"].ToString();
                }
                
                reader.Close();
                database.conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter_1(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void Booking_Load(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(paymentOpt.Text == "Direct Payments")
            {
                installment.Enabled = false;
                diveAmo.Enabled = false;
            }
            else 
            {
                installment.Enabled = true;
                diveAmo.Enabled = false;
            }
        }

        public void getCustomerID()
        {
            try
            {
                database.conn.Open();
                database.cmd = new SqlCommand(@"select cusId from Customers", database.conn);
                
                SqlDataReader reader = database.cmd.ExecuteReader();
                comboBox1.Items.Clear();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["cusId"].ToString());
                }

                reader.Close();
                database.conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedItem.ToString() == "Party")
            {
                splitContainer1.Panel2.Hide();
            }
            else
            {
                splitContainer1.Panel2.Show();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateSelected = dateTimePicker1.Value.Date;
            string formatDate =  dateSelected.ToString("yyyy-MM-dd");

            if (IsDatebooked(formatDate))
            {
                MessageBox.Show("This date is already booked");
            }

           

        }

        public bool IsDatebooked(string date)
        {
            int count = 0;
            try
            {
                database.conn.Open();
                database.cmd = new SqlCommand(@"select count(*) from Booking where CAST(EventDate AS DATE) = '" + date + "'", database.conn);

                count = (int)database.cmd.ExecuteScalar();

                database.conn.Close();
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            CalculateTotal();

        }
        private void CalculateTotal()
        {
            Regex amount = new Regex(@"^[0-9]+$");
            decimal platePrice = 0.0m;
            int numberOfPersons = 0;
            decimal foodTotal = 0.0m;

            try
            {
                if(amount.IsMatch(textBox2.Text))
                {
                    if(amount.IsMatch(textBox18.Text))
                    {
                        platePrice = decimal.Parse(textBox18.Text);
                        numberOfPersons = int.Parse(textBox2.Text);
                        foodTotal = platePrice * numberOfPersons;

                    }
                    else
                    {
                        MessageBox.Show("Plate Price must be a valid amount");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Number of Persons must be valid");
                }

                
                

                foodTotal = platePrice * numberOfPersons;

                decimal beverageTotal = 0;
                if (checkBox1.Checked)
                {   
                    bool checkSodaqty = amount.IsMatch(textBox7.Text);
                    bool checkSodaPrice = amount.IsMatch(textBox8.Text);

                    if (checkSodaqty && checkSodaPrice) 
                    {
                        int qtySoda = string.IsNullOrWhiteSpace(textBox7.Text) ? 0 : int.Parse(textBox7.Text);
                        decimal sodaPricePerBottle = string.IsNullOrWhiteSpace(textBox8.Text) ? 0 : decimal.Parse(textBox8.Text);
                        beverageTotal += qtySoda * sodaPricePerBottle;
                    }
                    else
                    {
                        MessageBox.Show("Quantity or Price must be valid amount");
                    }
                    
                }
         
                if (checkBox6.Checked)
                {
                    bool checkWineqty = amount.IsMatch(textBox10.Text);
                    bool checkWinePrice = amount.IsMatch(textBox9.Text);

                    if(checkWineqty && checkWinePrice)
                    {
                        int qtyWine = string.IsNullOrWhiteSpace(textBox10.Text) ? 0 : int.Parse(textBox10.Text);
                        decimal winePricePerBottle = string.IsNullOrWhiteSpace(textBox9.Text) ? 0 : decimal.Parse(textBox9.Text);
                        beverageTotal += qtyWine * winePricePerBottle;
                    }
                    else
                    {
                        MessageBox.Show("Quantity or Price must be valid amount");
                    }

                }
                if (checkBox5.Checked)
                {
                    bool checkBeerqty = amount.IsMatch(textBox10.Text);
                    bool checkBeerPrice = amount.IsMatch(textBox9.Text);

                    if( checkBeerqty && checkBeerPrice)
                    {
                        int qtyBeer = string.IsNullOrWhiteSpace(textBox12.Text) ? 0 : int.Parse(textBox12.Text);
                        decimal beerPricePerBottle = string.IsNullOrWhiteSpace(textBox11.Text) ? 0 : decimal.Parse(textBox11.Text);
                        beverageTotal += qtyBeer * beerPricePerBottle;
                    }
                    else
                    {
                        MessageBox.Show("Quantity or Price must be valid amount");
                    }

                    
                }
                if (checkBox6.Checked)
                {
                    bool checkCocaColaqty = amount.IsMatch(textBox14.Text);
                    bool checkCocaColaPrice = amount.IsMatch(textBox13.Text);

                    if(checkCocaColaqty && checkCocaColaPrice)
                    {
                        int qtyCocaCola = string.IsNullOrWhiteSpace(textBox14.Text) ? 0 : int.Parse(textBox14.Text);
                        decimal cocaColaPricePerBottle = decimal.Parse(textBox13.Text);
                        beverageTotal += qtyCocaCola * cocaColaPricePerBottle;
                    }
                    else
                    {
                        MessageBox.Show("Quantity or Price must be valid amount");
                    }


                }
                if (checkBox3.Checked)
                {
                    bool checkJuiceqty = amount.IsMatch(textBox16.Text);
                    bool checkJuicePrice = amount.IsMatch(textBox15.Text);

                    if(checkJuiceqty && checkJuicePrice)
                    {
                        int qtyJuice = string.IsNullOrWhiteSpace(textBox16.Text) ? 0 : int.Parse(textBox16.Text);
                        decimal juicePricePerBottle = string.IsNullOrWhiteSpace(textBox15.Text) ? 0 : decimal.Parse(textBox15.Text);
                        beverageTotal += qtyJuice * juicePricePerBottle;
                    }
                    else
                    {
                        MessageBox.Show("Quantity or Price must be valid amount");
                    }
                    
                }

                decimal serviceTotal = 0;
                if (checkBox2.Checked)
                {
                    serviceTotal += 10000m;  
                }
                if (checkBox4.Checked)
                {
                    serviceTotal += 15000m;  
                }
                if (checkBox8.Checked)
                {
                    serviceTotal += 20000m;  
                }
                if (checkBox9.Checked)
                {
                    serviceTotal += 5000m;  
                }
                if (checkBox10.Checked)
                {
                    serviceTotal += 8000m;  
                }

                decimal netTotal = foodTotal + beverageTotal + serviceTotal;

                decimal discount = 0;
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    decimal discountPercentage = decimal.Parse(textBox3.Text);
                    discount = (discountPercentage / 100) * netTotal;
                }


                decimal grossTotal = netTotal - discount ;

                textBox5.Text = netTotal.ToString("F2");  
                textBox6.Text = grossTotal.ToString("F2");


                if (decimal.TryParse(installment.Text, out decimal firstInstallment))
                {
                    decimal dueAmount = grossTotal - firstInstallment;
                    diveAmo.Text = dueAmount.ToString("F2");
                }
                
            }


            
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numbers for prices, quantities, and discount.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox7.Enabled = true;
                textBox8.Enabled = true;
            }
            else
            {
                textBox7.Enabled = false;
                textBox8.Enabled = false;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                textBox10.Enabled = true;
                textBox9.Enabled = true;
            }
            else
            {
                textBox10.Enabled = false;
                textBox9.Enabled = false;
            }

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                textBox12.Enabled = true;
                textBox11.Enabled = true;
            }
            else
            {
                textBox12.Enabled = false;
                textBox11.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                textBox14.Enabled = true;
                textBox13.Enabled = true;
            }
            else
            {
                textBox14.Enabled = false;
                textBox13.Enabled = false;
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox16.Enabled = true;
                textBox15.Enabled = true;
            }
            else
            {
                textBox16.Enabled = false;
                textBox15.Enabled = false;
            }
        }
    }
}
