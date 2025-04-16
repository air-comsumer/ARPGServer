using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class GameDataManager : SingletonMono<GameDataManager>
{
    string connsql = @"server=127.0.0.1; database=UnityArpg; uid=sa; pwd=sb961955";  // 使用sql验证的方式连接数据库
    SqlConnection conn;

    void Start()
    {
        SQLServerToConnection();
    }

    private void SQLServerToConnection()
    {
        conn = new SqlConnection(connsql);
        try
        {
            conn.Open();
            Debug.Log("连接数据库成功");
        }
        catch
        {
            Debug.LogError("连接数据库失败");
        }
    }   
    public bool Login(string userName,string password)
    {
        string query = "SELECT * FROM playerAccount WHERE username = @UserName AND password = @Password";
        SqlCommand command = new SqlCommand(query, conn);

        // 添加参数（自动处理转义和类型校验）
        command.Parameters.AddWithValue("@UserName", userName);
        command.Parameters.AddWithValue("@Password", password);
        using (SqlDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                Debug.Log("登录成功");
                return true;
            }
            else
            {
                Debug.Log("登录失败");
                return false;
            }
        }
    }
    public bool Reg(string userName, string password)
    {
        string query = "INSERT INTO playerAccount (username, password) VALUES (@UserName, @Password)";
        SqlCommand command = new SqlCommand(query, conn);
        command.Parameters.AddWithValue("@UserName", userName);
        command.Parameters.AddWithValue("@Password", password);
        int result = command.ExecuteNonQuery();
        return result > 0;
    }
}
