﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Design;

namespace LedControlToolkit
{
    public class PropertyDescriptorHandler
    {
        public bool Browsable = true;
        public string DisplayName = string.Empty;
        public string Description = string.Empty;
        public string Category = string.Empty;
        public bool ReadOnly = false;
        public Type TypeConverter = null;
        public Type TypeEditor = null;
    }
}