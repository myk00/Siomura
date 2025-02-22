using System;
using Microsoft.Maui.Controls;

namespace Siomura
{
    public partial class OrderNow : ContentPage
    {
        private string selectedProduct = "Siomai Bilao";
        private string selectedImage = "qpal1.jpg"; // Default image
        private decimal selectedPrice = 599; // Correct default price
        private string paymentMethod = string.Empty;

        public OrderNow()
        {
            InitializeComponent();
            UpdateProductDetails();
        }

        private void OnProductChanged(object sender, EventArgs e)
        {
            if (ProductPicker.SelectedIndex == -1) return;

            switch (ProductPicker.SelectedIndex)
            {
                case 0: // Siomai Bilao
                    selectedProduct = "Siomai Bilao";
                    selectedImage = "qpal1.jpg";
                    selectedPrice = 599;
                    break;
                case 1: // Siomai Pugo
                    selectedProduct = "Siomai Pugo";
                    selectedImage = "siopugo.png";
                    selectedPrice = 59;
                    break;
                case 2: // Fried Noodles
                    selectedProduct = "Fried Noodles";
                    selectedImage = "pbackground.jpg";
                    selectedPrice = 59;
                    break;
                case 3: // Premium Siomai
                    selectedProduct = "Premium Siomai";
                    selectedImage = "premiums.png";
                    selectedPrice = 49;
                    break;
            }

            UpdateProductDetails();
        }

        private void UpdateProductDetails()
        {
            ProductImage.Source = selectedImage;
            ProductName.Text = selectedProduct;
        }

        private void OnPaymentMethodChanged(object sender, CheckedChangedEventArgs e)
        {
            if (OnlinePayment.IsChecked)
            {
                GCashDetails.IsVisible = true;
                paymentMethod = "Online Payment (GCash)";
            }
            else
            {
                GCashDetails.IsVisible = false;
                paymentMethod = "Cash on Delivery";
            }
        }

        private async void OnConfirmOrderClicked(object sender, EventArgs e)
        {
            int quantity = 1;
            if (!int.TryParse(QuantityEntry.Text, out quantity) || quantity <= 0)
            {
                await DisplayAlert("Error", "Please enter a valid quantity.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(paymentMethod))
            {
                await DisplayAlert("Error", "Please select a payment method.", "OK");
                return;
            }

            decimal totalPrice = selectedPrice * quantity; // Corrected total price calculation

            string message = $"Order Confirmed!\n\nProduct: {selectedProduct}\nQuantity: {quantity}\nTotal Price:₱{totalPrice:N2}\nPayment: {paymentMethod}";

            if (paymentMethod == "Online Payment (GCash)")
            {
                message += "\n\nPlease send your payment to:\n📱 09222777666 (Siomura)";
            }

            await DisplayAlert("Success", message, "OK");
        }
    }
}
