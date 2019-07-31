using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;
using Newtonsoft.Json.Linq;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// UuidByName requests class
    /// </summary>
    public class UuidByNames : IEndpoint<UuidByNamesResponse>
    {
        /// <summary>
        /// Obtains a list of Uuid corresponding to the given usernames
        /// </summary>
        /// <param name="usernames"></param>
        public UuidByNames(List<string> usernames) : this(usernames.ToArray())
        {
        }

        /// <summary>
        /// Obtains a list of Uuid corresponding to the given usernames
        /// </summary>
        /// <param name="usernames"></param>
        public UuidByNames(params string[] usernames)
        {
            if (usernames.Length > 100)
            {
                throw new ArgumentException("Only up to 100 usernames per request are allowed.");
            }

            Address = new Uri($"https://api.mojang.com/profiles/minecraft");
            Arguments = usernames.ToList<string>();
        }

        /// <summary>
        /// Performs an UuidByNames request.
        /// </summary>
        /// <returns></returns>
        public override async Task<UuidByNamesResponse> PerformRequestAsync()
        {
            PostContent = "[" + string.Join(",", Arguments.ConvertAll(x => $"\"{x.ToString()}\"").ToArray()) + "]";
            Response = await Requester.Post(this);

            if (Response.IsSuccess)
            {
                JArray uuids = JArray.Parse(Response.RawMessage);
                List<Uuid> uuidList = new List<Uuid>() { };

                foreach (JObject uuid in uuids)
                {
                    uuidList.Add(uuid.ToObject<Uuid>());
                }

                return new UuidByNamesResponse(Response)
                {
                    UuidList = uuidList,
                };
            }
            else
            {
                if (Response.Code == HttpStatusCode.BadRequest)
                {
                    return new UuidByNamesResponse(new Response(Response)
                    {
                        Error =
                        {
                            ErrorMessage = "One of the usernames is empty.",
                            ErrorTag = "IllegalArgumentException"
                        }
                    });
                }
                else
                {
                    return new UuidByNamesResponse(Error.GetError(Response));
                }
            }
        }
    }
}
