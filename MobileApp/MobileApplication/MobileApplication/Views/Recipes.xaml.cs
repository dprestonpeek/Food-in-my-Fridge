using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Recipes : ContentPage
    {
        

        public Recipes()
        {
            InitializeComponent();
        }
        
        

        //on btn clicked
        public void searchBtn_Clicked(object sender, EventArgs e)
        {
            Database db = new Database();   //create database

            string searchInput = item.Text; //get user input from xaml entry


            //this will find recipes by keyword - does not look at inventory

            string[] recipeData = db.products.GetRecipe(searchInput); // currently only gets first result, but will eventually get all
            //GetProductData() method retuns string array - [0]barcode number [1]product name [2]description [3]image url [4]quantity
            //GetRecipes() method retuns string array - [0]label [1]source [2]image [3]calories [4]url


            // NEED TO GET MORE INFORMATION ABOUT RECIPE
            // Name, Calories, Time, Servings, Ingredients (array[name][amount]), Instructions, Image


            //results                         //when set up to return multiple results, go through this code for every (max10?20?) result
            string res_label = recipeData[0];
            string res_source = recipeData[1];
            string res_img = recipeData[2];
            string res_calories = recipeData[3];
            string res_url = recipeData[4]; //need to attatch this to label hyperlink "See full recipe"

            string ingr_1 = recipeData[5];  //may need to get another array for ingredients (name, amount)


            //display results
            recipeName.Text = "LABEL:" + res_label + "\tCAL: " + res_calories;
            recipeDesc.Text = "SOURCE: " + res_source;
            recipeUrl.Text = res_url;
            recipeImg.Source = res_img;
            ingr.Text = "INGR: " + ingr_1;
        }
        public void openlinkBtn_Clicked(object sender, EventArgs e)
        {
            //want to be able to open link to recipe webpage in browser
        }
        public void suggestBtn_Clicked(object sender, EventArgs e)
        {
            //want to be able to suggest recipes by comparing to names of items in inventory 'fridge' to recipe ingredients
        }
    }
}