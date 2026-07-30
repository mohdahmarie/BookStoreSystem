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

namespace BookStoreSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String ConnectionString = @"Server= localhost\SQLEXPRESS; Database= BookStoreDB; Trusted_connection=True; TrustServerCertificate=True;";

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT Role FROM Users WHERE username=@username AND Password=@password";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
            cmd.Parameters.AddWithValue("Password", txtPassword.Text);
            object role = cmd.ExecuteScalar();
           
            if (role != null)
            {
                if (role.ToString() == "Admin")
                {
                    MainForm main = new MainForm();
                    main.Show();
                    this.Hide();

                }
                else if (role.ToString() == "Cashier")
                {
                    MainForm main = new MainForm();
                    main.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Username or Password is incorrect!");
            }

            connection.Close();
        }
    }
    }

