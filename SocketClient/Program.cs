using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

try
{
    SendMessageFromSocket(00001);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);  
}
finally
{
    Console.ReadKey();
}

static void SendMessageFromSocket(int port)
{
    byte[] buffer = new byte[1024];

    // Соединяемя и устанавливаем точку

    IPHostEntry ipHost = Dns.GetHostEntry("localhost");
    IPAddress ipAdr = ipHost.AddressList[0];
    IPEndPoint ipEndPoint = new IPEndPoint(ipAdr, port);

    Socket sender = new Socket(ipAdr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    sender.Connect(ipEndPoint);

    Console.WriteLine("Вещайте");
    string message = Console.ReadLine();

    Console.WriteLine("Подключение к серваку {0}",sender.RemoteEndPoint.ToString());
    byte[] msg = Encoding.UTF8.GetBytes(message);
    sender.Send(msg);
    
    // Получаем ответ
    int byteRec = sender.Receive(buffer);
    Console.WriteLine("Ответ от сервера - {0}",Encoding.UTF8.GetString(buffer,0,byteRec));

    if (message.IndexOf("Выйти") == -1)
        SendMessageFromSocket(port);

    // Освобождаем сокет
    sender.Shutdown(SocketShutdown.Both);
    sender.Close();
}