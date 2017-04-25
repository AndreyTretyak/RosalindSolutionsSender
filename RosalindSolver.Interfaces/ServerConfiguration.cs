using System;

namespace RosalindSolver.Interfaces
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
}
