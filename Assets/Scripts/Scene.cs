using INTERNET_SERVER;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : SingletonMono<Scene>
{
    public  List<ScenePlayer> list = new List<ScenePlayer>();//场景内角色
    private ScenePlayer GetScenePlayer(string id)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].id == id)
                return list[i];
        }
        return null;
    }
    public void AddPlayer(string id)
    {
        lock (list)
        {
            ScenePlayer p = new ScenePlayer();
            p.id = id;
            list.Add(p);
        }
    }
    public void DelPlayer(string id)
    {
        lock (list)
        {
            ScenePlayer p = GetScenePlayer(id);
            if(p != null)
                list.Remove(p);
        }
        PlayerLeave playerLeave = new PlayerLeave();
        playerLeave.playerID = id;
        //通知其他客户端删除角色
        GameManager.Instance().socket.BroadCast(playerLeave);
    }
    /// <summary>
    /// 将场景中每一个角色信息发送给客户端
    /// </summary>
    /// <param name="socket"></param>
    public void SendPlayerList(ClientSocket socket)
    {
        int count = list.Count;
        GetListMsg getListMsg = new GetListMsg();
        getListMsg.personNum = count;
        for (int i = 0; i < count; i++)
        {
            ScenePlayer p = list[i];
            getListMsg.playerList.Add(new PlayerData() { id = p.id,x = p.x,y = p.y,z = p.z });
        }
        socket.Send(getListMsg);
    }
    public void UpdateInfo(string id , float x, float y, float z)
    {
        int count = list.Count;
        ScenePlayer p = GetScenePlayer(id);
        if (p == null) return;
        p.x = x; p.y = y; p.z = z;
    }
}
