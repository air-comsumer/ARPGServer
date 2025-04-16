using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolMsg : BaseMsg
{
    public int id;//对哪个消息进行的判断
    public bool result; //登录结果
    public override int GetBytesNum()
    {
        return 4+ 4 + 4 + 1;
    }

    public override int GetID()
    {
        return 888;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id=ReadInt(bytes,ref  index);
        result = ReadBool(bytes, ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteInt(bytes,id,ref index); 
        WriteBool(bytes, result, ref index);
        return bytes;
    }
}
