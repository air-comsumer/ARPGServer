using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UpdateInfoMsg : BaseMsg
{
    public string id;
    public float x;
    public float y;
    public float z;
    public override int GetBytesNum()
    {
        return 4//协议ID
            +4//长度
            +4+Encoding.UTF8.GetBytes(id).Length
            +4+4+4;
    }

    public override int GetID()
    {
        return 1002;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadString(bytes,ref index);
        x = ReadFloat(bytes, ref index);
        y = ReadFloat(bytes, ref index);
        z = ReadFloat(bytes, ref index);
        return index-beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes,GetID(),ref index);
        WriteInt(bytes,GetBytesNum()-8,ref index);
        WriteString(bytes,id,ref index);
        WriteFloat(bytes,x,ref index);
        WriteFloat(bytes,y,ref index);
        WriteFloat(bytes,z,ref index);
        return bytes;
    }
}
