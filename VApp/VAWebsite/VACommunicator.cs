using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VApp.ViewModels;

namespace VApp.VAWebsite
{
    public static class VACommunicator
    {
        private const string tokenId = "loginPortlet_learnAboutorg.apache.struts.taglib.html.TOKEN";
        private const string usernameId = "loginPortlet_learnAbout{actionForm.userName}";
        private const string passwordId = "loginPortlet_learnAbout{actionForm.password}";

        private static HttpClient client = new HttpClient();

        public static async Task<bool> Authenticate(string username, string password)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://www.myhealth.va.gov/mhv-portal-web/anonymous.portal");
                string page = await response.Content.ReadAsStringAsync();
                string token = ExtractToken(page);
                string postUri = ExtractAction(page);

                List<KeyValuePair<string, string>> postParamters = new List<KeyValuePair<string, string>>();
                postParamters.Add(new KeyValuePair<string, string>(tokenId, token));
                postParamters.Add(new KeyValuePair<string, string>(usernameId, username));
                postParamters.Add(new KeyValuePair<string, string>(passwordId, password));

                HttpContent content = new FormUrlEncodedContent(postParamters);

                response = await client.PostAsync(postUri, content);
                page = await response.Content.ReadAsStringAsync();

                return page.Contains("Logged On As:");
            }
            catch
            {
                return false;
            }
        }

        public static async void PopulateProfile(ProfileViewModel profile)
        {
            HttpResponseMessage response = await client.GetAsync("https://www.myhealth.va.gov/mhv-portal-web/mhv.portal?_nfpb=true&_pageLabel=profiles&_nfls=false");
            string page = await response.Content.ReadAsStringAsync();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(page);

            profile.FirstName = document.GetElementbyId("firstName").GetAttributeValue("value", string.Empty);
            profile.MiddleName = document.GetElementbyId("middleName").GetAttributeValue("value", string.Empty);
            profile.LastName = document.GetElementbyId("lastName").GetAttributeValue("value", string.Empty);
            profile.Alias = document.GetElementbyId("userAlias").GetAttributeValue("value", string.Empty);
            profile.Occupation = document.GetElementbyId("currentOccupation").GetAttributeValue("value", string.Empty);
            profile.MaritalStatus = document.GetElementbyId("maritalStatus").ChildNodes.Single(c => c.Attributes.Contains("selected")).NextSibling.InnerText;
        }

        private static string ExtractToken(string pageHtml)
        {
            Regex tokenRegex = new Regex("TOKEN\" value=\"[A-Za-z0-9]*\"");
            Match match = tokenRegex.Match(pageHtml);
            return match.Value.Replace("TOKEN\" value=", string.Empty).Replace("\"", string.Empty);
        }

        private static string ExtractAction(string pageHtml)
        {
            Regex tokenRegex = new Regex("<form action=\"https://www.myhealth.va.gov:443/mhv-portal-web/anonymous.portal.*\"");
            Match match = tokenRegex.Match(pageHtml);
            return match.Value.Replace("<form action=\"", string.Empty).Replace(" method=\"post\"", string.Empty).Replace("\"", string.Empty);
        }
    }
}
