using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.WebUtilities;
using Newtonsoft.Json.Linq;

namespace SE344.Services.Facebook
{
    public class FacebookApiService
    {
        private readonly HttpClient _client = new HttpClient();

        public FacebookApiService(string accessToken = "")
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; set; }

        /// <summary>
        /// Get the current user's feed
        /// </summary>
        /// <returns>A <see cref="JArray"/> of user's posts</returns>
        public async Task<JToken> GetUserFeedAsync()
        {
            var query = new Dictionary<string, string> {{"fields", "from{name,id,link},message,created_time"}};
            var res = await _client.GetStringAsync(BuildApiUrl("me/feed", query));
            return JObject.Parse(res)["data"];
        }

        /// <summary>
        /// Post a status update for the current user
        /// </summary>
        /// <param name="message">The post content</param>
        /// <returns>The ID of the newly created post if success</returns>
        public async Task<string> PostUserFeedAsync(string message)
        {
            var postData = new Dictionary<string, string> {{"message", message}};
            var res = await _client.PostAsync(BuildApiUrl("me/feed"), new FormUrlEncodedContent(postData));

            if (res.IsSuccessStatusCode)
            {
                return (string) JObject.Parse(await res.Content.ReadAsStringAsync())["id"];
            }
            return string.Empty;
        }

        private string BuildApiUrl(string path, IDictionary<string, string> query = null)
        {
            if (path == null) throw new ArgumentNullException();

            var queryString = new Dictionary<string, string>(query ?? new Dictionary<string, string>())
            {
                {"access_token", AccessToken}
            };

            return QueryHelpers.AddQueryString("https://graph.facebook.com/v2.4/" + path, queryString);
        }
    }
}
