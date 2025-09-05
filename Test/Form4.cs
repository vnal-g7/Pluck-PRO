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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
            private void Form4_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker1.ShowUpDown = true;

            dateTimePicker2.Format = DateTimePickerFormat.Time;
            dateTimePicker2.ShowUpDown = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int workerId = Convert.ToInt32(comboBox2.SelectedValue);
            DateTime date = DateTime.Now.Date;

            // Get Duty On and Off from pickers
            DateTime dutyOn = dateTimePicker1.Value;
            DateTime dutyOff = dateTimePicker2.Value;

            // Calculate total hours worked
            TimeSpan workedTime = dutyOff - dutyOn;
            decimal hoursWorked = (decimal)workedTime.TotalHours;

            // Calculate overtime (above 8 hours)
            decimal overtime = 0;
            if (hoursWorked > 8) overtime = hoursWorked - 8;

            // Save into database
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\vnala\\source\\repos\\Test\\Test\\Database1.mdf;Integrated Security=True";
            string query = "INSERT INTO Attendance (WorkerID, Date, DutyOn, DutyOff, HoursWorked, OvertimeHours) VALUES (@WorkerID, @Date, @DutyOn, @DutyOff, @HoursWorked, @OvertimeHours)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@WorkerID", workerId);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@DutyOn", dutyOn.TimeOfDay);
                cmd.Parameters.AddWithValue("@DutyOff", dutyOff.TimeOfDay);
                cmd.Parameters.AddWithValue("@HoursWorked", hoursWorked);
                cmd.Parameters.AddWithValue("@OvertimeHours", overtime);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            MessageBox.Show("Attendance Recorded. Hours Worked = " + hoursWorked + ", OT = " + overtime);


        }

        private void Form4_Load_1(object sender, EventArgs e)
        {
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vnala\source\repos\Test\Test\Database1.mdf;Integrated Security=True";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT WorkerID, Name FROM Worker";
                    SqlCommand cmd = new SqlCommand(query, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    // Assuming your dropdown is called comboBoxEmployee
                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "Name";   // shows worker name
                    comboBox2.ValueMember = "WorkerID"; // stores worker ID

                    con.Close();
                }
            }
        }
    }
}
