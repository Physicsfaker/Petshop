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
    class Client
    {
        public Client(HttpListenerContext Client)
        {
            string responseString = "";
            HttpListenerRequest request = Client.Request;
            HttpListenerResponse response = Client.Response; // Obtain a response object.
            Console.WriteLine("Raw URL: {0}", request.RawUrl);

            if (request.HttpMethod == "GET")
            {
                //?имя_параметра1 = значение_параметра1&
                if (request.RawUrl.EndsWith("?/pets")) responseString = "возвращать список всех питомцев";
                else responseString = "Hello world!";
            }
            if (request.HttpMethod == "POST") responseString = "создавать нового питомца";

            byte[] buffer = Encoding.UTF8.GetBytes("<HTML><BODY> " + responseString + "</BODY></HTML>"); // Construct a response.
            response.ContentLength64 = buffer.Length; // Get a response stream and write the response to it.
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close(); // You must close the output stream.
        }
    }
}
