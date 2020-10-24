using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MKWiiCTC2
{

    public partial class Setting : Form
    {
        public Setting()
        { 
            InitializeComponent();
        }

        //public static string blender = "";
        public static string szs = "";
        public static string brawlbox = "";
        public static string cloud = "";
        public static string modifier = "";
        public static string lorenzi = "";
        public static string dolphin = "";

        public void button1_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("https://www.blender.org/download/");
            }
            else
            {
                openFileDialog1.Title = "Set Blender Location";
                openFileDialog1.ShowDialog();
                Properties.Settings.Default.blender = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("https://www.sketchup.com/download/all");
            }
            else
            {
                openFileDialog8.Title = "Set Sketchup Location";
                openFileDialog8.ShowDialog();
                Properties.Settings.Default.sketchup = openFileDialog8.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("http://www.chadsoft.co.uk/wiicoder/");
            }
            else
            {
                openFileDialog2.Title = "Set SZS Modifier Location";
                openFileDialog2.ShowDialog();
                Properties.Settings.Default.szs = openFileDialog2.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("https://github.com/libertyernie/brawltools/releases");
            }
            else
            {
                openFileDialog3.Title = "Set BrawlBox Location";
                openFileDialog3.ShowDialog();
                Properties.Settings.Default.brawlbox = openFileDialog3.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("http://wiki.tockdom.com/wiki/KMP_Cloud");
            }
            else
            {
                openFileDialog4.Title = "Set KMP Cloud Location";
                openFileDialog4.ShowDialog();
                Properties.Settings.Default.cloud = openFileDialog4.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("http://wiki.tockdom.com/wiki/KMP_Modifier");
            }
            else
            {
                openFileDialog5.Title = "Set KMP Modifier Location";
                openFileDialog5.ShowDialog();
                Properties.Settings.Default.modifier = openFileDialog5.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("http://wiki.tockdom.com/wiki/Lorenzi%27s_KMP_Editor");
            }
            else
            {
                openFileDialog6.Title = "Set Lorenzi's KMP Editor Location";
                openFileDialog6.ShowDialog();
                Properties.Settings.Default.lorenzis = openFileDialog6.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("https://dolphin-emu.org/download/");
            }
            else
            {
                openFileDialog7.Title = "Set Dolphin Location";
                openFileDialog7.ShowDialog();
                Properties.Settings.Default.dolphin = openFileDialog7.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("https://www.autodesk.com/developer-network/platform-technologies/fbx-converter-archives");
            }
            else
            {
                folderBrowserDialog1.Description = "Set Fbx Converter Location";
                folderBrowserDialog1.ShowDialog();
                if (File.Exists(folderBrowserDialog1.SelectedPath + @"\FbxConverter.exe"))
                {
                    Properties.Settings.Default.fbx = folderBrowserDialog1.SelectedPath;
                }
                else if (File.Exists(@"C:\Program Files\Autodesk\FBX\FBX Converter\2013.3\bin\FbxConverter.exe"))
                {
                    Properties.Settings.Default.fbx = @"C:\Program Files\Autodesk\FBX\FBX Converter\2013.3\bin\FbxConverter.exe";
                    MessageBox.Show("Wrong path given but the correct path is known and the path was fixed");
                }
                else
                {
                    MessageBox.Show("Wrong path, folder should contain a file called " + @"""FbxConverter.exe""");
                    Properties.Settings.Default.fbx = "";
                }
                Properties.Settings.Default.Save();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.model = 0;
            Properties.Settings.Default.Save();
            foreach (MKWiiCTC2 Settings in Application.OpenForms.OfType<MKWiiCTC2>())
            {
                Settings.button2.Text = "Open Blender";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.model = 1;
            Properties.Settings.Default.Save();
            foreach (MKWiiCTC2 Settings in Application.OpenForms.OfType<MKWiiCTC2>())
            {
                Settings.button2.Text = "Open Sketchup";
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.model = 2;
            Properties.Settings.Default.Save();
            foreach (MKWiiCTC2 Settings in Application.OpenForms.OfType<MKWiiCTC2>())
            {
                Settings.button2.Text = "Open 3ds Max";
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("http://www.chadsoft.co.uk/wiicoder/");
            }
            else
            {
                openFileDialog9.Title = "Set Brres Editor Location";
                openFileDialog9.ShowDialog();
                Properties.Settings.Default.brres = openFileDialog9.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                System.Diagnostics.Process.Start("https://www.google.com/search?q=3ds+max+download");
            }
            else
            {
                openFileDialog10.Title = "Set 3ds Max Location";
                openFileDialog10.ShowDialog();
                Properties.Settings.Default.dsmax = openFileDialog10.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.help == 1)
            {
                Properties.Settings.Default.help = 0;
                Properties.Settings.Default.Save();
                foreach (MKWiiCTC2 Settings1 in Application.OpenForms.OfType<MKWiiCTC2>())
                {
                    Settings1.button19.Visible = true;
                    Settings1.button20.Visible = true;
                    Settings1.button21.Visible = true;
                    Settings1.button22.Visible = true;
                    Settings1.button23.Visible = true;
                }
            }
            else if (Properties.Settings.Default.help == 0)
            {
                Properties.Settings.Default.help = 1;
                Properties.Settings.Default.Save();
                foreach (MKWiiCTC2 Settings2 in Application.OpenForms.OfType<MKWiiCTC2>())
                {
                    Settings2.button19.Visible = false;
                    Settings2.button20.Visible = false;
                    Settings2.button21.Visible = false;
                    Settings2.button22.Visible = false;
                    Settings2.button23.Visible = false;
                }
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Setting_Load(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.extra = 0;
            Properties.Settings.Default.Save();
            foreach (MKWiiCTC2 Settings in Application.OpenForms.OfType<MKWiiCTC2>())
            {
                Settings.button4.Text = "Open CMD Prompt";
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.extra = 1;
            Properties.Settings.Default.Save();
            foreach (MKWiiCTC2 Settings in Application.OpenForms.OfType<MKWiiCTC2>())
            {
                Settings.button4.Text = "Open SZS Explorer";
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}