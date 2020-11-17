using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace MobileApplication
{
    class Database
    {
        public static string ApiKey { get; private set; }
        public WebClient client;
        public Products products;
        private string dbUrl;
        private string errorMessage;
        private Request request;

        public Database()
        {
            InitializeWebClient();
        }

        private void InitializeWebClient()
        {
            client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "Content-Type:application/json";
            client.Headers[HttpRequestHeader.Authorization] = "Basic secret-6323";
            dbUrl = "https://f1m5kuz1va.execute-api.us-east-1.amazonaws.com/Stage/";
            ApiKey = "&formatted=y&key=it5z09owihva4agg6jwnms0w06qihl";
            request = new Request(client);
            products = new Products(client);
        }

        public bool UserLogin(string username, string password)
        {
            if (username == null || password == null)
            {
                return false;
            }
            string url = dbUrl + "login";
            string parameters = "{\"username\":\"" + username.ToUpper() + "\",\"password\":\"" + password + "\",}";
            if (request.Post(url, parameters) != null)
            {
                return true;
            }
            errorMessage = request.GetLastErrorMessage();
            return false;
        }

        public bool UserRegister(string username, string password)
        {
            string url = dbUrl + "register";
            string parameters = "{\"username\":\"" + username.ToUpper() + "\",\"password\":\"" + password + "\",}";
            if (!UserLogin(username, password))
            {
                if (request.Post(url, parameters) != null)
                {
                    return true;
                }
                errorMessage = request.GetLastErrorMessage();
            }
            return false;
        }

        public bool AddToUserInventory(string username, string upcCode, string productName, string productDesc, string imageUrl, int quantity)
        {
            string url = dbUrl + "adduserinv";
            string parameters;
            if (upcCode == "")
            {
                int productId = 0;
                while (ItemExistsInInventory(username, productId.ToString()))
                {
                    productId++;
                }
                upcCode = productId.ToString();
            }
            if (productName == "")
            {
                return false;
            }
            if (productDesc == "")
            {
                productDesc = "no description";
            }
            else if (productDesc.Length > 255)
            {
                productDesc = productDesc.Substring(0, 255);
            }
            if (imageUrl == "")
            {
                imageUrl = "https://prestonpeek.weebly.com/uploads/2/2/4/9/22497912/foodinmyfridge_orig.png";
            }

            parameters = "{\"username\":\"" + username.ToUpper();
            parameters += "\",\"scanid\":\"" + upcCode;
            parameters += "\",\"productname\":\"" + productName;
            parameters += "\",\"description\":\"" + productDesc;
            parameters += "\",\"imageurl\":\"" + imageUrl;
            parameters += "\",\"quantity\":\"" + quantity + "\"}";

            if (request.Post(url, parameters) != null)
            {
                return true;
            }
            errorMessage = request.GetLastErrorMessage();
            return false;
        }

        public bool ItemExistsInInventory(string username, string upcCode)
        {
            if (GetItemFromInventory(username, upcCode) != null)
            {
                return true;
            }
            return false;
        }

        public string[] GetItemFromInventory(string username, string upcCode)
        {
            string url = dbUrl + "getuserinv";
            string parameters = "{\"username\":\"" + username.ToUpper() + "\",}";

            string jsonInventory = request.Post(url, parameters);
            if (jsonInventory != null)
            {
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(jsonInventory);
                string[,] inventory = new string[node["inventory"].Count, 5];
                for (int i = 0; i < node["inventory"].Count; i++)
                {
                    SimpleJSON.JSONNode item = SimpleJSON.JSON.Parse(node["inventory"][i]);
                    if (item["scanid"] == upcCode)
                    {
                        return new string[] { item["scanid"], item["productname"], item["description"], item["imageurl"], item["quantity"] };
                    }
                }
            }
            errorMessage = request.GetLastErrorMessage();
            return null;
        }

        //returns products in JSON format. Returns empty array upon error
        public string[,] GetUserInventory()
        {
            string url = dbUrl + "getuserinv";
            string parameters = "{\"username\":\"" + App.Username.ToUpper() + "\",}";

            string jsonInventory = request.Post(url, parameters);
            if (jsonInventory != null)
            {
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(jsonInventory);
                string[,] inventory = new string[node["inventory"].Count, 5];
                for (int i = 0; i < node["inventory"].Count; i++)
                {
                    SimpleJSON.JSONNode item = SimpleJSON.JSON.Parse(node["inventory"][i]);
                    inventory[i, 0] = item["scanid"];
                    inventory[i, 1] = item["productname"];
                    inventory[i, 2] = item["description"];
                    inventory[i, 3] = item["imageurl"];
                    inventory[i, 4] = item["quantity"];
                }
                return inventory;
            }
            errorMessage = request.GetLastErrorMessage();
            return null;
        }

        public bool RemoveFromUserInventory(string username, string upcCode)
        {
            string url = dbUrl + "deleteuserinv";
            string parameters = "{\"username\":\"" + username.ToUpper() + "\",\"scanid\":\"" + upcCode + "\"}";

            if (request.Post(url, parameters) != null)
            {
                return true;
            }
            errorMessage = request.GetLastErrorMessage();
            return false;
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }
    }

    //Class Recipe is being used as an object.
    public class Recipe
    {
        public string Label;
        public string Source;
        public string Image;
        public string Calories;
        public string Url;
        public int Time;
        public int Servings;
        public List<Ingredient> Ingredients;
        public string DietLabels;
        public string HealthLabels;

        public Recipe()
        {
            Label = "";
            Source = "";
            Image = "";
            Calories = "";
            Url = "";
            Time = 0;
            Servings = 0;
            Ingredients = new List<Ingredient>();
        }

        public Recipe(string label, string source, string calories)
        {
            this.Label = label;
            this.Source = source;
            this.Calories = calories;
        }

        public void AddImage(string image)
        {
            this.Image = image.Replace("http://", "https://");
        }

        public void AddURL(string url)
        {
            this.Url = url.Replace("http://", "https://");
        }

        public void AddIngredient(Ingredient newIngredient)
        {
            if (Ingredients == null)
            {
                Ingredients = new List<Ingredient>();
            }

            Ingredients.Add(newIngredient);
        }

        public void AddTime(int time)
        {
            this.Time = time;
        }

        public void AddServings(int servings)
        {
            this.Servings = servings;
        }
    }

    //Class Recipe is being used as an object.
    public class Ingredient
    {
        public string parentNode;
        public string text;
        public string weight;
        public string image;

        public Ingredient(string text, string weight, string image)
        {
            this.text = text;
            this.weight = weight;
            this.image = image;
        }
    }

    //Class Products is being used as a controller.
    class Products
    {
        WebClient client;
        string lastError = "";

        string upc;
        string name;
        string desc;
        string imagesource;

        public Products(WebClient theClient)
        {
            client = theClient;
        }

        public string GetProduct(string upcCode)
        {
            string[] data;
            try
            {
                data = GetProductData(upcCode);
            }
            catch (Exception e)
            {
                lastError = e.Message;
                return null;
            }

            upc = data[0];
            name = data[1];
            desc = data[2];
            imagesource = data[3];
            return name;
        }

        public string[] GetProductData(string upcCode)
        {
            string[] productData = new string[5];
            try
            {
                byte[] raw = client.DownloadData("https://api.barcodelookup.com/v2/products?barcode=" + upcCode + Database.ApiKey);
                string data = Encoding.UTF8.GetString(raw);
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(data);
                productData[0] = node["products"][0]["barcode_number"];
                productData[1] = node["products"][0]["product_name"];
                productData[2] = node["products"][0]["description"];
                productData[3] = node["products"][0]["images"][0];
                //quantity
                productData[4] = "1";

                if (productData[2].Length > 255)
                {
                    productData[2] = productData[2].Substring(0, 255);
                }
                productData[3] = productData[3].Replace("http://", "https://");

                return productData;
            }
            catch
            {
                productData[0] = "Barcode not recognized";
                productData[1] = "";
                productData[2] = "";
                productData[3] = "";
                //quantity
                productData[4] = "1";
                return productData;
            }
        }

        public List<Recipe> GetRecipes(string keyword)
        {
            string[] excludedString = new string[0];
            return GetRecipes(keyword, excludedString);
        }

        public List<Recipe> GetRecipes(string keyword, string[] excluded)
        {
            List<Recipe> recipes = new List<Recipe>();
            string excludedString = "";

            foreach (string word in excluded)
            {
                excludedString += "&excluded=" + word;
            }

            try
            {
                byte[] raw = client.DownloadData("https://api.edamam.com/search?q=" + keyword + excludedString + "&app_id=b7f31416&app_key=aa8d1187795346e20ef1b7e187c3a362");
                string data = Encoding.UTF8.GetString(raw);
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(data);

                for (int hit = 0; hit < 10; hit++)
                {
                    Recipe recipe = new Recipe(
                        node["hits"][hit]["recipe"]["label"],
                        node["hits"][hit]["recipe"]["source"],
                        node["hits"][hit]["recipe"]["calories"]);
                    recipe.AddImage(node["hits"][hit]["recipe"]["image"]);
                    recipe.AddURL(node["hits"][hit]["recipe"]["url"]);
                    recipe.AddTime(node["hits"][hit]["recipe"]["totalTime"]);
                    recipe.AddServings(node["hits"][hit]["recipe"]["yield"]);

                    int i = -1;
                    while (true)
                    {
                        if (node["hits"][0]["recipe"]["ingredients"][i + 1]["text"] != null)
                        {
                            recipe.AddIngredient(new Ingredient(
                                node["hits"][hit]["recipe"]["ingredients"][i + 1]["text"],
                                node["hits"][hit]["recipe"]["ingredients"][i + 1]["weight"],
                                node["hits"][hit]["recipe"]["ingredients"][i + 1]["image"]
                                ));
                        }
                        else
                        {
                            recipes.Add(recipe);
                            break;
                        }
                        i++;
                    }
                }
                return recipes;
            }
            catch
            {
                List<Recipe> theRecipes = new List<Recipe>
                {
                    new Recipe()
                };
                return theRecipes;
            }
        }

        public string GetLastErrorMessage()
        {
            return lastError;
        }
    }

    class Request
    {
        private WebClient client;
        private string lastError = "";

        public Request(WebClient theClient)
        {
            client = theClient;
        }

        public string Post(string url, string parameters)
        {
            string response;
            try
            {
                response = client.UploadString(url, parameters);
            }
            catch (Exception e)
            {
                lastError = e.Message;
                return null;
            }
            return response;
        }

        public string GetLastErrorMessage()
        {
            return lastError;
        }
    }
}
