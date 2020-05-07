using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Platform2DUtils.GameplaySystem;
using Cinemachine;

[Serializable]

public class Party
{
    /*
    [SerializeField]
    List<Player> currentParty;
    [SerializeField]
    Player[] players;

    public Player[] Players { get => players; set => players = value; }

    public void InitParty()
    {
        
        players = GameplaySystem.FindPlayer;

        for (int i = 0; i < Players.Length; i++)
        {
            Player p = Players[i];
            if (p.IsLeader)
            {
                p.IsNpc = false;
                if (currentParty.Count > 0)
                {
                    currentParty.Insert(0, p);
                }
                else
                {
                    currentParty.Add(p);
                }
            }
            else
            {
                p.IsNpc = true;
                currentParty.Add(p);
            }
        }
        for (int i = 1; i < currentParty.Count; i++)
        {
            currentParty[i].Target = currentParty[i - 1];
        }
    }

    public void SwapLeader()
    {
        if (Input.GetButtonDown("ChangeLeader") && currentParty.Count > 1)
        {
            Player currentLeader = currentParty[0];
            currentLeader.IsLeader = false;
            currentLeader.IsNpc = true;
            currentLeader.Target = currentParty[currentParty.Count - 1];
            currentParty.RemoveAt(0);
            currentLeader.gameObject.tag = "NPC";
            currentParty.Add(currentLeader);
            currentParty[0].gameObject.tag = "Player";
            currentParty[0].IsLeader = true;
            currentParty[0].IsNpc = false;
            currentParty[0].Target = null;
        }
    }

    public void KillLeader()
    {
        
        if(currentParty.Count > 1)
        {
            Player currentLeader = currentParty[0];
            currentLeader.IsLeader = false;
            currentLeader.IsNpc = true;
            currentParty.RemoveAt(0);
            currentParty[0].gameObject.tag = "Player";
            currentParty[0].IsLeader = true;
            currentParty[0].IsNpc = false;
            currentParty[0].Target = null;
        } 
        else
        {
            currentParty.RemoveAt(0);
        }
    }
    */
}
