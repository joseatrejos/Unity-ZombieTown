﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Platform2DUtils.GameplaySystem;
using Cinemachine;

[Serializable]

public class Party
{ 
    [SerializeField]
    List<Player> currentParty;

    [SerializeField]
    Player[] players;

    [SerializeField]
    bool canChange = true;
    public bool CanChange { get => canChange; }

    bool partyDeath = false;
    public bool PartyDeath { get => partyDeath; }

    public Player[] Players { get => players; set => players = value; }
    public List<Player> CurrentParty { get => currentParty; set => currentParty = value; }

    ///<summary>
    /// fill the party with the players that are in the scene
    ///</summary>

    public void InitParty()
    {
        players = GameplaySystem.FindPlayer;

        for (int i = 0; i < Players.Length; i++)
        {
            Player p = Players[i];
            if (p.IsLeader)
            {
                p.IsNpc = false;
                currentParty.Insert(0, p);
            }
            else
            {
                p.IsNpc = true;
            }
        }
    }
    
    public void JoinParty(Player p)
    {
        currentParty.Add(p);
        currentParty[currentParty.Count-1].Target = currentParty[currentParty.Count-2];
        p.HasParty = true;
        if(currentParty.Count == 2)
        {
            GameManager.instance.CantChange.SetActive(false);
        }
    }

    ///<summary>
    /// Change the leader of the party when you press a button that you assign in the input manager
    ///</summary>
    public void SwapLeader()
    {
        if (currentParty.Count > 1 && canChange)
        {
            Player currentLeader = currentParty[0];
            currentLeader.IsLeader = false;
            currentLeader.IsNpc = true;
            currentLeader.HasParty = true;
            currentLeader.Target = currentParty[currentParty.Count - 1];
            currentLeader.navMeshAgent.enabled = true;
            currentLeader.GetComponent<Collider>().isTrigger = true;
            currentParty.RemoveAt(0);
            currentLeader.gameObject.tag = "NPC";
            currentParty.Add(currentLeader);
            currentParty[0].gameObject.tag = "Player";
            currentParty[0].IsLeader = true;
            currentParty[0].IsNpc = false;
            currentParty[0].Target = null;
            currentParty[0].HasParty = true;
            canChange = false;
            currentParty[0].navMeshAgent.enabled = false;
            currentParty[0].GetComponent<Collider>().isTrigger = false;
            GameManager.instance.party.currentParty[0].ScaleLife();
        }
    }

    ///<summary>
    /// Allow the player that is leading the party to die and give control to the next one
    ///</summary>
    public void KillLeader()
    {
        if(currentParty.Count > 1)
        {
            Player currentLeader = currentParty[0];
            currentLeader.IsLeader = false;
            currentLeader.IsNpc = true;
            currentLeader.navMeshAgent.enabled = true;
            currentLeader.GetComponent<Collider>().isTrigger = true;
            currentParty.RemoveAt(0);
            currentParty[0].gameObject.tag = "Player";
            currentParty[0].IsLeader = true;
            currentParty[0].IsNpc = false;
            currentParty[0].Target = null;
            currentParty[0].navMeshAgent.enabled = false;
            currentParty[0].GetComponent<Collider>().isTrigger = false;
            GameManager.instance.party.currentParty[0].ScaleLife();
        } 
        else
        {
            GameManager.instance.CantChange.SetActive(false);
            currentParty.RemoveAt(0);
            partyDeath = true;
        }
    }

    public IEnumerator waitForChange()
    {
        yield return new WaitForSeconds(6.0f);
        GameManager.instance.CantChange.SetActive(false);
        canChange = true;
    }
}
