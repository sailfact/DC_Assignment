using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DistributedGameServer;

namespace DistributedGameGUI
{
    /// <summary>
    /// Interaction logic for HeroSelect.xaml
    /// </summary>
    public partial class HeroSelect : Window
    {
        private List<Hero> m_heroes;
        public Hero Hero { get; set; }

        public HeroSelect(List<Hero> heroes)
        {
            m_heroes = heroes;
            InitializeComponent();
            this.DataContext = m_heroes;
        }

        /// <summary>
        /// Window_Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IvwHeroes.ItemsSource = m_heroes;
        }

        /// <summary>
        /// ListView_SelectionChanged
        /// event handler for when hero is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Hero = (Hero)IvwHeroes.SelectedItem;
        }

        /// <summary>
        /// BtnOk_Click
        /// sets dialog result to true 
        /// then closes window
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
    }
}
