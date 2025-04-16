    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomMsg : BaseMsg
{
    public int roomID;

    public override int GetBytesNum()
    {
        return 4+4+4;
    }

    public override int GetID()
    {
        return 2005;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        roomID = ReadInt(bytes, ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteInt(bytes, roomID, ref index);
        return bytes;
    }
}
