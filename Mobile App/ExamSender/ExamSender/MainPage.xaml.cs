using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;

namespace ExamSender
{
    /// <summary>
    /// Main (and only) page of the application
    /// </summary>
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Link to the Azure Logic App (to be added)
        /// </summary>
        private const string link = "https://prod-00.northeurope.logic.azure.com:443/workflows/89f2a07ab3b6498387819131fe1db8de/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=RRA6YXgCxsmOnkUN-HNReQLRBG0gqUcPBZDrLx6npHE";
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handler of send button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void OnSendClicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var obj = new
                    {
                        name = Name.Text,
                        surname = Surname.Text,
                        text = Work.Text
                    };

                    var payload = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(payload, Encoding.UTF8, @"application/json");
                    var response = await client.PostAsync(link, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var phrases = createKeyPhrasesList(responseBody);
                        string result = string.Empty;
                        foreach(var phrase in phrases)
                        {
                            result += phrase + "\n";
                        }
                        await DisplayAlert("Ура",
                            "Список ключевых фраз\n" + result,
                            "OK");
                    }
                    else
                        throw new InvalidOperationException();
                }
            }
            catch
            {
                await DisplayAlert("Увы", "Что-то пошло не так. Попробуйте снова.", "OK");
            }
            
            
        }

        /// <summary>
        /// Function that creates an array of key phrases
        /// </summary>
        /// <param name="response">Response string from API</param>
        /// <returns></returns>
        string[] createKeyPhrasesList(string response)
        {
            response = response.Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace("\"", string.Empty);

            return response.Split(',');
        }
    }
}
