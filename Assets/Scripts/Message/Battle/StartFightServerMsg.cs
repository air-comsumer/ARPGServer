using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFightServerMsg : BaseMsg
{
    public int count;
    public List<PlayerData> players = new List<PlayerData>();
    public override int GetBytesNum()
    {
        int index = 0;
        index += 4; // ID
        index += 4; // Length
        index += 4; // count
        for (int i = 0; i < players.Count; i++)
        {
            index += players[i].GetBytesNum();
        }
        return index;
    }

    public override int GetID()
    {
        return 2008;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        count = ReadInt(bytes, ref index);
        for (int i = 0; i < count; i++)
        {
            PlayerData player = new PlayerData();
            player = ReadData<PlayerData>(bytes, ref index);
            players.Add(player);
        }
        return index-beginIndex;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetBytesNum()];
        int index = 0;
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteInt(bytes, count, ref index);
        for (int i=0;i < players.Count; i++)
        {
            WriteData(bytes, players[i], ref index);
        }
        return bytes;
    }
}
