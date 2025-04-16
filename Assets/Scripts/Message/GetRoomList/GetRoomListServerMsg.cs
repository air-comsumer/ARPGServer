using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRoomListServerMsg : BaseMsg
{
    public int roomCount;
    public List<RoomInfo> roomList = new List<RoomInfo>();
    public override int GetBytesNum()
    {
        int num = 0;
        num += 4;//协议ID
        num += 4;//长度
        num += 4;
        foreach (var r in roomList)
        {
            num += r.GetBytesNum();
        }
        return num;
    }

    public override int GetID()
    {
        return 2003;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        roomCount = ReadInt(bytes, ref index);
        for (int i = 0; i < roomCount; i++)
        {
            var a = ReadData<RoomInfo>(bytes, ref index);
            Debug.Log("序列化时的房间人数"+ a.num);
            roomList.Add(a);
        }
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteInt(bytes, roomCount, ref index);
        for (int i = 0; i < roomCount; i++)
        {
            Debug.Log("反序列化时的房间人数" + roomList[i].num);
            WriteData(bytes, roomList[i], ref index);
        }
        return bytes;
    }
}
