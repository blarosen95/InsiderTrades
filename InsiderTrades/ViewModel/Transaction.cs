using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsiderTrades.ViewModel
{
    class Transaction
    {
        public String AcqOrDis {get; set;}
        public String TransactionDate { get; set; }
        public String DeemedDate { get; set; }
        public String Owner { get; set; }
        public String Form { get; set; }
        public String TransType { get; set; }
        public String TypeOfOwner { get; set; }
        public String NumTransacted { get; set; }
        public String NumOwned { get; set; }
        public String LineNum { get; set; }
        public String OwnerCIK { get; set; }
        public String SecName { get; set; }

        public Transaction(string acqOrDis, string transactionDate, string deemedDate, string owner, string form, string transType, string typeOfOwner, string numTransacted, string numOwned, string lineNum, string ownerCIK, string secName)
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

        /*
        public static List<Transaction> Transactions()
        {
            return new List<Transaction>(new Transaction[3]
                {
                    new Transaction("Davis Stephanie A", "0001771886", "2019-06-26", "director"),
                    new Transaction("EDELMAN JOSEPH", "0001164426", "2019-06-18", "10 percent owner"),
                    new Transaction("PERCEPTIVE ADVISORS LLC", "0001224962", "2019-06-18", "10 percent owner")
                }
            );
        }
        */
    }
}