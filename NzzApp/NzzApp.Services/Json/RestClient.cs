using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sebastian.Toolkit.Logging;

namespace NzzApp.Services.Json
{
    public class RestClient
    {
        protected RestClient()
        {
        }

        protected async Task<TResponse> HttpClientGet<TResponse>(string url, string token = null)
        {
            var client = CreateHttpClient(token);
            var uri = new Uri(url);

            this.Logger().Debug("HttpClientGet Request: Url: {0}", uri.ToString());

            try
            {
                var response = await client.GetAsync(uri).ConfigureAwait(false);
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.Logger().Debug("HttpClientGet Response: StatusCode: {0}, Content: {1}", response.StatusCode, data ?? "");

                if (!string.IsNullOrWhiteSpace(data))
                {
                    var result = JsonConvert.DeserializeObject<TResponse>(data);
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.Logger().Error(string.Format("HttpClientGet request {0} failed", url), ex);
            }

            return default(TResponse);
        }

        protected async Task<TResponse> HttpClientPost<TRequest, TResponse>(string url, TRequest request, string token = null)
        {
            var client = CreateHttpClient(token);
            var uri = new Uri(url);

            var contentData = JsonConvert.SerializeObject(request);
            var content = new StringContent(contentData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            this.Logger().Debug("HttpClientPost Request: Url: {0}, Content: {1}", uri.ToString(), contentData);

            try
            {
                var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.Logger().Debug("HttpClientPost Response: StatusCode: {0}, Content: {1}", response.StatusCode, data ?? "");

                if (!string.IsNullOrWhiteSpace(data))
                {
                    var result = JsonConvert.DeserializeObject<TResponse>(data);
                    return result;
                }
            }
            catch (Exception ex)
            {
                this.Logger().Error(string.Format("HttpClientPost request {0} failed", url), ex);
            }

            return default(TResponse);
        }

        protected async Task<HttpResponseMessage> HttpClientPost<TRequest>(string url, TRequest request, string token = null)
        {
            var client = CreateHttpClient(token);
            var uri = new Uri(url);

            var contentData = JsonConvert.SerializeObject(request);
            var content = new StringContent(contentData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            this.Logger().Debug("HttpClientPost Request: Url: {0}, Content: {1}", uri.ToString(), contentData);

            try
            {
                var response = await client.PostAsync(uri, content).ConfigureAwait(false);
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.Logger().Debug("HttpClientPost Response: StatusCode: {0}, Content: {1}", response.StatusCode, data ?? "");
                return response;
            }
            catch (Exception ex)
            {
                this.Logger().Error(string.Format("HttpClientPost request {0} failed", url), ex);
                return null;
            }
        }

        protected HttpClient CreateHttpClient(string token = null)
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip |
                    DecompressionMethods.Deflate;
            }

            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Cache-Control", "no-chache");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("authentication", token);
            }

            return client;
        }
    }
}
