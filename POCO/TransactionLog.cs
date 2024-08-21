using System;
using System.Collections.Generic;

namespace P0_brendan_BankingApp.POCO
{
    public partial class TransactionLog
    {
        public int TransactionLogId { get; set; }
        public int AccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }

        public virtual Account Account { get; set; }
    }
}
