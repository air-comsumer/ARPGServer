using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LoginMsg : BaseMsg
{
    public string username;
    public string password;
    public override int GetBytesNum()
    {
        return 4//协议ID
            +4//长度
            +4+Encoding.UTF8.GetBytes(username).Length
            +4+Encoding.UTF8.GetBytes(password).Length;
    }


    public override int GetID()
    {
        return 2001;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        username =  ReadString(bytes, ref index);
        password = ReadString(bytes, ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes,GetID(),ref index);
        WriteInt(bytes,GetBytesNum()-8,ref index);
        WriteString(bytes,username,ref index);
        WriteString(bytes,password, ref index);
        return bytes;
    }
}
