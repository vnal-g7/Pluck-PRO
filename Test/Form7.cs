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

namespace Test
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
 SqlConnection Connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vnala\source\repos\Test\Test\Database1.mdf;Integrated Security=True");

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }
        void populate()
        {
            Connection.Open();
            // Select all columns plus a calculated Availability column
            string query = "SELECT *, (StockIn - StockOut) AS Availability FROM Stock";
            SqlDataAdapter da = new SqlDataAdapter(query, Connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            var ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            Connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\vnala\\source\\repos\\Test\\Test\\Database1.mdf;Integrated Security=True"))
                {
                    string query = "INSERT INTO Stock (ItemId, ItemName, StockIn, StockOut) " +
                                   "VALUES (@ItemId, @Itemname, @StockIn, @StockOut)";

                    SqlCommand cmd = new SqlCommand(query, con);

                 
                    cmd.Parameters.AddWithValue("@ItemId", textBox1.Text);
                    cmd.Parameters.AddWithValue("@ItemName", textBox2.Text);
                    cmd.Parameters.AddWithValue("@StockIn", textBox3.Text);
                    cmd.Parameters.AddWithValue("@StockOut", textBox4.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    populate(); 

                    MessageBox.Show("Stock Recorded Successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please select a record first.");
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vnala\source\repos\Test\Test\Database1.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Stock WHERE ItemID=@ItemID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ItemID", textBox1.Text);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                if (rows > 0)
                    MessageBox.Show("Stock deleted successfully!");
                else
                    MessageBox.Show("Delete failed!");
            }
            populate();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["ItemID"].Value.ToString();
                textBox2.Text = row.Cells["ItemName"].Value.ToString();
                textBox3.Text = row.Cells["StockIn"].Value.ToString();
                textBox4.Text = row.Cells["StockOut"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please select a record first.");
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vnala\source\repos\Test\Test\Database1.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Stock SET ItemName=@ItemName, StockIn=@StockIn, StockOut=@StockOut WHERE ItemID=@ItemID";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ItemID", textBox1.Text);
                cmd.Parameters.AddWithValue("@ItemName", textBox2.Text);
                cmd.Parameters.AddWithValue("@StockIn", textBox3.Text);
                cmd.Parameters.AddWithValue("@StockOut", textBox4.Text);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                if (rows > 0)
                    MessageBox.Show("Stock updated successfully!");
                else
                    MessageBox.Show("Update failed!");
            }
            populate();

        }
    }
    }

