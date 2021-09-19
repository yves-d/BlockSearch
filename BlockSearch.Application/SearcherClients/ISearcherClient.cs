using BlockSearch.Common.Models;
using System.Threading.Tasks;

namespace BlockSearch.Application.SearcherClients
{
    public interface ISearcherClient
    {
        Task<Block> GetBlockByBlockNumber(int blockNumber);
    }
}
