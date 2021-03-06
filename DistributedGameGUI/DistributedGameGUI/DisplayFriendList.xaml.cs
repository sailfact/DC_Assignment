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
        private FriendList m_friendList;
        public DisplayFriendList(FriendList friends)
        {
            m_friendList = friends;
            InitializeComponent();
        }

        /// <summary>
        /// Window_Loaded
        /// binds friends list to item source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IvwFriendList.ItemsSource = m_friendList.Friends;
        }
    }
}
