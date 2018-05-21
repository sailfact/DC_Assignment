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
    /// Interaction logic for TakeTurn.xaml
    /// </summary>
    public partial class TakeTurn : Window
    {
        private Hero m_hero;
        private Ability m_ability;
        private int m_index;

        public Ability Ability { get { return m_ability; } }
        public int Index { get { return m_index; } }

        public TakeTurn(Hero hero)
        {
            m_hero = hero;
            m_index = -1;
            InitializeComponent();
            this.DataContext = hero.Abilities;
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IvwAbilities.ItemsSource = m_hero.Abilities;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (m_ability == null || m_index == -1)
            {
                MessageBox.Show("Please select ability and enter valid target");
            }
            else
            {
                DialogResult = true;
                this.Close();
            }
        }

        private void IvwAbilities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_ability = (Ability)e.AddedItems[0];
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_index = Convert.ToInt32(TxtBox.Text);
        }
    }
}
