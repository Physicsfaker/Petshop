using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


/*Задание заключается в разработке HTTP-сервера «Petshop» в виде консольного приложения.
 * Требования к результату следующие:
 *   Чистое консольное приложение без использования asp.net api/mvc и прочего.
 *   У питомца должны быть следующие поля:
 *   "name": "string",
 *   "type": "string".
 *   На запрос POSТ /pets создавать нового питомца. 
 *   На запрос GET /pets возвращать список всех питомцев.
 *   На любой другой GET запрос он должен возвращать «Hello world!».
 *   Можно не хранить данные в БД а использовать просто статичные переменные.
 *   Будет плюсом если приложение сможет обрабатывать несколько запросов одновременно.*/

namespace Petshop
{

    class PetshopServer
    {
        #region Server
        HttpListener Listener; // Объект, принимающий клиентов
        public static string host; //адрес сервера

        public PetshopServer()
        {
            Listener = new HttpListener();
            Listener.Prefixes.Add($"http://{host}:80/");
            Listener.Start();

            Task.Run(() => { while (true) new Client(Listener.GetContext()); });
        }

        ~PetshopServer() { if (Listener != null) Listener.Stop(); } // Остановка сервера
        #endregion

        #region Client
        public static void SendRequest(string command)
        {
            command = command.ToLower();
            string responseFromServer ="";

            if (command.ToLower().StartsWith("post /pets"))  //post
            {
                var request = (HttpWebRequest)WebRequest.Create($"http://{host}:80/");
                var postData = "/pets";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                Console.WriteLine("Отправилось post...");
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseFromServer = responseString.ToString();
            }

            else if (command.ToLower().StartsWith("get ")) //get
            {

                var request = (HttpWebRequest)WebRequest.Create($"http://{host}:80/?"+ command.Substring(4));
                Console.WriteLine("Отправилось get...");
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseFromServer = responseString.ToString();
            }
            else { Console.Clear(); Console.WriteLine("Invalid command!"); return; } //неверные команды

            Console.WriteLine("Server:" + responseFromServer);
        }
        #endregion

        static void Main(string[] args)
        {
            string hostName = Dns.GetHostName();     // Получение имени компьютера.
            foreach (IPAddress address in Dns.GetHostEntry(hostName).AddressList)
            {
                //Console.WriteLine($"    {address}");
                //if (address.ToString().Length >= 7) { host = address.ToString(); break; } //защитка от "::1" адресса
            }
            host = "192.168.1.42";
            Console.WriteLine("host: " + host);
            new PetshopServer(); //Создаем прослушку и сам сервер

            while (true)
            {
                //SendRequest(Console.ReadLine());
                SendRequest("POST /pets");
                Console.ReadKey();
                SendRequest("GET /pets");
                Console.ReadKey();
                SendRequest("GET /petsi==ss");
                Console.ReadKey();
            }
        }
    }
}
