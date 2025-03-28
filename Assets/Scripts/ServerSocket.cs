using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
namespace INTERNET_SERVER
{
    public class ServerSocket
    {
        private Socket socket;
        private Dictionary<int, ClientSocket> clientDic = new Dictionary<int, ClientSocket>();
        public void Start(string ip, int port, int num)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            try
            {
                socket.Bind(ipPoint);
                socket.Listen(num);
                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.Completed += AcceptCallBack;
                socket.AcceptAsync(e);
            }
            catch (Exception e)
            {
                Debug.LogError("服务器启动失败");
            }
        }

        private void AcceptCallBack(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Socket clientSocket = e.AcceptSocket;
                ClientSocket client = new ClientSocket(clientSocket);
                clientDic.Add(client.clientID, client);
                Debug.Log("连接成功"+client.clientSocket.ToString());
                (sender as Socket).AcceptAsync(e);
            }
            else
            {
                Debug.LogError("连接客户端失败" + e.SocketError);
            }
        }
        public void CloseClientSocket(ClientSocket socket)
        {
            lock (clientDic)
            {
                socket.Close();
                if (clientDic.ContainsKey(socket.clientID))
                {
                    clientDic.Remove(socket.clientID);
                    Debug.LogFormat("客户端{0}主动断开连接了", socket.clientID);
                }
            }
        }
    }
}

