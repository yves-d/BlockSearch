using BlockSearch.Application.Exceptions;
using BlockSearch.Common.Enums;
using BlockSearch.Common.Models;
using BlockSearch.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using System.Linq;
using System.Threading.Tasks;

namespace BlockSearch.Application.SearcherClients
{
    public class EthereumSearcherClient : ISearcherClient
    {
        private string _baseUri;
        private readonly string _infuraProjectId;

        private IWeb3 _ethClient => new Web3($"{_baseUri}{_infuraProjectId}");

        public EthereumSearcherClient(IOptions<EthereumSearcherOptions> options)
        {
            if (string.IsNullOrEmpty(options.Value.BaseUri) || string.IsNullOrEmpty(options.Value.ProjectId))
                throw new InitialisationFailureException($"Failed to initialise {nameof(EthereumSearcherClient)}");

            _baseUri = options.Value.BaseUri;
            _infuraProjectId = options.Value.ProjectId;
        }
        
        public async Task<Block> GetBlockByBlockNumber(int blockNumber)
        {
            var block = await GetBlockWithTransactionsByNumberAsync(blockNumber);
            return block;
        }

        private async Task<Block> GetBlockWithTransactionsByNumberAsync(int blockNumber)
        {
            var blockWithTransactions = await _ethClient.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(
                    new HexBigInteger(blockNumber));

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
