using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;

namespace Restaurant_Software
{
    public partial class frmCashier : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();

        string p_name, p_code, p_price, p_qty;
        public string myinvoiceno;

        bool found;

        public frmCashier()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        string proceName = string.Empty;

        void getShopName()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblstore", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                lblRestaurantName.Text = dr["name"].ToString();
            }
            else
            {
                lblRestaurantName.Text = "Shop/Store Name";
            }
            dr.Close();
            cn.Close();
        }

        public void CategoryList_with_images()
        {
            flowLayoutCategoryPanel.Controls.Clear();
            try
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcategory ORDER BY name ASC", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    Button category = new Button();
                    category.Tag = dr["name"].ToString(); 
                    category.Click += new EventHandler(category_Click);
                    category.Cursor = Cursors.Hand;

                    category.Margin = new Padding(3, 3, 3, 3);

                    category.Size = new Size(110, 45);
                    category.Text.PadRight(1);
                    category.Text += dr["name"].ToString();

                    category.Font = new Font("Times New Roman", 14, FontStyle.Regular, GraphicsUnit.Point);
                    category.TextAlign = ContentAlignment.MiddleCenter;
                    category.TextImageRelation = TextImageRelation.Overlay;
                    flowLayoutCategoryPanel.Controls.Add(category);
                }
                dr.Close();
                cn.Close();
            }
            catch //(Exception)
            {

                //throw;
            }
        }

        protected void category_Click(object sender, EventArgs e)
        {
            Button category = sender as Button;
            string s;
            s = " ID: ";
            s += category.Tag;
            s += "\n Name: ";
            s += category.Name.ToString();

            loadProduct(category.Tag.ToString());
        }

        protected void productButton_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string s;
            s = " ID: ";
            s += b.Tag;
            s += "\n Name: ";
            s += b.Name.ToString();

            txtBarcode.Text = b.Tag.ToString();

        }

        public void loadProduct(string code)
        {
            flowLayoutProductPanel.Controls.Clear();
            try
            {
                if (code == "")
                {
                    cn.Open();
                    cm = new MySqlCommand("SELECT * FROM tblproduct WHERE status = 'Available' AND quantity > 0 ORDER BY pname ASC", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {

                        Button productButton = new Button();
                        productButton.Tag = dr["code"].ToString();
                        productButton.Click += new EventHandler(productButton_Click);
                        productButton.Cursor = Cursors.Hand;

                        string details = dr["code"].ToString() +
                         "\n Name: " + dr["pname"].ToString() +
                         "\n Stock: " + dr["quantity"].ToString() +
                         "\n Price: " + dr["price"].ToString();
                        productButton.Name = details;
                        toolTip1.ToolTipTitle = "Item Details"; 
                        toolTip1.SetToolTip(productButton, details);   

                        byte[] data = (byte[])dr["picture"];
                        MemoryStream ms = new MemoryStream(data);

                        ImageList il = new ImageList();
                        il.ColorDepth = ColorDepth.Depth32Bit;
                        il.TransparentColor = Color.Transparent;
                        il.ImageSize = new Size(80, 80);
                        il.Images.Add(Image.FromStream(ms));

                        productButton.Image = il.Images[0];
                        productButton.Margin = new Padding(3, 3, 3, 3);

                        productButton.Size = new Size(191, 100);
                        productButton.Text.PadRight(4);

                        productButton.Text += dr["pname"].ToString();
                        productButton.Text += "\nStock: " + dr["quantity"].ToString();
                        productButton.Text += "\nPrice: " + dr["price"].ToString();

                        productButton.Font = new Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point);
                        productButton.TextAlign = ContentAlignment.MiddleLeft;
                        productButton.TextImageRelation = TextImageRelation.ImageBeforeText;
                        flowLayoutProductPanel.Controls.Add(productButton);

                    }
                    dr.Close();
                    cn.Close();
                }
                else if (code != "")
                {
                    cn.Open();
                    cm = new MySqlCommand("SELECT * FROM tblproduct WHERE (category LIKE '%" + code + "%' AND status = 'Available' AND quantity > 0) OR (pname LIKE '%" + code + "%' AND status = 'Available' AND quantity > 0) OR (barcode LIKE '%" + code + "%' AND status = 'Available' AND quantity > 0) ORDER BY pname ASC", cn);
                    cm.Parameters.AddWithValue("@category", code);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {

                        Button productButton = new Button();
                        productButton.Tag = dr["code"].ToString();
                        productButton.Click += new EventHandler(productButton_Click);
                        productButton.Cursor = Cursors.Hand;

                        string details = dr["code"].ToString() +
                         "\n Name: " + dr["pname"].ToString() +
                         "\n Stock: " + dr["quantity"].ToString() +
                         "\n Price: " + dr["price"].ToString();
                        productButton.Name = details;
                        toolTip1.ToolTipTitle = "Item Details";  
                        toolTip1.SetToolTip(productButton, details);   

                        byte[] data = (byte[])dr["picture"];
                        MemoryStream ms = new MemoryStream(data);

                        ImageList il = new ImageList();
                        il.ColorDepth = ColorDepth.Depth32Bit;
                        il.TransparentColor = Color.Transparent;
                        il.ImageSize = new Size(80, 80);
                        il.Images.Add(Image.FromStream(ms));

                        productButton.Image = il.Images[0];
                        productButton.Margin = new Padding(3, 3, 3, 3);

                        productButton.Size = new Size(191, 100);
                        productButton.Text.PadRight(4);

                        productButton.Text += dr["pname"].ToString();
                        productButton.Text += "\nStock: " + dr["quantity"].ToString();
                        productButton.Text += "\nPrice: " + dr["price"].ToString();

                        productButton.Font = new Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point);
                        productButton.TextAlign = ContentAlignment.MiddleLeft;
                        productButton.TextImageRelation = TextImageRelation.ImageBeforeText;
                        flowLayoutProductPanel.Controls.Add(productButton);

                    }
                    dr.Close();
                    cn.Close();
                }
            }
            catch //(Exception)
            {

                //throw;
            }
        }

        void getUserDetails()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tbluser WHERE id =@id", cn);
            cm.Parameters.AddWithValue("@id", frmLogin.id);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                lblCashier.Text = dr["usertype"].ToString()  + " | " + dr["fullname"].ToString();
            }
            dr.Close();
            cn.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToLongDateString() + "\n " + DateTime.Now.ToLongTimeString();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                MessageBox.Show("Remove all items from cart before proceeding!", "REMOVE ALL ITEMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (MessageBox.Show("Are you sure?", "LOGGING OUT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Hide();
                    frmLogin f1 = new frmLogin();
                    f1.ShowDialog();
                }
            }
        }

        private void btnSystemLock_Click(object sender, EventArgs e)
        {
            frmSystemLock s1 = new frmSystemLock();
            s1.ShowDialog();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword c1 = new frmChangePassword();
            c1.ShowDialog();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn1.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn1.Text;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtAmountPaid.Text = "0";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn2.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn2.Text;
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn3.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn3.Text;
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn4.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn4.Text;
            }
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn5.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn5.Text;
            }
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn6.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn6.Text;
            }
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn7.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn7.Text;
            }
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn8.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn8.Text;
            }
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn9.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn9.Text;
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn0.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn0.Text;
            }
        }

        private void btn00_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn00.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn00.Text;
            }
        }

        private void btn000_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn000.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn000.Text;
            }
        }

        private void btn500_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn500.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn500.Text;
            }
        }

        private void btn1000_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn1000.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn1000.Text;
            }
        }

        private void btn2000_Click(object sender, EventArgs e)
        {
            if (txtAmountPaid.Text == "" || txtAmountPaid.Text == "0")
            {
                txtAmountPaid.Clear();
                txtAmountPaid.Text = btn2000.Text;
            }
            else
            {
                txtAmountPaid.Text = txtAmountPaid.Text + btn2000.Text;
            }
        }

        //SYSTEM PROGRAME OPEN FUNCTION
        private void IsSysProFileOpened()
        {
            try
            {
                if (proceName == "Calc")
                    Process.Start("Calc");
                else if (proceName == "WINWORD")
                    Process.Start("WINWORD");
                else if (proceName == "Notepad.exe")
                    Process.Start("Notepad.exe");
                else if (proceName == "osk.exe")
                    Process.Start("osk.exe");
                else
                    proceName = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {
                proceName = string.Empty;
                proceName = "Calc";
                IsSysProFileOpened();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmCashier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btnSalesRecord.PerformClick();
            }
            else if (e.KeyCode == Keys.F3)
            {
                btnProductReturn.PerformClick();
            }
            else if (e.KeyCode == Keys.F4)
            {
                btnSystemLock.PerformClick();
            }
            else if (e.KeyCode == Keys.F5)
            {
                btnPay.PerformClick();
            }
            else if (e.KeyCode == Keys.F6)
            {
                btnChangePassword.PerformClick();
            }
            else if (e.KeyCode == Keys.F9)
            {
                btnLogout.PerformClick();
            }
            else if (e.KeyCode == Keys.F10)
            {
                txtBarcode.Clear();
                txtBarcode.Focus();
            }
            else if (e.KeyCode == Keys.F12)
            {
                txtSearchItem.Clear();
                txtSearchItem.Focus();
                loadProduct("");
                CategoryList_with_images();
            }
        }

        private void frmCashier_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;

            getShopName();
            getUserDetails();

            CategoryList_with_images();
            loadProduct("");

            lblInvoiceNo.Text = createInvoiceNo();
        }

        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            try
            {

                loadProduct(txtSearchItem.Text);
            }
            catch
            {
            }
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            if(txtBarcode.Text != "")
            {
                insertToCart();
            }
        }

        void insertToCart()
        {
            string getbarcode = txtBarcode.Text;
            cn.Close();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE (barcode=@barcode) OR (code=@barcode)", cn);
            cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
            cm.Parameters.AddWithValue("@status", "Available");
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                cn.Close();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblproduct WHERE (barcode=@barcode) OR (code=@barcode) AND status=@status", cn);
                cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                cm.Parameters.AddWithValue("@status", "Available");
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("SELECT * FROM tblproduct WHERE (barcode=@barcode) OR (code=@barcode) AND quantity > 0", cn);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        p_code = dr["code"].ToString();
                        p_name = dr["pname"].ToString();
                        p_price = dr["price"].ToString();
                        p_qty = dr["quantity"].ToString();

                        found = true;
                    }
                    else
                    {
                        found = false;
                    }
                }
                else
                {
                    cn.Close();
                    dr.Close();
                    txtBarcode.Clear();
                    MessageBox.Show("This Item is Not Available, Please contact the Admin", "PRODUCT NOT AVAILABLE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cn.Close();
                dr.Close();
            }
            else
            {
                cn.Close();
                dr.Close();
                txtBarcode.Clear();
                MessageBox.Show("Invalid Product Code OR Barcode", "INVALID PRODUCT CODE/BARCODE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cn.Close();
            dr.Close();

            if (found == true)
            {
                cn.Close();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblcart WHERE invoiceno=@invoiceno AND code=@pcode", cn);
                cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                cm.Parameters.AddWithValue("@pcode", p_code);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblcart SET qty= qty + @quantity, total = qty * price WHERE invoiceno=@invoiceno AND code=@pcode", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@pcode", p_code);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity= quantity - @quantity WHERE code=@code", cn);
                    cm.Parameters.AddWithValue("@code", p_code);
                    cm.Parameters.AddWithValue("@price", p_price);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    getCart();
                    getItemCount();
                    getTotalItemPrice();
                    txtBarcode.Clear();
                }
                else
                {
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblcart (invoiceno,code,pname,price,qty,total,status,date,time) VALUES(@invoiceno,@pcode,@pname,@price,@qty,@total,@status,@date,@time)", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@pcode", p_code);
                    cm.Parameters.AddWithValue("@pname", p_name);
                    cm.Parameters.AddWithValue("@price", p_price);
                    cm.Parameters.AddWithValue("@qty", 1);
                    cm.Parameters.AddWithValue("@total", Convert.ToDecimal(p_price) * 1);
                    cm.Parameters.AddWithValue("@status", "PENDING");
                    cm.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
                    cm.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblproduct SET quantity= quantity - @quantity WHERE code=@code", cn);
                    cm.Parameters.AddWithValue("@code", p_code);
                    cm.Parameters.AddWithValue("@price", p_price);
                    cm.Parameters.AddWithValue("@quantity", 1);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    getCart();
                    getItemCount();
                    getTotalItemPrice();
                    txtBarcode.Clear();
                }
                cn.Close();
                dr.Close();

            }
            else if (found == false)
            {
                cn.Close();
                dr.Close();
                txtBarcode.Clear();
                MessageBox.Show("Item with Barcode/Product Code " + getbarcode + " is out of stock\nCurrent Available stock is 0", "ITEM OUT OF STOCK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        public void getCart()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcart WHERE invoiceno=@invoiceno ORDER BY pname ASC", cn);
            cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["pname"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["total"].ToString());
            }
            dr.Close();
            cn.Close();

            lblItemCount.Text = "0";
            lblSubTotal.Text = "0.00";
        }

        void getTotalItemPrice()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT ifnull(SUM(total), 0) FROM tblcart WHERE invoiceno=@invoiceno", cn);
            cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
            var num = cm.ExecuteScalar().ToString();
            cn.Close();

            //lblSubTotal.Text = num.ToString();
            lblSubTotal.Text = num.ToString();
        }

        void getItemCount()
        {
            lblItemCount.Text = dataGridView1.Rows.Count.ToString();
        }

        // generate invoice no
        public static string createInvoiceNo()
        {
            string invoice_no;

            var day = DateTime.Today.Day;
            var month = DateTime.Today.Month;
            var year = DateTime.Today.Year;

            // this will generate number from 0 - 25
            Random stringRand = new Random();
            int resultRand = stringRand.Next(0, 25);

            Random myrand = new Random();
            int rand = myrand.Next(111, 99999);

            var myarray = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            var result = myarray[resultRand]; // so the number generated from resultRand will be assigned to result

            invoice_no = "RMS" + year + month + day + result + rand;

            return invoice_no;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
                string pcode = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                string _qty = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

                if (ColName == "ColRemove")
                {
                    if (MessageBox.Show("Remove Product?", "REMOVE PRODUCT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("UPDATE tblproduct SET quantity= quantity + @quantity WHERE code=@code", cn);
                        cm.Parameters.AddWithValue("@code", pcode);
                        cm.Parameters.AddWithValue("@quantity", _qty);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();
                        cm = new MySqlCommand("DELETE FROM tblcart WHERE invoiceno=@invoiceno AND code=@pcode", cn);
                        cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                        cm.Parameters.AddWithValue("@pcode", pcode);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        getCart();
                        getItemCount();
                        getTotalItemPrice();
                    }
                }
                else if (ColName == "ColIncrease")
                {
                    cn.Close();
                    cn.Open();
                    cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code=@code AND quantity > 0", cn);
                    cm.Parameters.AddWithValue("@code", pcode);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        p_qty = dr["quantity"].ToString();

                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("UPDATE tblproduct SET quantity= quantity - @quantity WHERE code=@code", cn);
                        cm.Parameters.AddWithValue("@code", pcode);
                        cm.Parameters.AddWithValue("@quantity", 1);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();
                        cm = new MySqlCommand("UPDATE tblcart SET qty= qty + @quantity, total = price * qty WHERE invoiceno=@invoiceno AND code=@pcode", cn);
                        cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                        cm.Parameters.AddWithValue("@pcode", pcode);
                        cm.Parameters.AddWithValue("@quantity", 1);
                        cm.ExecuteNonQuery();
                        cn.Close();


                        getCart();
                        getItemCount();
                        getTotalItemPrice();
                    }
                    else
                    {
                        cn.Close();
                        dr.Close();
                        txtBarcode.Clear();
                        MessageBox.Show("The selected product is out of stock\nCurrent Available stock is 0", "ITEM OUT OF STOCK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cn.Close();
                    dr.Close();
                }
                else if (ColName == "ColDecrease")
                {
                    if (Int32.Parse(_qty) < 2)
                    {
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("UPDATE tblproduct SET quantity= quantity + @quantity WHERE code=@code", cn);
                        cm.Parameters.AddWithValue("@code", pcode);
                        cm.Parameters.AddWithValue("@quantity", 1);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();
                        cm = new MySqlCommand("DELETE FROM tblcart WHERE invoiceno=@invoiceno AND code=@pcode", cn);
                        cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                        cm.Parameters.AddWithValue("@pcode", pcode);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        getCart();
                        getItemCount();
                        getTotalItemPrice();

                    }
                    else
                    {
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity + @quantity WHERE code=@code", cn);
                        cm.Parameters.AddWithValue("@code", pcode);
                        cm.Parameters.AddWithValue("@quantity", 1);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();
                        cm = new MySqlCommand("UPDATE tblcart SET qty= qty - @quantity, total = price * qty WHERE invoiceno=@invoiceno AND code=@pcode", cn);
                        cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                        cm.Parameters.AddWithValue("@pcode", pcode);
                        cm.Parameters.AddWithValue("@quantity", 1);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        getCart();
                        getItemCount();
                        getTotalItemPrice();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtAmountPaid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal change = Convert.ToDecimal(txtAmountPaid.Text) - Convert.ToDecimal(lblSubTotal.Text);

                lblChange.Text = change.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            saveTransaction();
        }

        void saveTransaction()
        {
            if (dataGridView1.Rows.Count <= 0)
            {
                MessageBox.Show("Cart is empty", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (lblSubTotal.Text == "0.00" || lblSubTotal.Text == "0" || lblSubTotal.Text == String.Empty || txtAmountPaid.Text == "0.00" || txtAmountPaid.Text == "0" || txtAmountPaid.Text == "")
            {
                MessageBox.Show("Can't proceed with transaction, Please Enter a Valid Amount to Pay", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (cboPaymentMode.Text == String.Empty)
            {
                MessageBox.Show("Please select a valid payment mode", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (Convert.ToDecimal(lblChange.Text) < 0)
            {
                MessageBox.Show("Insufficient Amount, Please Enter a Valid Amount", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (MessageBox.Show("Do you want to proceed with this transaction?\nNOTE: This transaction cannot be reverse!", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblpayment (invoiceno,total,amountpaid,mchange,paymode,date,time,user,status) VALUES(@invoiceno,@total,@amountpaid,@mchange,@paymode,@date,@time,@user,@status)", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@total", lblSubTotal.Text);
                    cm.Parameters.AddWithValue("@amountpaid", txtAmountPaid.Text);
                    cm.Parameters.AddWithValue("@mchange", lblChange.Text);
                    cm.Parameters.AddWithValue("@paymode", cboPaymentMode.Text);
                    cm.Parameters.AddWithValue("@date", DateTime.Now.ToShortDateString());
                    cm.Parameters.AddWithValue("@time", DateTime.Now.ToShortTimeString());
                    cm.Parameters.AddWithValue("@user", frmLogin.username);
                    cm.Parameters.AddWithValue("@status", "COMPLETED");
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("UPDATE tblcart SET status=@status WHERE invoiceno=@invoiceno", cn);
                    cm.Parameters.AddWithValue("@invoiceno", lblInvoiceNo.Text);
                    cm.Parameters.AddWithValue("@status", "COMPLETED");
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Transaction has been saved successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmReceipt fpay = new frmReceipt();
                    fpay.invoiceno = this.lblInvoiceNo.Text;
                    fpay.ShowDialog();

                    lblInvoiceNo.Text = createInvoiceNo();
                    getCart();
                    lblItemCount.Text = "0";
                    lblSubTotal.Text = "0.00";
                    cboPaymentMode.SelectedIndex = -1;
                    txtAmountPaid.Text = "";
                    lblChange.Text = "0.00";
                    loadProduct("");
                }
            }

        }

        private void btnSalesRecord_Click(object sender, EventArgs e)
        {
            frmSalesHistory f1 = new frmSalesHistory();
            f1.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                proceName = string.Empty;
                proceName = "osk.exe";
                IsSysProFileOpened();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProductReturn_Click(object sender, EventArgs e)
        {
            frmReturnProduct r1 = new frmReturnProduct();
            r1.ShowDialog();
        }
    }
}
