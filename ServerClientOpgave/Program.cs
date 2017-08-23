using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerClientOpgave
{
    class Program
    {
        static StreamReader sr;
        static StreamWriter sw;
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener server = new TcpListener(ip, 11000);
            server.Start();
            int Counter = 0;
            
            while (true)
            {
                Counter += 1;
                Socket client = server.AcceptSocket();
                Console.WriteLine("Client number "+ Counter + " found with ip: " + client.RemoteEndPoint);
                new Thread(() => HandleClient(client)).Start();                
            }  
        }

        private static string Add(string[] toBeAdded)
        {
            int sum = 0;
            for (int i = 1; i < toBeAdded.Count(); i++)
            {
                sum += int.Parse(toBeAdded[i]);
            }

            return sum.ToString();
        }
        private static string Sub(string[] toBeAdded)
        {
            int sum = 0;

            sum = int.Parse(toBeAdded[1]) - int.Parse(toBeAdded[2]);
            return sum.ToString();
        }

        private static void GuessGame()
        {
            int numberToGuess;
            int guessLeft = 10;
            int guess;
            bool won = false;
            Random rand = new Random();
            numberToGuess = rand.Next(101);
            

            while (won == false && guessLeft > 0)
            {
                guess = int.Parse(sr.ReadLine());

                if (guess > numberToGuess)
                {
                    guessLeft--;
                    sw.WriteLine($"Wrong try something lower. You have {guessLeft} tries left");
                    sw.Flush();
                }
                else if(guess < numberToGuess)
                {
                    guessLeft--;
                    sw.WriteLine($"Wrong try something higher. You have {guessLeft} tries left");
                    sw.Flush();
                }
                else
                {
                    won = true;
                }
            }
            if (won == false)
            {
                sw.WriteLine("You lost! Try again!");
            }
            else if (won == true)
            {
                sw.WriteLine("You Won!");
            }
            
            sw.Flush();
        }

       private static void HandleClient(Socket client)
        {
            NetworkStream stream = new NetworkStream(client);
            sr = new StreamReader(stream);
            sw = new StreamWriter(stream);
            bool clientIsActive = true;
            while (clientIsActive)
            {
                try
                {
                    string read = sr.ReadLine();
                    Console.WriteLine(read);
                    string[] request = read.Split(' ');
                    string write;


                    switch (request[0].ToLower())
                    {
                        case "time?":
                            write = "Here is the time: " + DateTime.Now.ToString("hh:mm:ss");
                            sw.WriteLine(write);
                            Console.WriteLine(write);
                            break;
                        case "hello":
                            sw.WriteLine("Hello Client");
                            break;
                        case "ip?":
                            sw.WriteLine("Your IpAddress is: " + client.RemoteEndPoint.ToString());
                            break;
                        case "date?":
                            sw.WriteLine("The date is: " + DateTime.Now.ToString("dd/MM/yyyy"));
                            break;
                        case "add":
                            sw.WriteLine(Add(request));
                            break;
                        case "sub":
                            sw.WriteLine(Sub(request));
                            break;
                        case "exit":
                            client.Close();
                            clientIsActive = false;
                            break;
                        case "guessgame":
                            sw.WriteLine("GUESS GAME! Try to guess a number between 1 - 100. You have 10 tries! Good luck!");
                            sw.Flush();
                            GuessGame();
                            break;
                        default:
                            sw.WriteLine("Unknown command!!");
                            break;
                    }
                    sw.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                    stream.Close();
                    clientIsActive = false;
                }
            }
        }

    }
}
