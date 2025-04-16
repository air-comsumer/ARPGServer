using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    public enum Status
    {
        Prepare = 0,
        Fight =1,
    }
    public Status status = Status.Prepare;
    public int maxPlayers = 6;
    public Dictionary<string,Player> list = new Dictionary<string, Player>();
    public bool AddPlayer(Player player)
    {
        lock (list)
        {
            if(list.Count >= maxPlayers)
            {
                Debug.Log("房间人数已满");
                return false;
            }
            PlayerTempData playerTempData = player.playerTempData;
            playerTempData.room = this;
            playerTempData.status = PlayerTempData.Status.Room;
            if (list.Count==0)
                playerTempData.isOwner = true;
            string id = player.id;
            if(!list.ContainsKey(id))
            {
                list.Add(id, player);
            }
        }
        return true;
    }
    public void DelPlayer(string id)
    {
        lock (list)
        {
            if(!list.ContainsKey(id))
            {
                Debug.Log("房间没有这个玩家");
                return;
            }
            bool isOwner = list[id].playerTempData.isOwner;
            list[id].playerTempData.status = PlayerTempData.Status.None;
            list.Remove(id);
            if (isOwner)
            {
                UpdateOwner();
            }
        }
    }
    public void UpdateOwner()
    {
        lock (list)
        {
            if (list.Count <= 0)
                return;
            foreach (Player player in list.Values)
            {
                player.playerTempData.isOwner = false;
            }
            Player p = list.Values.First();
            p.playerTempData.isOwner = true;
        }
    }
    public void Broadcast(BaseMsg msg)//向房间内所有玩家发送消息
    {
        foreach(Player player in list.Values)
        {
            player.socket.Send(msg);
        }
    }
    public GetRoomInfoServerMsg GetRoomInfo()
    {
        GetRoomInfoServerMsg msg = new GetRoomInfoServerMsg();
        msg.num = list.Count;
        foreach(Player p in list.Values)
        {
            msg.roomPlayers.Add(new RoomPlayerData()
            {
                id = p.id,
                isOwner = p.playerTempData.isOwner
            });
        }
        return msg;
    }
}
