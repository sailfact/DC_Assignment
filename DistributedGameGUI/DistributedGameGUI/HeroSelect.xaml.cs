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
        private Hero m_hero;

        public Hero Hero { get { return m_hero; } }

        public HeroSelect(List<Hero> heroes)
        {
            m_heroes = heroes;
            InitializeComponent();
            this.DataContext = m_heroes;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IvwHeroes.ItemsSource = m_heroes;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_hero = (Hero)IvwHeroes.SelectedItem;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
