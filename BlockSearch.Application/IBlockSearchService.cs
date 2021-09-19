using BlockSearch.Common.Enums;
using BlockSearch.Common.Models;
using System.Threading.Tasks;

namespace BlockSearch.Application
{
    public interface IBlockSearchService
    {
        Task<Block> GetAddressTransactionsInBlock(CryptoType? cryptoType, int? blockNumber, string address);
    }
}
