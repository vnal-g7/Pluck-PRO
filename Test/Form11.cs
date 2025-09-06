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
    public partial class Form11 : Form
    {
        public Form11()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vnala\source\repos\Test\Test\Database1.mdf;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 'Attendance' AS ReportType, 
                   W.Name, 
                   A.Date, 
                   A.OnTime, 
                   A.OffTime, 
                   A.OvertimeHours, 
                   NULL AS ItemName, 
                   NULL AS StockIn, 
                   NULL AS StockOut, 
                   NULL AS AvailableStock
            FROM Attendance A
            JOIN Worker W ON A.WorkerID = W.WorkerID

            UNION ALL

            SELECT 'Stock' AS ReportType, 
                   NULL AS Name, 
                   NULL AS Date, 
                   NULL AS OnTime, 
                   NULL AS OffTime, 
                   NULL AS OvertimeHours, 
                   S.ItemName, 
                   S.StockIn, 
                   S.StockOut, 
                   (S.StockIn - S.StockOut) AS AvailableStock
            FROM Stock S

            UNION ALL

            SELECT 'Worker' AS ReportType, 
                   W.Name, 
                   NULL AS Date, 
                   NULL AS OnTime, 
                   NULL AS OffTime, 
                   NULL AS OvertimeHours, 
                   NULL AS ItemName, 
                   NULL AS StockIn, 
                   NULL AS StockOut, 
                   NULL AS AvailableStock
            FROM Worker W;";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt; // Your grid in Reports form
            }
        }
    }
}
