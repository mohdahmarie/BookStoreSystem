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
            string sql = "SELECT Role , UserID FROM Users WHERE username=@username AND Password=@password";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
            cmd.Parameters.AddWithValue("Password", txtPassword.Text);
            SqlDataReader reader = cmd.ExecuteReader();
           
            if (reader.Read())
            {
                int UserID = int.Parse(reader["UserID"].ToString());
                string role = reader["Role"].ToString();
                    MainForm main = new MainForm(role , txtUsername.Text , UserID);
                    main.Show();
                    this.Hide();

                
                
            }
            else
            {
                MessageBox.Show("Username or Password is incorrect!");
            }
            reader.Close();
            connection.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    }

