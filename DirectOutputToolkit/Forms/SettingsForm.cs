﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputToolkit
{
    public partial class SettingsForm : Form
    {
        public Settings Settings { get; set; } = null;

        public SettingsForm()
        {
            InitializeComponent();
        }

    }
}
