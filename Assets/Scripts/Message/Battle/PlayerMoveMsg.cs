using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerMoveMsg : BaseMsg
{
    public string id;
    public float posX;
    public float posY;
    public float posZ;
    public override int GetBytesNum()
    {
        return 4+4
            +4+Encoding.UTF8.GetBytes(id).Length
            +4+4+4;
    }

    public override int GetID()
    {
        return 2011;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadString(bytes, ref index);
        posX = ReadFloat(bytes, ref index);
        posY = ReadFloat(bytes, ref index);
        posZ = ReadFloat(bytes, ref index);
        return index-beginIndex;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetBytesNum()];
        int index = 0;
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteString(bytes, id, ref index);
        WriteFloat(bytes, posX, ref index);
        WriteFloat(bytes, posY, ref index);
        WriteFloat(bytes, posZ, ref index);
        return bytes;
    }
}
