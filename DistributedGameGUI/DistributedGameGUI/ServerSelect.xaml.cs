﻿using DistributedGamePortal;
using System;
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
    /// Interaction logic for ServerSelect.xaml
    /// </summary>
    public partial class ServerSelect : Window
    {
        private List<Server> m_serverList;
        private Server m_server;

        public Server Server { get { return m_server; } }

        public ServerSelect(List<Server> serverList)
        {
            m_serverList = serverList;
            InitializeComponent();
        }

        /// <summary>
        /// Window_Loaded
        /// binds the server list to the list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IvwServer.ItemsSource = m_serverList;
        }

        /// <summary>
        /// BtnOk_Click
        /// sets dialog result to true then closes window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;           
            this.Close();
        }

        /// <summary>
        /// BtnClose_Click
        /// sets dialog result to false 
        /// then closes window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;           
            this.Close();
        }

        /// <summary>
        /// IvwServer_SelectionChanged
        /// event handler for when server is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IvwServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_server = (Server)e.AddedItems[0];
        }
    }
}
