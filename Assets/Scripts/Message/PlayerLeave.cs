using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerLeave : BaseMsg
{
    public string playerID;
    public override int GetBytesNum()
    {
        return 4+4+4+Encoding.UTF8.GetBytes(playerID).Length;
    }

    public override int GetID()
    {
        return 1004;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        playerID = ReadString(bytes,ref index);
        return index-beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes,GetBytesNum()-8, ref index);
        WriteString(bytes,playerID,ref index);
        return bytes;
    }
}
