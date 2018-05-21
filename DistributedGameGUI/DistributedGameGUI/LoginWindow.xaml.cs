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
        private string m_username;
        private string m_password;

        public string Username { get { return m_username; } }
        public string Password { get { return m_password; } }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_button_Click(object sender, RoutedEventArgs e)
        {
            m_username = usernameBox.Text;
            m_password = passwordBox.Password;
            DialogResult = true;
            this.Close();
        }
    }
}
