using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool isLocked = true;

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    public void Open()
    {
        if (!isLocked)
        {
            // Логика открытия сундука и выдачи лута
        }
    }
}