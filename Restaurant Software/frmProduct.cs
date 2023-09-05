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
using System.IO;

namespace Restaurant_Software
{
    public partial class frmProduct : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();

        string pcode;

        public frmProduct()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        void fetchCategory()
        {
            cboCategory.Items.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcategory ORDER BY name ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboCategory.Items.Add(dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        void generateProductCode()
        {
            Random rand = new Random();
            int myrand = rand.Next(10000, 1000000);
            string pcode = "RMS-" + myrand;
            txtProductCode.Text = pcode;
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            generateProductCode();
            getAllProductRecord();
            fetchCategory();
        }

        void Clear()
        {
            txtProductCode.Clear();
            txtBarcode.Clear();
            txtDescription.Clear();
            txtName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtProductCode.Clear();
            cboCategory.SelectedIndex = -1;
            cboStatus.SelectedIndex = -1;
            generateProductCode();
            pictureBox1.Image = null;
            picBarcode.Image = null;
            btnSave.Text = "Save";
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Save")
            {
                if (MessageBox.Show("Save this Data? Click Yes to continue", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Validating Form
                    if (txtProductCode.Text == String.Empty || txtName.Text == String.Empty || txtBarcode.Text == String.Empty || txtDescription.Text == String.Empty)
                    {
                        MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else if (cboStatus.Text == String.Empty || cboCategory.Text == String.Empty)
                    {
                        MessageBox.Show("Please select a valid option!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {

                        // Check if supplier id exist in database
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code = @code", cn);
                        cm.Parameters.AddWithValue("@code", txtProductCode.Text);
                        dr = cm.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            dr.Close();
                            cn.Close();
                            MessageBox.Show("Product Code already exist!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            // Check if email address exist in database
                            cn.Close();
                            cn.Open();
                            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE barcode = @barcode", cn);
                            cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                            dr = cm.ExecuteReader();
                            dr.Read();
                            if (dr.HasRows)
                            {
                                dr.Close();
                                cn.Close();
                                MessageBox.Show("Barcode is already in use!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                byte[] bytImage = new byte[0];
                                MemoryStream ms = new System.IO.MemoryStream();
                                Bitmap bmpImage = new Bitmap(pictureBox1.Image);

                                bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                ms.Seek(0, 0);
                                bytImage = ms.ToArray();
                                ms.Close();

                                // Barcode Image
                                byte[] BarcodeImage = new byte[1];
                                MemoryStream ms1 = new System.IO.MemoryStream();
                                Bitmap bmpImage1 = new Bitmap(picBarcode.Image);

                                bmpImage1.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
                                ms1.Seek(0, 0);
                                BarcodeImage = ms1.ToArray();
                                ms1.Close();

                                // if email address and username does not exist in database, it will save the user's data into the database
                                cn.Close();
                                cn.Open();
                                cm = new MySqlCommand("INSERT INTO tblproduct (code,barcode,pname,category,description,status,quantity,price,picture,barcode_picture) VALUES(@code,@barcode,@pname,@category,@description,@status,@quantity,@price,@picture,@barcode_picture) ", cn);
                                cm.Parameters.AddWithValue("@code", txtProductCode.Text);
                                cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                                cm.Parameters.AddWithValue("@pname", txtName.Text);
                                cm.Parameters.AddWithValue("@category", cboCategory.Text);
                                cm.Parameters.AddWithValue("@description", txtDescription.Text);
                                cm.Parameters.AddWithValue("@status", cboStatus.Text);
                                cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                                cm.Parameters.AddWithValue("@price", txtPrice.Text);
                                cm.Parameters.AddWithValue("@picture", bytImage);
                                cm.Parameters.AddWithValue("@barcode_picture", BarcodeImage);
                                cm.ExecuteNonQuery();
                                cn.Close();
                                MessageBox.Show("Product Added Successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                getAllProductRecord();
                                Clear();
                                generateProductCode();
                                picBarcode.Image = null;
                            }
                        }
                    }
                }
            }
            else if (btnSave.Text == "Update")
            {
                if (MessageBox.Show("Update this Product? Click Yes to continue", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Validating Form
                    if (txtProductCode.Text == String.Empty || txtName.Text == String.Empty || txtBarcode.Text == String.Empty || txtDescription.Text == String.Empty)
                    {
                        MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else if (cboStatus.Text == String.Empty || cboCategory.Text == String.Empty)
                    {
                        MessageBox.Show("Please select a valid option!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        byte[] bytImage = new byte[0];
                        MemoryStream ms = new System.IO.MemoryStream();
                        Bitmap bmpImage = new Bitmap(pictureBox1.Image);

                        bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ms.Seek(0, 0);
                        bytImage = ms.ToArray();
                        ms.Close();

                        // Barcode Image
                        byte[] BarcodeImage = new byte[1];
                        MemoryStream ms1 = new System.IO.MemoryStream();
                        Bitmap bmpImage1 = new Bitmap(picBarcode.Image);

                        bmpImage1.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ms1.Seek(0, 0);
                        BarcodeImage = ms1.ToArray();
                        ms1.Close();

                        // if email address and username does not exist in database, it will save the user's data into the database
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("UPDATE tblproduct SET barcode=@barcode,pname=@pname,category=@category,description=@description,status=@status,quantity=@quantity,price=@price,picture=@picture,barcode_picture=@barcode_picture WHERE code=@code ", cn);
                        cm.Parameters.AddWithValue("@code", txtProductCode.Text);
                        cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                        cm.Parameters.AddWithValue("@pname", txtName.Text);
                        cm.Parameters.AddWithValue("@category", cboCategory.Text);
                        cm.Parameters.AddWithValue("@description", txtDescription.Text);
                        cm.Parameters.AddWithValue("@status", cboStatus.Text);
                        cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                        cm.Parameters.AddWithValue("@price", txtPrice.Text);
                        cm.Parameters.AddWithValue("@picture", bytImage);
                        cm.Parameters.AddWithValue("@barcode_picture", BarcodeImage);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Product Updated Successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        getAllProductRecord();
                        Clear();
                        generateProductCode();
                        picBarcode.Image = null;


                    }
                }
            }
        }

        private void getAllProductRecord()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct ORDER BY pname ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["pname"].ToString(), dr["category"].ToString(), dr["barcode"].ToString(), dr["quantity"].ToString(), dr["price"].ToString(), dr["status"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            } 
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int myrand = rand.Next(100000, 999999999);
            
            string barcode = myrand.ToString();
            txtBarcode.Text = barcode.ToString();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "image files(*.bmp;*.jpg;*.png;*.gif)|*.bmp*;*.jpg;*.png;*.gif;";

            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox1.InitialImage;
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            string barCode = txtBarcode.Text;
            try
            {
                Zen.Barcode.Code128BarcodeDraw brCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                picBarcode.Image = brCode.Draw(barCode, 60);
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            pcode = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            if (ColName == "ColEdit")
            {
                btnSave.Text = "Update";

                cn.Close();
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code = @code", cn);
                cm.Parameters.AddWithValue("@code", pcode);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    txtProductCode.Text = dr["code"].ToString();
                    txtName.Text = dr["pname"].ToString();
                    cboCategory.Text = dr["category"].ToString();
                    txtPrice.Text = dr["price"].ToString();
                    txtQuantity.Text = dr["quantity"].ToString();
                    txtDescription.Text = dr["description"].ToString();
                    txtBarcode.Text = dr["barcode"].ToString();
                    cboStatus.Text = dr["status"].ToString();

                    byte[] data = (byte[])dr["picture"];
                    MemoryStream ms = new MemoryStream(data);
                    pictureBox1.Image = Image.FromStream(ms);

                    byte[] data1 = (byte[])dr["barcode_picture"];
                    MemoryStream ms1 = new MemoryStream(data1);
                    picBarcode.Image = Image.FromStream(ms1);
                }

            }
            else if (ColName == "ColDelete")
            {
                if (MessageBox.Show("Remove this data? Click Yes to continue", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblproduct WHERE code = @code", cn);
                    cm.Parameters.AddWithValue("@code", pcode);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product have been removed successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getAllProductRecord();
                    Clear();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE code LIKE '" + txtSearch.Text + "%' OR pname LIKE '" + txtSearch.Text + "%' OR barcode LIKE '" + txtSearch.Text + "%' OR category LIKE '" + txtSearch.Text + "%' ORDER BY pname ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["code"].ToString(), dr["pname"].ToString(), dr["category"].ToString(), dr["barcode"].ToString(), dr["quantity"].ToString(), dr["price"].ToString(), dr["status"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //only allows numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
