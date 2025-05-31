namespace Siomura
{

    public partial class Products : ContentPage
    {
        public Products()
        {
            InitializeComponent();
        }

        public async void OnOrderNowTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderNow());
        }
    }
}