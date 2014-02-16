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
        private const string loginTokenName = "loginPortlet_learnAboutorg.apache.struts.taglib.html.TOKEN";
        private const string profileTokenName = "manageUserProfile_profilesorg.apache.struts.taglib.html.TOKEN";
        private const string usernameName = "loginPortlet_learnAbout{actionForm.userName}";
        private const string passwordName = "loginPortlet_learnAbout{actionForm.password}";
        private const string firstNameName = "manageUserProfile_profiles{actionForm.userProfileFirstName}";
        private const string middleNameName = "manageUserProfile_profiles{actionForm.userProfileMiddleName}";
        private const string lastNameName = "manageUserProfile_profiles{actionForm.userProfileLastName}";
        private const string genderName = "manageUserProfile_profiles{actionForm.userProfileGender}";
        private const string birthMonthName = "manageUserProfile_profiles{actionForm.userProfileBirthDateMonth}";
        private const string birthDateName = "manageUserProfile_profiles{actionForm.userProfileBirthDateDay}";
        private const string birthYearName = "manageUserProfile_profiles{actionForm.userProfileBirthDateYear}";
        private const string aliasName = "manageUserProfile_profiles{actionForm.userProfileUserAlias}";
        private const string maritalStatusName = "manageUserProfile_profileswlw-select_key:{actionForm.userProfileMaritalStatus}";
        private const string occupationName = "manageUserProfile_profiles{actionForm.userProfileCurrentOccupation}";
        private const string contactMethodName = "manageUserProfile_profileswlw-select_key:{actionForm.userProfileContactInfoContactMethod}";
        private const string emailName = "manageUserProfile_profiles{actionForm.userProfileContactInfoEmail}";
        private const string mobilePhoneName = "manageUserProfile_profiles{actionForm.userProfileContactInfoMobilePhone}";
        private const string homePhoneName = "manageUserProfile_profiles{actionForm.userProfileContactInfoHomePhone}";
        private const string workPhoneName = "manageUserProfile_profiles{actionForm.userProfileContactInfoWorkPhone}";
        private const string faxName = "manageUserProfile_profiles{actionForm.userProfileContactInfoFax}";
        private const string pagerName = "manageUserProfile_profiles{actionForm.userProfileContactInfoPager}";
        private const string address1Name = "manageUserProfile_profiles{actionForm.userProfileAddressStreet1}";
        private const string address2Name = "manageUserProfile_profiles{actionForm.userProfileAddressStreet2}";
        private const string cityName = "manageUserProfile_profiles{actionForm.userProfileAddressCity}";
        private const string stateName = "manageUserProfile_profileswlw-select_key:{actionForm.userProfileAddressState}";
        private const string zipName = "manageUserProfile_profiles{actionForm.userProfileAddressPostalCode}";
        private const string provinceName = "manageUserProfile_profiles{actionForm.userProfileAddressProvince}";
        private const string countryName = "manageUserProfile_profileswlw-select_key:{actionForm.userProfileAddressCountry}";
        private const string question1Name = "manageUserProfile_profileswlw-select_key:{actionForm.userProfilePasswordHintQuestion1}";
        private const string question2Name = "manageUserProfile_profileswlw-select_key:{actionForm.userProfilePasswordHintQuestion2}";
        private const string answer1Name = "manageUserProfile_profiles{actionForm.userProfilePasswordHintAnswer1}";
        private const string answer2Name = "manageUserProfile_profiles{actionForm.userProfilePasswordHintAnswer2}";
        private const string privacyName = "manageUserProfile_profiles{actionForm.userProfileAcceptPrivacy}";
        private const string termsName = "manageUserProfile_profiles{actionForm.userProfileAcceptTerms}";
        private const string isVAPatientName = "manageUserProfile_profileswlw-checkbox_key:{actionForm.userProfileIsPatient}";
        private const string isVeteranName = "manageUserProfile_profileswlw-checkbox_key:{actionForm.userProfileIsVeteran}";
        private const string isVeteranAdvocateName = "manageUserProfile_profileswlw-checkbox_key:{actionForm.userProfileIsPatientAdvocate}";
        private const string isVAEmployeeName = "manageUserProfile_profileswlw-checkbox_key:{actionForm.userProfileIsEmployee}";
        private const string isHealthProviderName = "manageUserProfile_profileswlw-checkbox_key:{actionForm.userProfileIsHealthCareProvider}";
        private const string isOtherName = "manageUserProfile_profileswlw-checkbox_key:{actionForm.userProfileIsOther}";

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
                postParamters.Add(new KeyValuePair<string, string>(loginTokenName, token));
                postParamters.Add(new KeyValuePair<string, string>(usernameName, username));
                postParamters.Add(new KeyValuePair<string, string>(passwordName, password));

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

            profile.Token = ExtractToken(page);
            profile.FirstName = document.GetElementbyId("firstName").GetAttributeValue("value", string.Empty);
            profile.MiddleName = document.GetElementbyId("middleName").GetAttributeValue("value", string.Empty);
            profile.LastName = document.GetElementbyId("lastName").GetAttributeValue("value", string.Empty);
            profile.Alias = document.GetElementbyId("userAlias").GetAttributeValue("value", string.Empty);
            profile.Gender = document.GetElementbyId("gender").GetAttributeValue("value", string.Empty);
            profile.BirthMonth = document.GetElementbyId("monthSelect").GetAttributeValue("value", string.Empty);
            profile.BirthDate = document.GetElementbyId("daySelect").GetAttributeValue("value", string.Empty);
            profile.BirthYear = document.GetElementbyId("yearSelect").GetAttributeValue("value", string.Empty);
            profile.Occupation = document.GetElementbyId("currentOccupation").GetAttributeValue("value", string.Empty);
            profile.MaritalStatus = document.GetElementbyId("maritalStatus").ChildNodes.Single(c => c.Attributes.Contains("selected")).NextSibling.InnerText;
            profile.ContactMethod = document.GetElementbyId("contactMethod").ChildNodes.Single(c => c.Attributes.Contains("selected")).NextSibling.InnerText;
            profile.Email = document.GetElementbyId("primaryEmail").GetAttributeValue("value", string.Empty);
            profile.MobilePhone = document.GetElementbyId("mobilePhone").GetAttributeValue("value", string.Empty);
            profile.HomePhone = document.GetElementbyId("homePhone").GetAttributeValue("value", string.Empty);
            profile.WorkPhone = document.GetElementbyId("workPhone").GetAttributeValue("value", string.Empty);
            profile.Fax = document.GetElementbyId("fax").GetAttributeValue("value", string.Empty);
            profile.Pager = document.GetElementbyId("pager").GetAttributeValue("value", string.Empty);
            profile.AddressLine1 = document.GetElementbyId("street1").GetAttributeValue("value", string.Empty);
            profile.AddressLine2 = document.GetElementbyId("street2").GetAttributeValue("value", string.Empty);
            profile.City = document.GetElementbyId("city").GetAttributeValue("value", string.Empty);
            profile.State = document.GetElementbyId("state").ChildNodes.Single(c => c.Attributes.Contains("selected")).NextSibling.InnerText;
            profile.ZipCode = document.GetElementbyId("postalCode").GetAttributeValue("value", string.Empty);
            profile.Province = document.GetElementbyId("province").GetAttributeValue("value", string.Empty);
            profile.Country = document.GetElementbyId("country").ChildNodes.Single(c => c.Attributes.Contains("selected")).NextSibling.InnerText;
            profile.Question1 = document.GetElementbyId("hintQuestion1").ChildNodes.Single(c => c.Attributes.Contains("selected")).NextSibling.InnerText;
            profile.Question2 = document.GetElementbyId("hintQuestion2").ChildNodes.Single(c => c.Attributes.Contains("selected")).NextSibling.InnerText;
            profile.Answer1 = document.GetElementbyId("hintAnswer1").GetAttributeValue("value", string.Empty);
            profile.Answer2 = document.GetElementbyId("hintAnswer2").GetAttributeValue("value", string.Empty);
            profile.PrivacyAccepted = document.DocumentNode.Descendants().Single(d => string.Equals(d.GetAttributeValue("name", string.Empty), privacyName, StringComparison.CurrentCultureIgnoreCase)).GetAttributeValue("value", false);
            profile.TermsAccepted = document.DocumentNode.Descendants().Single(d => string.Equals(d.GetAttributeValue("name", string.Empty), termsName, StringComparison.CurrentCultureIgnoreCase)).GetAttributeValue("value", false);
            profile.IsVAPatient = document.GetElementbyId("vaPatient").GetAttributeValue("value", true);
            profile.IsVeteranAdvocate = document.GetElementbyId("patientAdvocate").Attributes.Contains("checked");
            profile.IsVeteran = document.GetElementbyId("veteran").Attributes.Contains("checked");
            profile.IsVAEmployee = document.GetElementbyId("vaEmployee").Attributes.Contains("checked");
            profile.IsHealthProvider = document.GetElementbyId("healthCareProvider").Attributes.Contains("checked");
            profile.IsOther = document.GetElementbyId("isOther").Attributes.Contains("checked");
        }

        public static async void SaveProfile(ProfileViewModel profile)
        {
            List<KeyValuePair<string, string>> postParamters = new List<KeyValuePair<string, string>>();
            postParamters.Add(new KeyValuePair<string, string>(profileTokenName, profile.Token));
            postParamters.Add(new KeyValuePair<string, string>(firstNameName, profile.FirstName));
            postParamters.Add(new KeyValuePair<string, string>(middleNameName, profile.MiddleName));
            postParamters.Add(new KeyValuePair<string, string>(lastNameName, profile.LastName));
            postParamters.Add(new KeyValuePair<string, string>(aliasName, profile.Alias));
            postParamters.Add(new KeyValuePair<string, string>(genderName, profile.Gender));
            postParamters.Add(new KeyValuePair<string, string>(birthMonthName, profile.BirthMonth));
            postParamters.Add(new KeyValuePair<string, string>(birthDateName, profile.BirthDate));
            postParamters.Add(new KeyValuePair<string, string>(birthYearName, profile.BirthYear));
            postParamters.Add(new KeyValuePair<string, string>(maritalStatusName, profile.MaritalStatus));
            postParamters.Add(new KeyValuePair<string, string>(occupationName, profile.Occupation));
            postParamters.Add(new KeyValuePair<string, string>(contactMethodName, profile.ContactMethod));
            postParamters.Add(new KeyValuePair<string, string>(emailName, profile.Email));
            postParamters.Add(new KeyValuePair<string, string>(mobilePhoneName, profile.MobilePhone));
            postParamters.Add(new KeyValuePair<string, string>(homePhoneName, profile.HomePhone));
            postParamters.Add(new KeyValuePair<string, string>(workPhoneName, profile.WorkPhone));
            postParamters.Add(new KeyValuePair<string, string>(faxName, profile.Fax));
            postParamters.Add(new KeyValuePair<string, string>(pagerName, profile.Pager));
            postParamters.Add(new KeyValuePair<string, string>(address1Name, profile.AddressLine1));
            postParamters.Add(new KeyValuePair<string, string>(address2Name, profile.AddressLine2));
            postParamters.Add(new KeyValuePair<string, string>(cityName, profile.City));
            postParamters.Add(new KeyValuePair<string, string>(stateName, profile.State));
            postParamters.Add(new KeyValuePair<string, string>(zipName, profile.ZipCode));
            postParamters.Add(new KeyValuePair<string, string>(provinceName, profile.Province));
            postParamters.Add(new KeyValuePair<string, string>(countryName, profile.Country));
            postParamters.Add(new KeyValuePair<string, string>(question1Name, profile.Question1));
            postParamters.Add(new KeyValuePair<string, string>(question2Name, profile.Question2));
            postParamters.Add(new KeyValuePair<string, string>(answer1Name, profile.Answer1));
            postParamters.Add(new KeyValuePair<string, string>(answer2Name, profile.Answer2));
            postParamters.Add(new KeyValuePair<string, string>(privacyName, profile.PrivacyAccepted.ToString().ToLower()));
            postParamters.Add(new KeyValuePair<string, string>(termsName, profile.TermsAccepted.ToString().ToLower()));
            postParamters.Add(new KeyValuePair<string, string>(isVAPatientName, profile.IsVAPatient.ToString().ToLower()));
            postParamters.Add(new KeyValuePair<string, string>(isVeteranAdvocateName, profile.IsVeteranAdvocate.ToString().ToLower()));
            postParamters.Add(new KeyValuePair<string, string>(isVeteranName, profile.IsVeteran.ToString().ToLower()));
            postParamters.Add(new KeyValuePair<string, string>(isVAEmployeeName, profile.IsVAEmployee.ToString().ToLower()));
            postParamters.Add(new KeyValuePair<string, string>(isHealthProviderName, profile.IsHealthProvider.ToString().ToLower()));
            postParamters.Add(new KeyValuePair<string, string>(isOtherName, profile.IsOther.ToString().ToLower()));

            HttpContent content = new FormUrlEncodedContent(postParamters);

            HttpResponseMessage response = await client.PostAsync("https://www.myhealth.va.gov:443/mhv-portal-web/mhv.portal?_nfpb=true&_windowLabel=manageUserProfile_profiles&manageUserProfile_profiles_actionOverride=%2Fgov%2Fva%2Fmed%2Fmhv%2Fusermgmt%2Fportlet%2FsaveAction&_pageLabel=profilesHome", content);

            string page = await response.Content.ReadAsStringAsync();
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
