using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
namespace SocketServer
{
    class Program
    {
        public static List<ClientBotNet> listClient = new List<ClientBotNet>();

        public static void HandleMessage(string message)
        {
            //Message format : MAC/Message -> Message:90-2B-32-9E-0E-C0/shutdown
            string mac = message.Substring(8, 17);
            string request = message.Substring(26);
            Console.WriteLine("mac: " + mac);
            foreach(ClientBotNet bot in listClient)
            {
                Console.WriteLine("bot net mac address: " + bot.MacAddress);
                if (bot.MacAddress.Trim().Equals(mac.Trim()))
                {
                    byte[] sendData = new byte[1024];
                    sendData = Encoding.ASCII.GetBytes(request);
                    bot.socket.Send(sendData);
                    Console.WriteLine("sending to " + mac + " " + request);
                    break;
                }
            }
        }

        public static void AddToList(ClientBotNet bot, string mac)
        {
            Console.WriteLine("mac: " + mac);
            for(int i = 0; i < listClient.Count; i++)
            {
                if(listClient[i].MacAddress.Equals(mac))
                {
                    listClient[i] = bot;
                    break;
                }
            }
            //if non exist
            listClient.Add(bot);
        }

        public static bool ReciveFile(Socket clientSocket)
        {

            string path = @"G:\\WebRemotePBL4\\WebRemotePBL4\\Content\\DataReceive\\";

            Console.WriteLine("getting file....");
            byte[] clientData = new byte[1024 * 5000];

            int receivedBytesLen = clientSocket.Receive(clientData);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string message = Encoding.ASCII.GetString(clientData).Trim('\0');
            if (message.Contains("Message:") && message.Length < 50)
            {
                //Xu ly thong diep
                Console.WriteLine(message);
                HandleMessage(message);
                return true;
            }
            if (message.Contains("MacAddress:") && message.Length < 50)
            {
                //Add vao list
                Console.WriteLine(message);
                ClientBotNet clientBotNet = new ClientBotNet(clientSocket, message.Substring(12));
                AddToList(clientBotNet, message.Substring(12));
                return true;
            }

            string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
            string subFileName = fileName.Substring(fileName.Length - 25);
            BinaryWriter bWrite = new BinaryWriter(File.Open(path + subFileName, FileMode.Append));
            bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);

            while (receivedBytesLen != 0)
            {
                try
                {
                    receivedBytesLen = clientSocket.Receive(clientData, clientData.Length, 0);
                    bWrite.Write(clientData, 0, receivedBytesLen);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("close2");
                    ex.ToString();
                    bWrite.Close();
                    return false;
                }

            }
            Console.WriteLine(fileName);
            bWrite.Close();
            Console.WriteLine("close3");

            return true;
            // clientSocket.Close();
        }

        static void Main(string[] args)
        {

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            Console.WriteLine("Starting TCP listener...");

            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 3004);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP); ;
            serverSocket.Bind(ipEnd);

            serverSocket.Listen(3004);
            Console.WriteLine(" >> " + "Server Started");
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                new Thread(delegate ()
                {
                    try
                    {
                        ReciveFile(clientSocket);
                       
                    }
                    catch(Exception ex)
                    {
                        ex.ToString();
                    }
                    
                }).Start();
            }
        }
    }
}