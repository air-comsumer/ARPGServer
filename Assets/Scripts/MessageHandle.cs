using INTERNET_SERVER;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Sockets;
using UnityEngine;

public class MessageHandle : SingletonMono<MessageHandle>
{
    /// <summary>
    /// 更新数据信息，对UpdateInfoMsg的处理
    /// </summary>
    /// <param name="socket"></param>
    /// <param name="playerData"></param>
    public void MsgUpdateInfo(ClientSocket socket, UpdateInfoMsg playerData)
    {
        int start = 0;
        float x = playerData.x;
        float y = playerData.y;
        float z = playerData.z;
        socket.playerID = playerData.id;
        Scene.Instance().AddPlayer(socket.playerID);
        Scene.Instance().UpdateInfo(socket.playerID, x, y, z);
        UpdateInfoServerMsg updateInfoMsg = new UpdateInfoServerMsg();
        updateInfoMsg.x = x;
        updateInfoMsg.y = y;
        updateInfoMsg.z = z;
        updateInfoMsg.id = socket.playerID;
        GameManager.Instance().socket.BroadCast(updateInfoMsg);
    }
    public void PlayerLogin(ClientSocket socket, LoginMsg msg)
    {
        Debug.Log("传数据");
        BoolMsg resultMsg = new BoolMsg();
        resultMsg.id = 2001;
        resultMsg.result = GameDataManager.Instance().Login(msg.username, msg.password);
        if (resultMsg.result)
        {
            var player = new Player();
            player.id = msg.username;
            socket.playerID = player.id;
            player.socket = socket;
            player.playerTempData = new PlayerTempData();
            PlayerManager.Instance().AddPlayer(player);
        }
        //Debug.Log(resultMsg.result.ToString()+resultMsg.id);
        socket.Send(resultMsg);
    }
    public void PlayerReg(ClientSocket socket, RegMsg msg)
    {
        Debug.Log("传数据");
        BoolMsg resultMsg = new BoolMsg();
        resultMsg.id = 2002;
        resultMsg.result = GameDataManager.Instance().Reg(msg.username, msg.password);
        //Debug.Log(resultMsg.result.ToString()+resultMsg.id);
        socket.Send(resultMsg);
    }
    public void RefershRoomList(ClientSocket socket, GetRoomListMsg msg)
    {
        var getRoomList = RoomMgr.Instance().GetRoomList();
        socket.Send(getRoomList);
    }
    public void CreateRoom(ClientSocket socket, CreateRoomMsg msg)
    {
        var player = PlayerManager.Instance().GetPlayer(socket.playerID);
        CreateRoomServerMsg createRoomServerMsg = new CreateRoomServerMsg();
        if (player.playerTempData.status == PlayerTempData.Status.None)
        {
            Debug.Log("玩家在房间中");
            int roomIndex = RoomMgr.Instance().CreateRoom(player);
            createRoomServerMsg.roomID = roomIndex;
            createRoomServerMsg.result = true;
        }
        else
            createRoomServerMsg.result = false;
        socket.Send(createRoomServerMsg);
    }
    public void EnterRoom(ClientSocket socket, EnterRoomMsg msg)
    {
        BoolMsg resultMsg = new BoolMsg();
        resultMsg.id = 2005;
        if (RoomMgr.Instance().EnterRoom(PlayerManager.Instance().GetPlayer(socket.playerID), msg.roomID))
        {
            resultMsg.result = true;
        }
        else
        {
            resultMsg.result = false;
        }
        socket.Send(resultMsg);
    }
    public void StartFight(ClientSocket socket,StartFightMsg msg)
    {
        Debug.Log("收到开始战斗");
        var player = PlayerManager.Instance().GetPlayer(socket.playerID);
        if (player.playerTempData.isOwner)
        {
            var room = player.playerTempData.room;
            StartFightServerMsg startFightServerMsg = new StartFightServerMsg();
            startFightServerMsg.count = room.list.Count;
            foreach (var p in room.list.Values)
            {
                var playerData = new PlayerData();
                playerData.id = p.id;
                playerData.isOwner = p.playerTempData.isOwner;
                System.Random random = new System.Random();
                playerData.x = (float)random.Next(0, 20);
                playerData.y = 0;
                playerData.z = (float)random.Next(0, 20);
                startFightServerMsg.players.Add(playerData);
            }
            room.Broadcast(startFightServerMsg);
            room.status = Room.Status.Fight;
        }
    }

    internal void PlayerChange(ClientSocket clientSocket, PlayerChangeMessage msg)
    {
        var player = PlayerManager.Instance().GetPlayer(clientSocket.playerID);
        player.playerTempData.room.Broadcast(msg);
    }

    internal void PlayerAnime(ClientSocket clientSocket, PlayerAnimeMsg msg)
    {
        var player = PlayerManager.Instance().GetPlayer(clientSocket.playerID);
        player.playerTempData.room.Broadcast(msg);
    }

    internal void PlayerMove(ClientSocket clientSocket, PlayerMoveMsg msg)
    {
        var player = PlayerManager.Instance().GetPlayer(clientSocket.playerID);
        player.playerTempData.room.Broadcast(msg);
    }
}
