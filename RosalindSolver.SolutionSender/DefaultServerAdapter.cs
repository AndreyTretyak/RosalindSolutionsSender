using RosalindSolver.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RosalindSolver.ServerAdapter
{
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

        private async Task<string> GetDatasetAsync(HttpClient client, string key)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _serverConfiguration.GetDatasetUri(key));
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<bool> SendResultAsync(HttpClient client, string token, string key, string result, string code)
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
}
