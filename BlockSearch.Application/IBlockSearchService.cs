using BlockSearch.Common.Enums;
using BlockSearch.Common.Models;

namespace BlockSearch.Application
{
    public interface IBlockSearchService
    {
        Block GetEthereumAddressTransactionsInBlock(CryptoType cryptoType, int blockNumber, string address);
    }
}
