﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DistributedGameGUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string Username { get; set; } 
        public string Password { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Login_button_Click
        /// sets username and password
        /// when login button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_button_Click(object sender, RoutedEventArgs e)
        {
            this.Username = usernameBox.Text;
            this.Password = passwordBox.Password;
            DialogResult = true;
            this.Close();
        }
    }
}
