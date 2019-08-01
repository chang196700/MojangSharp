using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;

namespace MojangSharpCore.Endpoints
{
    public class Texture : IEndpoint<Response>
    {
        public Texture(string hash) => Address = new Uri($"http://textures.minecraft.net/texture/{hash}");

        public override async Task<Response> PerformRequestAsync() => Response = await Requester.Get(this);
    }
}
