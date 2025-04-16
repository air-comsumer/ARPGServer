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
            Debug.LogError("�Ѿ�����������");
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
        Debug.LogError("û��������");
        return null;
    }
}
