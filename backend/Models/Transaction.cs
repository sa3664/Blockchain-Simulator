using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockchainSimulator.Models
{
    public class Transaction
    {
        public string TransactionHash { get; set; }
        public int Amount { get; set; }
        public string Recipient { get; set; }
        public string Sender { get; set; }

    }
}
