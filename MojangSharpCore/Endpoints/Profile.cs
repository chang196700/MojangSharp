using System;
using System.Net;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;
using Newtonsoft.Json.Linq;
using static MojangSharpCore.Responses.ProfileResponse;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Profile request class
    /// </summary>
    public class Profile : IEndpoint<ProfileResponse>
    {
        // TODO RATE LIMIT

        /// <summary>
        /// Applies unsigned setting to the request
        /// </summary>
        public bool Unsigned { get; private set; }

        /// <summary>
        /// Returns player's username and additional informations
        /// </summary>
        /// <param name="uuid">Player UUID</param>
        /// <param name="unsigned"></param>
        public Profile(string uuid, bool unsigned = true)
        {
            Unsigned = unsigned;

            if (Unsigned)
            {
                Address = new Uri($"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}");
            }
            else
            {
                Address = new Uri($"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}?unsigned=false");
            }

            Arguments.Add(uuid);
            Arguments.Add(unsigned.ToString());
        }

        /// <summary>
        /// Performs the profile request.
        /// </summary>
        public override async Task<ProfileResponse> PerformRequestAsync()
        {
            Response = await Requester.Get(this);

            if (Response.IsSuccess)
            {
                JObject profile = JObject.Parse(Response.RawMessage);

                return new ProfileResponse(Response)
                {
                    Uuid = new Uuid()
                    {
                        PlayerName = profile["name"].ToObject<string>(),
                        Value = profile["id"].ToObject<string>(),
                        Legacy = null,
                        Demo = null,
                    },
                    Properties = new ProfileProperties(profile["properties"].ToObject<JArray>()[0]["value"].ToObject<string>())
                };
            }
            else
            {
                if (Response.Code == (HttpStatusCode)429)
                {
                    ProfileResponseError error = new ProfileResponseError(JObject.Parse(Response.RawMessage));
                    return new ProfileResponse(Response) { Error = error };
                }
                else
                {
                    return new ProfileResponse(Error.GetError(Response));
                }
            }
        }
    }
}
