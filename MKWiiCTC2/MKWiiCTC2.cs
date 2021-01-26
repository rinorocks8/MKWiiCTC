using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace MKWiiCTC2
{
    public partial class MKWiiCTC2 : Form
    {
        List<string> list = new List<string> { };
        List<string> imglist = new List<string> { };

        public ContextMenuStrip fruitContextMenuStrip;

        System.String path = @AppDomain.CurrentDomain.BaseDirectory;
        int kmp = 0;

        //string currentcourse = "None Selected";
        public string currentcourse = Properties.Settings.Default.last;
        public string filesfolder;


        public MKWiiCTC2()
        {
            InitializeComponent();

            start();

            //label3.Text = "None Selected";
            if (Properties.Settings.Default.last.Equals("None Selected"))
            {
                label3.Text = Properties.Settings.Default.last;
            }
            else
            {
                label3.Text = Properties.Settings.Default.last.Substring(0, currentcourse.Length - 2);
            }

            // Create a new ContextMenuStrip control.
            fruitContextMenuStrip = new ContextMenuStrip();

            // Attach an event handler for the 
            // ContextMenuStrip control's Opening event.
            fruitContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(cms_Opening);

            // Create a new MenuStrip control and add a ToolStripMenuItem.
            MenuStrip ms = new MenuStrip();
            ToolStripMenuItem fruitToolStripMenuItem = new ToolStripMenuItem("Courses", null, null, "Courses");
            ms.Items.Add(fruitToolStripMenuItem);

            ToolStripMenuItem fruitToolStripMenuItem1 = new ToolStripMenuItem("Settings", null, null, "Settings");
            ms.Items.Add(fruitToolStripMenuItem1);

            ms.BackColor = System.Drawing.ColorTranslator.FromHtml("#3F79AB");
            ms.ForeColor = System.Drawing.Color.White;

            //what to do when settings is clicked
            ms.Items[1].Click += new EventHandler(settings);

            // Dock the MenuStrip control to the top of the form.
            ms.Dock = DockStyle.Top;

            // Assign the MenuStrip control as the 
            // ToolStripMenuItem's DropDown menu.
            fruitToolStripMenuItem.DropDown = fruitContextMenuStrip;

            // Add the MenuStrip control last.
            // This is important for correct placement in the z-order.
            this.Controls.Add(ms);

            if (Properties.Settings.Default.extra == 0)
            {
                button4.Text = "Open CMD Promt";
            }
            else if (Properties.Settings.Default.extra == 1)
            {
                button4.Text = "Open SZS Explorer";
            }

            if (Properties.Settings.Default.model == 0)
            {
                button2.Text = "Open Blender";
            } else if (Properties.Settings.Default.model == 1)
            {
                button2.Text = "Open Sketchup";
            } else if (Properties.Settings.Default.model == 2)
            {
                button2.Text = "Open 3ds Max";
            }

            //display help button
            if (Properties.Settings.Default.help == 0)
            {
                button19.Visible = true;
                button20.Visible = true;
                button21.Visible = true;
                button22.Visible = true;
                button23.Visible = true;
            }
            else if (Properties.Settings.Default.help == 1)
            {
                button19.Visible = false;
                button20.Visible = false;
                button21.Visible = false;
                button22.Visible = false;
                button23.Visible = false;
            }

            AutoScaleMode = AutoScaleMode.None;

            // Sets the initial size of the variables
            initialWidth = Width;
            initialHeight = Height;

            initialFontSize1 = label1.Font.Size;
            label1.Resize += label1_Resize;

            initialFontSize4 = label2.Font.Size;
            label2.Resize += label2_Resize;

            initialFontSize2 = label3.Font.Size + 2;
            label3.Resize += label3_Resize;

            initialFontSize3 = button2.Font.Size + 2;
            button2.Resize += button2_Resize;

            initialFontSize5 = label4.Font.Size;
            label4.Resize += label4_Resize;

            label2.MouseDown += new MouseEventHandler(label2_MouseDown);

            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

            if (filesAndDirs.Length <= 1)
            {
                foreach (DirectoryInfo foundDir in filesAndDirs)
                {
                    string fullName = foundDir.FullName;
                    Console.WriteLine(fullName);
                    filesfolder = fullName;
                }
            }
            else
            {
                MessageBox.Show("Unable to identify files folder");
            }
            if (currentcourse != "None Selected")
            {
                label2.Text = filesfolder.Substring(path.Length, filesfolder.IndexOf("(" + currentcourse.Substring(0, currentcourse.Length - 2) + ")") - path.Length);
            }
        }

        public int initialWidth;
        public int initialHeight;

        public float initialFontSize1;
        public float initialFontSize2;
        public float initialFontSize3;
        public float initialFontSize4;
        public float initialFontSize5;

        private void label1_Resize(object sender, EventArgs e)
        {
            SuspendLayout();
            // Get the proportionality of the resize
            float proportionalNewWidth = (float)Width / initialWidth;
            float proportionalNewHeight = (float)Height / initialHeight;

            // Calculate the current font size
            label1.Font = new Font(label1.Font.FontFamily, initialFontSize1 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), label1.Font.Style);
            ResumeLayout();
        }

        private void label4_Resize(object sender, EventArgs e)
        {
            SuspendLayout();
            // Get the proportionality of the resize
            float proportionalNewWidth = (float)Width / initialWidth;
            float proportionalNewHeight = (float)Height / initialHeight;

            // Calculate the current font size
            label4.Font = new Font(label4.Font.FontFamily, initialFontSize5 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), label4.Font.Style);
            checkBox1.Font = new Font(checkBox1.Font.FontFamily, initialFontSize5 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), checkBox1.Font.Style);
            numericUpDown1.Font = new Font(numericUpDown1.Font.FontFamily, initialFontSize5 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), numericUpDown1.Font.Style);
            ResumeLayout();
        }

        private void label2_Resize(object sender, EventArgs e)
        {
            SuspendLayout();
            // Get the proportionality of the resize
            float proportionalNewWidth = (float)Width / initialWidth;
            float proportionalNewHeight = (float)Height / initialHeight;

            // Calculate the current font size
            label2.Font = new Font(label2.Font.FontFamily, initialFontSize4 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), label2.Font.Style);
            ResumeLayout();
        }

        private void label3_Resize(object sender, EventArgs e)
        {
            SuspendLayout();
            float proportionalNewWidth = (float)Width / initialWidth;
            float proportionalNewHeight = (float)Height / initialHeight;
            label3.Font = new Font(label3.Font.FontFamily, (initialFontSize2) * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), label3.Font.Style);
            ResumeLayout();
        }

        private void button2_Resize(object sender, EventArgs e)
        {
            SuspendLayout();
            float proportionalNewWidth = (float)Width / initialWidth;
            float proportionalNewHeight = (float)Height / initialHeight;
            button2.Font = new Font(button2.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button2.Font.Style);
            button18.Font = new Font(button18.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button18.Font.Style);
            button4.Font = new Font(button4.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button4.Font.Style);
            button3.Font = new Font(button3.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button3.Font.Style);
            button1.Font = new Font(button1.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button1.Font.Style);
            button5.Font = new Font(button5.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button5.Font.Style);
            button9.Font = new Font(button9.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button9.Font.Style);
            comboBox2.Font = new Font(button9.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button9.Font.Style);
            button14.Font = new Font(button14.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button14.Font.Style);
            button6.Font = new Font(button6.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button6.Font.Style);
            button10.Font = new Font(button10.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button10.Font.Style);
            button7.Font = new Font(button7.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button7.Font.Style);
            button16.Font = new Font(button16.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button16.Font.Style);
            button15.Font = new Font(button15.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button15.Font.Style);
            button11.Font = new Font(button11.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button11.Font.Style);
            button12.Font = new Font(button12.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button12.Font.Style);
            button13.Font = new Font(button13.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button13.Font.Style);
            button8.Font = new Font(button8.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button8.Font.Style);
            button17.Font = new Font(button17.Font.FontFamily, initialFontSize3 * (proportionalNewWidth > proportionalNewHeight ? proportionalNewHeight : proportionalNewWidth), button17.Font.Style);
            ResumeLayout();
        }

        void cms_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Acquire references to the owning control and item.
            Control c = fruitContextMenuStrip.SourceControl as Control;
            ToolStripDropDownItem tsi = fruitContextMenuStrip.OwnerItem as ToolStripDropDownItem;

            // Clear the ContextMenuStrip control's Items collection.
            fruitContextMenuStrip.Items.Clear();

            // Set Cancel to false. 
            // It is optimized to true based on empty entry.
            e.Cancel = false;

            ToolStripMenuItem addmenu;
            addmenu = new ToolStripMenuItem();
            addmenu.Text = "Create a new course";

            addmenu.DropDownItems.Add("Slots");                                      //0
            addmenu.DropDownItems.Add("-");                                          //1
            addmenu.DropDownItems.Add("Import SZS");                                 //2
            addmenu.DropDownItems.Add("-");                                          //3

            if (Directory.Exists(path + @"/CustomTrackFiles/beginner_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("beginner_course (Luigi Circuit)");            //4
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/farm_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("farm_course (Moo Moo Meadows)");              //5
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/kinoko_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("kinoko_course (Mushroom Gorge)");             //6
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/factory_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("factory_course (Toads Factory)");             //7
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/castle_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("castle_course (Mario Circuit)");              //8
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/shopping_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("shopping_course (Coconut Mall)");             //9
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/boardcross_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("boardcross_course (DK's Snowboard Cross)");   //10
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/truck_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("truck_course (Wario's Gold Mine)");           //11
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/senior_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("senior_course (Daisy Circuit)");              //12
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/water_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("water_course (Koopa Cape)");                  //13
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/treehouse_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("treehouse_course (Maple Treeway)");           //14
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/volcano_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("volcano_course (Grumble Volcano)");           //15
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/desert_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("desert_course (Dry Dry Ruins)");              //16
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/ridgehighway_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("ridgehighway_course (Moonview Highway)");     //17
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/koopa_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("koopa_course (Bowser's Castle)");             //18
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/rainbow_course.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("rainbow_course (Rainbow Road)");              //19
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_peach_gc.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_peach_gc (GCN Peach Beach)");             //20
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_falls_ds.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_falls_ds (DS Yoshi Falls)");              //21
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_obake_sfc.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_obake_sfc (DS Yoshi Falls)");             //22
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_mario_64.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_mario_64 (N64 Mario Raceway)");           //23
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_sherbet_64.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_sherbet_64 (N64 Mario Raceway)");         //24
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_heyho_gba.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_heyho_gba (N64 Mario Raceway)");          //25
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_town_ds.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_town_ds (DS Delfino Square)");            //26
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_waluigi_gc.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_waluigi_gc (GCN Waluigi Stadium)");       //27
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_desert_ds.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_desert_ds (DS Desert Hills)");            //28
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_koopa_gba.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_koopa_gba (GBA Bowser Castle)");          //29
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_donkey_64.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_donkey_64 (N64 DK's Jungle Parkway)");    //30
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_mario_gc.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_mario_gc (GCN Mario Circuit)");           //31
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_mario_sfc.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_mario_sfc (SNES Mario Circuit 3)");       //32
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_garden_ds.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_garden_ds (DS Peach Gardens)");           //33
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_donkey_gc.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_donkey_gc (NGC DK Mountain)");            //34
            }

            if (Directory.Exists(path + @"/CustomTrackFiles/old_koopa_64.d") == true)
            {
                addmenu.DropDownItems.Add("");            //4
            }
            else
            {
                addmenu.DropDownItems.Add("old_koopa_64 (N64 Bowser Castle)");           //35
            }

            ToolStripMenuItem removemenu;
            removemenu = new ToolStripMenuItem();
            removemenu.Text = "Remove a course";
            for (int i = 0; i < list.Count; i++)
            {
                removemenu.DropDownItems.Add(list[i]);
            }

            removemenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(removemenu_Click);

            // Populate the ContextMenuStrip control with its default items.
            fruitContextMenuStrip.Items.Add(addmenu);
            fruitContextMenuStrip.Items.Add(removemenu);
            fruitContextMenuStrip.Items.Add("-");

            fruitContextMenuStrip.ShowItemToolTips = true;


            /*//fills the menu with the items in list
            for (int i = 0; i < list.Count; i++)
            {
                fruitContextMenuStrip.Items.Add(list[i]);
            }*/

            for (int i = 0; i < list.Count; i++)
            {
                if (list[0] != "None")
                {
                    DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
                    FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + list[i].Substring(0, list[i].Length - 2) + "*");

                    if (filesAndDirs.Length <= 1)
                    {
                        foreach (DirectoryInfo foundDir in filesAndDirs)
                        {
                            string fullName = foundDir.FullName;
                            Console.WriteLine(fullName);
                            filesfolder = fullName;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to identify files folder");
                    }
                    fruitContextMenuStrip.Items.Add(filesfolder.Substring(path.Length, filesfolder.IndexOf("(" + list[i] + ")") - path.Length) + "(" + list[i] + ")");
                } else {
                    fruitContextMenuStrip.Items.Add(list[i]);
                }
            }

            addmenu.DropDownItems[2].Click += new EventHandler(import_course);
            addmenu.DropDownItems[4].Click += new EventHandler(beginner_course);
            addmenu.DropDownItems[5].Click += new EventHandler(farm_course);
            addmenu.DropDownItems[6].Click += new EventHandler(kinoko_course);
            addmenu.DropDownItems[7].Click += new EventHandler(factory_course);
            addmenu.DropDownItems[8].Click += new EventHandler(castle_course);
            addmenu.DropDownItems[9].Click += new EventHandler(shopping_course);
            addmenu.DropDownItems[10].Click += new EventHandler(boardcross_course);
            addmenu.DropDownItems[11].Click += new EventHandler(truck_course);
            addmenu.DropDownItems[12].Click += new EventHandler(senior_course);
            addmenu.DropDownItems[13].Click += new EventHandler(water_course);
            addmenu.DropDownItems[14].Click += new EventHandler(treehouse_course);
            addmenu.DropDownItems[15].Click += new EventHandler(volcano_course);
            addmenu.DropDownItems[16].Click += new EventHandler(desert_course);
            addmenu.DropDownItems[17].Click += new EventHandler(ridgehighway_course);
            addmenu.DropDownItems[18].Click += new EventHandler(koopa_course);
            addmenu.DropDownItems[19].Click += new EventHandler(rainbow_course);
            addmenu.DropDownItems[20].Click += new EventHandler(old_peach_gc);
            addmenu.DropDownItems[21].Click += new EventHandler(old_falls_ds);
            addmenu.DropDownItems[22].Click += new EventHandler(old_obake_sfc);
            addmenu.DropDownItems[23].Click += new EventHandler(old_mario_64);
            addmenu.DropDownItems[24].Click += new EventHandler(old_sherbet_64);
            addmenu.DropDownItems[25].Click += new EventHandler(old_heyho_gba);
            addmenu.DropDownItems[26].Click += new EventHandler(old_town_ds);
            addmenu.DropDownItems[27].Click += new EventHandler(old_waluigi_gc);
            addmenu.DropDownItems[28].Click += new EventHandler(old_desert_ds);
            addmenu.DropDownItems[29].Click += new EventHandler(old_koopa_gba);
            addmenu.DropDownItems[30].Click += new EventHandler(old_donkey_64);
            addmenu.DropDownItems[31].Click += new EventHandler(old_mario_gc);
            addmenu.DropDownItems[32].Click += new EventHandler(old_mario_sfc);
            addmenu.DropDownItems[33].Click += new EventHandler(old_garden_ds);
            addmenu.DropDownItems[34].Click += new EventHandler(old_donkey_gc);
            addmenu.DropDownItems[35].Click += new EventHandler(old_koopa_64);

            fruitContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(fruitContextMenuStrip_ItemClicked);
        }

        void fruitContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if ((e.ClickedItem != fruitContextMenuStrip.Items[0]) && (e.ClickedItem != fruitContextMenuStrip.Items[1]) && (e.ClickedItem != fruitContextMenuStrip.Items[2]))
            {
                if (e.ClickedItem.Text.Equals("None"))
                {
                    currentcourse = "None Selected";
                    label3.Text = "None Selected";

                    Properties.Settings.Default.last = "None Selected";
                    Properties.Settings.Default.Save();

                    this.Update();
                }
                else
                {
                    currentcourse = e.ClickedItem.Text.Substring(e.ClickedItem.Text.IndexOf("(") + 1, e.ClickedItem.Text.Length - e.ClickedItem.Text.IndexOf("(") - 2) + ".d";

                    Properties.Settings.Default.last = currentcourse;
                    Properties.Settings.Default.Save();

                    DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
                    FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

                    if (filesAndDirs.Length <= 1)
                    {
                        foreach (DirectoryInfo foundDir in filesAndDirs)
                        {
                            string fullName = foundDir.FullName;
                            Console.WriteLine(fullName);
                            filesfolder = fullName;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to identify files folder");
                    }

                    label3.Text = e.ClickedItem.Text.Substring(e.ClickedItem.Text.IndexOf("(") + 1, e.ClickedItem.Text.Length - e.ClickedItem.Text.IndexOf("(") - 2);
                    label2.Text = filesfolder.Substring(path.Length, filesfolder.IndexOf("(" + currentcourse.Substring(0, currentcourse.Length - 2) + ")") - path.Length);
                    this.Update();
                }
            }
        }

        public void start()
        {
            //startup
            if (!Directory.Exists(path + @"/CustomTrackFiles/"))
            {
                Directory.CreateDirectory(path + @"/CustomTrackFiles/");
            }

            File.WriteAllBytes(path + @"file.zip", Properties.Resources.file);
            using (ZipFile zip4 = ZipFile.Read(path + @"file.zip"))
            {
                // here, we extract every entry, but we could extract conditionally
                // based on entry name, size, date, checkbox status, etc.  
                foreach (ZipEntry b in zip4)
                {
                    b.Extract(path, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            File.Delete(path + @"file.zip");

            String[] item = { "beginner_course", "farm_course", "kinoko_course", "factory_course", "castle_course", "shopping_course", "boardcross_course", "truck_course", "senior_course", "water_course", "treehouse_course", "volcano_course", "desert_course", "ridgehighway_course", "koopa_course", "rainbow_course", "old_peach_gc", "old_falls_ds", "old_obake_sfc", "old_mario_64", "old_sherbet_64", "old_heyho_gba", "old_town_ds", "old_waluigi_gc", "old_desert_ds", "old_koopa_gba", "old_donkey_64", "old_mario_gc", "old_mario_sfc", "old_garden_ds", "old_donkey_gc", "old_koopa_64" };
            /*Properties.Settings.Default.courses.Split =
            string[] courses = Properties.Settings.Default.courses.Split(',');
            list.Clear();
            list.AddRange(courses);
            */
            //adds unknown tracks to list
            foreach (string a in item)
            {
                if (Directory.Exists(path + @"/CustomTrackFiles/" + a + @".d"))
                {
                    DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
                    FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + a + "*");

                    if (filesAndDirs.Length == 1)
                    {
                        foreach (DirectoryInfo foundDir in filesAndDirs)
                        {
                            string fullName = foundDir.FullName;
                            Console.WriteLine(fullName);
                            filesfolder = fullName;
                        }
                    } else if (filesAndDirs.Length == 0) {
                        Directory.CreateDirectory(path + @"\rename me (" + a + ")");
                        filesfolder = path + @"\rename me (" + a + ")";
                    }
                    else
                    {
                        MessageBox.Show("Unable to identify files folder");
                    }

                    if (!list.Contains(a))
                    {
                        list.Add(a);
                        convert();
                        /* adds missing files
                        File.WriteAllBytes(path + @"CustomTrackFiles\moving.zip", Properties.Resources.moving);
                        using (ZipFile zip3 = ZipFile.Read(path + @"CustomTrackFiles\moving.zip"))
                        {
                            // here, we extract every entry, but we could extract conditionally
                            // based on entry name, size, date, checkbox status, etc.  
                            foreach (ZipEntry b in zip3)
                            {
                                b.Extract(path + @"CustomTrackFiles\" + a + ".d", ExtractExistingFileAction.OverwriteSilently);
                            }
                        }
                        File.Delete(path + @"CustomTrackFiles\moving.zip");
                        */
                    }
                }
            }

            //removes unknown tracks from list
            foreach (string a in item)
            {
                if (!Directory.Exists(path + @"/CustomTrackFiles/" + a + @".d"))
                {
                    if (list.Contains(a))
                    {
                        list.Remove(a);
                    }
                }
            }

            //cant selected unknown courses
            if (!list.Contains(Properties.Settings.Default.last.Substring(0, currentcourse.Length - 2)) & Properties.Settings.Default.last.Substring(0, currentcourse.Length) != "None Selected")
            {
                currentcourse = "None Selected";

                Properties.Settings.Default.last = "None Selected";
                Properties.Settings.Default.Save();

                label3.Text = "None Selected";
                this.Update();
            }

            if (list.Count == 0)
            {
                list.Add("None");
            }
        }

        private void removemenu_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

            if (filesAndDirs.Length <= 1)
            {
                foreach (DirectoryInfo foundDir in filesAndDirs)
                {
                    string fullName = foundDir.FullName;
                    Console.WriteLine(fullName);
                    filesfolder = fullName;
                }
            }
            else
            {
                MessageBox.Show("Unable to identify files folder");
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (e.ClickedItem.Text == list[i])
                {
                    if (list[i].Equals("None"))
                    {
                        return;
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete " + list[i] + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (dialogResult == DialogResult.Yes)
                        {
                            string rc = list[i];
                            if (currentcourse.Substring(0, currentcourse.Length - 2) == list[i])
                            {
                                currentcourse = "None Selected";

                                Properties.Settings.Default.last = "None Selected";
                                Properties.Settings.Default.Save();

                                label3.Text = "None Selected";
                                this.Update();
                            }
                            list.RemoveAt(i);
                            if (Directory.Exists(path + @"CustomTrackFiles\" + rc + ".d"))
                            {
                                Directory.Delete(path + @"CustomTrackFiles\" + rc + ".d", true);
                                Directory.Delete(filesfolder, true);
                            }
                            convertkeep();
                        }
                    }
                }
            }
            label2.Text = "";
        }



        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentcourse == "None Selected") {

            } else {
                if (e.Button == MouseButtons.Right)
                {

                    Rename f3 = new Rename();
                    f3.StartPosition = FormStartPosition.CenterParent;
                    f3.ShowDialog();

                    DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
                    FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

                    if (filesAndDirs.Length <= 1)
                    {
                        foreach (DirectoryInfo foundDir in filesAndDirs)
                        {
                            string fullName = foundDir.FullName;
                            Console.WriteLine(fullName);
                            filesfolder = fullName;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to identify files folder");
                    }

                    if (filesfolder != path + label2.Text + "(" + currentcourse.Substring(0, currentcourse.Length - 2) + ")")
                    {
                        System.IO.Directory.Move(filesfolder, path + label2.Text + "(" + currentcourse.Substring(0, currentcourse.Length - 2) + ")");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.courses = "None";
            Properties.Settings.Default.Save();
        }

        private void settings(object sender, EventArgs e)
        {
            Setting f2 = new Setting();
            f2.StartPosition = FormStartPosition.CenterParent;
            f2.ShowDialog();
        }

        public void convert()
        {
            //removes the "None" from the list when something else is in it
            if (list[0].Equals("None"))
            {
                list.RemoveAt(0);
            }
            Properties.Settings.Default.courses = String.Join(",", list);
            Properties.Settings.Default.Save();
        }

        public void convertkeep()
        {
            //converts for the remove and adds back the default none
            if (list.Count == 0)
            {
                list.Add("None");
            }
            Properties.Settings.Default.courses = String.Join(",", list);
            Properties.Settings.Default.Save();
        }

        public void duplicate()
        {
            /*ProcessStartInfo cmd = new ProcessStartInfo("cmd");
            cmd.RedirectStandardError = true;
            cmd.RedirectStandardInput = true;
            cmd.RedirectStandardOutput = false;
            cmd.UseShellExecute = false;
            cmd.WorkingDirectory = (path);

            Process cmdStart = new Process();
            cmdStart.StartInfo = cmd;
            cmdStart.Start();

            cmdStart.StandardInput.WriteLine(@"xcopy """ + path + @"CustomTrackFiles\default"" """ + path + @"CustomTrackFiles\moving"" /e /y /i");
            cmdStart.StandardInput.WriteLine("exit");
            cmdStart.WaitForExit();
            */

            File.WriteAllBytes(path + @"CustomTrackFiles\moving.zip", Properties.Resources.moving);
            using (ZipFile zip2 = ZipFile.Read(path + @"CustomTrackFiles\moving.zip"))
            {
                // here, we extract every entry, but we could extract conditionally
                // based on entry name, size, date, checkbox status, etc.  
                foreach (ZipEntry a in zip2)
                {
                    a.Extract(path + @"CustomTrackFiles\moving\", ExtractExistingFileAction.OverwriteSilently);
                }
            }
            File.Delete(path + @"CustomTrackFiles\moving.zip");
        }

        void label2update()
        {
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

            if (filesAndDirs.Length <= 1)
            {
                foreach (DirectoryInfo foundDir in filesAndDirs)
                {
                    string fullName = foundDir.FullName;
                    Console.WriteLine(fullName);
                    filesfolder = fullName;
                }
            }
            else
            {
                MessageBox.Show("Unable to identify files folder");
            }

            label2.Text = filesfolder.Substring(path.Length, filesfolder.IndexOf("(" + currentcourse.Substring(0, currentcourse.Length - 2) + ")") - path.Length);
        }

        private void import_course(object sender, EventArgs e)
        {
            Import f3 = new Import(this);
            f3.StartPosition = FormStartPosition.CenterParent;
            f3.ShowDialog();
        }

        private void beginner_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void farm_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void kinoko_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void factory_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void castle_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void shopping_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void boardcross_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void truck_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void senior_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void water_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void treehouse_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void volcano_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void desert_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void ridgehighway_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void koopa_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void rainbow_course(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_peach_gc(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_falls_ds(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_obake_sfc(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_mario_64(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_sherbet_64(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_heyho_gba(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_town_ds(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_waluigi_gc(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_desert_ds(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_koopa_gba(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_donkey_64(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_mario_gc(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_mario_sfc(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_garden_ds(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_donkey_gc(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void old_koopa_64(object sender, EventArgs e)
        {
            string name = System.Reflection.MethodBase.GetCurrentMethod().Name;
            if (!Directory.Exists(path + @"CustomTrackFiles\" + name + ".d"))
            {
                list.Add(name);
                convert();
                duplicate();
                System.IO.Directory.Move(path + @"CustomTrackFiles\moving\", path + @"CustomTrackFiles\" + name + ".d");
                Directory.CreateDirectory(path + @"\rename me (" + name + ")");
            }
            else
            {
                MessageBox.Show("Slot Already Used");
            }
            if (currentcourse == "None Selected")
            {
                currentcourse = name + ".d";
                label3.Text = name;
                Properties.Settings.Default.last = currentcourse;
                Properties.Settings.Default.Save();
                label2update();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //update flag
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

            if (filesAndDirs.Length <= 1)
            {
                foreach (DirectoryInfo foundDir in filesAndDirs)
                {
                    string fullName = foundDir.FullName;
                    Console.WriteLine(fullName);
                    filesfolder = fullName;
                }
            }
            else
            {
                MessageBox.Show("Unable to identify files folder");
            }

            if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else if (File.Exists(filesfolder + @"\course.obj") == false)
            {
                MessageBox.Show(@"""course.obj"" Does Not Exist");
            }
            else
            {
                ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                cmd.RedirectStandardError = true;
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = false;
                cmd.UseShellExecute = false;
                cmd.WorkingDirectory = (filesfolder + @"\");

                Process cmdStart = new Process();
                cmdStart.StartInfo = cmd;
                cmdStart.Start();

                cmdStart.StandardInput.WriteLine("wkclt cff course.obj -o");
                cmdStart.StandardInput.WriteLine("Closing in 2 Seconds");
                Thread.Sleep(2000);
                cmdStart.StandardInput.WriteLine("exit");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //update kcl
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

            if (filesAndDirs.Length <= 1)
            {
                foreach (DirectoryInfo foundDir in filesAndDirs)
                {
                    string fullName = foundDir.FullName;
                    Console.WriteLine(fullName);
                    filesfolder = fullName;
                }
            }
            else
            {
                MessageBox.Show("Unable to identify files folder");
            }

            if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else if (File.Exists(filesfolder + @"\course.obj") == false)
            {
                MessageBox.Show(@"""course.obj"" Does Not Exist");
            }
            else if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else
            {
                if (currentcourse == "None Selected")
                {
                    MessageBox.Show("Select a Course");
                }
                else
                {
                    ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                    cmd.RedirectStandardError = true;
                    cmd.RedirectStandardInput = true;
                    cmd.RedirectStandardOutput = false;
                    cmd.UseShellExecute = false;
                    cmd.WorkingDirectory = (filesfolder + @"\");

                    Process cmdStart = new Process();
                    cmdStart.StartInfo = cmd;
                    cmdStart.Start();
                    cmdStart.StandardInput.WriteLine(@"echo off");
                    StreamWriter file = new StreamWriter(filesfolder + @"\lower-walls.txt");
                    file.Write(@"
###############################
###  (c) Wiimm, 2015-12-18  ###
###############################

# Setup
@def start	= mSec()	# start time, for the status line
@def mod_count	= 0		# modification counter

# Get the value for lowering the walls.
# If 'lower' is defined as number by option --const => use it.
# Otherwise use the default of 30.
@def lower = isNumeric(lower) ? lower : 30

# Limit to walls with inclination <45 degree
# If 'degree' is defined by option --const as number >0 => use it.
# Otherwise use the default of 45 degrees.
@def degree = isNumeric(degree) && degree > 0 ? degree : 45
@def sin_degree = sin(degree) 

# Define a function to test the KCL flag for walls
@function isWall # flag
    @pdef t = $1 & 0x1f
    @return t == 0x0c || t == 0x0d || t == 0x0f || t == 0x14 || t == 0x1e || t == 0x1f
@endfunction

# Main loop: Iterate through all triangles
@for t=0;tri$n()-1
    @if isWall(tri$flag(t))
    	@def norm = tri$normal(t,0) #  get the first normal
    	@if abs(norm.y) < sin_degree
	    # it's a vertical wall -> lower the wall & increment counter
	    @def status = tri$shift(t,-vy(lower))
	    @def mod_count = mod_count+1
	@endif
    @endif
@endfor

# Print a little status line
@echo "" - "" mod_count "" of "" tri$n() "" triangles lowered by "" lower
    > "" in ""(mSec() - start) "" msec.""");
                    file.Close();

                    cmdStart.StandardInput.WriteLine("wkclt encode course.obj -o --kcl-script lower-walls.txt");
                    cmdStart.StandardInput.WriteLine(@"del """ + filesfolder + @"\" + @"lower-walls.txt""");
                    cmdStart.StandardInput.WriteLine(@"move /Y course.kcl """ + path + @"CustomTrackFiles\" + currentcourse + @"""");
                    cmdStart.StandardInput.WriteLine("Closing in 2 Seconds");
                    Thread.Sleep(2000);
                    cmdStart.StandardInput.WriteLine("exit");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Opens Model
            if (Properties.Settings.Default.model == 0)
            {
                button2.Text = "Open Blender";
                //Open Blender
                if (Properties.Settings.Default.blender.Equals(""))
                {
                    MessageBox.Show("Set The File Location in Settings");
                }
                else
                {
                    Process.Start(Properties.Settings.Default.blender);
                }
            }
            else if (Properties.Settings.Default.model == 1)
            {
                button2.Text = "Open Sketchup";
                //Open Sketchup
                if (Properties.Settings.Default.sketchup.Equals(""))
                {
                    MessageBox.Show("Set The File Location in Settings");
                }
                else
                {
                    Process.Start(Properties.Settings.Default.sketchup);
                }
            }
            else if (Properties.Settings.Default.model == 2)
            {
                button2.Text = "Open 3ds Max";
                //Open Sketchup
                if (Properties.Settings.Default.dsmax.Equals(""))
                {
                    MessageBox.Show("Set The File Location in Settings");
                }
                else
                {
                    Process.Start(Properties.Settings.Default.dsmax);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Open Extra
            if (Properties.Settings.Default.extra == 0)
            {
                button4.Text = "Open CMD Promt";
                //Open CMD Prompt
                Process.Start("CMD.exe", "cd " + path);
            }
            else if (Properties.Settings.Default.extra == 1)
            {
                button4.Text = "Open SZS Explorer";
                //Open SZS Explorer
                if (Properties.Settings.Default.szs.Equals(""))
                {
                    MessageBox.Show("Set The File Location in Settings");
                }
                else
                {
                    Process.Start(Properties.Settings.Default.szs);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //Open Dolphin
            if (Properties.Settings.Default.dolphin.Equals(""))
            {
                MessageBox.Show("Set The File Location in Settings");
            }
            else
            {
                Process.Start(Properties.Settings.Default.dolphin, @"/b /e " + path + @"mariokartwii.wbfs");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Open Flag File
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

            if (filesAndDirs.Length <= 1)
            {
                foreach (DirectoryInfo foundDir in filesAndDirs)
                {
                    string fullName = foundDir.FullName;
                    Console.WriteLine(fullName);
                    filesfolder = fullName;
                }
            }
            else
            {
                MessageBox.Show("Unable to identify files folder");
            }

            if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else if (File.Exists(filesfolder + @"\course.flag") == false)
            {
                MessageBox.Show(@"""course.flag"" Does Not Exist");
            }
            else if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else
            {
                Process.Start(filesfolder + @"\course.flag");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Open course_model.brres
            if (Properties.Settings.Default.brawlbox.Equals(""))
            {
                MessageBox.Show("Set The File Location in Settings");
            }
            else
            {
                if (currentcourse == "None Selected")
                {
                    MessageBox.Show("Select a Course");
                }
                else
                {
                    if (File.Exists(path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres") == false)
                    {
                        MessageBox.Show(@"""course_model.brres"" Does Not Exist");
                    }
                    else
                    {
                        Process.Start(Properties.Settings.Default.brawlbox, path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //fbx to dae
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

            if (filesAndDirs.Length <= 1)
            {
                foreach (DirectoryInfo foundDir in filesAndDirs)
                {
                    string fullName = foundDir.FullName;
                    Console.WriteLine(fullName);
                    filesfolder = fullName;
                }
            }
            else
            {
                MessageBox.Show("Unable to identify files folder");
            }

            if (Properties.Settings.Default.fbx.Equals(""))
            {
                MessageBox.Show(@"Set The File Location in Settings. Normally at C:\Program Files\Autodesk\FBX\FBX Converter\2013.3\bin");
            }
            else if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else
            {
                if (File.Exists(filesfolder + @"\course.fbx") == false)
                {
                    MessageBox.Show(@"""course.fbx"" Does Not Exist");
                }
                else
                {
                    ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                    cmd.CreateNoWindow = true;
                    cmd.RedirectStandardError = true;
                    cmd.RedirectStandardInput = true;
                    cmd.RedirectStandardOutput = false;
                    cmd.UseShellExecute = false;
                    cmd.WorkingDirectory = (Properties.Settings.Default.fbx);

                    Process cmdStart = new Process();
                    cmdStart.StartInfo = cmd;
                    cmdStart.Start();

                    cmdStart.StandardInput.WriteLine(@"echo off");
                    cmdStart.StandardInput.WriteLine(@"fbxconverter """ + filesfolder + @"\course.fbx"" """ + filesfolder + @"\course.dae""");
                    cmdStart.StandardInput.WriteLine("exit");
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //fbx to obj
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + currentcourse.Substring(0, currentcourse.Length - 2) + "*");

            if (filesAndDirs.Length <= 1)
            {
                foreach (DirectoryInfo foundDir in filesAndDirs)
                {
                    string fullName = foundDir.FullName;
                    Console.WriteLine(fullName);
                    filesfolder = fullName;
                }
            }
            else
            {
                MessageBox.Show("Unable to identify files folder");
            }

            if (Properties.Settings.Default.fbx.Equals(""))
            {
                MessageBox.Show(@"Set The File Location in Settings. Normally at C:\Program Files\Autodesk\FBX\FBX Converter\2013.3\bin");
            }
            else if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else
            {
                if (File.Exists(filesfolder + @"\course.fbx") == false)
                {
                    MessageBox.Show(@"""course.fbx"" Does Not Exist");
                }
                else
                {
                    ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                    cmd.RedirectStandardError = true;
                    cmd.CreateNoWindow = true;
                    cmd.RedirectStandardInput = true;
                    cmd.RedirectStandardOutput = false;
                    cmd.UseShellExecute = false;
                    cmd.WorkingDirectory = (Properties.Settings.Default.fbx);

                    Process cmdStart = new Process();
                    cmdStart.StartInfo = cmd;
                    cmdStart.Start();

                    cmdStart.StandardInput.WriteLine(@"echo off");
                    cmdStart.StandardInput.WriteLine(@"fbxconverter """ + filesfolder + @"\course.fbx"" """ + filesfolder + @"\convert\course.obj""");
                    cmdStart.StandardInput.WriteLine("copy /y " + @"""" + filesfolder + @"\convert\course.obj"" " + @"""" + filesfolder + @"\course.obj""");
                    cmdStart.StandardInput.WriteLine("rmdir /s /q " + @"""" + filesfolder + @"\convert\""");
                    cmdStart.StandardInput.WriteLine("exit");
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else
            {
                //create szs
                ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                cmd.RedirectStandardError = true;
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = false;
                cmd.UseShellExecute = false;
                cmd.WorkingDirectory = (path);

                Process cmdStart = new Process();
                cmdStart.StartInfo = cmd;
                cmdStart.Start();
                cmdStart.StandardInput.WriteLine(@"echo off");
                cmdStart.StandardInput.WriteLine(@"cd """ + path + @"CustomTrackFiles\""");
                cmdStart.StandardInput.WriteLine(@"wszst create " + currentcourse + @" -o");
                cmdStart.StandardInput.WriteLine("Closing in 2 Seconds");
                Thread.Sleep(2000);
                cmdStart.StandardInput.WriteLine("exit");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //szs check
            if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else
            {
                if (File.Exists(path + @"CustomTrackFiles\" + currentcourse.Substring(0, currentcourse.Length - 2) + ".szs") == false)
                {
                    MessageBox.Show(@"""" + currentcourse.Substring(0, currentcourse.Length - 2) + @".szs"" Does Not Exist");
                }
                else
                {
                    ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                    cmd.RedirectStandardError = true;
                    cmd.RedirectStandardInput = true;
                    cmd.RedirectStandardOutput = false;
                    cmd.UseShellExecute = false;
                    cmd.WorkingDirectory = (path);

                    Process cmdStart = new Process();
                    cmdStart.StartInfo = cmd;
                    cmdStart.Start();
                    cmdStart.StandardInput.WriteLine(@"echo off");
                    cmdStart.StandardInput.WriteLine(@"cd """ + path + @"CustomTrackFiles\""");
                    cmdStart.StandardInput.WriteLine(@"wszst check " + currentcourse.Substring(0, currentcourse.Length - 2) + ".szs");
                    cmdStart.StandardInput.WriteLine("Closing in 4 Seconds");
                    Thread.Sleep(4000);
                    cmdStart.StandardInput.WriteLine("exit");
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //Update Game File
            if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else
            {
                if (File.Exists(path + @"CustomTrackFiles\" + currentcourse.Substring(0, currentcourse.Length - 2) + ".szs") == false)
                {
                    MessageBox.Show(@"""" + currentcourse.Substring(0, currentcourse.Length - 2) + @".szs"" Does Not Exist");
                }
                else
                {
                    ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                    cmd.RedirectStandardError = true;
                    cmd.RedirectStandardInput = true;
                    cmd.RedirectStandardOutput = false;
                    cmd.UseShellExecute = false;
                    cmd.WorkingDirectory = (path);

                    Process cmdStart = new Process();
                    cmdStart.StartInfo = cmd;
                    cmdStart.Start();
                    cmdStart.StandardInput.WriteLine(@"echo off");
                    cmdStart.StandardInput.WriteLine("copy /Y " + path + @"CustomTrackFiles\" + currentcourse.Substring(0, currentcourse.Length - 2) + ".szs " + path + @"MKWiiGameFiles\workdir.tmp\files\Race\Course\" + currentcourse.Substring(0, currentcourse.Length - 2) + ".szs");
                    cmdStart.StandardInput.WriteLine("cd " + path);
                    cmdStart.StandardInput.WriteLine(@"del *.tmp");
                    cmdStart.StandardInput.WriteLine("wit copy MKWiiGameFiles mariokartwii.wbfs -o");
                    cmdStart.StandardInput.WriteLine("exit");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (kmp == 0)
            {
                if (Properties.Settings.Default.cloud.Equals(""))
                {
                    MessageBox.Show("Set The File Location in Settings");
                }
                else
                {
                    if (currentcourse == "None Selected")
                    {
                        MessageBox.Show("Select a Course");
                    }
                    else
                    {
                        Process.Start(Properties.Settings.Default.cloud, path + @"CustomTrackFiles\" + currentcourse + @"\course.kmp");
                    }
                }
            }
            else if (kmp == 1)
            {
                if (Properties.Settings.Default.modifier.Equals(""))
                {
                    MessageBox.Show("Set The File Location in Settings");
                }
                else
                {
                    if (currentcourse == "None Selected")
                    {
                        MessageBox.Show("Select a Course");
                    }
                    else
                    {
                        Process.Start(Properties.Settings.Default.modifier, path + @"CustomTrackFiles\" + currentcourse + @"\course.kmp");
                    }
                }
            }
            else if (kmp == 2)
            {
                if (Properties.Settings.Default.lorenzis.Equals(""))
                {
                    MessageBox.Show("Set The File Location in Settings");
                }
                else
                {
                    if (currentcourse == "None Selected")
                    {
                        MessageBox.Show("Select a Course");
                    }
                    else
                    {
                        Process.Start(Properties.Settings.Default.lorenzis, path + @"CustomTrackFiles\" + currentcourse + @"\course.kmp");
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            kmp = this.comboBox2.SelectedIndex;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //create minimap
            if (Properties.Settings.Default.brres.Equals(""))
            {
                MessageBox.Show("Set The File Location for Brres Editor in Settings");
            }
            else
            {
                Process.Start(Properties.Settings.Default.brres);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //minimap patch
            if (File.Exists(path + @"\CustomTrackFiles\" + currentcourse + @"\map_model.brres") == false)
            {
                MessageBox.Show(@"""map_model.brres"" Does Not Exist");
            }
            else
            {
                if (currentcourse == "None Selected")
                {
                    MessageBox.Show("Select a Course");
                }
                else
                {
                    ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                    cmd.RedirectStandardError = true;
                    cmd.RedirectStandardInput = true;
                    cmd.RedirectStandardOutput = false;
                    cmd.UseShellExecute = false;
                    cmd.WorkingDirectory = (path);

                    Process cmdStart = new Process();
                    cmdStart.StartInfo = cmd;
                    cmdStart.Start();
                    cmdStart.StandardInput.WriteLine(@"echo off");
                    cmdStart.StandardInput.WriteLine(@"cd """ + path + @"CustomTrackFiles\" + currentcourse + @"\""");
                    cmdStart.StandardInput.WriteLine(@"wszst minimap --auto map_model.brres");
                    cmdStart.StandardInput.WriteLine("Closing in 2 Seconds");
                    Thread.Sleep(2000);
                    cmdStart.StandardInput.WriteLine("exit");
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            //KMP Object Query
            System.Diagnostics.Process.Start("https://szs.wiimm.de/cgi/mkw/object");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //Do all
            if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else
            {
                //create szs
                ProcessStartInfo cmd = new ProcessStartInfo("cmd");
                cmd.RedirectStandardError = true;
                cmd.RedirectStandardInput = true;
                cmd.RedirectStandardOutput = false;
                cmd.UseShellExecute = false;
                cmd.WorkingDirectory = (path);

                Process cmdStart = new Process();
                cmdStart.StartInfo = cmd;
                cmdStart.Start();
                cmdStart.StandardInput.WriteLine(@"@echo off");
                cmdStart.StandardInput.WriteLine(@"cd """ + path + @"CustomTrackFiles\""");
                cmdStart.StandardInput.WriteLine(@"wszst create " + currentcourse + @" -o");

                //Update Game File
                if (currentcourse == "None Selected")
                {
                    MessageBox.Show("Select a Course");
                }
                else
                {
                    if (File.Exists(path + @"CustomTrackFiles\" + currentcourse.Substring(0, currentcourse.Length - 2) + ".szs") == false)
                    {
                        MessageBox.Show(@"""" + currentcourse.Substring(0, currentcourse.Length - 2) + @".szs"" Does Not Exist");
                    }
                    else
                    {
                        cmdStart.StandardInput.WriteLine("copy /Y " + path + @"CustomTrackFiles\" + currentcourse.Substring(0, currentcourse.Length - 2) + ".szs " + path + @"MKWiiGameFiles\workdir.tmp\files\Race\Course\" + currentcourse.Substring(0, currentcourse.Length - 2) + ".szs");
                        cmdStart.StandardInput.WriteLine("cd " + path);
                        cmdStart.StandardInput.WriteLine(@"del *.tmp");
                        cmdStart.StandardInput.WriteLine("wit copy MKWiiGameFiles mariokartwii.wbfs -o");
                        cmdStart.StandardInput.WriteLine("exit");
                        cmdStart.WaitForExit();
                        if (Properties.Settings.Default.dolphin.Equals(""))
                        {
                            MessageBox.Show("Set The File Location in Settings");
                        }
                        else
                        {
                            Process.Start(Properties.Settings.Default.dolphin, @"/b /e " + path + @"mariokartwii.wbfs");
                        }
                    }
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            } else
            {
                Process.Start(path + @"CustomTrackFiles\" + currentcourse);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        public void button19_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://wiki.tockdom.com/wiki/Creating_a_KCL_with_Wiimms_Tools");
        }

        public void button20_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://wiki.tockdom.com/wiki/Encoding_Mipmaps_with_Wiimms_Image_Tool");
        }

        public void button21_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://wiki.tockdom.com/wiki/KMP_Editing");
            System.Diagnostics.Process.Start("http://wiki.tockdom.com/wiki/Minimap");
        }

        public void button22_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://wiki.tockdom.com/wiki/Testing_a_Track");
        }

        public void button23_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://wiki.tockdom.com/wiki/Modeling_with_3D_Editor");
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click_2(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            //Add Mipmaps

            if (currentcourse == "None Selected")
            {
                MessageBox.Show("Select a Course");
            }
            else
            {
                if (File.Exists(path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres") == false)
                {
                    MessageBox.Show(@"""course_model.brres"" Does Not Exist");
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (Directory.Exists(path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres.d"))
                        {
                            Directory.Delete(path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres.d", true);
                        }

                        decimal i = numericUpDown1.Value-1;

                        StreamWriter file = new StreamWriter(path + @"CustomTrackFiles\" + currentcourse + @"\hello.bat");
                        file.Write(@"
wszst x course_model.brres
cd course_model.brres.d\Textures(NW4R)
mkdir here
wimgt decode * -d" + '\u0022' + "%~dp0/course_model.brres.d/Textures(NW4R)/here" + '\u0022' + @" --no-mipmaps
");
                        file.Close();

                        Process cmd = new Process();
                        cmd.StartInfo.WorkingDirectory = path + @"CustomTrackFiles\" + currentcourse;
                        cmd.StartInfo.FileName = "hello.bat";
                        cmd.StartInfo.CreateNoWindow = true;
                        cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        cmd.Start();
                        cmd.WaitForExit();
                        File.Delete(path + @"CustomTrackFiles\" + currentcourse + @"\hello.bat");

                        Imgs f4 = new Imgs(path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres.d\Textures(NW4R)\here\");
                        f4.StartPosition = FormStartPosition.CenterParent;
                        f4.ShowDialog();

                        string[] names = Directory.GetFiles(path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres.d\Textures(NW4R)\here\", "*.png").Select(filename => Path.GetFileNameWithoutExtension(filename)).ToArray();
                        /*
                        ProcessStartInfo cmd1 = new ProcessStartInfo("cmd");
                        cmd1.RedirectStandardError = true;
                        cmd1.CreateNoWindow = true;
                        cmd1.RedirectStandardInput = true;
                        cmd1.RedirectStandardOutput = false;
                        cmd1.UseShellExecute = false;
                        cmd1.WorkingDirectory = (path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres.d\Textures(NW4R)\");
                        Process cmdStart1 = new Process();
                        cmdStart1.StartInfo = cmd1;
                        cmdStart1.Start();
                        */

                        StreamWriter file1 = new StreamWriter(path + @"CustomTrackFiles\" + currentcourse + @"\hello.bat");
                        file1.WriteLine(@"cd " + path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres.d\Textures(NW4R)\");
                        for (int a = 0; a < names.Length; a++)
                        {
                            file1.WriteLine(@"wimgt encode " + names[a] + " --n-mm=" + i + " -o");
                        }
                        file1.WriteLine(@"cd " + path + @"CustomTrackFiles\" + currentcourse);
                        file1.WriteLine(@"wszst create course_model.brres.d -d" + '\u0022' + "%~dp0/course_model.brres" + '\u0022' + @" -o");
                        file1.WriteLine(@"rmdir /s /q """ + path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres.d\");
                        file1.Close();

                        Directory.Delete(path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres.d\Textures(NW4R)\here\", true);

                        Process cmd1 = new Process();
                        cmd1.StartInfo.WorkingDirectory = path + @"CustomTrackFiles\" + currentcourse;
                        cmd1.StartInfo.FileName = "hello.bat";
                        cmd1.StartInfo.CreateNoWindow = true;
                        cmd1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        cmd1.Start();
                        cmd1.WaitForExit();

                        File.Delete(path + @"CustomTrackFiles\" + currentcourse + @"\hello.bat");

                        //Turns Linear_Mipmap_Linear on for materials with mipmaps
                        Linear_Mipmap_Linear();
                        list.Clear();
                        imglist.Clear();
                        MessageBox.Show("Done");
                    }
                    else
                    {
                        MessageBox.Show("Please Select Confirm");
                    }
                }
            }
        }

        private void Linear_Mipmap_Linear()
        {
            EndianBinaryReader fs = new EndianBinaryReader(new FileStream(path + @"CustomTrackFiles\" + currentcourse + @"\course_model.brres", FileMode.Open, FileAccess.ReadWrite));
            
            fs.offset(0x0C);
            byte[] rootOffset = fs.readBytes(2); //offset to root from start
            fs.goTo(0x00);
            fs.offset(rootOffset); //root
            fs.offset(0x04);
            byte[] rootLength = fs.readBytes(4); //root length
            fs.goTo(rootOffset);
            fs.offset(rootLength);
            fs.seek("MDL0");
            byte[] mdl0 = fs.getLoc(); //mdl0 start

            fs.offset(0x48); //goes to name of subfile
            byte[] mdl0NameOffset = fs.readBytes(4);
            fs.goTo(mdl0);
            fs.offset(mdl0NameOffset);
            if (fs.readBytesToString(6) == "course")
            {
                fs.offset(6);
                if (fs.readBytes(4).SequenceEqual(new byte[4]))
                {
                   
                    //check mip
                    fs.goTo(mdl0);
                    fs.offset(0x04);
                    byte[] mdl0Length = fs.readBytes(4);
                    fs.goTo(mdl0);
                    fs.offset(mdl0Length);
                    while (true)
                    {
                        if (fs.readBytesToString(4) == "SRT0")
                        {
                            byte[] str0 = fs.getLoc();
                            fs.offset(0x08);
                            byte[] str0Length = fs.readBytes(4);
                            fs.goTo(str0);
                            fs.offset(str0Length);
                        }
                        else if (fs.readBytesToString(4) == "TEX0")
                        {
                            break;
                        }
                        else
                        {
                            fs.seek("TEX0");
                            break;
                        }
                    }

                    //found tex0
                    while (true)
                    {
                        if (fs.readBytesToString(4) == "TEX0")
                        {
                            byte[] tex0 = fs.getLoc();
                            fs.offset(0x24);
                            byte[] tex0Images = fs.readBytes(4);
                            //determines if it has mipmaps
                            if (BitConverter.ToInt32(tex0Images, 0) > 1)
                            {
                                //gets name of image
                                fs.goTo(tex0);
                                fs.offset(0x10);
                                fs.offset(0x04);
                                byte[] tex0NameOffset = fs.readBytes(4);
                                fs.goTo(tex0);
                                fs.offset(tex0NameOffset);
                                imglist.Add(fs.scan());
                            }
                            fs.goTo(tex0);
                            fs.offset(0x04);
                            byte[] tex0Length = fs.readBytes(4);
                            fs.goTo(tex0);
                            fs.offset(tex0Length);
                        }
                        else { 
                            break; 
                        }
                    }

                    //find start of materials
                    fs.goTo(mdl0);
                    fs.offset(0x10);
                    fs.offset(0x20);
                    byte[] matOffset = fs.readBytes(4);
                    fs.goTo(mdl0);
                    fs.offset(matOffset);
                    fs.offset(0x24);
                    byte[] firstMatOffset = fs.readBytes(4);

                    //materials #
                    fs.goTo(mdl0);
                    fs.offset(matOffset);
                    fs.offset(firstMatOffset);
                    for (uint i = 0; i >= 0; i++)
                    {
                        byte[] currentMat = fs.getLoc();
                        //check that it is a material
                        fs.offset(0x0C);
                        byte[] matIndex = fs.readBytes(4);
                        if (BitConverter.ToInt32(matIndex, 0) != i)
                        {
                            break;
                        }
                        //number of images
                        fs.goTo(currentMat);
                        fs.offset(0x2C);
                        int numImages = BitConverter.ToInt32(fs.readBytes(4), 0);

                        List<int> number = new List<int> { };

                        //logs image location in terms of layer
                        for (int bb = 0; bb < numImages; bb++)
                        {
                            fs.goTo(currentMat);
                            fs.offset(new byte[] { 0x18, 0x04 });
                            //offsets each layer
                            for (int bc = bb; bc > 0; bc--)
                            {
                                fs.offset(0x34);
                            }
                            byte[] layerNameOffset = fs.readBytes(4);
                            fs.offset(layerNameOffset);
                            if (imglist.Contains(fs.scan()))
                            {
                                number.Add(bb);
                            }
                        }

                        for (int loop = 0; loop < numImages; loop++)
                        {
                            if (number.Contains(loop))
                            {
                                fs.goTo(currentMat);
                                fs.offset(new byte[] { 0x38, 0x04 });
                                //writing in wrong spot for multiple images
                                for (int bc = loop; bc > 0; bc--)
                                {
                                    fs.offset(0x34);
                                }
                                fs.writeBytes(new byte[] {0x00, 0x00, 0x00, 0x05});
                            }
                            if (number.Contains(loop) == false)
                            {
                                fs.goTo(currentMat);
                                fs.offset(new byte[] { 0x38, 0x04 });
                                //writing in wrong spot for multiple images
                                for (int bc = loop; bc > 0; bc--)
                                {
                                    fs.offset(0x34);
                                }
                                fs.writeBytes(new byte[] {0x00, 0x00, 0x00, 0x01});
                            }
                        }
                        //reset to next material
                        fs.goTo(currentMat);
                        byte[] currentMatLength = fs.readBytes(4);
                        fs.offset(currentMatLength);
                    }
                }
                else { MessageBox.Show("Error Occured1"); }
            }
            else { MessageBox.Show("Error Occured2"); }
            fs.Close();
            //end of liner
        }
    }
}
