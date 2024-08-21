using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;

namespace Event_Management_Application
{
    public partial class DueHandlingcs : Form
    {
        Database database;
        int bookingId;
        string cusName;
        float firstInstall;
        float dueAmo;
        float grossAmo;
        public DueHandlingcs()
        {
            InitializeComponent();
            database = Database.getInstance();
            database.conn = new SqlConnection(database.path);

            Display();
        }

        private void DueHandlingcs_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void Display()
        {
            try
            {
                string payStat = "Pending";
                database.conn.Open();
                database.cmd = new SqlCommand("select BookingID, CustomerName, FirstInstallment, DueAmount, GrossTotal from Booking where PaymentStatus = '"+ payStat + "'", database.conn);
                database.cmd.ExecuteNonQuery();
                database.conn.Close();

                SqlDataAdapter sd = new SqlDataAdapter(database.cmd);
                DataTable dt = new DataTable();
                sd.Fill(dt);
                dataGridView1.DataSource = dt;
                database.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Events events = new Events();
            events.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bookingId = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            cusName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            firstInstall = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
            dueAmo = float.Parse(dataGridView1.Rows[e.RowIndex ].Cells[3].Value.ToString());
            grossAmo = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());

            DueName.Text = cusName;
            DueInstall.Text = firstInstall.ToString();
            DueGross.Text = grossAmo.ToString();
            DuePay.Text = dueAmo.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string payStat = "Paid";
                database.conn.Open();
                database.cmd = new SqlCommand("update Booking set DueAmount = '" + 0 + "', PaymentStatus = '"+ payStat + "' where BookingID = '" + bookingId + "'", database.conn);
                database.cmd.ExecuteNonQuery();
                database.conn.Close();
                MessageBox.Show("Due Payment Sucessfull!");
                Reset();
                Display();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Reset()
        {
            DueName.Text = string.Empty;
            DueInstall.Text = string.Empty;
            DuePay.Text = string.Empty;
            DueGross.Text = string.Empty;
        }
    }
}
