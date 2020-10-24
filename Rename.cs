using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKWiiCTC2
{
    public partial class Rename : Form
    {
        public Rename()
        {
            InitializeComponent();
            foreach (MKWiiCTC2 Settings in Application.OpenForms.OfType<MKWiiCTC2>())
            {
                textBox1.Text = Settings.label2.Text;
            }
            textBox1.KeyDown += textbox1_KeyDown;
        }

        private void Rename_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please type the name");
            } else {
                foreach (MKWiiCTC2 Settings in Application.OpenForms.OfType<MKWiiCTC2>())
                {
                    if (textBox1.Text.Substring(textBox1.Text.Length - 1, 1) == " "){
                        Settings.label2.Text = textBox1.Text;
                    } else {
                        Settings.label2.Text = textBox1.Text + " ";
                    }
                }
                this.Close();
            }
        }
    }
}
