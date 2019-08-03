using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;
using Newtonsoft.Json.Linq;

namespace MojangSharpCore.Endpoints
{
    public class Join : IEndpoint<Response>
    {
        public Join(string accessToken, string selectedProfile, string serverId)
        {
            Address = new Uri("https://sessionserver.mojang.com/session/minecraft/join");
            Arguments.Add(accessToken);
            Arguments.Add(selectedProfile);
            Arguments.Add(serverId);
        }

        public override async Task<Response> PerformRequestAsync()
        {
            PostContent = new JObject(
                new JProperty("accessToken", Arguments[0]),
                new JProperty("selectedProfile", Arguments[1]),
                new JProperty("serverId", Arguments[2])).ToString();

            Response = await Requester.Post(this);

            if (string.IsNullOrWhiteSpace(Response.RawMessage))
            {
                return new Response(Response) { IsSuccess = true };
            }
            else
            {
                return new Response(Error.GetError(Response));
            }
        }
    }

    public class HasJoin : IEndpoint<HasJoinResponse>
    {
        public HasJoin(string username, string serverId, string ip)
        {
            Address = ip == null ? new Uri($"https://sessionserver.mojang.com/session/minecraft/hasJoined?username={username}&serverId={serverId}") :
                new Uri($"https://sessionserver.mojang.com/session/minecraft/hasJoined?username={username}&serverId={serverId}&ip={ip}");
        }

        public override async Task<HasJoinResponse> PerformRequestAsync()
        {
            Response = await Requester.Get(this);

            if (Response.IsSuccess)
            {
                JObject res = JObject.Parse(Response.RawMessage);
                HasJoinResponse obj = res.ToObject<HasJoinResponse>();

                return new HasJoinResponse(Response)
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Properties = obj.Properties
                };
            }
            else
            {
                return new HasJoinResponse(Error.GetError(Response));
            }
        }
    }
}
