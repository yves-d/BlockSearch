using Nethereum.RPC.Eth.DTOs;
using System.Threading.Tasks;

namespace BlockSearch.ExternalClients.CryptoClients
{
    public interface IEthereumClient
    {
        Task<BlockWithTransactions> GetBlockWithTransactionsByNumberAsync(int blockNumber);
    }
}
