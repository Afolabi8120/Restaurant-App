using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Restaurant_Software
{
    public partial class frmGenerateBarcode : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        char x = (char)8358;

        public frmGenerateBarcode()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmGenerateBarcode_Load(object sender, EventArgs e)
        {
            

            getAllStock();
        }

        void getAllStock()
        {
            datagridStockInventory.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct ORDER BY pname ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                datagridStockInventory.Rows.Add(i, dr["barcode"].ToString(), dr["code"].ToString(), dr["pname"].ToString(), dr["price"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            datagridStockInventory.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code LIKE '" + txtSearch.Text + "%' OR barcode LIKE '" + txtSearch.Text + "%' OR pname LIKE '" + txtSearch.Text + "%' ORDER BY pname ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                datagridStockInventory.Rows.Add(i, dr["barcode"].ToString(), dr["code"].ToString(), dr["pname"].ToString(), x + " " + dr["price"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void datagridStockInventory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = datagridStockInventory.Columns[e.ColumnIndex].Name;
            string barcode = datagridStockInventory.Rows[e.RowIndex].Cells[1].Value.ToString();
            string pname = datagridStockInventory.Rows[e.RowIndex].Cells[3].Value.ToString();

            if (ColName == "ColPrint")
            {
                if (MessageBox.Show("Print " + pname + " Barcode?", "PRINT BARCODE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var naira_sign = x.ToString();

                    frmBarcodeReport f1 = new frmBarcodeReport();
                    f1.barcode = barcode;
                    f1.naira_sign = naira_sign;
                    f1.LoadHeader();
                    f1.LoadBarcode();
                    f1.ShowDialog();
                }
            }
        }
    }
}
