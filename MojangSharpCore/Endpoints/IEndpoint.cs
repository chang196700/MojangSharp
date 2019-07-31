using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MojangSharpCore.Responses;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Endpoint parent class
    /// </summary>
    /// <typeparam name="T">Response type of that endpoint.</typeparam>
    public abstract class IEndpoint<T>
    {
        /// <summary>
        /// Endpoint's address
        /// </summary>
        public Uri Address { get; set; }

        /// <summary>
        /// Response after request is performed.
        /// </summary>
        public Response Response { get; set; }

        /// <summary>
        /// Arguments to be sent.
        /// </summary>
        public List<string> Arguments
        {
            get => _arguments ?? (_arguments = new List<string>() { });
            set => _arguments = value;
        }

        private List<string> _arguments;

        /// <summary>
        /// Contents for a post request, must be set before performing it.
        /// </summary>
        public string PostContent { get; set; }

        /// <summary>
        /// Performs the request
        /// </summary>
        /// <returns></returns>
        public abstract Task<T> PerformRequestAsync();
    }
}
