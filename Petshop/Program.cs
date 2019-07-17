using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
        TcpListener Listener; // Объект, принимающий TCP-клиентов
        public static string host; //адрес сервера

        public PetshopServer()
        {
            Listener = new TcpListener(IPAddress.Any, 80); // Создаем "слушателя" для указанного порта
            Listener.Start();

            Task.Run(() => { while (true) new Client(Listener.AcceptTcpClient()); });
        }

        ~PetshopServer() { if (Listener != null) Listener.Stop(); } // Остановка сервера
        #endregion

        public static void SendRequest(string command)
        {
            command = command.ToLower();
            WebRequest request;

            if (command.ToLower().StartsWith("post /pets"))  //post
            {
                request = WebRequest.Create($@"http://{host}:80/");  
                request.Method = "POST";
                string param = "/pets";
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(param);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Console.WriteLine("Отправилось post...");
            }
            else if (command.ToLower().StartsWith("get ")) //get
            {
                request = WebRequest.Create($@"http://{host}:80?param=/pets"+ command);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                Console.WriteLine("Отправилось get...");
            }
            else { Console.Clear(); Console.WriteLine("Invalid command!");  return; } //неверные команды

            WebResponse responsee = request.GetResponse();
            using (Stream stream = responsee.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            responsee.Close();
            Console.WriteLine("Все конченно!");
        }

        static void Main(string[] args)
        {
            new PetshopServer(); //Создаем прослушку и сам сервер

            string hostName = Dns.GetHostName();     // Получение имени компьютера.
            foreach (IPAddress address in Dns.GetHostEntry(hostName).AddressList)
            {
                //Console.WriteLine($"    {address}");
                if (address.ToString().Length >= 7) { host = address.ToString(); break; } //защитка от "::1" адресса
            }
            Console.WriteLine("host: " + host);

            while (true)
            {
                //SendRequest(Console.ReadLine());
                SendRequest("GET /pets");
                Console.ReadKey();
                SendRequest("POST /pets");
                Console.ReadKey();
            }

            //Task.Run(async () =>
            //{
            WebRequest requestt = WebRequest.Create($@"http://{host}:80/g=123"); ///get 1
            requestt.Credentials = CredentialCache.DefaultCredentials;
            requestt.Method = "GET";
            requestt.ContentType = "application/x-www-form-urlencoded";
            WebResponse responsee = requestt.GetResponse();
            Console.WriteLine("Отправилось...");
            using (Stream stream = responsee.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
            responsee.Close();
            Console.WriteLine("Все конченно!");
            //});

            //WebRequest request = WebRequest.Create("http://localhost:5374/Home/PostData?sName=Иван Иванов&age=31"); //get2
            //WebResponse response = await request.GetResponseAsync();
            //using (Stream stream = response.GetResponseStream())
            //{
            //    using (StreamReader reader = new StreamReader(stream))
            //    {
            //        Console.WriteLine(reader.ReadToEnd());
            //    }
            //}
            //response.Close();

            //WebRequest request = WebRequest.Create("http://localhost:5374/Home/PostData");  //post
            //request.Method = "POST";
            //string sName = "sName=Иван Иванов&age=31";
            //byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(sName);
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = byteArray.Length;
            //using (Stream dataStream = request.GetRequestStream())
            //{
            //    dataStream.Write(byteArray, 0, byteArray.Length);
            //}
        }
    }
}
