using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerData : BaseData
{
    public string id;
    public float x;
    public float y;
    public float z;
    public bool isOwner;
    public override int GetBytesNum()
    {
        return 4+4+4+4+1+Encoding.UTF8.GetBytes(id).Length;
    }


    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadString(bytes, ref index);
        x = ReadFloat(bytes, ref index);
        y = ReadFloat(bytes, ref index);
        z = ReadFloat(bytes, ref index);
        isOwner = ReadBool(bytes, ref index);
        return index-beginIndex;
    }

    public override byte[] Writing()
    {
        int index =0; 
        byte[] bytes = new byte[GetBytesNum()];
        WriteString(bytes,id,ref index);
        WriteFloat(bytes,x,ref index);
        WriteFloat(bytes,y,ref index);
        WriteFloat(bytes,z,ref index);
        WriteBool(bytes, isOwner, ref index);
        return bytes;
    }

    
}
