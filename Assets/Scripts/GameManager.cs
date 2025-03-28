using INTERNET_SERVER;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public ServerSocket socket;
    void Start()
    {
        socket = new ServerSocket();
        socket.Start("127.0.0.1", 12345, 10);
    }


}
