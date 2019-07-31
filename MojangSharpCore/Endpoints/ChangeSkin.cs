using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Changes a player's skin from an url
    /// </summary>
    public class ChangeSkin : IEndpoint<Response>
    {
        /// <summary>
        /// Creates a change skin request with a given UUID.
        /// </summary>
        /// <param name="accessToken">User Access Token</param>
        /// <param name="uuid">UUID of the player.</param>
        /// <param name="skinUrl">URL of the skin.</param>
        /// <param name="slim">Defines if slim model is used.</param>
        public ChangeSkin(string accessToken, string uuid, string skinUrl, bool slim = false)
        {
            Address = new Uri($"https://api.mojang.com/user/profile/{uuid}/skin");
            Arguments.Add(accessToken);
            Arguments.Add(skinUrl);
            Arguments.Add(slim.ToString());
        }

        /// <summary>
        /// Performs the skin change.
        /// </summary>
        public override async Task<Response> PerformRequestAsync()
        {
            Response = await Requester.Post(this, new Dictionary<string, string>() {
                { "model", (bool.Parse(Arguments[2]) == true ? "slim" : null) },
                { "url", Arguments[1] },
            });

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
