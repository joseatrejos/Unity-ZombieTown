using System.Collections;
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
    }

    ///<summary>
    /// Change the leader of the party when you press a button that you assign in the input manager
    ///</summary>
    public void SwapLeader()
    {
        if (Input.GetButtonDown("ChangeLeader") && currentParty.Count > 1 && canChange)
        {
            Debug.Log("No puedes cambiar de jugador por 6 segundos");

            Player currentLeader = currentParty[0];
            currentLeader.IsLeader = false;
            currentLeader.IsNpc = true;
            currentLeader.HasParty = true;
            currentLeader.Target = currentParty[currentParty.Count - 1];
            currentParty.RemoveAt(0);
            currentLeader.gameObject.tag = "NPC";
            currentParty.Add(currentLeader);
            currentParty[0].gameObject.tag = "Player";
            currentParty[0].IsLeader = true;
            currentParty[0].IsNpc = false;
            currentParty[0].Target = null;
            currentParty[0].HasParty = true;
            canChange = false;
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
            currentParty.RemoveAt(0);
            currentParty[0].gameObject.tag = "Player";
            currentParty[0].IsLeader = true;
            currentParty[0].IsNpc = false;
            currentParty[0].Target = null;
        } 
        else
        {
            currentParty.RemoveAt(0);
            partyDeath = true;
        }
    }

    public IEnumerator waitForChange()
    {
        yield return new WaitForSeconds(6.0f);
        canChange = true;
        Debug.Log("Puedes cambiar");
    }
}
