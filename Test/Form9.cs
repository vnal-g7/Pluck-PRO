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
    public partial class Form9 : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vnala\source\repos\Test\Test\Database1.mdf;Integrated Security=True";

        public Form9()
        {
            InitializeComponent();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
           LoadEmployees();

            dateTimePicker2.Format = DateTimePickerFormat.Short;

            // Time only
            dateTimePicker1.Format = DateTimePickerFormat.Time;
            dateTimePicker1.ShowUpDown = true;

            
        }
        private void LoadEmployees()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vnala\source\repos\Test\Test\Database1.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT WorkerID, Name FROM Worker";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "WorkerID";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int workerID = Convert.ToInt32(comboBox1.SelectedValue);
            DateTime date = dateTimePicker2.Value.Date;
            DateTime onTime = dateTimePicker1.Value;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Attendance (WorkerID, [Date], OnTime) VALUES (@WorkerID, @Date, @OnTime)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@WorkerID", workerID);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@OnTime", onTime);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                if (rows > 0)
                    MessageBox.Show("Duty On recorded!");
                else
                    MessageBox.Show("Insert failed!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form10 form10 = new Form10();
            form10.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
