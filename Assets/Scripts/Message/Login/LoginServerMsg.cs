using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginServerMsg : BaseMsg
{
    public bool result; //µÇÂ¼½á¹û
    public override int GetBytesNum()
    {
        return 4+4+1;
    }

    public override int GetID()
    {
        return 2001;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        result = ReadBool(bytes, ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes,GetID(),ref index);
        WriteInt(bytes, GetBytesNum()-8, ref index);
        WriteBool(bytes, result, ref index);
        return bytes;
    }
}
