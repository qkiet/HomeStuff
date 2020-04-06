using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeStuff.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage
    {
        public EntryPage()
        {
            InitializeComponent();
        }

        async void NextPageHandler(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new MainPage(network_id_text.Text, pass_text.Text));
        }
    }
}