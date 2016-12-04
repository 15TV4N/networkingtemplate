using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BoardGameNetwork
{
    public partial class ChooseNickForm : Form
    {
        public string nick;

        public ChooseNickForm()
        {
            InitializeComponent();
            nick = String.Empty;
            nickBox.Text = "User" + new Random().Next(100, 999);
            this.DialogResult = DialogResult.Abort;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.nick = nickBox.Text;
            if (nick.Length < 5) (new Thread(()=>Application.Run(new ErrorForm("Error", "Chosen nick is too short")))).Start();
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void nickBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsLetterOrDigit(e.KeyChar) &&
                e.KeyChar != '_' &&
                e.KeyChar != '-')

                e.Handled = true;
        }
    }
}
