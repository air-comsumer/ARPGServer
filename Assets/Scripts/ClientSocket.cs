using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
namespace INTERNET_SERVER
{
    public class ClientSocket
    {
        public Socket clientSocket;
        public static int CLIENT_ID = 1;
        public int clientID;
        private byte[] cacheBytes = new byte[1024*1024];
        private int cacheNum = 0;
        private long frontTime = -1;
        private static int TIME_OUT_TIME = 10;
        public ClientSocket(Socket socket)
        {
            clientSocket = socket;
            clientID = CLIENT_ID++;
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(new byte[1024*1024],0, 1024*1024);
            e.Completed += ReceiveCallBack;
            clientSocket.ReceiveAsync(e);
            ThreadPool.QueueUserWorkItem(CheckTimeOut);
        }
        private void CheckTimeOut(object obj)
        {
            while (this.clientSocket != null && this.clientSocket.Connected)
            {
                if (frontTime != -1 && DateTime.Now.Ticks / TimeSpan.TicksPerSecond - frontTime >= TIME_OUT_TIME)
                {
                    GameManager.Instance().socket.CloseClientSocket(this);
                }
                Thread.Sleep(5000);
            }
        }
        private void ReceiveCallBack(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Debug.Log("连接成功");
                e.Buffer.CopyTo(cacheBytes, cacheNum);
                cacheNum += e.BytesTransferred;
                Debug.Log(e.BytesTransferred);
                Debug.Log(Encoding.UTF8.GetString(cacheBytes));
                HandleReceiveMsg(e.BytesTransferred);
                e.SetBuffer(0, e.Buffer.Length);
                (sender as Socket).ReceiveAsync(e);
            }
            else
            {
                GameManager.Instance().socket.CloseClientSocket(this);
            }
        }
        public void Send(BaseMsg msg)
        {
            byte[] buffer = msg.Writing();
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(buffer,0,buffer.Length);
            e.Completed += (socket, args) =>
            {
                if (args.SocketError == SocketError.Success)
                {
                    Debug.Log("发送成功");
                }
                else
                {
                    Debug.Log("发送失败");
                    GameManager.Instance().socket.CloseClientSocket(this);
                }
            };
            clientSocket.SendAsync(e);
        }
        public void Close()
        {
            if (clientSocket != null)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                clientSocket = null;
            }
        }
        private void HandleReceiveMsg(int num)
        {
            int msgID = 0;
            int msgLength = 0;
            int nowIndex = 0;
            while (true)
            {
                msgLength = -1;
                if(cacheNum-nowIndex>=8)
                {
                    msgID = BitConverter.ToInt32(cacheBytes,nowIndex);
                    nowIndex += 4;
                    msgLength = BitConverter.ToInt32(cacheBytes,nowIndex);
                    nowIndex += 4;
                }
                if (cacheNum - nowIndex >= msgLength && msgLength != -1)
                {
                    BaseMsg baseMsg = null;
                    switch (msgID)
                    {
                        case 999:
                            baseMsg = new HeartMsg();
                            break;
                        case 1003:
                            baseMsg = new QuitMsg();
                            break;
                        default:
                            {
                                Debug.Log(Encoding.UTF8.GetString(cacheBytes));
                                break;
                            }
                    }
                    if (baseMsg != null)
                        ThreadPool.QueueUserWorkItem(MsgHandle, baseMsg);
                    nowIndex += msgLength;
                    if (nowIndex == cacheNum)
                    {
                        cacheNum = 0;
                        break;
                    }
                }
                else
                {
                    if (msgLength != -1)
                        nowIndex -= 8;
                    Array.Copy(cacheBytes, nowIndex, cacheBytes, 0, cacheNum - nowIndex);
                    cacheNum = cacheNum - nowIndex;
                    break;
                }
            }
        }

        private void MsgHandle(object obj)
        {
            switch(obj)
            {
                case HeartMsg msg:
                    frontTime = DateTime.Now.Ticks/TimeSpan.TicksPerSecond;
                    break;
                case QuitMsg msg:
                    Debug.Log("客户端主动断开连接");
                    GameManager.Instance().socket.CloseClientSocket(this);
                    break;

            }
        }
    }
}

