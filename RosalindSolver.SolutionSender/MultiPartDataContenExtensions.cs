using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RosalindSolver.ServerAdapter
{
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
