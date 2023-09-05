using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Software
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void frmSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void btnStoreSettings_Click(object sender, EventArgs e)
        {
            this.Dispose();
            frmStoreSettings s1 = new frmStoreSettings();
            s1.ShowDialog();
        }

        private void btnBarcode_Click(object sender, EventArgs e)
        {
            this.Dispose();
            frmGenerateBarcode g1 = new frmGenerateBarcode();
            g1.ShowDialog();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            this.Dispose();
            frmChangePassword c1 = new frmChangePassword();
            c1.ShowDialog();
        }

        private void btnBackUp_Click(object sender, EventArgs e)
        {
            this.Dispose();
            frmBackUpDatabase b1 = new frmBackUpDatabase();
            b1.ShowDialog();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            this.Dispose();
            frmRestoreDatabase r1 = new frmRestoreDatabase();
            r1.ShowDialog();
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            this.Dispose();
            frmOverAllReport o1 = new frmOverAllReport();
            o1.ShowDialog();
        }
    }
}
