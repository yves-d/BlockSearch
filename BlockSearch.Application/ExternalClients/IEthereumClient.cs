using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks;

namespace BlockSearch.Application.ExternalClients
{
    public interface IEthereumClient
    {
        Task<BlockWithTransactions> GetBlockWithTransactionsByNumberAsync(int blockNumber);
    }
}
