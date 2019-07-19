using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using InsiderTrades.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InsiderTrades
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        internal HomePage HomeView;// = new HomePage();
        internal ListPage ListView;// = new ListPage(this.HomeView);

        public MainPage()
        {
            InitializeComponent();
            HomeView = new HomePage(this);
            ListView = new ListPage(HomeView);
        }

        #region NavigationView event handlers

        private void NvTopLevelNav_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (NavigationViewItemBase item in NvTopLevelNav.MenuItems)
            {
                if (item == null || item.Tag.ToString() != "Home_Page") continue;
                NvTopLevelNav.SelectedItem = item;
                break;
            }

            //ContentFrame.Navigate(typeof(HomePage));
            ContentFrame.Content = HomeView;
        }

        private void NvTopLevelNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            //Intentionally left blank
        }

        internal void NvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                if (!(args.InvokedItem is TextBlock itemContent)) return;
                switch (itemContent.Tag)
                {
                    case "Nav_Home":
                        ContentFrame.Content = HomeView;
                        break;

                    case "Nav_List":
                        ContentFrame.Content = ListView;
                        break;
                }
            }
        }

        #endregion

        internal void GoToListView()
        {
            ContentFrame.Content = ListView;
        }
        
    }
}