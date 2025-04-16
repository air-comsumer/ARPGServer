using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMgr : SingletonMono<RoomMgr>
{
    public List<Room> list = new List<Room>();
    public int CreateRoom(Player player)
    {
        Room room = new Room();
        lock(list)
        {
            list.Add(room);
            room.AddPlayer(player);
            int i = 0;
            foreach (Room r in list)
            {
                if (r == room)
                    return i;
                else
                    i++;
            }
            return -1;
        }
    }
    public void LeaveRoom(Player player)
    {
        PlayerTempData tempData = player.playerTempData;
        if(tempData.status == PlayerTempData.Status.None)
        {
            return;
        }
        Room room = tempData.room;
        lock(list)
        {
            room.DelPlayer(player.id);
            if ((room.list.Count==0))
            {
                list.Remove(room);
            }
        }
    }
    public bool EnterRoom(Player player, int roomIndex)
    {
        if (roomIndex < 0 || roomIndex >= list.Count)
        {
            Debug.Log("房间不存在");
            return false;
        }
        Room room = list[roomIndex];
        lock (list)
        {
            if (room.status==Room.Status.Prepare)
            {
                if(!room.AddPlayer(player))
                {
                    Debug.Log("房间已满");
                    return false;
                }
                else
                {
                    GetRoomInfoServerMsg msg = new GetRoomInfoServerMsg();
                    msg.num = room.list.Count;
                    foreach(var r in room.list)
                    {
                        RoomPlayerData roomPlayerData = new RoomPlayerData();
                        Debug.Log("房间里是谁"+ r.Key);
                        roomPlayerData.id = r.Key;
                        roomPlayerData.isOwner = r.Value.playerTempData.isOwner;
                        msg.roomPlayers.Add(roomPlayerData);
                    }
                    room.Broadcast(msg);
                    return true;
                }
            }
            return false;
        }
    }
    public GetRoomListServerMsg GetRoomList()
    {
        GetRoomListServerMsg msg = new GetRoomListServerMsg();
        msg.roomCount = list.Count;
        for(int i=0; i < msg.roomCount; i++)
        {
            Room room = list[i];
            RoomInfo roomInfo = new RoomInfo();
            roomInfo.num = room.list.Count;
            roomInfo.Status = room.status == Room.Status.Prepare ? 0 : 1;
            Debug.Log("roomInfo.num =" + roomInfo.num+"roomInfo.Status="+ roomInfo.Status);
            msg.roomList.Add(roomInfo);
        }
        return msg;
    }
}
