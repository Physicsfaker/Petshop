using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Petshop
{
    class Client
    {
        // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
        public Client(TcpClient Client)
        {
            // Код простой HTML-странички
            //string Html = "<html><body><h1>It works!</h1></body></html>";
            string Html = "Лови ответ сучка!";
            // Необходимые заголовки: ответ сервера, тип и длина содержимого. После двух пустых строк - само содержимое
            string Str = "HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            // Приведем строку к виду массива байт
            byte[] Buffer = Encoding.UTF8.GetBytes(Str); 
            // Отправим его клиенту
            Client.GetStream().Write(Buffer, 0, Buffer.Length);
            // Закроем соединение
            Console.WriteLine("Получилось?");
            Client.Close();
        }
    }
}
