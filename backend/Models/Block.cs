using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockchainSimulator.Models
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Nonce { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }

        public override string ToString()
        {
            return $"{Index} [{Timestamp.ToString("yyyy-MM-dd HH:mm:ss")}] Nonce: {Nonce} | PrevHash: {PreviousHash} | Trx: {Transactions.Count}";
        }
    }
}