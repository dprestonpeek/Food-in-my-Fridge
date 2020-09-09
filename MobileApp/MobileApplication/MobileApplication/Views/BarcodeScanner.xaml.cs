using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MobileApplication.Views
{
    public partial class BarcodeScanner : ContentPage
    {
        ZXingScannerPage scanPage;
        public BarcodeScanner()
        {
            InitializeComponent();
            OpenScannerImmediately();
        }

        public async Task OpenBarcodeScanner()
        {
            scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) => {
                scanPage.IsScanning = true;

                Device.BeginInvokeOnMainThread(() => {
                    App.ScannedUPC = result.Text;
                    if (new Database().GetItemFromInventory(App.Username, App.ScannedUPC) != null)
                    {
                        App.editingItem = true;
                    }
                    Navigation.PushModalAsync(new NewItemPage());
                });
            };
            await Navigation.PushModalAsync(scanPage);
        }

        private async void OpenScannerImmediately()
        {
            scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) => {
                scanPage.IsScanning = true;

                Device.BeginInvokeOnMainThread(() => {
                    App.ScannedUPC = result.Text;
                    if (new Database().GetItemFromInventory(App.Username, App.ScannedUPC) != null)
                    {
                        App.editingItem = true;
                    }
                    Navigation.PushModalAsync(new NewItemPage());
                });
            };
            await Navigation.PushModalAsync(scanPage);
        }
    }
}
