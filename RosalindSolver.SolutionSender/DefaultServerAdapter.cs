using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RosalindSolver.SolutionSender
{
    public class ServerConfiguration
    {
        private readonly Uri _host;
        private readonly Uri _loginUri;
        private readonly string _datasetUriTemplate;
        private readonly string _resultUriTemplate;
        public ServerConfiguration(string host)
        {
            _host = new Uri(host);
            _loginUri = new Uri(_host, "/accounts/login/");
            _datasetUriTemplate = host + "/problems/{0}/dataset/";
            _resultUriTemplate = "/problems/{0}/";
        }

        public Uri GetHost() => _host;
        public Uri GetLoginUri() => _loginUri;
        public Uri GetDatasetUri(string key) => new Uri(string.Format(_datasetUriTemplate, key));
        public Uri GetResultUri(string key) => new Uri(_host, string.Format(_resultUriTemplate, key));
    }

    public class UserConfiguration
    {
        public string Username { get; }
        public string Password { get; }
        public UserConfiguration(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    public struct SolverCheckResult
    {
        public bool IsCorrect { get; }
        public Stream Dataset { get; }
        public Stream Answer { get; }

        public SolverCheckResult(bool isCorrect, Stream dataset, Stream answer)
        {
            IsCorrect = isCorrect;
            Dataset = dataset;
            Answer = answer;
        }
    }

    public interface ISolver
    {
        string Key { get; }
        Task<Stream> SolveAsync(Stream dataset);
        Task<Stream> GetSourceCodeAsync();
    }

    public interface IServerAdapter
    {
        Task<SolverCheckResult> SendSolutionAsync(ISolver solver);
    }

    public class DefaultServerAdapter : IServerAdapter
    {
        private readonly ServerConfiguration _serverConfiguration;
        private readonly UserConfiguration _userCongfiguration;

        public DefaultServerAdapter(ServerConfiguration serverConfiguration, UserConfiguration userCongfiguration)
        {
            _serverConfiguration = serverConfiguration;
            _userCongfiguration = userCongfiguration;
        }

        public async Task<SolverCheckResult> SendSolutionAsync(ISolver solver)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = _serverConfiguration.GetHost() })
            {
                var loginUri = _serverConfiguration.GetLoginUri();
                var request = new HttpRequestMessage(HttpMethod.Get, loginUri);
                await client.SendAsync(request);

                var token = cookieContainer.GetCookies(loginUri)["csrftoken"]?.Value;
                request = new HttpRequestMessage(HttpMethod.Post, loginUri)
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"csrfmiddlewaretoken", token},
                        {"username", _userCongfiguration.Username},
                        {"password", _userCongfiguration.Password},
                        //{ "next", "/problems/fibo/dataset/" },
                    })
                };

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                
                var dataset = await GetDatasetAsync(client, solver.Key);
                var answer = await solver.SolveAsync(dataset);
                var code = await solver.GetSourceCodeAsync();
                var isCorrect = await SendResultAsync(client, token, solver.Key, answer, code);
                return new SolverCheckResult(isCorrect, dataset, answer);
            }
        }

        private async Task<Stream> GetDatasetAsync(HttpClient client, string key)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _serverConfiguration.GetDatasetUri(key));
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStreamAsync();
        }

        private async Task<bool> SendResultAsync(HttpClient client, string token, string key, Stream result, Stream code)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _serverConfiguration.GetResultUri(key))
            {
                Content = new MultipartFormDataContent()
                    .AddPartContent("csrfmiddlewaretoken", token)
                    .AddPartContent("output_text", result)
                    .AddPartContent("output_file", new MemoryStream(new byte[0]))
                    .AddPartContent("code", new MemoryStream(new byte[0]))
            };

            request.Content.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

            var response = await client.SendAsync(request);
            var answer = await response.Content.ReadAsStringAsync();
            
            var isSuccess = answer.Contains("alert-success");
            if (isSuccess) return true;

            var isWrong = answer.Contains("alert-error");
            if (isWrong) return false;

            throw new InvalidOperationException();
        }

        //var td = new ByteArrayContent(Encoding.UTF8.GetBytes(token));
        //td.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
        //td.Headers.ContentDisposition.Name = "\"csrfmiddlewaretoken\"";
        //dataContent.Add(td, "\"csrfmiddlewaretoken\"");

        //var text = new ByteArrayContent(Encoding.UTF8.GetBytes(result));
        //text.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
        //text.Headers.ContentDisposition.Name = "\"output_text\"";
        //dataContent.Add(text, "\"output_text\"");

        //var file = new StreamContent(new MemoryStream(new byte[0]));
        //file.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
        //file.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //file.Headers.ContentDisposition.Name = "\"output_file\"";
        //file.Headers.ContentDisposition.FileName = "\"\"";
        //dataContent.Add(file, "\"output_file\"");

        //var code = new StreamContent(new MemoryStream(new byte[0]));
        //code.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
        //code.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //code.Headers.ContentDisposition.Name = "\"code\"";
        //code.Headers.ContentDisposition.FileName = "\"\"";
        //dataContent.Add(code, "\"code\"");

        //public async void PostFile(string url, string fileNameLocal, CancellationToken cancellationToken)
        //{
        //    var fileName = Path.GetFileName(fileNameLocal);
        //    using (var client = new HttpClient(new NativeMessageHandler()))
        //    {
        //        var fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read);
        //        var streamContent = new StreamContent(fileStream);
        //        streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
        //        streamContent.Headers.ContentDisposition.Name = "\"file\"";
        //        streamContent.Headers.ContentDisposition.FileName = "\"" + fileName + "\"";
        //        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //        string boundary = "12345";
        //        var fContent = new MultipartFormDataContent(boundary);
        //        fContent.Headers.Remove("Content-Type");
        //        fContent.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);
        //        fContent.Add(streamContent);
        //        var response = await client.PostAsync(new Uri(url), fContent, cancellationToken);
        //        response.EnsureSuccessStatusCode();
        //        streamContent.Dispose();
        //        fileStream.Dispose();
        //    }
        //}
    }

    internal static class MultiPartDataContenExtensions
    {
        private static readonly MediaTypeHeaderValue StreamMediaTypeValue = new MediaTypeHeaderValue("application/octet-stream");
        private static readonly string EmptyWrappedName = WrapName(string.Empty);

        internal static MultipartFormDataContent AddPartContent(this MultipartFormDataContent baseContent, string name, string stringContent)
        {
            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(stringContent));
            return baseContent.AddPartContent(name, content);
        }

        internal static MultipartFormDataContent AddPartContent(this MultipartFormDataContent baseContent, string name, Stream streamContent, string fileName = null)
        {
            var content = new StreamContent(streamContent);
            content.Headers.ContentType = StreamMediaTypeValue;
            var result = baseContent.AddPartContent(name, content);
            content.Headers.ContentDisposition.FileName = WrapName(fileName);
            return result;
        }

        private static MultipartFormDataContent AddPartContent(this MultipartFormDataContent baseContent, string name, HttpContent content)
        {
            var wrappedName = WrapName(name);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = wrappedName
            };

            baseContent.Add(content, wrappedName);
            return baseContent;
        }

        private static string WrapName(string name) => string.IsNullOrWhiteSpace(name) ? EmptyWrappedName : $"\"{name}\"";
    }
}
