using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using BlockchainSimulator.Models;
using Newtonsoft.Json;

namespace BlockchainSimulator.Controllers
{
    public class Blockchain
    {
            private List<Transaction> _currentTransactions = new List<Transaction>();
            private List<Block> _blocks= new List<Block>();
            private List<Node> _nodes = new List<Node>();
            private Block _lastBlock => _blocks.Last();

            public string NodeId { get; private set; }

        internal string Mine(List<Transaction> transactionsList)
        {
            int proof = GetProofOfWork(_lastBlock.Nonce, _lastBlock.PreviousHash);

            _currentTransactions.AddRange(transactionsList);
            _currentTransactions = _currentTransactions.Distinct().ToList();

            CreateTransaction(sender: "0", recipient: NodeId, amount: 1);
            Block block = CreateNewBlock(proof , null);

            var response = new
            {
                Message = "New Block Forged",
                Index = block.Index,
                Transactions = block.Transactions.ToArray(),
                Nonce = block.Nonce,
                PreviousHash = block.PreviousHash
            };

            return JsonConvert.SerializeObject(block);
        }

        //ctor
        public Blockchain()
            {
                NodeId = Guid.NewGuid().ToString().Replace("-", "");
                CreateNewBlock(nonce:0, previousHash: "00000000000000000000000000000000000"); 
            }

            //private functionality
            private void RegisterNode(string address)
            {
                _nodes.Add(new Node { Address = new Uri(address) });
            }

            private bool IsValidChain(List<Block> chain)
            {
                Block block = null;
                Block lastBlock = chain.First();
                int currentIndex = 1;
                while (currentIndex < chain.Count)
                {
                    block = chain.ElementAt(currentIndex);
                    Debug.WriteLine($"{lastBlock}");
                    Debug.WriteLine($"{block}");
                    Debug.WriteLine("----------------------------");

                    //Check that the hash of the block is correct
                    if (block.PreviousHash != GetHash(lastBlock))
                        return false;

                    //Check that the Proof of Work is correct
                    if (!IsValidProof(lastBlock.Nonce, block.Nonce, lastBlock.PreviousHash))
                        return false;

                    lastBlock = block;
                    currentIndex++;
                }

                return true;
            }

            private bool ResolveConflicts()
            {
                List<Block> newChain = null;
                int maxLength = _blocks.Count;

                foreach (Node node in _nodes)
                {
                    var url = new Uri(node.Address, "/chain");
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    var response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var model = new
                        {
                            chain = new List<Block>(),
                            length = 0
                        };
                        string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        var data = JsonConvert.DeserializeAnonymousType(json, model);

                        if (data.chain.Count > _blocks.Count && IsValidChain(data.chain))
                        {
                            maxLength = data.chain.Count;
                            newChain = data.chain;
                        }
                    }
                }

                if (newChain != null)
                {
                    _blocks = newChain;
                    return true;
                }

                return false;
            }

            private Block CreateNewBlock(int nonce, string previousHash = null, List<Transaction> transactionsList = null)
            {
            var block = new Block
            {
                Index = _blocks.Count,
                Timestamp = DateTime.UtcNow,
                Transactions = transactionsList != null ? transactionsList : _currentTransactions.ToList(),
                Nonce = nonce,
                PreviousHash = previousHash ?? GetTransactionHash(_blocks.Last().Transactions),
                Hash = GetTransactionHash(_currentTransactions)
        };
            _currentTransactions.Clear();
               
                _blocks.Add(block);

                return block;
            }

        private string GetTransactionHash(List<Transaction> list)
        {
            string blockText = JsonConvert.SerializeObject(list);
            return GetSha256(blockText);
        }

        private int GetProofOfWork(int lastProof, string previousHash)
            {
                int proof = 0;
                while (!IsValidProof(lastProof, proof, previousHash))
                    proof++;

                return proof;
            }

            private bool IsValidProof(int lastProof, int proof, string previousHash)
            {
                string guess = $"{lastProof}{proof}{previousHash}";
                string result = GetSha256(guess);
                return result.EndsWith("000");
            }

            private string GetHash(Block block)
            {
                string blockText = JsonConvert.SerializeObject(block);
                return GetSha256(blockText);
            }

            private string GetSha256(string data)
            {
                var sha256 = new SHA256Managed();
                var hashBuilder = new StringBuilder();

                byte[] bytes = Encoding.Unicode.GetBytes(data);
                byte[] hash = sha256.ComputeHash(bytes);

                foreach (byte x in hash)
                    hashBuilder.Append($"{x:x2}");

                return hashBuilder.ToString();
            }

            //web server calls
            internal string Mine()
            {
                int proof = GetProofOfWork(_lastBlock.Nonce, _lastBlock.PreviousHash);

                CreateTransaction(sender: "0", recipient: NodeId, amount: 1);
                Block block = CreateNewBlock(proof /*, _lastBlock.Hash*/);

                var response = new
                {
                    Message = "New Block Forged",
                    Index = block.Index,
                    Transactions = block.Transactions.ToArray(),
                    Nonce = block.Nonce,
                    PreviousHash = block.PreviousHash
                };

                return JsonConvert.SerializeObject(block);
            }

            internal string GetFullChain()
            {
                var response = new
                {
                    chain = _blocks.ToArray(),
                    length = _blocks.Count
                };

                return JsonConvert.SerializeObject(response);
            }

            internal string RegisterNodes(string[] nodes)
            {
                var builder = new StringBuilder();
                foreach (string node in nodes)
                {
                    string url = $"http://{node}";
                    RegisterNode(url);
                    builder.Append($"{url}, ");
                }

                builder.Insert(0, $"{nodes.Count()} new nodes have been added: ");
                string result = builder.ToString();
                return result.Substring(0, result.Length - 2);
            }

            internal string Consensus()
            {
                bool replaced = ResolveConflicts();
                string message = replaced ? "was replaced" : "is authoritive";

                var response = new
                {
                    Message = $"Our chain {message}",
                    Chain = _blocks
                };

                return JsonConvert.SerializeObject(response);
            }

        internal int CreateTransaction(string sender, string recipient, int amount)
            {
            var transaction = new Transaction
            {
                Sender = sender,
                Recipient = recipient,
                Amount = amount,
                TransactionHash = Guid.NewGuid().ToString().Replace("-", "")
        };

                _currentTransactions.Add(transaction);

                return _lastBlock != null ? _lastBlock.Index + 1 : 0;
            }
        }
    }
