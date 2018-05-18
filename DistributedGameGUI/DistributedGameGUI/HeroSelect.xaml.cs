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

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_hero = (Hero)e.AddedItems[0];
        }

        public Hero GetHero()
        {
            return m_hero;
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
