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
        private const string link = "<LINK_TO_LOGIC_APP>";
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
                        work = Work.Text
                    };

                    var payload = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(payload, Encoding.UTF8, @"application/json");
                    var response = await client.PostAsync(link, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        if (responseBody == "true")
                        {
                            await DisplayAlert("Ура!", "Работа успешно отправлена.", "OK");
                        }
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
    }
}
