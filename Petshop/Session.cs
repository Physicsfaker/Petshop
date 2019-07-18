using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Petshop
{
    public class Pets
    {
        readonly string name;
        readonly string type;

        static readonly string[] namesParts = { "Tyson", "Jack", "Ooh!", "Gray", "Ray", "Lucky", "Butch", "Charlie", "Max", "Bagheera", "Tyson",
                                                "Alpha", "Bim", "Ooh" ,"Jesse", "Alex", "Gerda", "Ricci", "Thunder" };
        static readonly string[] typesPets = { "Cat", "Dog", "Rabbit", "Mouse", "Rat", "Hamster",
                                                 "Squirrel", "Chinchilla", "Snake", "Lizard",
                                                    "Turtle", "Parrot", "Owl ", " Crow "};
        public static readonly List<string> PetsStats = new List<string>();

        public Pets()
        {
            name = GenerateName();
            type = GenerateType();
            PetsStats.Add($"Name: {name}; Type:{type}");
        }

        private string GenerateName() => namesParts[new Random().Next(0, namesParts.Length - 1)];
        private string GenerateType() => typesPets[new Random().Next(0, typesPets.Length - 1)];
    }

    class Session
    {
        public Session(HttpListenerContext session)
        {
            string responseString = "\n";
            HttpListenerRequest request = session.Request;
            HttpListenerResponse response = session.Response; // Obtain a response object.

            if (request.HttpMethod == "GET")
            {
                //?имя_параметра1 = значение_параметра1&
                if (request.RawUrl.EndsWith("?/pets"))
                {
                    if (Pets.PetsStats.Count == 0) responseString = "Pet store is empty =(";
                    else foreach (string item in Pets.PetsStats)
                        {
                            responseString += item + " \n";
                        }
                }
                else responseString = "Hello world!";
            }
            if (request.HttpMethod == "POST") { responseString = "New pet!"; new Pets(); }

            byte[] buffer = Encoding.UTF8.GetBytes("<HTML><BODY> " + responseString + "</BODY></HTML>"); // Construct a response.
            response.ContentLength64 = buffer.Length; // Get a response stream and write the response to it.
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close(); // You must close the output stream.
        }
    }
}
