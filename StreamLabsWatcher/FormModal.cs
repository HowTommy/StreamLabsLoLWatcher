using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace StreamLabsWatcher
{
    public partial class FormModal : Form
    {
        private Color color;
        //private KeyHandler ghk;
        private GlobalKeyboardHook gkh;
        private Keys modKey;
        private Keys key;
        private bool keyDown = false;
        private bool modKeyDown = false;

        public FormModal(string modKey, string key, Color color)
        {
            InitializeComponent();
            this.color = color;
            //this.gkh = new GlobalKeyboardHook();

            this.key = (Keys)Enum.Parse(typeof(Keys), key, true);
            this.modKey = (Keys)Enum.Parse(typeof(Keys), modKey, true);

            //this.gkh.HookedKeys.Add(this.key);
            //this.gkh.HookedKeys.Add(this.modKey);
            //this.gkh.KeyDown += Gkh_KeyDown;
            //this.gkh.KeyUp += Gkh_KeyUp;
        }

        //private void Gkh_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if(e.KeyData == this.key)
        //    {
        //        this.keyDown = true;
        //    }
        //    else if(e.KeyData == this.modKey)
        //    {
        //        this.modKeyDown = true;
        //    }
        //    if(this.keyDown && this.modKeyDown)
        //    {
        //        this.Close();
        //    }
        //    e.Handled = true;
        //}

        //private void Gkh_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyData == this.key)
        //    {
        //        this.keyDown = false;
        //    }
        //    else if (e.KeyData == this.modKey)
        //    {
        //        this.modKeyDown = false;
        //    }
        //    e.Handled = true;
        //}

        private void FormModal_Load(object sender, EventArgs e)
        {
            this.BackColor = this.color;
            this.label1.Text = "PRESS > " + this.modKey + " + " + this.key;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (((Control.ModifierKeys & this.modKey) == this.modKey) && e.KeyCode == this.key)
            {
                this.Close();
            }
        }
    }
}
