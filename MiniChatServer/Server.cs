using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MiniChatServer
{
    public class Server
    {
        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, 7070);

            server.Start();


            while (true)
            {
                TcpClient socket = server.AcceptTcpClient();
                Task.Run(() =>
                {
                    TcpClient tmpsocket = socket;
                    DoClient(tmpsocket);
                });
            }
        }

        public void DoClient(TcpClient socket)
        {
            Console.WriteLine("SERVER");
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                while (true)
                {
                    Task.Run(() => { Receive(sr, socket); });
                    Task.Run(() => { Send(sw, socket); });
                }
            }
        }

        public void Send(StreamWriter sw, TcpClient socket)
        {
            string myline = Console.ReadLine();

            sw.WriteLine("SERVER: "+myline);
            sw.Flush();
            if (myline == "STOP") socket.Close();
        }

        public void Receive(StreamReader sr, TcpClient socket)
        {
            string line = sr.ReadLine();
            Console.WriteLine(line);
            if (line == "STOP") socket.Close();
        }
    }
}