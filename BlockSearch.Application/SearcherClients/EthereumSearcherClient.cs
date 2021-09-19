﻿using BlockSearch.Application.Exceptions;
using BlockSearch.Application.ExternalClients;
using BlockSearch.Common.Enums;
using BlockSearch.Common.Models;
using Nethereum.Web3;
using System.Linq;
using System.Threading.Tasks;

namespace BlockSearch.Application.SearcherClients
{
    public class EthereumSearcherClient : ISearcherClient
    {
        private IEthereumClient _ethereumClient;

        public EthereumSearcherClient(IEthereumClient ethereumClient)
        {
            _ethereumClient = ethereumClient;
        }
        
        public async Task<Block> GetBlockByBlockNumber(int blockNumber)
        {
            var block = await GetBlockWithTransactionsByNumberAsync(blockNumber);
            return block;
        }

        private async Task<Block> GetBlockWithTransactionsByNumberAsync(int blockNumber)
        {
            var blockWithTransactions = await _ethereumClient.GetBlockWithTransactionsByNumberAsync(blockNumber);

            if (blockWithTransactions == null)
                throw new BlockNotFoundException("Block with that number was not found.");

            return new Block()
            {
                Hash = blockWithTransactions.BlockHash,
                Number = blockWithTransactions.Number.ToString(),
                Crypto = CryptoType.Ethereum,
                Transactions = blockWithTransactions.Transactions.Select(x => new Transaction()
                {
                    BlockHash = x.BlockHash,
                    BlockNumber = x.BlockNumber.ToString(),
                    Gas = x.Gas.ToString(),
                    Hash = x.TransactionHash,
                    From = x.From,
                    To = x.To,
                    Value = Web3.Convert.FromWei(x.Value).ToString()
                }).ToList()
            };
        }
    }
}
