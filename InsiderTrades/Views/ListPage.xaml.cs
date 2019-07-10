// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InsiderTrades.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListPage
    {
        public ListPage(HomePage passedHome)
        {
            InitializeComponent();
            HomePage = passedHome;
            HomePage.DataContextChanged += (s, e) => Bindings.Update();
        }

        private HomePage HomePage { get; set; }
    }
}