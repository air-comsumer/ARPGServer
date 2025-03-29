using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GetListMsg : BaseMsg
{
    public int personNum;
    public List<PlayerData> playerList = new List<PlayerData>();

    public override int GetBytesNum()
    {
        int num = 0;
        num += 4;//协议ID
        num += 4;//长度
        num += 4;
        foreach (PlayerData p in playerList)
        {
            num += p.GetBytesNum();
        }
        return num;
    }

    public override int GetID()
    {
        return 1001;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        personNum = ReadInt(bytes,ref index);
        while (index < bytes.Length)
        {
            playerList.Add(ReadData<PlayerData>(bytes, ref index));
        }
        return index-beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes,GetID(),ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteInt(bytes,personNum,ref index);
        foreach(PlayerData p in playerList)
        {
            WriteData(bytes, p,ref index);
        }
        return bytes;
    }
}
