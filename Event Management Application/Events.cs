using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Event_Management_Application
{
    public partial class Events : Form
    {
        Database database;
        string bookId;
        public Events()
        {
            InitializeComponent();
            database = Database.getInstance();
            database.conn = new SqlConnection(database.path);

            Display();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                database.conn.Open();
                database.cmd = new SqlCommand("delete from Booking where BookingID = '" + bookId + "'", database.conn);
                database.cmd.ExecuteNonQuery();
                database.conn.Close();
                Display();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void Events_Load(object sender, EventArgs e)
        {

        }

        public void Display()
        {
            try
            {
                database.conn.Open();
                database.cmd = new SqlCommand("select * from Booking", database.conn);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bookId = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            saveToaFile(dataGridView1.Rows[e.RowIndex]);
        }

        public void saveToaFile(DataGridViewRow selectedRow)
        {
            string filePath = @"C:\Users\Avishka Udara\OneDrive - Lanka Nippon Biztech Institute\Desktop\LNBTI\LNBTI 2nd Year\2nd 1st Sem Lnbti\Visual Application Programming\Group Project\Customer_Info.txt";

            using (StreamWriter sw = new StreamWriter(filePath)) 
            {
                sw.WriteLine("Booking Information");

                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    sw.WriteLine($"{cell.OwningColumn.HeaderText} : {cell.Value}");
                }

                sw.WriteLine("-------------------------------------------------------------------------------------------");
            }

            MessageBox.Show("Booking information saved to text file.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            DueHandlingcs duehandling = new DueHandlingcs();
            duehandling.Show();
        }
    }
}
