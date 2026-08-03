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
    public partial class BooksForm : Form
    {
        public BooksForm()
        {
            InitializeComponent();
            LoadBooks();
        }
       

        private void LoadBooks()
        {
           SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Select * From Book";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgvBooks.DataSource = table;
            con.Close();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtAuthor.Text =="" || txtPrice.Text=="" || txtQuantity.Text=="" || txtTitle.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Insert Into Book (Title, Author, Category, Price, Quantity) VALUES ( @Title , @Author, @Category, @Price, @Quantity)";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
            cmd.Parameters.AddWithValue("@Author", txtAuthor.Text);
            cmd.Parameters.AddWithValue("@Category", cmbCategory.Text);
            cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));
            cmd.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));
            int Result = cmd.ExecuteNonQuery();
            if (Result > 0)
            {
                MessageBox.Show("Book added successfuly.");
                LoadBooks();
            }
            txtAuthor.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtTitle.Clear();
            cmbCategory.SelectedIndex = -1;
            con.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtAuthor.Text == "" || txtPrice.Text == "" || txtQuantity.Text == "" || txtTitle.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure you want to update the book ?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            if (dgvBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book first.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Update Book SET Title=@title , Author= @author, Category=@category, Price=@price, Quantity=@quantity WHERE BookId=@bookId;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@title", txtTitle.Text);
            cmd.Parameters.AddWithValue("@author", txtAuthor.Text);
            cmd.Parameters.AddWithValue("@category", cmbCategory.Text);
            cmd.Parameters.AddWithValue("@price", decimal.Parse(txtPrice.Text));
            cmd.Parameters.AddWithValue("@quantity", int.Parse(txtQuantity.Text));
            cmd.Parameters.AddWithValue("@bookId", int.Parse(dgvBooks.SelectedRows[0].Cells["BookID"].Value.ToString()));
            int Result = cmd.ExecuteNonQuery();
            if (Result >0)
            {
                MessageBox.Show("Book Updated Successfully.");
                LoadBooks();
            }
            con.Close();
            txtAuthor.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtTitle.Clear();
            cmbCategory.SelectedIndex = -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book first.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure you want to Delete the book ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Delete From Book WHERE BookId=@bookId;";
            SqlCommand cmd = new SqlCommand(sql, con);
            
            cmd.Parameters.AddWithValue("@bookId", int.Parse(dgvBooks.SelectedRows[0].Cells["BookID"].Value.ToString()));
            int Result = cmd.ExecuteNonQuery();
            if (Result > 0)
            {
                MessageBox.Show("Book Deleted Successfully.");
                LoadBooks();
            }
            con.Close();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtAuthor.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtTitle.Clear();
            cmbCategory.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim()=="")
            {
                LoadBooks();
                return;
            }
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Select * From Book WHERE Title LIKE @search; ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);
            adapter.SelectCommand.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgvBooks.DataSource = table;
            con.Close();

        }
    }
}
