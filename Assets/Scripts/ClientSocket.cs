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
        public string playerID;
        private byte[] cacheBytes = new byte[1024*1024];
        private int cacheNum = 0;
        public long frontTime = -1;
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
                //Debug.Log("连接成功");
                e.Buffer.CopyTo(cacheBytes, cacheNum);
                cacheNum += e.BytesTransferred;
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
                    if (msgID != 999)
                        Debug.Log(msgID);
                    switch (msgID)
                    {
                        case 999:
                            baseMsg = new HeartMsg();
                            break;
                        case 1001:
                            baseMsg = new GetListClientMsg();
                            baseMsg.Reading(cacheBytes,nowIndex);
                            break;
                        case 1002:
                            baseMsg = new UpdateInfoMsg();
                            baseMsg.Reading(cacheBytes,nowIndex);
                            break;
                        case 1003:
                            baseMsg = new QuitMsg();
                            break;
                        case 888:
                            baseMsg = new BoolMsg();
                            break;
                        case 2001:
                            baseMsg = new LoginMsg();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        case 2002:
                            baseMsg = new RegMsg();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        case 2003:
                            baseMsg = new GetRoomListMsg();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        case 2004:
                            baseMsg = new CreateRoomMsg();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        case 2005:
                            baseMsg = new EnterRoomMsg();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        case 2006:
                            baseMsg = new GetRoomInfoMsg();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        case 2007:
                            baseMsg = new LeaveRoomMsg();
                            break;
                        case 2008:
                            baseMsg = new StartFightMsg();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        case 2009:
                            baseMsg = new PlayerChangeMessage();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        case 2010:
                            baseMsg = new PlayerAnimeMsg();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        case 2011:
                            baseMsg = new PlayerMoveMsg();
                            baseMsg.Reading(cacheBytes, nowIndex);
                            break;
                        default:
                            {
                                Debug.Log(Encoding.UTF8.GetString(cacheBytes));
                                break;
                            }
                    }
                    if (baseMsg != null)
                        ThreadPool.QueueUserWorkItem(MsgHandle,baseMsg);
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
        public void MsgHandle(object obj)
        {
            switch (obj)
            {
                case HeartMsg msg:

                    frontTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                    break;
                case GetListClientMsg msg:
                    {
                        Scene.Instance().SendPlayerList(this);
                    }
                    break;
                case UpdateInfoMsg msg:
                    {
                        MessageHandle.Instance().MsgUpdateInfo(this, msg);
                    }
                    break;
                case QuitMsg msg:
                    Debug.Log("客户端主动断开连接");
                    GameManager.Instance().socket.CloseClientSocket(this);
                    break;
                case LoginMsg msg:
                    { 
                        Debug.Log("登录请求");
                        MessageHandle.Instance().PlayerLogin(this, msg);
                    }
                    break;
                case RegMsg msg:
                    {
                        MessageHandle.Instance().PlayerReg(this, msg);
                    }
                    break;
                case GetRoomListMsg msg:
                    {
                        MessageHandle.Instance().RefershRoomList(this, msg);
                    }
                    break;
                case CreateRoomMsg msg:
                    {
                        MessageHandle.Instance().CreateRoom(this, msg);
                    }
                    break;
                case EnterRoomMsg msg:
                    {
                        MessageHandle.Instance().EnterRoom(this, msg);
                    }
                    break;
                case GetRoomInfoMsg msg:
                    {
                       
                    }
                    break;
                case LeaveRoomMsg msg:
                    {

                    }
                    break;
                case StartFightMsg msg:
                    {
                        Debug.Log("开始战斗");
                        MessageHandle.Instance().StartFight(this, msg);
                    }
                    break;
                case PlayerChangeMessage msg:
                    {
                        Debug.Log("玩家变更");
                        MessageHandle.Instance().PlayerChange(this, msg);
                    }
                    break;
                case PlayerAnimeMsg msg:
                    {
                        Debug.Log("玩家动画");
                        MessageHandle.Instance().PlayerAnime(this, msg);
                    }
                    break;
                case PlayerMoveMsg msg:
                    {
                        Debug.Log("玩家移动");
                        MessageHandle.Instance().PlayerMove(this, msg);
                    }
                    break;

            }
        }

    }
}

