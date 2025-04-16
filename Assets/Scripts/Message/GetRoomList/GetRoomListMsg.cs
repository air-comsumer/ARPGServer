using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRoomListMsg : BaseMsg
{

    public override int GetBytesNum()
    {
        return 4+4;
    }

    public override int GetID()
    {
        return 2003;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        return 0;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes,GetID(), ref index);
        WriteInt(bytes,0, ref index);
        return bytes;
    }
}
