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
    public partial class UsersForm : Form
    {
        public UsersForm()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "SELECT * From Users;";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgvUsers.DataSource = table;
            con.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Insert Into Users (Username, Password, Role) Values( @username, @password, @role);";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@password", txtPassword.Text);
            cmd.Parameters.AddWithValue("@role", cmbRole.Text);
            int result = cmd.ExecuteNonQuery();
            if (result> 0)
            {
                MessageBox.Show("User Added sucessfully");
            }
            txtPassword.Clear();
            txtUsername.Clear();
            cmbRole.SelectedIndex = -1;
            LoadUsers();
            con.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0) 
            {
                MessageBox.Show("Plaese select a user first ");
                return;
            }
            if (txtPassword.Text == "" || txtUsername.Text=="")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure you want to update the user?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Update Users Set Username = @username , Password= @password, Role= @role Where UserID= @userId;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@password", txtPassword.Text);
            cmd.Parameters.AddWithValue("role", cmbRole.Text);
            cmd.Parameters.AddWithValue("@userId", dgvUsers.SelectedRows[0].Cells["UserID"].Value.ToString());
            int Result = cmd.ExecuteNonQuery();
            if (Result > 0)
            {
                MessageBox.Show("User Updated Successfully ");
            }
            txtPassword.Clear();
            txtUsername.Clear();
            cmbRole.SelectedIndex = -1;
            LoadUsers();
            con.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Plaese select a user first ");
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure you want to deactive this user?", "Deactive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Update Users SeT IsActive = 0  Where UserID = @userId;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@userId", dgvUsers.SelectedRows[0].Cells["UserID"].Value.ToString());
            int Result = cmd.ExecuteNonQuery();
            if (Result > 0)
            {
                MessageBox.Show("User deactivated successfully.");
            }
            LoadUsers();
            con.Close();


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
            txtUsername.Clear();
            cmbRole.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() == "")
            {
                LoadUsers();
                return;
            }
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "select * from Users Where Username Like @username;";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            adapter.SelectCommand.Parameters.AddWithValue("@username", "%" + txtSearch.Text + "%");
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgvUsers.DataSource = table;
            con.Close();
            txtSearch.Clear();

        }
    }
}
