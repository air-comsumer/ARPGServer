using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMono<PlayerManager>
{
    private Dictionary<string,Player> playerDic = new Dictionary<string, Player>();
    public void AddPlayer(Player p)
    {
        if(playerDic.ContainsKey(p.id))
        {
            Debug.LogError("已经有这个玩家了");
            return;
        }
        playerDic.Add(p.id, p);
    }
    public void DelPlayer(string id)
    {
        if (playerDic.ContainsKey(id))
        {
            playerDic.Remove(id);
        }
    }
    public Player GetPlayer(string id)
    {
        if (playerDic.ContainsKey(id))
        {
            return playerDic[id];
        }
        Debug.LogError("没有这个玩家");
        return null;
    }
}
