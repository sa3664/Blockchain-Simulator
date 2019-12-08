using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using BlockchainSimulator.Models;

namespace BlockchainSimulator.Controllers
{
    [EnableCors(origins: "http://localhost:1234", headers: "*", methods: "*")]
      [RoutePrefix("api/block")]
    public class BlockController : ApiController
    {
       static Blockchain _blockchain = new Blockchain();

        [Route("mine")]
        [HttpGet]
        public string Mine()
        {
           return _blockchain.Mine();
        }

        [Route("mine")]
        [HttpPost]
        public string Mine(List<Transaction> transactionsList)
        {
            return _blockchain.Mine(transactionsList);
        }

        [Route("getTHash")]
        [HttpPost]
        public string getTHash(List<Transaction> transactionsList)
        {
            return _blockchain.GetTransactionHash(transactionsList);
        }

        [Route("chain")]
        [HttpGet]
        public string Chain()
        {
            return _blockchain.GetFullChain();
        }

        [Route("transactions/new")]
        [HttpPost]
        public string NewTransaction(Transaction transaction)
        { 
            int blockId = _blockchain.CreateTransaction(transaction.Sender, transaction.Recipient, transaction.Amount);
            return $"Your transaction is part of block {blockId}";

        }

        [Route("resolve")]
        [HttpGet]
        public string Resolve()
        {
            return _blockchain.Consensus();
        }

        [Route("register")]
        [HttpPost]
        public string NodeRegister(string[] node)
        {
            return _blockchain.RegisterNodes(node);
        }

    }
}
