using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTempData
{
    public PlayerTempData()
    {
        status = Status.None;
        room = null;
        isOwner = false;
    }
    public enum Status
    {
        None,
        Room,
        Fight,
    }
    public Status status;
    public Room room;
    public bool isOwner = false;

}
