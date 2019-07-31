using System;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Location secured IP request class
    /// </summary>
    public class SecureIP : IEndpoint<Response>
    {
        /// <summary>
        /// Instantiates the endpoints which allows to see if this IP is secured.
        /// </summary>
        /// <param name="token">A valid user token.</param>
        public SecureIP(string accessToken)
        {
            Address = new Uri("https://api.mojang.com/user/security/location");
            Arguments.Add(accessToken);
        }

        /// <summary>
        /// Performs the request and return the Response property.
        /// </summary>
        public override async Task<Response> PerformRequestAsync()
        {
            Response = await Requester.Get(this, true);

            if (Response.IsSuccess)
            {
                return new Response(Response);
            }
            else
            {
                return new Response(Error.GetError(Response));
            }
        }
    }
}
