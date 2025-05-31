using Microsoft.Maui.Controls;

namespace Siomura
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent(); 
        }

        private async void OnProductsTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Products());
        }
    }
}
