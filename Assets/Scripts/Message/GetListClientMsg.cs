using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetListClientMsg : BaseMsg
{
    public override int GetBytesNum()
    {
        int num = 0;
        num += 4;//协议ID
        num += 4;//长度
        return num;
    }

    public override int GetID()
    {
        return 1001;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        return 0;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        return bytes;
    }
}
