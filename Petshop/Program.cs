using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Petshop
{

    class PetshopServer
    {
        TcpListener Listener; // Объект, принимающий TCP-клиентов

        // Запуск сервера

        public PetshopServer()
        {
            // Создаем "слушателя" для указанного порта
            Listener = new TcpListener(IPAddress.Any, 80);
            Listener.Start(); // Запускаем его

            // В бесконечном цикле

            Task.Run(() =>
            {
                while (true)
                {
                    // Принимаем новых клиентов и передаем их на обработку новому экземпляру класса Client
                    new Client(Listener.AcceptTcpClient());
                }
            });

        }

        // Остановка сервера

        ~PetshopServer()
        {
            // Если "слушатель" был создан
            if (Listener != null)
            {
                // Остановим его
                Listener.Stop();
            }
        }


        static void Main(string[] args)
        {
            string hostName = Dns.GetHostName();     // Получение имени компьютера.
            string host = Dns.GetHostEntry(hostName).AddressList[1].ToString();    // Получение ip-адреса.
            //foreach (IPAddress address in host.AddressList)
            //{
            //    Console.WriteLine($"    {address}");
            //}
            Console.WriteLine(hostName);
            Console.WriteLine(host);

            new PetshopServer();

            Task.Run(async () => 
            {
                HttpWebRequest requestt = (HttpWebRequest)WebRequest.Create($@"http://{host}:80/g=123"); ///get 1
                requestt.Credentials = CredentialCache.DefaultCredentials;
                requestt.Method = "GET";
                requestt.ContentType = "application/x-www-form-urlencoded";
                WebResponse responsee = await requestt.GetResponseAsync();
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
            });

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


            Console.ReadKey();
        }
    }
}
