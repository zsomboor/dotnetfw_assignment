using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public class RESTService : IRESTService
    {
        private string _baseApiUrl;

        public RESTService(string baseApiUrl)
        {
            _baseApiUrl = baseApiUrl;
            //JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
            //IsoDateTimeConverter dateConverter = new IsoDateTimeConverter
            //{
            //    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"
            //};
            //jsonSettings.Converters.Add(dateConverter);

            //FlurlHttp.Configure(settings => {
            //    settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
            //});
        }

        public async Task<T> GetAsync<T>(string resourceUri, params string[] query)
        {
            IFlurlRequest request = _baseApiUrl.AppendPathSegment(resourceUri).WithHeader("X-API-Key", "dummy-secret");
            foreach (string param in query)
            {
                request = request.SetQueryParam(param);
            }
            Console.WriteLine(request);
            return await request.GetJsonAsync<T>();
        }

        public async Task<List<T>> GetAllAsync<T>(string resourceUri, params string[] query)
        {
            IFlurlRequest request = _baseApiUrl.AppendPathSegment(resourceUri).WithHeader("X-API-Key", "dummy-secret");
            List<T> response = new List<T>();
            foreach (string param in query)
            {
                request = request.SetQueryParam(param);
            }

            return await request.GetJsonAsync<List<T>>();
        }

        public async Task<List<T>> GetAllAfterDateAsync<T>(string resourceUri, DateTime time, int? previousId = null)
        {
            IFlurlRequest request = _baseApiUrl.AppendPathSegment(resourceUri).WithHeader("X-API-Key", "dummy-secret").SetQueryParam("after", time);
            if (previousId.HasValue)
                request.SetQueryParam("previousId", previousId.Value);
            return await request.GetJsonAsync<List<T>>();
        }

        public async Task<List<T>> GetAllBetweenDatesAsync<T>(string resourceUri, DateTime from, DateTime to, int? previousId = null, bool isDone = false)
        {
            IFlurlRequest request = _baseApiUrl.AppendPathSegment(resourceUri).WithHeader("X-API-Key", "dummy-secret").SetQueryParam("after", from).SetQueryParam("before", to);
            if (previousId.HasValue)
                request.SetQueryParam("previousId", previousId.Value);
            if (isDone)
                request.SetQueryParam("isDone", isDone);
            return await request.GetJsonAsync<List<T>>();
        }

        public async Task PostAsync<T>(string resourceUri, T obj)
        {
            await _baseApiUrl.AppendPathSegment(resourceUri).WithHeader("X-API-Key", "dummy-secret").PostJsonAsync(obj);

        }

    }
}
