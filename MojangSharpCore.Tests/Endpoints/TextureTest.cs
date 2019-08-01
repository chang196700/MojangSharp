using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MojangSharpCore.Endpoints;

namespace MojangSharpCore.Tests.Endpoints
{
    [TestClass]
    public class TextureTest
    {
        [DataTestMethod]
        [DataRow("6f2dec282967a2e068755da69c251adf944f2fa022eca8e31c0b18a9413305ec")]
        public async Task PerformRequest_Success(string hash)
        {
            var result = await new Texture(hash).PerformRequestAsync();

            Assert.IsTrue(result.IsSuccess);
        }

        [DataTestMethod]
        [DataRow("6f2dec282967a2e068755da69c251adf944f2fa022eca8e31c0b18a9413305e")]
        public async Task PerformRequest_Failed(string hash)
        {
            var result = await new Texture(hash).PerformRequestAsync();

            Assert.IsFalse(result.IsSuccess);
        }
    }
}
