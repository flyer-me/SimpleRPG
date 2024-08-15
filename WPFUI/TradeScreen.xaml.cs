using System.Windows;
using Engine.Models;
using Engine.ViewModels;
namespace WPFUI
{
    /// <summary>
    /// TradeScreen.xaml交互逻辑
    /// </summary>
    public partial class TradeScreen : Window
    {
        public GameSession Session => DataContext as GameSession;
        public TradeScreen()
        {
            InitializeComponent();
        }

        private void OnClick_Sell(object sender, RoutedEventArgs e)
        {
            GameItem? item = ((FrameworkElement)sender).DataContext as GameItem;
            if(item != null)
            {
                Session.CurrentPlayer.Assets += item.Price;
                Session.CurrentTrader.AddItemToInventory(item);
                Session.CurrentPlayer.RemoveItemFromInventory(item);
            }
        }
        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            GameItem? item = ((FrameworkElement)sender).DataContext as GameItem;
            if(item != null)
            {
                if(Session.CurrentPlayer.Assets >= item.Price)
                {
                    Session.CurrentPlayer.Assets -= item.Price;
                    Session.CurrentTrader.RemoveItemFromInventory(item);
                    Session.CurrentPlayer.AddItemToInventory(item);
                }
                else
                {
                    MessageBox.Show("金钱不够");
                }
            }
        }
        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}