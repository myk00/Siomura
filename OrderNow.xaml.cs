using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using MySql.Data.MySqlClient;

namespace Siomura
{
    public partial class OrderNow : ContentPage
    {
        
        private string connectionString = "server=10.0.2.2;port=3306;database=SiomuraDB;user=root;password=toytoynitutoy21;";

        private string selectedProduct = "Siomai Bilao";
        private decimal selectedPrice = 599.00m;
        private string paymentMethod = "";

        public OrderNow()
        {
            InitializeComponent();
        }

        private void OnProductChanged(object sender, EventArgs e)
        {
            if (ProductPicker.SelectedIndex == -1) return;

            string selectedItem = ProductPicker.SelectedItem.ToString();

            switch (selectedItem)
            {
                case "Siomai Bilao - ₱599":
                    selectedProduct = "Siomai Bilao";
                    selectedPrice = 599.00m;
                    ProductImage.Source = "qpal1.jpg";
                    break;
                case "Siomai Pugo - ₱59":
                    selectedProduct = "Siomai Pugo";
                    selectedPrice = 59.00m;
                    ProductImage.Source = "siopugo.png";
                    break;
                case "Fried Noodles - ₱59":
                    selectedProduct = "Fried Noodles";
                    selectedPrice = 59.00m;
                    ProductImage.Source = "pbackground.jpg";
                    break;
                case "Premium Siomai - ₱49":
                    selectedProduct = "Premium Siomai";
                    selectedPrice = 49.00m;
                    ProductImage.Source = "premiums.png";
                    break;
            }

            ProductName.Text = selectedProduct;
            ProductPrice.Text = $"₱{selectedPrice:N2}";
        }

        private void OnPaymentMethodChanged(object sender, CheckedChangedEventArgs e)
        {
            if (CashOnDelivery.IsChecked)
            {
                paymentMethod = "Cash on Delivery";
                GCashDetails.IsVisible = false;
            }
            else if (OnlinePayment.IsChecked)
            {
                paymentMethod = "Online Payment (GCash)";
                GCashDetails.IsVisible = true;
            }
        }

        private async void OnConfirmOrderClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CustomerNameEntry.Text) || string.IsNullOrWhiteSpace(ContactNumberEntry.Text))
            {
                await DisplayAlert("Error", "Please enter your name and contact number.", "OK");
                return;
            }

            if (!int.TryParse(QuantityEntry.Text, out int quantity) || quantity <= 0)
            {
                await DisplayAlert("Error", "Please enter a valid quantity.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(paymentMethod))
            {
                await DisplayAlert("Error", "Please select a payment method.", "OK");
                return;
            }

            decimal totalPrice = selectedPrice * quantity;

            bool isSaved = await SaveOrderToDatabase(CustomerNameEntry.Text, ContactNumberEntry.Text, selectedProduct, quantity, totalPrice, paymentMethod);

            if (isSaved)
            {
                string message = $"Order Confirmed!\n\n" +
                                 $"👤 Name: {CustomerNameEntry.Text}\n" +
                                 $"📞 Contact: {ContactNumberEntry.Text}\n\n" +
                                 $"🍱 Product: {selectedProduct}\n" +
                                 $"🔢 Quantity: {quantity}\n" +
                                 $"💰 Total Price: ₱{totalPrice:N2}\n" +
                                 $"💳 Payment: {paymentMethod}";

                if (paymentMethod == "Online Payment (GCash)")
                {
                    message += "\n\n📲 Please send your payment to:\n📱 0917-123-4567 (Siomura)";
                }

                await DisplayAlert("Success", message, "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to save order. Try again.", "OK");
            }
        }

        private async Task<bool> SaveOrderToDatabase(string name, string contact, string product, int quantity, decimal totalPrice, string payment)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var db = new SiomuraDB();
                    return db.SaveOrder(name, contact, product, quantity, totalPrice, payment);
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Database Error", ex.Message, "OK");
                return false;
            }
        }

    }
}
