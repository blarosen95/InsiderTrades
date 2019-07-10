using System.Collections.ObjectModel;

namespace InsiderTrades.ViewModel
{
    public class TransactionList : ObservableCollection<Transaction>
    {
    }

    public class Transaction
    {
        public string AcqOrDis { get; set; }
        public string TransactionDate { get; set; }
        public string DeemedDate { get; set; }
        public string Owner { get; set; }
        public string Form { get; set; }
        public string TransType { get; set; }
        public string TypeOfOwner { get; set; }
        public string NumTransacted { get; set; }
        public string NumOwned { get; set; }
        public string LineNum { get; set; }
        public string OwnerCIK { get; set; }
        public string SecName { get; set; }

        public Transaction(string acqOrDis, string transactionDate, string deemedDate, string owner, string form,
            string transType, string typeOfOwner, string numTransacted, string numOwned, string lineNum,
            string ownerCIK, string secName)
        {
            AcqOrDis = acqOrDis;
            TransactionDate = transactionDate;
            DeemedDate = deemedDate;
            Owner = owner;
            Form = form;
            TransType = transType;
            TypeOfOwner = typeOfOwner;
            NumTransacted = numTransacted;
            NumOwned = numOwned;
            LineNum = lineNum;
            OwnerCIK = ownerCIK;
            SecName = secName;
        }

        public override string ToString()
        {
            return
                $"{AcqOrDis}, {TransactionDate}, {DeemedDate}, {Owner}, {Form}, {TransType}, {TypeOfOwner}, {NumTransacted}, {NumOwned}, {LineNum}, {OwnerCIK}, {SecName}";
        }
    }
}