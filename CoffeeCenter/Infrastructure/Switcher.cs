﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CoffeeCenter.Infrastructure
{
    public class Switcher
    {
        public static INavigate Content { get; set; }
        public static void Switch(UserControl control)
        {
            Content.Navigate(control);
        }
    }
}
