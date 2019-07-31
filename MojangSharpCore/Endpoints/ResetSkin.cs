using System;
using System.Net;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Deletes a user skin
    /// </summary>
    public class ResetSkin : IEndpoint<Response>
    {
        /// <summary>
        /// Creates a change skin request with a given UUID.
        /// </summary>
        /// <param name="accessToken">Access token of the player</param>
        /// <param name="uuid">UUID of the player.</param>
        public ResetSkin(string accessToken, string uuid)
        {
            Address = new Uri($"https://api.mojang.com/user/profile/{uuid}/skin");
            Arguments.Add(accessToken);
        }

        /// <summary>
        /// Performs the skin change.
        /// </summary>
        public override async Task<Response> PerformRequestAsync()
        {
            Response = await Requester.Delete(this);

            if (Response.Code == HttpStatusCode.NoContent || Response.IsSuccess)
            {
                return new Response(Response) { IsSuccess = true };
            }
            else
            {
                return new Response(Response);
            }
        }
    }
}
