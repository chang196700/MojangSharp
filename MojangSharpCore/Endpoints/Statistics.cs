using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MojangSharpCore.Api;
using MojangSharpCore.Responses;
using Newtonsoft.Json.Linq;

namespace MojangSharpCore.Endpoints
{
    /// <summary>
    /// Statistics requets class
    /// </summary>
    public class Statistics : IEndpoint<StatisticsResponse>
    {
        /// <summary>
        /// Asks Mojang for its statistics about the given item
        /// </summary>
        public Statistics(List<Item> items) : this(items.ToArray()) { }

        /// <summary>
        /// Asks Mojang for its statistics about the given item
        /// </summary>
        public Statistics(params Item[] items)
        {
            Address = new Uri($"https://api.mojang.com/orders/statistics");
            foreach (Item item in items)
            {
                Arguments.Add(Statistics.StatisticItems[item]);
            }
        }

        /// <summary>
        /// Performs a statistic request
        /// </summary>
        /// <returns></returns>
        public override async Task<StatisticsResponse> PerformRequestAsync()
        {
            PostContent = "{ \"metricKeys\": [" + string.Join(",", Arguments.ConvertAll(x => $"\"{x.ToString()}\"").ToArray()) + "]}";
            Response = await Requester.Post(this);

            if (Response.IsSuccess)
            {
                JObject stats = JObject.Parse(Response.RawMessage);

                return new StatisticsResponse(Response)
                {
                    Total = stats["total"].ToObject<int>(),
                    Last24h = stats["last24h"].ToObject<int>(),
                    SaleVelocity = stats["saleVelocityPerSeconds"].ToObject<double>(),
                };
            }
            else
            {
                if (Response.Code == HttpStatusCode.BadRequest)
                {
                    return new StatisticsResponse(new Response(Response)
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
                    return new StatisticsResponse(Error.GetError(Response));
                }
            }
        }

        /// <summary>
        /// List of available statistic items
        /// </summary>
        public static Dictionary<Item, string> StatisticItems = new Dictionary<Item, string>()
        {
            {  Item.MinecraftAccountsSold, "item_sold_minecraft" },
            {  Item.MinecraftPrepaidCardsRedeemed, "prepaid_card_redeemed_minecraft" },
            {  Item.CobaltAccountsSold, "item_sold_cobalt" },
            {  Item.ScrollsAccountsSold, "item_sold_scrolls" },
        };

        /// <summary>
        /// Available statistics entries
        /// </summary>
        public enum Item
        {
            /// <summary>
            /// Amount of minecraft accounts sold
            /// </summary>
            MinecraftAccountsSold,

            /// <summary>
            /// Amount of minecraft prepaid card redeemed
            /// </summary>
            MinecraftPrepaidCardsRedeemed,

            /// <summary>
            /// Amount of cobalt accounts sold
            /// </summary>
            CobaltAccountsSold,

            /// <summary>
            /// Amount of scrolls accounts sold
            /// </summary>
            ScrollsAccountsSold
        }
    }
}
