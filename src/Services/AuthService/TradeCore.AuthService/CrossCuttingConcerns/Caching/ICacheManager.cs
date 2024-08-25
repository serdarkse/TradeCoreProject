namespace TradeCore.AuthService.CrossCuttingConcerns.Caching
{
    /// <summary>
    /// Tüm Cache Managerlar bu interface i kullanacaktır.
    /// </summary>
    public interface ICacheManager
    {
        Task<T> Get<T>(string key);
        Task<object> Get(string key);
        Task Add(string key, object data, int duration);
        Task Add(string key, object data);
        Task<bool> IsAdd(string key);
        Task Remove(string key);
        Task RemoveByPattern(string pattern);
        Task Clear();
    }
}
