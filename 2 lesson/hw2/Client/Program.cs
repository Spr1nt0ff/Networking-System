using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TcpChatClient
{
    private static TcpClient client;
    private static NetworkStream stream;

    static void Main()
    {
        try
        {
            client = new TcpClient("127.0.0.1", 49152);
            stream = client.GetStream();
            Console.WriteLine("Подключено к серверу.");

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();

            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
        finally
        {
            client.Close();
        }
    }

    private static void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("\nСообщение от сервера: " + message);
            }
        }
        catch
        {
            Console.WriteLine("Отключено от сервера.");
        }
    }
}