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
        socket.Start("192.168.3.57", 12345, 10);
    }
    public void OnLogin(ClientSocket clientSocket)
    {
        Scene.Instance().AddPlayer(clientSocket.playerID);
    }
    public void OnLogout(ClientSocket clientSocket)
    {
        Scene.Instance().DelPlayer(clientSocket.playerID);
    }
    private void OnDestroy()
    {
        socket.Close();
    }
}
