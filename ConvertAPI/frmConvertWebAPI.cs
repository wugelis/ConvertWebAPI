using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConvertAPI
{
    public partial class frmConvertWebAPI : Form
    {
        private string _csContentSource = string.Empty;
        public frmConvertWebAPI()
        {
            InitializeComponent();
        }

        public frmConvertWebAPI(string csContentSource)
        {
            InitializeComponent();

            _csContentSource = csContentSource;
            csTextSource.Text = _csContentSource;
        }

        public void SetTargetCsSource(string csContentTarget)
        {
            csTextTarget.Text = csContentTarget;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
