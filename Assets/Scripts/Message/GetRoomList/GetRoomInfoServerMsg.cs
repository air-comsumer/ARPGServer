using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRoomInfoServerMsg : BaseMsg
{
    public int num;
    public List<RoomPlayerData> roomPlayers = new List<RoomPlayerData>();
    public override int GetBytesNum()
    {
        int num = 0;
        num += 4;//协议ID
        num += 4;//长度
        num += 4;
        foreach (var r in roomPlayers)
        {
            num += r.GetBytesNum();
        }
        return num;
    }

    public override int GetID()
    {
        return 2006;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        num = ReadInt(bytes, ref index);
        for (int i = 0; i < num; i++)
        {
            roomPlayers.Add(ReadData<RoomPlayerData>(bytes, ref index));
        }
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteInt(bytes, num, ref index);
        for (int i = 0; i < num ; i++)
        {
            WriteData(bytes, roomPlayers[i], ref index);
        }
        return bytes;
    }
}
