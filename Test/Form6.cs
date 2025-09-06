using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form6 : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vnala\source\repos\Test\Test\Database1.mdf;Integrated Security=True";

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT WorkerID, Name FROM Worker";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Name";     // shows employee name
                comboBox1.ValueMember = "WorkerID";   // stores worker ID
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please select an employee!");
                return;
            }

            int workerID = Convert.ToInt32(comboBox1.SelectedValue);
            double overtimeHours = 0;
            int daysWorked = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Get overtime hours
                string query = "SELECT SUM(OvertimeHours) FROM Attendance WHERE WorkerID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", workerID);

                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                if (result != DBNull.Value)
                    overtimeHours = Convert.ToDouble(result);
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Count distinct days worked
                string query = "SELECT COUNT(DISTINCT Date) FROM Attendance WHERE WorkerID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", workerID);

                con.Open();
                daysWorked = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }

            // Payment rules
            double basePay = daysWorked * 2000;       // Rs.2000 per day
            double overtimePay = overtimeHours * 500; // Rs.500 per OT hour
            double totalPay = basePay + overtimePay;

            // Show result
            label4.Text =    $"Days Worked: {daysWorked}\n" +
                             $"Overtime: {overtimeHours} hrs\n\n" +
                             $"Base Pay: Rs {basePay}\n" +
                             $"Overtime Pay: Rs {overtimePay}\n" +
                             $"Total Pay: Rs {totalPay}";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }
    }
}
