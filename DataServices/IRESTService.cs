using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public interface IRESTService
    {
        Task<T> GetAsync<T>(string resourceUri, params string[] query);
        Task<List<T>> GetAllAsync<T>(string resourceUri, params string[] query);
        Task<List<T>> GetAllAfterDateAsync<T>(string resourceUri, DateTime time, int? previousId = null);
        Task<List<T>> GetAllBetweenDatesAsync<T>(string resourceUri, DateTime from, DateTime to, int? previousId = null, bool isDone = false);
        Task PostAsync<T>(string resourceUri, T obj);
    }
}
