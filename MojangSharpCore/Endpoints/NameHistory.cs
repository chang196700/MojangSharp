using System;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;
using Newtonsoft.Json.Linq;
using static MojangSharpCore.Responses.NameHistoryResponse;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// UUID to name history endpoint.
    /// </summary>
    public class NameHistory : IEndpoint<NameHistoryResponse>
    {
        /// <summary>
        /// Return all the usernames the user has used in the past.
        /// </summary>
        /// <param name="uuid">User's UUID.</param>
        public NameHistory(string uuid) => Address = new Uri($"https://api.mojang.com/user/profiles/{uuid}/names");

        /// <summary>
        /// Performs a name history request.
        /// </summary>
        /// <returns></returns>
        public override async Task<NameHistoryResponse> PerformRequestAsync()
        {
            Response = await Requester.Get(this);

            if (Response.IsSuccess)
            {
                JArray entries = JArray.Parse(Response.RawMessage);

                NameHistoryResponse history = new NameHistoryResponse(Response);
                foreach (JToken entry in entries)
                {
                    history.NameHistory.Add(new NameHistoryEntry()
                    {
                        Name = entry["name"].ToObject<string>(),
                        ChangedToAt = (entry.Last != entry.First ?
                                      new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(entry["changedToAt"].ToObject<double>()).ToLocalTime() :
                                      new DateTime?())
                    });
                }
                return history;
            }
            else
            {
                return new NameHistoryResponse(Error.GetError(Response));
            }
        }
    }
}
