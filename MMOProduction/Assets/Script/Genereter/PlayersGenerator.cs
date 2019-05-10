using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersGenerator : MonoBehaviour
{
    
    public OtherPlayers GenerateOtherPlayer()
    {
        GameObject prefab = (GameObject)Resources.Load("OtherPlayer");
        GameObject other = Instantiate(prefab);
        Debug.Log("ジェネレート");
        return other.GetComponent<OtherPlayers>();
    }

    public PlayerController GeneratePlayer()
    {
        Debug.Log("Playerのジェネレート");
        GameObject prefab = (GameObject)Resources.Load("Player");
        GameObject player = Instantiate(prefab);
        return player.GetComponent<PlayerController>();
    }

}
