using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public delegate void DeathHandler(GameObject monster);
    public event DeathHandler OnDeath;

    public void Die()
    {
        
        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }
}