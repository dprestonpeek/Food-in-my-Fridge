using MobileApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading;

namespace MobileApplication
{
    class Database
    {
        public static string ApiKey { get; private set; }
        public WebClient client;
        public string ErrorMessage { get; private set; }
        private string dbUrl;
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
        }

        #region User
        /// <summary>
        /// Logs a user in with username and password. Returns true on success.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
            ErrorMessage = request.GetLastErrorMessage();
            return false;
        }

        /// <summary>
        /// Registers a user with username and password. Returns true on success.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
                ErrorMessage = request.GetLastErrorMessage();
            }
            else
            {
                ErrorMessage = "Account with username already exists";
            }
            return false;
        }

        public bool UserExists(string username)
        {
            string url = dbUrl + "doesusernameexist";
            string parameters = "{\"username\":\"" + username.ToUpper() + "\",}";
            if (request.Post(url, parameters) != null)
            {
                return true;
            }
            return false;
        }

        public bool WarmupServer()
        {
            string url = dbUrl + "warmupserver";
            if (request.Post(url, "") == null)
            {
                return true;
            }
            ErrorMessage = "Unable to warm up server";
            return false;
        }
        #endregion

        #region Inventory
        /// <summary>
        /// Add to user inventory with productName and imageUrl. Returns true on success.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="productName"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public bool AddToUserInventory(string productName, string imageUrl)
        {
            return AddToUserInventory(new Item() {
                UPC = "",
                ProductName = productName,
                Description = "",
                ImageUrl = imageUrl,
                Quantity = "1" });
        }

        /// <summary>
        /// Add to user Inventory with Item object. Returns true on success.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddToUserInventory(Item item)
        {
            string url = dbUrl + "adduserinv";
            string parameters;
            if (item.UPC == "")
            {
                int productId = 0;
                while (ItemExistsInInventory(App.Username, productId.ToString()))
                {
                    productId++;
                }
                item.UPC = productId.ToString();
            }
            if (item.ProductName == "")
            {
                return false;
            }
            if (item.Description == "")
            {
                item.Description = "no description";
            }
            else if (item.Description.Length > 255)
            {
                item.Description = item.Description.Substring(0, 255);
            }
            if (item.ImageUrl == "")
            {
                item.ImageUrl = "http://prestonpeek.weebly.com/uploads/2/2/4/9/22497912/scaleblack_orig.png";
            }

            parameters = "{\"username\":\"" + App.Username.ToUpper();
            parameters += "\",\"scanid\":\"" + item.UPC;
            parameters += "\",\"productname\":\"" + item.ProductName;
            parameters += "\",\"description\":\"" + item.Description;
            parameters += "\",\"imageurl\":\"" + item.ImageUrl;
            parameters += "\",\"quantity\":\"" + item.Quantity + "\"}";

            if (request.Post(url, parameters) != null)
            {
                return true;
            }
            ErrorMessage = request.GetLastErrorMessage();
            return false;
        }

        /// <summary>
        /// Add to user inventory with all parameters. Returns true on success. [deprecated]
        /// </summary>
        /// <param name="username"></param>
        /// <param name="upcCode"></param>
        /// <param name="productName"></param>
        /// <param name="productDesc"></param>
        /// <param name="imageUrl"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool AddToUserInventory(string upcCode, string productName, string productDesc, string imageUrl, int quantity)
        {
            string url = dbUrl + "adduserinv";
            string parameters;
            if (upcCode == "")
            {
                int productId = 0;
                while (ItemExistsInInventory(App.Username, productId.ToString()))
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
                imageUrl = "http://prestonpeek.weebly.com/uploads/2/2/4/9/22497912/scaleblack_orig.png";
            }

            parameters = "{\"username\":\"" + App.Username.ToUpper();
            parameters += "\",\"scanid\":\"" + upcCode;
            parameters += "\",\"productname\":\"" + productName;
            parameters += "\",\"description\":\"" + productDesc;
            parameters += "\",\"imageurl\":\"" + imageUrl;
            parameters += "\",\"quantity\":\"" + quantity + "\"}";

            if (request.Post(url, parameters) != null)
            {
                return true;
            }
            ErrorMessage = request.GetLastErrorMessage();
            return false;
        }

        public Item GetItemFromInventory(string upcCode)
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
                    SimpleJSON.JSONNode item = SimpleJSON.JSON.Parse(node["inventory"][i].ToString());
                    if (item["scanid"] == upcCode)
                    {
                        return new Item() {
                            UPC = item["scanid"],
                            ProductName = item["productname"],
                            Description = item["description"],
                            ImageUrl = item["imageurl"],
                            Quantity = item["quantity"] };
                    }
                }
            }
            ErrorMessage = request.GetLastErrorMessage();
            return null;
        }

        /// <summary>
        /// Returns item data from user inventory. [deprecated]
        /// </summary>
        /// <param name="username"></param>
        /// <param name="upcCode"></param>
        /// <returns></returns>
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
                    SimpleJSON.JSONNode item = SimpleJSON.JSON.Parse(node["inventory"][i].ToString());
                    if (item["scanid"] == upcCode)
                    {
                        return new string[] { item["scanid"], item["productname"], item["description"], item["imageurl"], item["quantity"] };
                    }
                }
            }
            ErrorMessage = request.GetLastErrorMessage();
            return null;
        }

        public bool ItemExistsInInventory(string username, string upcCode)
        {
            if (GetItemFromInventory(username, upcCode) != null)
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Returns inventory products in a 2 dimensional string array. Returns empty array upon error.
        /// </summary>
        /// <returns></returns>
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
                    SimpleJSON.JSONNode item = SimpleJSON.JSON.Parse(node["inventory"][i].ToString());
                    inventory[i, 0] = item["scanid"];
                    inventory[i, 1] = item["productname"];
                    inventory[i, 2] = item["description"];
                    inventory[i, 3] = item["imageurl"];
                    inventory[i, 4] = item["quantity"];
                }
                return inventory;
            }
            ErrorMessage = request.GetLastErrorMessage();
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
            ErrorMessage = request.GetLastErrorMessage();
            return false;
        }
        #endregion

        #region Shopping List

        public bool AddItemToShoppingList(Item item)
        {
            string url = dbUrl + "addshoppinglist";
            string parameters;
            if (item.UPC == "")
            {
                int productId = 0;
                while (ItemExistsInInventory(App.Username, productId.ToString()))
                {
                    productId++;
                }
                item.UPC = productId.ToString();
            }
            if (item.ProductName == "")
            {
                return false;
            }
            if (item.Description == "")
            {
                item.Description = "no description";
            }
            else if (item.Description.Length > 255)
            {
                item.Description = item.Description.Substring(0, 255);
            }
            if (item.ImageUrl == "")
            {
                item.ImageUrl = "http://prestonpeek.weebly.com/uploads/2/2/4/9/22497912/scaleblack_orig.png";
            }

            parameters = "{\"username\":\"" + App.Username.ToUpper();
            parameters += "\",\"scanid\":\"" + item.UPC;
            parameters += "\",\"productname\":\"" + item.ProductName;
            parameters += "\",\"description\":\"" + item.Description;
            parameters += "\",\"imageurl\":\"" + item.ImageUrl;
            parameters += "\",\"quantity\":\"" + item.Quantity + "\"}";

            if (request.Post(dbUrl, parameters) != null)
            {
                return true;
            }
            ErrorMessage = request.GetLastErrorMessage();
            return false;
        }

        public string[] GetItemFromShoppingList(string upcCode)
        {
            string url = dbUrl + "getshoppinglist";
            string parameters = "{\"username\":\"" + App.Username.ToUpper() + "\",}";

            string jsonShoppingList = request.Post(url, parameters);
            if (jsonShoppingList != null)
            {
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(jsonShoppingList);
                string[,] shoppingList = new string[node["shoppinglist"].Count, 5];
                for (int i = 0; i < node["shoppinglist"].Count; i++)
                {
                    SimpleJSON.JSONNode item = SimpleJSON.JSON.Parse(node["shoppinglist"][i].ToString());
                    if (item["scanid"] == upcCode)
                    {
                        return new string[] { item["scanid"], item["productname"], item["description"], item["imageurl"], item["quantity"] };
                    }
                }
            }
            ErrorMessage = request.GetLastErrorMessage();
            return null;
        }

        public bool ItemExistsInShoppingList(string upcCode)
        {
            if (GetItemFromShoppingList(upcCode) != null)
            {
                return true;
            }
            return false;
        }

        public  List<Item> GetUserShoppingList(string username)
        {
            string url = dbUrl + "getshoppinglist";
            string parameters = "{\"username\":\"" + username.ToUpper() + "\",}";

            string jsonShoppingList = request.Post(url, parameters);
            if (jsonShoppingList != null)
            {
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(jsonShoppingList);

                List<Item> shoppingList = new List<Item>();
                
                for (int i = 0; i < node["shoppinglist"].Count; i++)
                {
                    SimpleJSON.JSONNode item = SimpleJSON.JSON.Parse(node["shoppinglist"][i].ToString());

                    Item shoppingListItem = new Item()
                    {
                        UPC = item["scanid"],
                        ProductName = item["productname"],
                        Description = item["description"],
                        ImageUrl = item["imageurl"],
                        Quantity = item["quantity"],
                    };
             
                    shoppingList.Add(shoppingListItem);
                }
                return shoppingList;
            }
            ErrorMessage = request.GetLastErrorMessage();
            return null;
        }

        public bool RemoveFromShoppingList(string upcCode)
        {
            string url = dbUrl + "deleteshoppinglist";
            string parameters = "{\"username\":\"" + App.Username.ToUpper() + "\",\"scanid\":\"" + upcCode + "\"}";

            if (request.Post(url, parameters) != null)
            {
                return true;
            }
            ErrorMessage = request.GetLastErrorMessage();
            return false;
        }
        #endregion

        #region Products
        /// <summary>
        /// Returns a list of 6 products to choose from.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public Product[] GetProductsByKeyword(string keyword)
        {
            Product[] products = new Product[6];
            try
            {
                byte[] raw = client.DownloadData("https://api.barcodelookup.com/v2/products?search=" + keyword + ApiKey);
                string data = Encoding.UTF8.GetString(raw);
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(data);

                for (int i = 0; i < 6; i++)
                {
                    Product product = new Product()
                    {
                        Image = node["products"][i]["images"][0],
                        Category = node["products"][i]["category"],
                        Weight = node["products"][i]["weight"]
                    };
                    product.Image = product.Image.Replace("http://", "https://");
                    products[i] = product;
                }

                return products;
            }
            catch
            {
                ErrorMessage = "No Results Found. Try using a simpler search term. (i.e. \"chicken\", \"pork\" etc.)";
                throw new Exception(ErrorMessage);
            }
        }

        /// <summary>
        /// Returns a list of 6 image sources for 1 item.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string[] GetProductImagesByKeyword(string keyword)
        {
            Product[] products;
            try
            {
                products = GetProductsByKeyword(keyword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            return new string[] {
                products[0].Image,
                products[1].Image,
                products[2].Image,
                products[3].Image,
                products[4].Image,
                products[5].Image
            };
        }

        /// <summary>
        /// Returns the name of the product.
        /// </summary>
        /// <param name="upcCode"></param>
        /// <returns></returns>
        public string GetProductNameByUPC(string upcCode)
        {
            return GetProductDataByUPC(upcCode)[1];
        }

        /// <summary>
        /// Returns an array of string data from the product. [deprecated]
        /// </summary>
        /// <param name="upcCode"></param>
        /// <returns></returns>
        public string[] GetProductDataByUPC(string upcCode)
        {
            string[] productData = new string[5];
            try
            {
                byte[] raw = client.DownloadData("https://api.barcodelookup.com/v2/products?barcode=" + upcCode + ApiKey);
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
                productData[0] = "";
                productData[1] = "";
                productData[2] = "";
                productData[3] = "";
                //quantity
                productData[4] = "1";
                ErrorMessage = "Barcode not recognized";
                return productData;
            }
        }

        /// <summary>
        /// Returns a Product object with data.
        /// </summary>
        /// <param name="upcCode"></param>
        /// <returns></returns>
        public Product GetProductByUPC(string upcCode)
        {
            try
            {
                byte[] raw = client.DownloadData("https://api.barcodelookup.com/v2/products?barcode=" + upcCode + ApiKey);
                string data = Encoding.UTF8.GetString(raw);
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(data);
                Product product = new Product()
                {
                    Barcode = node["products"][0]["barcode_number"],
                    Name = node["products"][0]["product_name"],
                    Description = node["products"][0]["description"],
                    Image = node["products"][0]["images"][0],
                    Quantity = "1"
                };

                if (product.Description.Length > 255)
                {
                    product.Description = product.Description.Substring(0, 255);
                }
                product.Image = product.Image.Replace("http://", "https://");

                return product;
            }
            catch
            {
                ErrorMessage = "Barcode not recognized";
                return new Product()
                {
                    Name = "Barcode not recognized"
                };
            }
        }
        #endregion

        #region Recipes
        public List<Recipe> GetRecipes(string keyword)
        {
            return GetRecipes(keyword, null, 10, null, null, -1, -1, -1);
        }

        public List<Recipe> GetRecipes(string keyword, int numRecipes)
        {
            return GetRecipes(keyword, null, numRecipes, null, null, -1, -1, -1);
        }

        public List<Recipe> GetRecipes(string keyword, List<DietLabels> dietLabels, List<HealthLabels> healthLabels)
        {
            return GetRecipes(keyword, null, 10, dietLabels, healthLabels, -1, -1, -1);
        }

        public List<Recipe> GetRecipes(string keyword, string[] excluded, int numRecipes, List<DietLabels> dietLabels, List<HealthLabels> healthLabels, int maxIngr, int time, int numCalories)
        {
            List<Recipe> recipes = new List<Recipe>();
            string excludedString = "";
            string dietString = "";
            string healthString = "";
            string maxIngrString = "";
            string timeString = "";
            string caloriesString = "";

            if (excluded != null)
            {
                foreach (string word in excluded)
                {
                    excludedString += "&excluded=" + word;
                }
            }
            if (dietLabels != null)
            {
                foreach (DietLabels label in dietLabels)
                {
                    dietString += "&dietLabels=" + label;
                }
            }
            if (healthLabels != null)
            {
                foreach (HealthLabels label in healthLabels)
                {
                    healthString += "&healthLabels=" + label;
                }
            }
            //-1 is "null" value
            if (maxIngr > -1)
            {
                maxIngrString = "&ingr=" + maxIngr;
            }
            if (time > -1)
            {
                timeString = "&time=" + time;
            }
            if (numCalories > -1)
            {
                caloriesString = "&calories=" + numCalories;
            }

            try
            {
                byte[] raw = client.DownloadData("https://api.edamam.com/search?q=" + keyword + excludedString + "&to=" + numRecipes + dietString + healthString + maxIngrString + timeString + caloriesString + "&app_id=b7f31416&app_key=aa8d1187795346e20ef1b7e187c3a362");
                string data = Encoding.UTF8.GetString(raw);
                SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(data);

                for (int hit = 0; hit < numRecipes; hit++)
                {
                    string calories = node["hits"][hit]["recipe"]["calories"];
                    if (calories.Contains("."))
                    {
                        calories = calories.Split('.')[0];
                    }
                    Recipe recipe = new Recipe(
                        node["hits"][hit]["recipe"]["label"],
                        node["hits"][hit]["recipe"]["source"],
                        calories);
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
            catch (Exception e)
            {
                List<Recipe> theRecipes = new List<Recipe>();
                theRecipes.Add(new Recipe() { Label = e.Message });

                return theRecipes;
            }
        }
        #endregion
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    public class Item
    {
        public string UPC { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Quantity { get; set; }
    }

    public class Product
    {
        public string Name;
        public string Barcode;
        public string Description;
        public string Image;
        public string Category;
        public string Weight;
        public string Quantity;
    }

    public enum DietLabels { BALANCED, HIGHPROTEIN, LOWFAT, LOWCARB, LOWSODIUM }
    public enum HealthLabels { VEGETARIAN, VEGAN, SUGARCONSCIOUS, PEANUTFREE, TREENUTFREE, ALCOHOLFREE }

    //The recipe object returned when GetRecipes() is called
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
        public List<DietLabels> dietLabels { get; private set; }
        public List<HealthLabels> healthLabels { get; private set; }
        public string mealType;
        public int score;

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
            dietLabels = new List<DietLabels>();
            healthLabels = new List<HealthLabels>();
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
            if (newIngredient.text != "" || newIngredient.text != null)
            {
                Ingredients.Add(newIngredient);
            }
        }

        public void AddTime(int time)
        {
            this.Time = time;
        }

        public void AddServings(int servings)
        {
            this.Servings = servings;
        }

        private void AddCalories(string calories)
        {
            int caloriesInt = int.Parse(calories);
            this.Calories = caloriesInt.ToString();
        }

        private void AddDietLabel(DietLabels dietLabel)
        {
            dietLabels.Add(dietLabel);
        }

        private void AddHealthLabel(HealthLabels healthLabel)
        {
            healthLabels.Add(healthLabel);
        }
    }

    //The ingredient object returned with each Recipe
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

    //An object to hold a separate error message when Posting to DynamoDB
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
