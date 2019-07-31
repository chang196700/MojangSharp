using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;
using Newtonsoft.Json.Linq;
using static MojangSharpCore.Responses.ChallengesResponse;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Challenges request class
    /// </summary>
    public class Challenges : IEndpoint<ChallengesResponse>
    {
        /// <summary>
        /// Instantiates the endpoints which allows to get an user's challenges.
        /// </summary>
        /// <param name="token">A valid user token.</param>
        public Challenges(string accessToken)
        {
            Address = new Uri("https://api.mojang.com/user/security/challenges");
            Arguments.Add(accessToken);
        }

        /// <summary>
        /// Performs the request and return the Response property.
        /// </summary>
        public override async Task<ChallengesResponse> PerformRequestAsync()
        {
            Response = await Requester.Get(this, true);

            if (Response.IsSuccess)
            {
                JArray jchallenges = JArray.Parse(Response.RawMessage);
                List<Challenge> challenges = new List<Challenge>();
                foreach (JToken token in jchallenges)
                {
                    challenges.Add(ChallengesResponse.Parse(token));
                }

                return new ChallengesResponse(Response)
                {
                    Challenges = challenges
                };
            }
            else
            {
                return new ChallengesResponse(Error.GetError(Response));
            }
        }
    }
}
