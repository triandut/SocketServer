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


        public static bool ReciveFile(Socket clientSocket)
        {
            string path = @"G:\\WebRemotePBL4\\WebRemotePBL4\\Content\\DataReceive\\";

            Console.WriteLine("getting file....");
            byte[] clientData = new byte[1024 * 5000];

            int receivedBytesLen = clientSocket.Receive(clientData);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
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
                catch(Exception ex)
                {
                    Console.WriteLine("close2");
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
                    bool isSuccess = ReciveFile(clientSocket);
                    //if (isSuccess)
                    //{
                    //    byte[] sendData = new byte[1024];
                    //    string message = "success";
                    //    sendData = Encoding.ASCII.GetBytes(message);
                    //    clientSocket.Send(sendData);

                    //}
                    clientSocket.Close();
                }).Start();
            }
        }
    }
}
