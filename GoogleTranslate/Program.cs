using Google.Apis.Services;
using Google.Apis.Translate.v2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trx = Google.Apis.Translate.v2.TranslationsResource;

namespace GoogleTranslate
{
    class Program
    {
        static async Task Main(string[] args)
        {
             Dictionary<string, string> keyValue = new Dictionary<string, string>();

            var service = new TranslateService(new BaseClientService.Initializer()
            {
                ApiKey = ConfigurationManager.AppSettings.Get("GoogleTranslateAPIKey").ToString(),
                ApplicationName = ConfigurationManager.AppSettings.Get("GoogleTranslateApplicationName").ToString()
            });

            string json = @"{""Name"":""Mahesh"",""LastName"":""B"",""Gender"":""Male"",""Sample"":""Test""}";

            //Deserialize the JSON string to Disctionary, as Google translate API requires IEnumerable<string> as input
            keyValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            
            //Call google API for Translation
            var response = await service.Translations.List(keyValue.Values.ToArray<string>(), "fr").ExecuteAsync();

            for (int i = 0; i < response.Translations.Count; i++)
            {
                //Get specific key at element and replace the translated text
                keyValue[keyValue.Keys.ElementAt(i)] = response.Translations[i].TranslatedText;
            }

            //Serialize key value pair back to JSON 
            var outputJsonResult = JsonConvert.SerializeObject(keyValue);
        }
    }
}
