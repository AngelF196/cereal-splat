using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Results : MonoBehaviour
{
    public int GetWinner()
    {
        PlayerDie[] players = FindObjectsOfType<PlayerDie>();

        if (players.Length == 0)
        {
            return 0;
        }

        PlayerDie winner = players[0];

        foreach (PlayerDie player in players)
        {
            if (player.timesDied < winner.timesDied)
            {
                winner = player;
            }
        }

        PlayerStats winnerstats = winner.GetComponent<PlayerStats>();

        int winningIndex = winnerstats.playerIndex;

        return winningIndex;
    }
}
