using MKWiiCTC2.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKWiiCTC2
{
    public partial class Imgs : Form
    {
        ImageList images = new ImageList();
        List<string> img = new List<string>();
        string pathfile;

        public Imgs(string path)
        {
            InitializeComponent();

            pathfile = path;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#3F79AB");
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Sans Sherif", 9.75F, FontStyle.Bold);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            int width;
            int height;

            while (1 == 1)
            {
                try 
                {
                    DirectoryInfo dir1 = new DirectoryInfo(path);
                    foreach (FileInfo file in dir1.GetFiles())
                    {
                        FileStream fileStream = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.None);
                        fileStream.Dispose();
                    }
                    break;
                }
                catch (System.IO.IOException)
                {
                    Thread.Sleep(250);
                }
            }

            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles())
            {
                using (Image image = Image.FromFile(file.FullName))
                {
                    if ((image.Width * 100 / image.Height) < 100)
                    {
                        width = (image.Width * 100 / image.Height);
                        height = 100;
                    }
                    else
                    {
                        width = 100;
                        height = (image.Height * 100 / image.Width);
                    }
                    using (Image newImage = new Bitmap(image, width, height))
                    {
                        image.Dispose();
                        newImage.Save(file.FullName);
                        img.Add(file.ToString());
                    }
                }
            }
            dir.Refresh();

            foreach (FileInfo file in dir.GetFiles())
            {
                using (FileStream fileStream = File.Open(file.FullName, FileMode.Open))
                {
                    Bitmap bitmap = new Bitmap(fileStream);
                    dataGridView1.Rows.Add(bitmap);
                    img.Add(file.ToString());
                }
            }
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = img[i].Substring(0, img[i].Length - 4);
            }
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[2].Value) == false)
                {
                    File.Delete(pathfile + @"/" + img[i]);
                }
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //select all
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[2];
                chk.Value = true; //because chk.Value is initialy null
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //deselect all
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[2];
                chk.Value = false; //because chk.Value is initialy null
            }
        }
    }
}
