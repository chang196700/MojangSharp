using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Upload Skin endpoint class
    /// </summary>
    public class UploadSkin : IEndpoint<Response>
    {
        /// <summary>
        /// Chosen skin local path
        /// </summary>
        public FileInfo Skin { get; internal set; }

        /// <summary>
        /// Creates a change skin request with a given UUID.
        /// </summary>
        /// <param name="accessToken">Access Token of the player.</param>
        /// <param name="uuid">UUID of the player.</param>
        /// <param name="skin">Path to the skin.</param>
        /// <param name="slim">Defines if slim model is used.</param>
        public UploadSkin(string accessToken, string uuid, FileInfo skin, bool slim = false)
        {
            Address = new Uri($"https://api.mojang.com/user/profile/{uuid}/skin");
            Arguments.Add(accessToken);
            Arguments.Add(slim.ToString());
            Skin = skin;
        }

        /// <summary>
        /// Performs the skin change.
        /// </summary>
        public override async Task<Response> PerformRequestAsync()
        {
            Response = await Requester.Put(this, Skin);

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
