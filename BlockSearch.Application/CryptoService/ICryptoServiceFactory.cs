using BlockSearch.Common.Enums;

namespace BlockSearch.Application.CryptoService
{
    public interface ICryptoServiceFactory
    {
        ICryptoService GetCryptoService(CryptoType cryptoType);
    }
}
