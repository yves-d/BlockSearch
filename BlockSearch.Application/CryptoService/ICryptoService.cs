using BlockSearch.Common.Models;
using System.Threading.Tasks;

namespace BlockSearch.Application.CryptoService
{
    public interface ICryptoService
    {
        Task<Block> GetBlockByBlockNumber(int blockNumber);
    }
}
