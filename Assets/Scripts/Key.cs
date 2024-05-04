using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            transform.SetParent(player.ItemPoint);
            transform.localPosition = Vector3.zero;
        }
    }
}
