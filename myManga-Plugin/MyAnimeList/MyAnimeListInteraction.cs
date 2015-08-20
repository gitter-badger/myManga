using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyAnimeList
{
    // TODO The username / password should be encrypted and stored on the machine so that it doesn't have to be input every time

    /// <summary>
    /// Interaction library for MyAnimeList
    /// </summary>
    public class MyAnimeListInteraction : IMyAnimeList
    {
        #region Members
        private const string BaseUrl = "http://myanimelist.net/api/";
        private byte[] _credentials;
        private string _username;
        #endregion

        #region Constructor
        public MyAnimeListInteraction() { }
        #endregion

        #region Methods
        /// <summary>
        /// Confirm Login Details for users MyAnimeList account
        /// As there is no "Login" for MyAnimeList, this just confirms that the credentials are correct
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        async Task<bool> IMyAnimeList.ConfirmLoginDetails(string username, string password)
        {
            _username = username;
            _credentials = Encoding.ASCII.GetBytes($"{username}:{password}");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(_credentials));

            var res = await client.GetAsync(BaseUrl + "account/verify_credentials.xml");
            return res.IsSuccessStatusCode;
        }

        /// <summary>
        /// Lists the users manga in the console window
        /// </summary>
        [Conditional("DEBUG")]
        async void GetUserManga(string username = "")
        {
            var client = new HttpClient();

            var res = await client.GetAsync($"http://myanimelist.net/malappinfo.php?u={_username}&status=all&type=manga");
            var responseData = await res.Content.ReadAsStringAsync();
            Console.WriteLine(responseData);
        }
        #endregion
    }
}
