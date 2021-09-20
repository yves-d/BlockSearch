using BlockSearch.Common.Enums;
using BlockSearch.Common.Exceptions;
using System;
using System.Collections.Generic;

namespace BlockSearch.Application.CryptoService
{
    public class CryptoServiceFactory : ICryptoServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private static Dictionary<CryptoType, Func<ICryptoService>> cryptoServices;

        public CryptoServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadCryptoServices();
        }

        public ICryptoService GetCryptoService(CryptoType cryptoType)
        {
            if (cryptoServices.ContainsKey(cryptoType))
                return cryptoServices[cryptoType]();

            throw new ServiceNotImplementedException($"Crypto Service not implemented - {cryptoType}");
        }

        private void LoadCryptoServices()
        {
            cryptoServices = new Dictionary<CryptoType, Func<ICryptoService>>();
            cryptoServices.Add(CryptoType.Ethereum, () => (ICryptoService)_serviceProvider.GetService(typeof(EthereumService)));
        }
    }
}
