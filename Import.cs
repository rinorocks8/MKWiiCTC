using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MKWiiCTC2
{
    public partial class Import : Form
    {
        private MKWiiCTC2 _ParentForm;

        public Import(MKWiiCTC2 parentForm)
        {
            InitializeComponent();
            this.comboBox1.DataSource = item;
            _ParentForm = parentForm;
        }

        String[] item = { "Select a slot", "beginner_course", "farm_course", "kinoko_course", "factory_course", "castle_course", "shopping_course", "boardcross_course", "truck_course", "senior_course", "water_course", "treehouse_course", "volcano_course", "desert_course", "ridgehighway_course", "koopa_course", "rainbow_course", "old_peach_gc", "old_falls_ds", "old_obake_sfc", "old_mario_64", "old_sherbet_64", "old_heyho_gba", "old_town_ds", "old_waluigi_gc", "old_desert_ds", "old_koopa_gba", "old_donkey_64", "old_mario_gc", "old_mario_sfc", "old_garden_ds", "old_donkey_gc", "old_koopa_64" };
        System.String path = @AppDomain.CurrentDomain.BaseDirectory;
        string coursepath = "";
        string course = "";

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select SZS";
            openFileDialog1.Filter = "SZS files (*.szs)|*.szs";
            openFileDialog1.ShowDialog();
            label1.Text = openFileDialog1.FileName;
            coursepath = openFileDialog1.FileName;
            this.Update();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            course = item[this.comboBox1.SelectedIndex];
        }

        private void Import_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //import
            if (coursepath.Equals(""))
            {
                MessageBox.Show("Select a SZS file");
            }
            else if (course.Equals("Select a slot"))
            {
                MessageBox.Show("Select a valid slot");
            }
            else
            {
                if (Directory.Exists(path + @"/CustomTrackFiles/" + course + @".d"))
                {
                    MessageBox.Show("Slot already being used");
                }
                else
                {
                    if (!Directory.Exists(path + @"/temp/"))
                    {
                        Directory.CreateDirectory(path + @"/temp/");
                    }
                    File.Copy(coursepath, path + @"/temp/" + course + ".szs");

                    //decode szs
                    ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                    cmd.RedirectStandardError = true;
                    cmd.RedirectStandardInput = true;
                    cmd.RedirectStandardOutput = false;
                    cmd.UseShellExecute = false;
                    cmd.WorkingDirectory = (path + @"/temp/");
             
                    Process cmdStart = new Process();
                    cmdStart.StartInfo = cmd;
                    cmdStart.Start();
                    cmdStart.StandardInput.WriteLine(@"echo off");
                    cmdStart.StandardInput.WriteLine(@"wszst extract " + course + @".szs --dest " + path + @"/CustomTrackFiles/" + course + ".d");
                    cmdStart.StandardInput.WriteLine("Closing in 2 Seconds");
                    Thread.Sleep(2000);
                    cmdStart.StandardInput.WriteLine("exit");
                    cmdStart.WaitForExit();
                    Directory.CreateDirectory(path + @"\rename me (" + course + ")");
                    Directory.Delete(path + @"/temp/", true);
                    _ParentForm.start();
                    
                    if (_ParentForm.currentcourse == "None Selected")
                    {
                        _ParentForm.currentcourse = course + ".d";
                        Properties.Settings.Default.last = course;
                        Properties.Settings.Default.Save();
                        _ParentForm.label3.Text = course;
                        _ParentForm.Update();
                    }
                }
            }
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
