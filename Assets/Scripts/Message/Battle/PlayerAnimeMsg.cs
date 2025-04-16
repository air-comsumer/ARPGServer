using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerAnimeMsg : BaseMsg
{
    public string id;
    public bool HasInputID;
    public bool RunID;
    public float MovementID;
    public override int GetBytesNum()
    {
        return 4+Encoding.UTF8.GetBytes(id).Length+
            4 +4+1+1+4;
    }

    public override int GetID()
    {
        return 2010;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadString(bytes, ref index);
        HasInputID = ReadBool(bytes, ref index);
        RunID = ReadBool(bytes, ref index);
        MovementID = ReadFloat(bytes, ref index);
        return index-beginIndex;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetBytesNum()];
        int index = 0;
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum() - 8, ref index);
        WriteString(bytes, id, ref index);
        WriteBool(bytes, HasInputID, ref index);
        WriteBool(bytes, RunID, ref index);
        WriteFloat(bytes, MovementID, ref index);
        return bytes;
    }
}
