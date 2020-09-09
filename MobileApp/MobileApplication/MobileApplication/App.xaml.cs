using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApplication.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MobileApplication
{
    public partial class App : Application
    {
        Page MyFridge;
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string ScannedUPC { get; set; }
        public static bool editingItem = false;
        public static bool showImages = true;

        public App()
        {
            InitializeComponent();

            MainPage = new FrontPage();
            MyFridge = new ItemsPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
