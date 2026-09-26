using Kepas.Core.Api.Domain.Interfaces;
using Microsoft.Extensions.Localization;

namespace Kepas.Core.Api.Infrastructure.Services
{
    public class AppTranslationService : ITranslationService
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AppTranslationService(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public string GetString(string key)
        {
            var localizedString = _localizer[key];
            // Se no encontrar, o localizer devolve a prpria key. Vamos fazer um fallback simples.
            if (localizedString.ResourceNotFound)
            {
                return GetFallback(key);
            }
            return localizedString;
        }

        public string GetString(string key, params object[] arguments)
        {
            var localizedString = _localizer[key, arguments];
            if (localizedString.ResourceNotFound)
            {
                return string.Format(GetFallback(key), arguments);
            }
            return localizedString;
        }

        private string GetFallback(string key)
        {
            return key switch
            {
                Domain.Constants.TranslationKeys.GenericError => "Ocorreu um erro interno ao processar a operação.",
                Domain.Constants.TranslationKeys.TenantsLoadedSuccess => "Inquilinos carregados com sucesso.",
                Domain.Constants.TranslationKeys.ServiceAccountsLoadedSuccess => "Contas de serviço carregadas com sucesso.",
                Domain.Constants.TranslationKeys.ServiceAccountCreatedSuccess => "Conta de serviço criada com sucesso.",
                _ => key
            };
        }
    }
}
