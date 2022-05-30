using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyEats
{
    public partial class Form1 : Form
    {
        public string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\EasyEats";
        public string filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\EasyEats\EasyEats.txt";
        public Form1()
        {
            InitializeComponent();
            panel1.Location = new Point(787, 145);
            panel2.Visible = false;
            panel3.Visible = false;
            panel2.Location = new Point(0, 0);
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
                File.Create(filepath).Close();
            }
            ReloadList();
            timer2.Start();
        }
        void ReloadList()
        {
            string[] readfile = File.ReadAllLines(filepath); // read the file
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            if (readfile.Length > 0) // if there is some text in the file
            {
                for (int i = 0; i < readfile.Length; i++) // loop all lines in the file
                {
                    string[] split = readfile[i].Split(';'); // split the line
                    listBox1.Items.Add(split[0] + split[1] + " $" + split[2]); //show the line on the screen
                    listBox2.Items.Add(split[0] + split[1] + " $" + split[2]);
                    listBox3.Items.Add(split[0] + split[1] + " $" + split[2]);
                }
            }
            if (selectedIndex != -1)
            {
                listBox1.SelectedIndex = selectedIndex;
            }

            //listBox2.SelectedIndex = -1;

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        string going = "left";
        private void panel1_Click(object sender, EventArgs e)
        {
            // just some animation stuff
            if (panel1.Location == new Point(787, 145))
            {
                going = "left";
                timer1.Start();

            }
            if (panel1.Location == new Point(699, 145))
            {
                going = "right";
                timer1.Start();

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // just some animation stuff
            if (going == "left")
            {
                if (panel1.Location != new Point(699, panel1.Location.Y))
                {
                    panel1.Location = new Point(panel1.Location.X - 8, panel1.Location.Y);
                }
                else
                {
                    timer1.Stop();
                }
            }
            if (going == "right")
            {
                if (panel1.Location != new Point(787, panel1.Location.Y))
                {
                    panel1.Location = new Point(panel1.Location.X + 8, panel1.Location.Y);
                }
                else
                {
                    timer1.Stop();
                }
            }
        }
        void rewriteData(Boolean AddMinus)
        {
            string[] readfile = File.ReadAllLines(filepath); // Read the file
            StreamWriter sww = new StreamWriter(filepath);
            sww.Write(""); //clears the file
            sww.Close();
            for (int i = 0; i < readfile.Length; i++) // Loop all lines
            {
                if (i != listBox1.SelectedIndex) // If it's not the line we want
                {
                    StreamWriter sw = new StreamWriter(filepath, true); // just rewrite it back
                    sw.WriteLine(readfile[i]);
                    sw.Close();
                }
                else
                {
                    if (AddMinus == true) // if we are adding
                    {
                        string[] split = readfile[i].Split(';'); // we split the line into chunks
                        split[1] = Convert.ToString((Convert.ToInt32(split[1]) + 1)); // add it together
                        StreamWriter sw = new StreamWriter(filepath, true);
                        sw.WriteLine(split[0] + ";" + split[1] + ";" + split[2]); // and rewrite it
                        sw.Close();
                    }
                    else // if we are subtracting
                    {

                        string[] split = readfile[i].Split(';'); // split the file
                        if (split[1] == "0")
                        {
                            split[1] = "1";
                        }
                        split[1] = Convert.ToString((Convert.ToInt32(split[1]) - 1)); // we subtract one from the total
                        StreamWriter sw = new StreamWriter(filepath, true);
                        sw.WriteLine(split[0] + ";" + split[1] + ";" + split[2]); // and rewrite to file
                        sw.Close();


                    }
                }

            }
            ReloadList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Add button

            rewriteData(true);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Minus button
            rewriteData(false);
        }

        private void listBox1_Leave(object sender, EventArgs e)
        {
            if (selectedIndex != -1)
            {
                listBox1.SelectedIndex = selectedIndex;
            }
        }
        public int selectedIndex;
        public int selectedIndex2;
        private void timer2_Tick(object sender, EventArgs e)
        {

            selectedIndex = listBox1.SelectedIndex;


            selectedIndex2 = listBox2.SelectedIndex;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Finish button
            panel3.Visible = true;
            FindMoney();

        }
        void FindMoney()
        {
            double subtotal = 0;
            string[] readlines = File.ReadAllLines(filepath);
            for (int i = 0; i < readlines.Length; i++)
            {
                string[] line = readlines[i].Split(';');
                subtotal += Convert.ToDouble(line[2]);
            }
            label3.Text = "Sub-Total: $" + subtotal;

            double total = subtotal;
            total = Math.Round(total + (total * (Convert.ToDouble(textBox2.Text)/100)), 2);
            label5.Text = "Total: $" + total;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Edit mode button
            going = "right";
            timer1.Start();
            panel2.Visible = true;
        }

        private void listBox2_Leave(object sender, EventArgs e)
        {
            if (selectedIndex2 != -1)
            {
                listBox2.SelectedIndex = selectedIndex2;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //add item button
            AddItem();
            
        }

        void AddItem()
        {
            if (richTextBox1.Text == "" && richTextBox1.Text == "") // if the boxes are empty
            {
                MessageBox.Show("Please fill in the boxes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // error message
                return;
            }
            double result;
            bool isnum = double.TryParse(richTextBox2.Text, out result);
            if (isnum == false) // if the money is not a number
            {
                MessageBox.Show("Please type a number (Ex. 19.99)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // error message
                return;
            }
            else
            {
                StreamWriter sw = new StreamWriter(filepath, true);
                sw.WriteLine(richTextBox1.Text + " x;0;" + richTextBox2.Text);
                sw.Close();
                ReloadList();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            // remove button (edit mode)
            string[] fileread = File.ReadAllLines(filepath);
            StreamWriter clear = new StreamWriter(filepath);
            clear.Write(""); //clear the file
            clear.Close();
            StreamWriter sw = new StreamWriter(filepath, true);
            for (int i = 0; i < fileread.Length; i++)
            {
                if (i != selectedIndex2)
                {
                    sw.WriteLine(fileread[i]);
                }
            }
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            sw.Close();
      
            ReloadList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = trackBar1.Value.ToString();
            FindMoney();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int s;
            bool result = int.TryParse(Convert.ToString(textBox2.Text), out s);
            
            if (result != true)
            {
                textBox2.Text = "0";
            }
            else
            {
                if (s > 100)
                {
                    s = 100;
                    textBox2.Text = "100";
                }
                trackBar1.Value = s;
            }
            FindMoney();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }
    }
}
