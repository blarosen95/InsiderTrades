using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using InsiderTrades.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InsiderTrades.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public List<String> Cells = new List<string>();

        public HomePage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Edgar edgar = new Edgar();
            Cells = await edgar.GetInfo(TickerBox.Text);
            //TODO: Test performance of this method before this line and also after this line
            var subCells = Cells.ChunkBy(12);

            //Skip the first item in the list (it should just be the tables column names).
            for (var i = 1; i < subCells.Count; i++)
            {
                Transaction transaction = new Transaction(subCells[i][0], subCells[i][1], subCells[i][2],
                    subCells[i][3], subCells[i][4], subCells[i][5], subCells[i][6], subCells[i][7], subCells[i][8],
                    subCells[i][9], subCells[i][10], subCells[i][11]);

                var messageDialog = new MessageDialog(transaction.ToString());
                await messageDialog.ShowAsync();
            }

            //var messageDialog = new MessageDialog(subCells.Count.ToString());

            //await messageDialog.ShowAsync();
        }
    }
}