using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoardGameNetwork
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(string header, string message)
        {
            InitializeComponent();
            this.Text = header;
            this.errorLabel.Text = message;
            this.errorLabel.Location = new Point((this.Width - 2*SystemInformation.BorderSize.Width - errorLabel.Width)/2, errorLabel.Location.Y);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
