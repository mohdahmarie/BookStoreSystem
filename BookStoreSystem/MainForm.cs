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

namespace BookStoreSystem
{
    public partial class MainForm : Form
    {
        public MainForm(String role, string username )
        {
            InitializeComponent();

            lblWelcome.Text = "Welcome, " + username;

            if (role == "Cashier")
            {
                btnUsers.Visible = false;
                btnReports.Visible = false;
            }
            LoadDashboard();


        }

        public void LoadDashboard()
        {
            String ConnectionString = @"Server = localhost\SQLEXPRESS; Database = BookstoreDB; Trusted_Connection= True; TrustServerCertificate=True;";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string sqlBooks = "Select COUNT(*) From Book";
            SqlCommand cmdBooks = new SqlCommand(sqlBooks,con);
            object BooksResult = cmdBooks.ExecuteScalar();
            lblTotalBooks.Text = BooksResult.ToString();


            String sqlSales = "Select COUNT(*) From Sales";
            SqlCommand cmdSales = new SqlCommand(sqlSales, con);
            object SalesResult = cmdSales.ExecuteScalar();
            lblTotalSales.Text = SalesResult.ToString();

            String sqlUsers = "Select COUNT(*) From Users";
            SqlCommand cmdUsers = new SqlCommand(sqlUsers, con);
            object UsersResult = cmdUsers.ExecuteScalar();
            lblTotalUsers.Text = UsersResult.ToString();

            String sqlRevenues = "Select SUM(TotalAmount) From Sales";
            SqlCommand cmdRevenues = new SqlCommand(sqlRevenues, con);
            object RevenuesResult = cmdRevenues.ExecuteScalar();
            if (RevenuesResult== DBNull.Value)
            {
                RevenuesResult = "$ 0.00";
            }
            else
            {
                lblTotalRevenues.Text = "$"+ RevenuesResult.ToString();
            }

            con.Close();

        }

      

        private void btnBooks_Click(object sender, EventArgs e)
        {
            BooksForm booksForm = new BooksForm();
            booksForm.Show();
            this.Hide();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            SalesForm salesForm = new SalesForm();
            salesForm.Show();
            this.Hide();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            UsersForm usersForm = new UsersForm();
            usersForm.Show();
            this.Hide();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?" , "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
            else { }
        }
    }
}
