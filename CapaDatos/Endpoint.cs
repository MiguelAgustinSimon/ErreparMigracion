using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace CapaDatos
{
    public class Endpoint
    {
        private readonly RestClient _client;

        // https://www.luisllamas.es/consumir-un-api-rest-en-c-facilmente-con-restsharp/
        //https://pokeapi.co/
        public void GetItems()
        {
            try
            {
                var client = new RestClient("https://pokeapi.co/api/v2/");
                var request = new RestRequest("pokemon/ditto", Method.Get);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                    Console.WriteLine(response.Content);
                    this.ExampleAsync();
                }
                else {
                    Console.WriteLine(response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            
        }

        public async Task ExampleAsync()
        {
            string text ="A class is the most powerful data type in C#. Like a structure, " +
                "a class defines the data and behavior of the data typeeeeeeeee. ";

            string nombre = DateTime.Now.ToLongDateString();
            string ext = ".txt";
            string ruta = "..\\..\\..\\Logs\\"+nombre+ext;
            await File.WriteAllTextAsync(ruta, text);

          
        }


        private void PostItem(string data)
        {
            try
            {
                var client = new RestClient("http://localhost:8080");
                var request = new RestRequest("items", Method.Post);
                request.AddParameter("data", data);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content);
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            
        }
        private void PutItem(int id, string data)
        {
            try
            {
                var client = new RestClient("http://localhost:8080");
                var request = new RestRequest("items", Method.Put);
                request.AddParameter("id", id);
                request.AddParameter("data", data);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content);
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            
        }
        private void DeleteItem(int id)
        {
            try
            {
                var client = new RestClient("http://localhost:8080");
                var request = new RestRequest($"items/{id}", Method.Delete);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content);
                }
                else
                {
                    Console.WriteLine(response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            
        }

    }
}
