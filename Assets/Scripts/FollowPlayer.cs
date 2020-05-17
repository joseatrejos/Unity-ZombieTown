using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject tPlayer;
    public Transform tFollowTarget;
    private CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void LateUpdate()
    {
        if (GameManager.instance.party.CurrentParty.Count >= 1 && (tPlayer == null || Input.GetButtonDown("ChangeLeader") ) )
        {
            tPlayer = GameManager.instance.party.CurrentParty[0].gameObject;
            tFollowTarget = tPlayer.transform;
            vcam.Follow = tPlayer.transform;
        }
    }
}
