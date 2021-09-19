using BlockSearch.Application.Exceptions;
using BlockSearch.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System.Threading.Tasks;

namespace BlockSearch.Application.ExternalClients
{
    public class NethereumClient : IEthereumClient
    {
        private string _baseUri;
        private readonly string _infuraProjectId;

        private IWeb3 _ethClient => new Web3($"{_baseUri}{_infuraProjectId}");
        
        public NethereumClient(IOptions<NethereumClientOptions> options)
        {
            if (string.IsNullOrEmpty(options.Value.BaseUri) || string.IsNullOrEmpty(options.Value.ProjectId))
                throw new InitialisationFailureException($"Failed to initialise {nameof(NethereumClient)}");

            _baseUri = options.Value.BaseUri;
            _infuraProjectId = options.Value.ProjectId;
        }

        public async Task<BlockWithTransactions> GetBlockWithTransactionsByNumberAsync(int blockNumber)
        {
            return await _ethClient.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(
                    new HexBigInteger(blockNumber));
        }
    }
}
