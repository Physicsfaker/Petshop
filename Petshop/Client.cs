using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Petshop
{
    class Client
    {
        // Конструктор класса. Ему нужно передавать принятого клиента от TcpListener
        public Client(TcpClient Client)
        {
            // Код простой HTML-странички
            //string Html = "<html><body><h1>It works!</h1></body></html>";
            string Html = "Ловиответсучка!";
            // Необходимые заголовки: ответ сервера, тип и длина содержимого. После двух пустых строк - само содержимое
            string Str = "HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            // Приведем строку к виду массива байт
            byte[] Buffer = Encoding.UTF8.GetBytes(Str); 
            // Отправим его клиенту
            Client.GetStream().WriteAsync(Buffer, 0, Buffer.Length);
            //GetStream(Buffer);
            // Закроем соединение
            Console.WriteLine("Получилось?");
            //Client.Close();
        }

        //private void GetStream(byte[] buf) 
        //{
        //    Client.GetStream().Write(buf, 0, buf.Length);
        //}
    }
}
