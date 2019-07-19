using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using InsiderTrades.ViewModel;
using System.Threading;
using System.Threading.Tasks;

namespace InsiderTrades.Views
{
    public sealed partial class HomePage
    {
        public List<string> Cells = new List<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        public TransactionList TransactionList = new TransactionList();

        private MainPage _mainPage;

        private Queue<List<List<string>>> myQueue;

        public ObservableCollection<Transaction> oc;

        Object lockMe = new Object();

        public HomePage(MainPage mainPage)
        {
            InitializeComponent();
            _mainPage = mainPage;
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

                ParallelOptions options = new ParallelOptions {MaxDegreeOfParallelism = 3};
                //Skip the first item in the list (it should just be the tables column names).
                //for (var i = 1; i < subCells.Count; i++)
                Parallel.For(1, subCells.Count, options, i =>
                    {
                        var transaction = new Transaction(subCells[i][0], subCells[i][1], subCells[i][2],
                            subCells[i][3], subCells[i][4], subCells[i][5], subCells[i][6], subCells[i][7],
                            subCells[i][8],
                            subCells[i][9], subCells[i][10], subCells[i][11], i);

                        TransactionList.Push(transaction);
                        //TransactionStack.Add(transaction);
                    });
                oc = new ObservableCollection<Transaction>(TransactionList.AsParallel().OrderBy(transaction => transaction.SortingKey ));
                OnPropertyChanged("Transactions");

                //Switch to ListPage if ready
                _mainPage.ListView.UpdateBindings();
                _mainPage.GoToListView();
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