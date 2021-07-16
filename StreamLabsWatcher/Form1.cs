using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreamLabsWatcher
{
    public partial class Form1 : Form
    {
        private bool isRunning = false;
        private string processToWatch = "";
        private bool isProcessPresentNow = false;

        public Form1()
        {
            InitializeComponent();
        }

        private bool IsValidToStartWatching()
        {
            string modKey1 = (string)this.listBox1.SelectedItem;
            string key1 = textBox1.Text;
            string modKey2 = (string)this.listBox2.SelectedItem;
            string key2 = textBox1.Text;

            if (string.IsNullOrEmpty(this.processToWatch)
                || string.IsNullOrEmpty(modKey1)
                || string.IsNullOrEmpty(modKey2)
                || string.IsNullOrEmpty(key1)
                || string.IsNullOrEmpty(key2))
            {
                MessageBox.Show("Erreur, veuillez remplir tous les critères");
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.isRunning)
            {
                this.StopWatching();
            }
            else if (this.IsValidToStartWatching())
            {
                this.StartWatching();
            }
        }

        private bool checkIfProcessPresentNow()
        {
            Process[] processCollection = Process.GetProcesses();
            return processCollection.FirstOrDefault(p => p.ProcessName == this.processToWatch) != null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            this.listBox1.Items.Add(Keys.Control.ToString());
            this.listBox1.Items.Add(Keys.Alt.ToString());
            this.listBox1.Items.Add(Keys.Shift.ToString());

            this.listBox2.Items.Clear();
            this.listBox2.Items.Add(Keys.Control.ToString());
            this.listBox2.Items.Add(Keys.Alt.ToString());
            this.listBox2.Items.Add(Keys.Shift.ToString());

            this.LoadAndStartIfSetupRight();
        }

        private void RefreshProcessList()
        {
            this.comboBox1.Items.Clear();
            Process[] processCollection = Process.GetProcesses();
            foreach (Process p in processCollection)
            {
                this.comboBox1.Items.Add(p.ProcessName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.processToWatch))
            {
                string processName = null;
                if (!string.IsNullOrEmpty(this.comboBox1.Text))
                {
                    processName = this.comboBox1.Text;
                }
                else if (this.comboBox1.SelectedItem != null)
                {
                    processName = (string)this.comboBox1.SelectedItem;
                }


                if (processName != null)
                {
                    this.processToWatch = processName;
                    this.comboBox1.Enabled = false;
                    this.label2.Text = "Processus surveillé : " + this.processToWatch;
                    this.button2.Text = "Changer";
                }
            }
            else
            {
                this.processToWatch = "";
                this.button2.Text = "Choisir";
                this.comboBox1.Text = "";
                this.comboBox1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.KeyDown += TextBox1_KeyUp;
            this.textBox1.Enabled = true;
            this.textBox1.Focus();
        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            this.SetKey(this.textBox1, e.KeyData.ToString());
            this.textBox1.KeyUp -= TextBox1_KeyUp;
            this.textBox1.Enabled = false;
        }
        private async void SetKey(TextBox textbox, string newValue)
        {
            await Task.Delay(50);
            textbox.Text = newValue;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.textBox2.KeyDown += TextBox2_KeyUp;
            this.textBox2.Enabled = true;
            this.textBox2.Focus();
        }

        private void TextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            this.SetKey(this.textBox2, e.KeyData.ToString());
            this.textBox2.KeyUp -= TextBox2_KeyUp;
            this.textBox2.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.RefreshProcessList();
        }

        private void LoadAndStartIfSetupRight()
        {
            if (File.Exists("setup.json"))
            {
                var data = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText("setup.json", Encoding.UTF8));
                this.processToWatch = data.processName;
                this.comboBox1.Text = data.processName;
                this.listBox1.SelectedIndex = this.GetIndex(data.modKey1);
                this.textBox1.Text = data.key1;
                this.listBox2.SelectedIndex = this.GetIndex(data.modKey2);
                this.textBox2.Text = data.key2;
                this.comboBox1.Enabled = false;
                this.label2.Text = "Processus surveillé : " + this.processToWatch;
                this.button2.Text = "Changer";
                this.StartWatching();
            }
            else
            {
                this.RefreshProcessList();
            }
        }

        private int GetIndex(string modKey1)
        {
            if (modKey1 == "Control")
            {
                return 0;
            }
            return modKey1 == "Alt" ? 1 : 2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.IsValidToStartWatching())
            {
                this.SaveSettings();
            }
        }

        private void SaveSettings()
        {
            string modKey1 = (string)this.listBox1.SelectedItem;
            string key1 = textBox1.Text;
            string modKey2 = (string)this.listBox2.SelectedItem;
            string key2 = textBox2.Text;

            var data = new SaveData()
            {
                processName = this.processToWatch,
                modKey1 = modKey1,
                key1 = key1,
                modKey2 = modKey2,
                key2 = key2
            };
            File.WriteAllText("setup.json", JsonConvert.SerializeObject(data), Encoding.UTF8);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete the setup ??",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                File.Delete("setup.json");
            }
        }

        private void StopWatching()
        {
            this.isRunning = false;
            this.timer1.Stop();
            this.button1.Text = this.isRunning ? "Désactiver" : "Activer";

            this.listBox1.Enabled = true;
            this.listBox2.Enabled = true;
            this.button2.Enabled = true;
            this.button3.Enabled = true;
            this.button4.Enabled = true;
            this.button5.Enabled = true;
        }

        private void StartWatching()
        {
            this.isRunning = true;
            this.isProcessPresentNow = this.checkIfProcessPresentNow();
            this.timer1.Start();
            this.button1.Text = this.isRunning ? "Désactiver" : "Activer";

            this.listBox1.Enabled = false;
            this.listBox2.Enabled = false;
            this.button2.Enabled = false;
            this.button3.Enabled = false;
            this.button4.Enabled = false;
            this.button5.Enabled = false;
            this.HideFast();
        }

        private async void HideFast()
        {
            await Task.Delay(50);
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.IsValidToStartWatching() && this.IsStreamLabActive())
            {
                string modKey1 = (string)this.listBox1.SelectedItem;
                string key1 = textBox1.Text;
                string modKey2 = (string)this.listBox2.SelectedItem;
                string key2 = textBox2.Text;

                if (this.isProcessPresentNow && !this.checkIfProcessPresentNow())
                {
                    this.isProcessPresentNow = false;
                    var newForm = new FormModal(modKey2, key2, Color.FromArgb(255, 0, 0));
                    newForm.ShowDialog();
                }
                else if (!this.isProcessPresentNow && this.checkIfProcessPresentNow())
                {
                    this.isProcessPresentNow = true;
                    var newForm = new FormModal(modKey1, key1, Color.FromArgb(0, 255, 0));
                    newForm.ShowDialog();
                }
            }
        }

        private bool IsStreamLabActive()
        {
            Process[] processCollection = Process.GetProcesses();
            if (processCollection.FirstOrDefault(p => p.ProcessName == "Streamlabs OBS") == null)
            {
                this.timer1.Interval = 60000;
                Console.WriteLine("No streamlabs found, sleeping mode");
                return false;
            }
            else
            {
                Console.WriteLine("Streamlabs found, active mode");
                this.timer1.Interval = 5000;
                return true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
