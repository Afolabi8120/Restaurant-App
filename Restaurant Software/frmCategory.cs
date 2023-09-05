using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Restaurant_Software
{
    public partial class frmCategory : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();

        public frmCategory()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == String.Empty)
            {
                MessageBox.Show("All Fields Are Required!", "EMPTY FIELD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("Add Category?", "ADD CATEGORY", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("SELECT name FROM tblcategory WHERE name=@name ", cn);
                    cm.Parameters.AddWithValue("@name", txtName.Text.ToUpper());
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        cn.Close();
                        MessageBox.Show("Category Name Already Exist", "DUPLICATE NAME FOUND", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("INSERT INTO tblcategory (name) VALUES(@name) ", cn);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Category Added Successfully", "CATEGORY ADDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRecord();
                        txtName.Clear();
                        txtName.Focus();
                    }
                    dr.Close();
                    cn.Close();
                }
            }
        }

        void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcategory ORDER BY name ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            string name = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            if (ColName == "ColRemove")
            {
                if (MessageBox.Show("Remove this category?", "REMOVE CATEGORY", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblcategory WHERE name=@name", cn);
                    cm.Parameters.AddWithValue("@name", name);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Category Removed Successfully", "CATEGORY REMOVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtName.Focus();
            LoadRecord();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcategory WHERE name LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }
    }
}
