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
    public partial class Form10 : Form


    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vnala\source\repos\Test\Test\Database1.mdf;Integrated Security=True";

        public Form10()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show();
            this.Hide();

        }

        private void LoadOnDutyEmployees()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT A.AttendanceID, W.Name 
                         FROM Attendance A
                         INNER JOIN Worker W ON A.WorkerID = W.WorkerID
                         WHERE A.OffTime IS NULL";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "AttendanceID";
            }
        }
        private void Form10_Load(object sender, EventArgs e)
        {
            LoadOnDutyEmployees();
            // Time only
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker1.ShowUpDown = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int attendanceID = Convert.ToInt32(comboBox1.SelectedValue);
            DateTime offTime = dateTimePicker1.Value;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Get OnTime first
                string getOnTimeQuery = "SELECT OnTime FROM Attendance WHERE AttendanceID=@id";
                SqlCommand getCmd = new SqlCommand(getOnTimeQuery, con);
                getCmd.Parameters.AddWithValue("@id", attendanceID);

                con.Open();
                DateTime onTime = (DateTime)getCmd.ExecuteScalar();
                con.Close();

                // Calculate work hours
                double totalHours = (offTime - onTime).TotalHours;
                double overtime = totalHours > 8 ? totalHours - 8 : 0;

                // Update Attendance
                string updateQuery = "UPDATE Attendance SET OffTime=@OffTime, HoursWorked=@Hours, OvertimeHours=@OT WHERE AttendanceID=@id";
                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@OffTime", offTime);
                updateCmd.Parameters.AddWithValue("@Hours", totalHours);
                updateCmd.Parameters.AddWithValue("@OT", overtime);
                updateCmd.Parameters.AddWithValue("@id", attendanceID);

                con.Open();
                updateCmd.ExecuteNonQuery();
                con.Close();
            }

            MessageBox.Show("Duty Off recorded!");
            LoadOnDutyEmployees(); // refresh list
        }
    }
}
