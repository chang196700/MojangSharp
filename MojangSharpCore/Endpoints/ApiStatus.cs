﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;
using Newtonsoft.Json.Linq;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Api Status request class
    /// </summary>
    public class ApiStatus : IEndpoint<ApiStatusResponse>
    {
        /// <summary>
        /// Instantiates the endpoints which allows to get Mojang's APIs status.
        /// </summary>
        public ApiStatus() => Address = new Uri("https://status.mojang.com/check");

        /// <summary>
        /// Performs the request and return the Response property.
        /// </summary>
        public override async Task<ApiStatusResponse> PerformRequestAsync()
        {
            Response = await Requester.Get(this);

            if (Response.IsSuccess)
            {
                JArray jstatuses = JArray.Parse(Response.RawMessage);

                // Fixes #6 - 13/04/2018
                Dictionary<string, string> statuses = new Dictionary<string, string>();
                foreach (JObject item in jstatuses)
                {
                    JToken token = item.First;
                    statuses.Add(
                        ((JProperty)token).Name,
                        token.First.ToString());
                }

                return new ApiStatusResponse(Response)
                {
                    Minecraft = ApiStatusResponse.Parse(statuses.TryGetValue("minecraft.net", out string v) ? v : null),
                    Sessions = ApiStatusResponse.Parse(statuses.TryGetValue("session.minecraft.net", out v) ? v : null),
                    MojangAccounts = ApiStatusResponse.Parse(statuses.TryGetValue("account.mojang.com", out v) ? v : null),
                    MojangAutenticationServers = ApiStatusResponse.Parse(statuses.TryGetValue("authserver.mojang.com", out v) ? v : null),
                    MojangSessionsServer = ApiStatusResponse.Parse(statuses.TryGetValue("sessionserver.mojang.com", out v) ? v : null),
                    MojangApi = ApiStatusResponse.Parse(statuses.TryGetValue("api.mojang.com", out v) ? v : null),
                    Textures = ApiStatusResponse.Parse(statuses.TryGetValue("textures.minecraft.net", out v) ? v : null),
                    Mojang = ApiStatusResponse.Parse(statuses.TryGetValue("mojang.com", out v) ? v : null),

                    // These two seems to not get taken into account anymore
                    MojangAuthenticationService = ApiStatusResponse.Parse(statuses.TryGetValue("auth.mojang.com", out v) ? v : null),
                    Skins = ApiStatusResponse.Parse(statuses.TryGetValue("skins.minecraft.net", out v) ? v : null),
                };
            }
            else
            {
                return new ApiStatusResponse(Error.GetError(Response));
            }
        }
    }
}
