namespace Kepas.Core.Api.Domain.Interfaces
{
    public interface ITranslationService
    {
        string GetString(string key);
        string GetString(string key, params object[] arguments);
    }
}
