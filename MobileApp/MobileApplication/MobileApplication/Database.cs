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
            apiKey = "&formatted=y&key=qpzbr92sz831bfdyauu5f6ub1l450k";
            request = new Request(client);
            products = new Products(client);
        }

        public bool UserLogin(string username, string password)
        {
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
            string parameters = "{\"username\":\"" + username.ToUpper() + "\",\"scanid\":\"" + upcCode + "\",\"productname\":\"" + productName + "\",\"description\":\"" + productDesc + "\",\"imageurl\":\"" + imageUrl + "\",\"quantity\":\"" + quantity + "\"}";

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
            byte[] raw = client.DownloadData("https://api.barcodelookup.com/v2/products?barcode=" + upcCode + Database.apiKey);
            string data = Encoding.UTF8.GetString(raw);
            SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(data);
            productData[0] = node["products"][0]["barcode_number"];
            productData[1] = node["products"][0]["product_name"];
            productData[2] = node["products"][0]["description"];
            productData[3] = node["products"][0]["images"][0];
            productData[4] = "1";

            return productData;
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
