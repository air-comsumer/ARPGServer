using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomServerMsg : BaseMsg
{
    public int roomID;
    public bool result;

    public override int GetBytesNum()
    {
        return 4 + 4 + 4 + 1;
    }

    public override int GetID()
    {
        return 2004;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        roomID = ReadInt(bytes, ref index);
        result = ReadBool(bytes, ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteInt(bytes, roomID, ref index);
        WriteBool(bytes, result, ref index);
        return bytes;
    }
}
