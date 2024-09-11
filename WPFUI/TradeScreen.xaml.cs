using System.Windows;
using RPG.Models;
using RPG.ViewModels;

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
            GroupedInventoryItem? groupedInventoryItem =
                ((FrameworkElement)sender).DataContext as GroupedInventoryItem;
            if(groupedInventoryItem != null)
            {
                Session.CurrentPlayer.ReceiveAssets(groupedInventoryItem.Item.Price);
                Session.CurrentTrader!.AddItemToInventory(groupedInventoryItem.Item);
                Session.CurrentPlayer.RemoveItemFromInventory(groupedInventoryItem.Item);
            }
        }
        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem? groupedInventoryItem =
                ((FrameworkElement)sender).DataContext as GroupedInventoryItem;
            if(groupedInventoryItem != null)
            {
                if(Session.CurrentPlayer.Assets >= groupedInventoryItem.Item.Price)
                {
                    Session.CurrentPlayer.SpendAssets(groupedInventoryItem.Item.Price);
                    Session.CurrentTrader.RemoveItemFromInventory(groupedInventoryItem.Item);
                    Session.CurrentPlayer.AddItemToInventory(groupedInventoryItem.Item);
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