using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MiniChatClient
{
    public class Client
    {
        public string name;
        public void Start()
        {
            Console.Write("Please enter your name: ");
            name = Console.ReadLine();

            using (TcpClient socket = new TcpClient("localhost", 7070))
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                while (true)
                {
                    Task.Run(() => { Send(sw, socket); });
                    Task.Run(() => { Receive(sr, socket); });
                }
            }
        }

        public void Send(StreamWriter sw, TcpClient socket)
        {
            string myLine = Console.ReadLine();

            sw.Write(name + ": ");
            sw.WriteLine(myLine);
            sw.Flush();
            if (myLine == "STOP") socket.Close();
        }

        public void Receive(StreamReader sr, TcpClient socket)
        {
            string line = sr.ReadLine();

            Console.WriteLine(line);
            if (line == "STOP") socket.Close();
        }
    }
}