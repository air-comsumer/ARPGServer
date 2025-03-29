using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMsg : BaseMsg
{
    public override int GetBytesNum()
    {
        return 4+4;
    }

    public override int GetID()
    {
        return 999;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        return 0;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetBytesNum()];
        int nowIndex = 0;
        WriteInt(bytes,GetID(),ref nowIndex);
        WriteInt(bytes,0,ref nowIndex);
        return bytes;
    }
}
