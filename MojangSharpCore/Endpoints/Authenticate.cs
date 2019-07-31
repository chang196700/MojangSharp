using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;
using Newtonsoft.Json.Linq;
using static MojangSharpCore.Responses.AuthenticateResponse;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Represents a couple of username and password for authentication purposes
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Authenticate request class
    /// </summary>
    public class Authenticate : IEndpoint<AuthenticateResponse>
    {
        /// <summary>
        /// Sends a request of authentication
        /// </summary>
        public Authenticate(Credentials credentials)
        {
            Address = new Uri($"https://authserver.mojang.com/authenticate");
            Arguments.Add(credentials.Username);
            Arguments.Add(credentials.Password);
        }

        /// <summary>
        /// Performs the authentication.
        /// </summary>
        public override async Task<AuthenticateResponse> PerformRequestAsync()
        {
            PostContent = new JObject(
                                    new JProperty("agent",
                                        new JObject(
                                            new JProperty("name", "Minecraft"),
                                            new JProperty("version", "1"))),
                                    new JProperty("username", Arguments[0]),
                                    new JProperty("password", Arguments[1]),
                                    new JProperty("clientToken", Requester.ClientToken),
                                    new JProperty("requestUser", true)).ToString();

            Response = await Requester.Post(this);
            if (Response.IsSuccess)
            {
                JObject user = JObject.Parse(Response.RawMessage);
                List<Uuid> availableProfiles = new List<Uuid>();

                foreach (JObject profile in user["availableProfiles"])
                {
                    availableProfiles.Add(new Uuid()
                    {
                        PlayerName = profile["name"].ToObject<string>(),
                        Value = profile["id"].ToObject<string>(),
                        Legacy = (profile.ToString().Contains("legacyProfile") ? profile["legacyProfile"].ToObject<bool>() : false),
                        Demo = null
                    });
                }

                return new AuthenticateResponse(Response)
                {
                    AccessToken = user["accessToken"].ToObject<string>(),
                    ClientToken = user["clientToken"].ToObject<string>(),
                    AvailableProfiles = availableProfiles,
                    SelectedProfile = new Uuid()
                    {
                        PlayerName = user["selectedProfile"]["name"].ToObject<string>(),
                        Value = user["selectedProfile"]["id"].ToObject<string>(),
                        Legacy = (user["selectedProfile"].ToString().Contains("legacyProfile") ? user["selectedProfile"]["legacyProfile"].ToObject<bool>() : false),
                        Demo = null
                    },
                    User = user["user"].ToObject<UserData>()
                };
            }
            else
            {
                try
                {
                    AuthenticationResponseError error = new AuthenticationResponseError(JObject.Parse(Response.RawMessage));
                    return new AuthenticateResponse(Response) { Error = error };
                }
                catch (Exception)
                {
                    return new AuthenticateResponse(Error.GetError(Response));
                }
            }
        }
    }

    /// <summary>
    /// Refresh authentication request class
    /// </summary>
    public class Refresh : IEndpoint<TokenResponse>
    {
        /// <summary>
        /// Refreshes the access token. Must be the same instance as authenticate.
        /// </summary>
        public Refresh(string accessToken)
        {
            Address = new Uri($"https://authserver.mojang.com/refresh");
            Arguments.Add(accessToken);
        }

        /// <summary>
        /// Performs refresh token.
        /// </summary>
        /// <returns></returns>
        public override async Task<TokenResponse> PerformRequestAsync()
        {
            PostContent = new JObject(
                                    new JProperty("accessToken", Arguments[0]),
                                    new JProperty("clientToken", Requester.ClientToken)).ToString();

            Response = await Requester.Post(this);

            if (Response.IsSuccess)
            {
                JObject refresh = JObject.Parse(Response.RawMessage);

                return new TokenResponse(Response)
                {
                    AccessToken = refresh["accessToken"].ToObject<string>()
                };
            }
            else
            {
                return new TokenResponse(Error.GetError(Response));
            }
        }
    }

    /// <summary>
    /// Validate token request class
    /// </summary>
    public class Validate : IEndpoint<Response>
    {
        /// <summary>
        /// Refreshes the access token. Must be the same instance as authenticate.
        /// </summary>
        public Validate(string accessToken)
        {
            Address = new Uri($"https://authserver.mojang.com/validate");
            Arguments.Add(accessToken);
        }

        /// <summary>
        /// Performs validate token.
        /// </summary>
        public override async Task<Response> PerformRequestAsync()
        {
            PostContent = new JObject(
                                    new JProperty("accessToken", Arguments[0]),
                                    new JProperty("clientToken", Requester.ClientToken)).ToString();

            Response = await Requester.Post(this);

            if (Response.Code == HttpStatusCode.NoContent)
            {
                return new Response(Response) { IsSuccess = true };
            }
            else
            {
                return new Response(Error.GetError(Response));
            }
        }
    }

    /// <summary>
    /// Sign out request class
    /// </summary>
    public class Signout : IEndpoint<Response>
    {
        /// <summary>
        /// Refreshes the access token. Must be the same instance as authenticate.
        /// </summary>
        public Signout(Credentials credentials)
        {
            Address = new Uri($"https://authserver.mojang.com/signout");
            Arguments.Add(credentials.Username);
            Arguments.Add(credentials.Password);
        }

        /// <summary>
        /// Performs signing out
        /// </summary>
        public override async Task<Response> PerformRequestAsync()
        {
            PostContent = new JObject(
                                    new JProperty("username", Arguments[0]),
                                    new JProperty("password", Arguments[1])).ToString();

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

    /// <summary>
    /// Invalidate token request class
    /// </summary>
    public class Invalidate : IEndpoint<Response>
    {
        /// <summary>
        /// Refreshes the access token. Must be the same instance as authenticate.
        /// </summary>
        public Invalidate(string accessToken)
        {
            Address = new Uri($"https://authserver.mojang.com/invalidate");
            Arguments.Add(accessToken);
        }

        /// <summary>
        /// Performs validate token.
        /// </summary>
        public override async Task<Response> PerformRequestAsync()
        {
            PostContent = new JObject(
                                    new JProperty("accessToken", Arguments[0]),
                                    new JProperty("clientToken", Requester.ClientToken)).ToString();

            Response = await Requester.Post(this);

            if (Response.Code == HttpStatusCode.NoContent)
            {
                return new Response(Response) { IsSuccess = true }; ;
            }
            else
            {
                return new Response(Error.GetError(Response));
            }
        }
    }
}
