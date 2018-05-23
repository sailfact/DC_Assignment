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
using DistributedGamePortal;

namespace DistributedGameGUI
{
    /// <summary>
    /// Interaction logic for FriendList.xaml
    /// </summary>
    public partial class DisplayFriendList : Window
    {
        private List<string> m_friendList;
        public DisplayFriendList(List<string> friends)
        {
            m_friendList = friends;
            InitializeComponent();
            this.DataContext = m_friendList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IvwFriendList.ItemsSource = m_friendList;
        }
    }
}
