using System.Collections.Generic;
using MojangSharpCore.Api;

namespace MojangSharpCore.Responses
{
    /// <summary>
    /// Response containing the list of UUIDs
    /// </summary>
    public class UuidByNamesResponse : Response
    {
        internal UuidByNamesResponse(Response response) : base(response)
        {
        }

        /// <summary>
        /// The list of UUID corresponding to the names.
        /// </summary>
        public List<Uuid> UuidList { get; internal set; }
    }
}
