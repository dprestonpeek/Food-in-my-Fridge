using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MobileApplication
{
    class Database
    {
        public static string apiKey { get; private set; }
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
            apiKey = "&formatted=y&key=it5z09owihva4agg6jwnms0w06qihl";
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
                upcCode = "0";
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
        public string[,] GetUserInventory(string username)
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

    //Scrapes product info from source code
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
                byte[] raw = client.DownloadData("https://api.barcodelookup.com/v2/products?barcode=" + upcCode + Database.apiKey);
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

        public string[] GetRecipes(string keyword)
        {
            string[] excludedString = new string[0];
            return GetRecipes(keyword, excludedString);
        }

        public string[] GetRecipes(string keyword, string[] excluded)
        {
            string[] recipes = new string[10];
            string[] recipeData = new string[10];
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
                recipeData[0] = node["hits"][0]["recipe"]["label"];
                recipeData[1] = node["hits"][0]["recipe"]["source"];
                recipeData[2] = node["hits"][0]["recipe"]["image"];
                recipeData[3] = node["hits"][0]["recipe"]["calories"];
                recipeData[4] = node["hits"][0]["recipe"]["url"];
                recipeData[5] = node["hits"][0]["ingredient"]["food"]; //show ingredients

                if (recipeData[2].Length > 255)
                {
                    recipeData[2] = recipeData[2].Substring(0, 255);
                }
                recipeData[4] = recipeData[4].Replace("http://", "https://");

                return recipeData;
            }
            catch
            {
                recipeData[0] = "";
                recipeData[1] = "";
                recipeData[2] = "";
                recipeData[3] = "";
                recipeData[4] = "";
                return recipeData;
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
