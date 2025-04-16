using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class GameDataManager : SingletonMono<GameDataManager>
{
    string connsql = @"server=127.0.0.1; database=UnityArpg; uid=sa; pwd=sb961955";  // ʹ��sql��֤�ķ�ʽ�������ݿ�
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
            Debug.Log("�������ݿ�ɹ�");
        }
        catch
        {
            Debug.LogError("�������ݿ�ʧ��");
        }
    }   
    public bool Login(string userName,string password)
    {
        string query = "SELECT * FROM playerAccount WHERE username = @UserName AND password = @Password";
        SqlCommand command = new SqlCommand(query, conn);

        // ��Ӳ������Զ�����ת�������У�飩
        command.Parameters.AddWithValue("@UserName", userName);
        command.Parameters.AddWithValue("@Password", password);
        using (SqlDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                Debug.Log("��¼�ɹ�");
                return true;
            }
            else
            {
                Debug.Log("��¼ʧ��");
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
