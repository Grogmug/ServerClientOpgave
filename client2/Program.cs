using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ReadLine();
            TcpClient server = new TcpClient("127.0.0.1", 11000);

            NetworkStream stream = server.GetStream();
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);
            string input;

            while (true)
            {
                input = Console.ReadLine();
                sw.WriteLine(input);
                sw.Flush();

                string read = sr.ReadLine();
                Console.WriteLine(read);
            }


        }
    }
}
