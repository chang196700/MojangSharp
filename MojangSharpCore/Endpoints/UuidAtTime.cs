using System;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;
using Newtonsoft.Json.Linq;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// UuidAtTime request class
    /// </summary>
    public class UuidAtTime : IEndpoint<UuidAtTimeResponse>
    {
        /// <summary>
        /// Instantiates the endpoints which allows to get a player's UUID at a certain time.
        /// <paramref name="username">Username of the player you want to get UUID's</paramref>
        /// <paramref name="date">Date at which you want to get the UUID</paramref>
        /// </summary>
        public UuidAtTime(string username, DateTime date)
        {
            int timespan = (int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            Address = new Uri($"https://api.mojang.com/users/profiles/minecraft/{username}?at={timespan}");
            Arguments.Add(username);
            Arguments.Add(timespan.ToString());
        }

        /// <summary>
        /// Performs an UuidAtTime request.
        /// </summary>
        /// <returns></returns>
        public override async Task<UuidAtTimeResponse> PerformRequestAsync()
        {
            Response = await Requester.Get(this);

            if (Response.IsSuccess)
            {
                JObject uuid = JObject.Parse(Response.RawMessage);

                // Fixing #6 - 13/04/2018
                return new UuidAtTimeResponse(Response)
                {
                    Uuid = new Uuid()
                    {
                        PlayerName = uuid["name"].ToObject<string>(),
                        Value = uuid["id"].ToObject<string>(),

                        // The accuracy of these verifications have to be verified.
                        Legacy = Response.RawMessage.Contains("\"legacy\""),
                        Demo = Response.RawMessage.Contains("\"demo\""),
                    },
                    //Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(this.Arguments[1])).ToLocalTime()
                };
            }
            else
            {
                return new UuidAtTimeResponse(Error.GetError(Response));
            }
        }
    }
}
