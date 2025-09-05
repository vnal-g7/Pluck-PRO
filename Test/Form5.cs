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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\vnala\\source\\repos\\Test\\Test\\Database1.mdf;Integrated Security=True"))
                {
                    string query = "INSERT INTO Worker (WorkerId, Name, NIC, Contact, Address, JobRole) " +
                                   "VALUES (@WorkerId, @Name, @NIC, @Contact, @Address, @JobRole)";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // Replace textBox1..textBox5 with your real textbox names
                    cmd.Parameters.AddWithValue("@WorkerId", textBox2.Text);
                    cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@NIC", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Contact", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Address", textBox5.Text);
                    cmd.Parameters.AddWithValue("@JobRole", textBox6.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Employee Registered Successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
