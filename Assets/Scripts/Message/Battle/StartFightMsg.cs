using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFightMsg : BaseMsg
{
    public int myRoomID;
    public override int GetBytesNum()
    {
        return 4+4+4;
    }

    public override int GetID()
    {
        return 2008;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        myRoomID = ReadInt(bytes, ref index);
        return index-beginIndex;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetBytesNum()];
        int index = 0;
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteInt(bytes, myRoomID, ref index);
        return bytes;
    }
}
