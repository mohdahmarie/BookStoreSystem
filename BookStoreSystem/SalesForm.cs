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
    public partial class SalesForm : Form
    {
        int UserID;
        public SalesForm(int UserID)
        {
            InitializeComponent();
            this.UserID = UserID;
        }
       
        decimal total = 0;


        private void btnSearchBook_Click(object sender, EventArgs e)
        {
            if (txtSearchBook.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the book title ");
                return;
            }
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Select * from Book WHERE Title LIKE @search;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@search", "%" + txtSearchBook.Text.Trim() + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txtBookTitle.Text = reader["Title"].ToString();
                txtPrice.Text = reader["Price"].ToString();
                int quantity = int.Parse(reader["Quantity"].ToString());
                if (quantity > 0)
                {
                    txtAvailableQuantity.Text = quantity.ToString() + " Avaiable";
                }
                else
                {
                    txtAvailableQuantity.Text = "Out of Stock";
                }
            }
            else
            {
                MessageBox.Show("Book not found! ");
            }
            reader.Close();
            con.Close();

        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            SqlConnection con = DatabaseHelper.GetConnection();
            con.Open();
            string sql = "Select * From Book Where Title = @title;";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@title", txtBookTitle.Text);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int quantity = int.Parse(reader["Quantity"].ToString());
                decimal price = decimal.Parse(reader["Price"].ToString());
                string BookId = reader["BookID"].ToString();

                if (int.TryParse(txtSaleQuantity.Text, out int result))
                {
                    if (result > quantity)
                    {
                        MessageBox.Show("Only " + quantity + " books are available");
                    }
                    else
                    {
                        decimal subtotal = result * price;
                        dgvCart.Rows.Add(BookId, txtBookTitle.Text, result, price, subtotal);
                        total = total + subtotal;
                        lblTotal.Text = "Total: $" + total.ToString();

                    }
                }
                else
                {
                    MessageBox.Show("Please enter invalid input ");
                }
            }
            else
            {
                MessageBox.Show("Book not found!");
            }

            txtAvailableQuantity.Clear();
            txtBookTitle.Clear();
            txtPrice.Clear();
            txtSaleQuantity.Clear();
            txtSearchBook.Clear();

            reader.Close();
            con.Close();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item first ");
            }
            else
            {
                DialogResult result = MessageBox.Show("Are you sure you want to DELETE this item?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    decimal DeletedSubtotal = decimal.Parse(dgvCart.SelectedRows[0].Cells["colSubtotal"].Value.ToString());
                    int rowIndex = dgvCart.CurrentRow.Index;
                    dgvCart.Rows.RemoveAt(rowIndex);
                    total = total - DeletedSubtotal;
                    lblTotal.Text = "Total: $" + total.ToString();
                }
            }
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Clear the cart ?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            else
            {
                dgvCart.Rows.Clear();
                total = 0;
                lblTotal.Text = "Total: $" + total.ToString();
            }
        }

        private void btnCompeleteSale_Click(object sender, EventArgs e)
        {
            if (dgvCart.Rows.Count == 0)
            {
                MessageBox.Show("The Cart is empty!");
                return;
            }
            DialogResult result = MessageBox.Show("Are you want to compelete this sale ?", "Complete sale", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }
            else
            {
                SqlConnection con = DatabaseHelper.GetConnection();
                con.Open();
                string sql = "Insert Into Sales ( UserID , SaleDate, TotalAmount) VALUES (@userId, @saleDate, @totalAmount); SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(sql, con);
                 DateTime Date = DateTime.Now;
                cmd.Parameters.AddWithValue("@userId", UserID);
                cmd.Parameters.AddWithValue("@saleDate", Date);
                cmd.Parameters.AddWithValue("@totalAmount", total);
                int SaleID = int.Parse(cmd.ExecuteScalar().ToString());
                foreach(DataGridViewRow row in dgvCart.Rows)
                {
                    int BookID = int.Parse(row.Cells["colBookID"].Value.ToString());
                    int Quantity = int.Parse(row.Cells["colQuantity"].Value.ToString());
                    decimal UnitPrice = decimal.Parse(row.Cells["colUnitPrice"].Value.ToString());
                    decimal Subtotal = decimal.Parse(row.Cells["colSubtotal"].Value.ToString());
                    string sqlSaleDetails = "Insert Into SalesDetails (SaleID, BookID, Quantity, UnitPrice,Subtotal) VALUES (@SaleId, @BookId, @quantity, @unitPrice,@subtotal);";
                    SqlCommand cmdSaleDetails = new SqlCommand(sqlSaleDetails, con);
                    cmdSaleDetails.Parameters.AddWithValue("@SaleId", SaleID);
                    cmdSaleDetails.Parameters.AddWithValue("@BookId", BookID);
                    cmdSaleDetails.Parameters.AddWithValue("@quantity", Quantity);
                    cmdSaleDetails.Parameters.AddWithValue("@unitPrice", UnitPrice);
                    cmdSaleDetails.Parameters.AddWithValue("@subtotal", Subtotal);
                    int Result = cmdSaleDetails.ExecuteNonQuery();
                    if (Result > 0)
                    {
                        
                        string sqlBook = "Update Book SET Quantity - @quantity Where BookID = @BookId";
                        SqlCommand cmdBook = new SqlCommand(sqlBook, con);
                        cmdBook.Parameters.AddWithValue("@quantity", Quantity);
                        cmdBook.Parameters.AddWithValue("@BookId", BookID);
                        int BookUpdate = cmdBook.ExecuteNonQuery();

                    }
                    

                }
                MessageBox.Show("Sale completed successfully");
                dgvCart.Rows.Clear();
                total = 0;
                lblTotal.Text = "Total: $" + total.ToString();
                con.Close();
            }
        }
    }
}
