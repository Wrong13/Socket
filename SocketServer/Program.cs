using System.Text;
using System.Net;
using System.Net.Sockets;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// Установка для сокета локальной конечной точки
IPHostEntry ipHost = Dns.GetHostEntry("localhost");
IPAddress ipAdr = ipHost.AddressList[0];
IPEndPoint ipEndPoint = new IPEndPoint(ipAdr, 00001);

// Создаем сокет 

Socket listener = new Socket(ipAdr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

// Назначаем сокет локальной конечной точке и слушаем входящие сокеты

try
{
    listener.Bind(ipEndPoint);
    listener.Listen(10);

    // Начинаем прослушку
    while (true)
    {
        Console.WriteLine("Ожидаю соединение через порт {0}", ipEndPoint);

        // Ожидание входящего соединения
        Socket handler = listener.Accept();
        string data = null;

        //Дождались клиента
        byte[] buffer = new byte[1024];
        int bytesRec = handler.Receive(buffer);

        data += Encoding.UTF8.GetString(buffer, 0, bytesRec);

        Console.WriteLine("Я получил это - {0}", data);

        //Ответочка клиенту
        byte[] msg = Encoding.UTF8.GetBytes("Спс за прикол длиною - " + data.Length.ToString());
        handler.Send(msg);

        if (data.IndexOf("Выйти") > -1)
        {
            Console.WriteLine("Сервер завершил соединение с клиентом!");
            break;
        }
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
finally
{
    Console.ReadLine();
}