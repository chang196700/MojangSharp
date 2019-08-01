using System.Net;

namespace MojangSharpCore.Responses
{
    /// <summary>
    /// Default response class, can be inherited.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Status code of the response.
        /// </summary>
        public HttpStatusCode Code { get; internal set; }

        /// <summary>
        /// Defines weither or note the request is a success.
        /// </summary>
        public bool IsSuccess { get; internal set; }

        /// <summary>
        /// Response's raw message contents.
        /// </summary>
        public string RawMessage { get; internal set; }

        public byte[] RawContent { get; set; }

        /// <summary>
        /// Contains an error if the request failed.
        /// </summary>
        public Error Error { get; internal set; }

        internal Response()
        {
        }

        internal Response(Response response) : this()
        {
            Code = response.Code;
            IsSuccess = response.IsSuccess;
            RawMessage = response.RawMessage;
            Error = response.Error;
        }
    }
}
