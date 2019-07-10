using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using InsiderTrades.ViewModel;

namespace InsiderTrades.Views
{
    public sealed partial class HomePage
    {
        public List<string> Cells = new List<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        public TransactionList TransactionList = new TransactionList();

        public HomePage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Clear the TransactionList so that we don't include previously searched data in this list.
            TransactionList.Clear();
            Edgar edgar = new Edgar();
            try
            {
                Cells = await edgar.GetInfo(TickerBox.Text);
                var subCells = Cells.ChunkBy(12);

                //Skip the first item in the list (it should just be the tables column names).
                for (var i = 1; i < subCells.Count; i++)
                {
                    Transaction transaction = new Transaction(subCells[i][0], subCells[i][1], subCells[i][2],
                        subCells[i][3], subCells[i][4], subCells[i][5], subCells[i][6], subCells[i][7], subCells[i][8],
                        subCells[i][9], subCells[i][10], subCells[i][11]);

                    TransactionList.Add(transaction);
                }

                OnPropertyChanged("Transactions");
            }
            catch (System.Net.Http.HttpRequestException err)
            {
                var messageDialog =
                    new MessageDialog(
                        $"Error: {err.Message}\nEither your internet is down (most likely), or the website/service needed is down (least likely).");
                await messageDialog.ShowAsync();
            }
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}