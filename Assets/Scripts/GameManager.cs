using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] Player player;

    public Player Player { get => player; }

    int score = 0;
    
    public int Score { get => score; }
    [SerializeField] Text txtScore;

    
    int kills = 0;

    bool isInCombat = false;
    public bool IsInCombat { get => isInCombat; set => isInCombat = value; }
    bool isInChase = false;
    public bool IsInChase { get => isInChase; set => isInChase = value; }
    public int Kills { get => kills; }

    [SerializeField] SoundManager soundManager;

    AudioSource audioSource;

    [SerializeField]
    public Party party; 

    [SerializeField]
    Enemy enemy;

    void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        soundManager.AudioSource = GetComponent<AudioSource>();
        soundManager.PlayBGM();
        party.InitParty();
    }

    public void StartCombat()
    {
        soundManager.WeaponDrawn();
        StartCoroutine(DelayedCombatMusic());
        isInCombat = true;
       // player.Animator.SetLayerWeight(1, 1);
       // player.WeaponVisibility(true);
        isInCombat = true;
    }

    public void EscapeCombatAndChase()
    {
        if(isInCombat || isInChase)
            soundManager.PlayBGM();
        isInCombat = false;
       // player.Animator.SetLayerWeight(player.Animator.GetLayerIndex("Base Layer"), 1);
        //player.Animator.SetLayerWeight(player.Animator.GetLayerIndex("Combat"), 0);
       // player.WeaponVisibility(false);
        isInChase = false;

        /*
        Debug.Log("Base Layer Index: " + player.Animator.GetLayerIndex("Base Layer"));
        Debug.Log("Base Layer Weight: " + player.Animator.GetLayerWeight(player.Animator.GetLayerIndex("Base Layer")));
        Debug.Log("Combat Layer Index: " + player.Animator.GetLayerIndex("Combat"));
        Debug.Log("Combat Layer Weight: " + player.Animator.GetLayerWeight(player.Animator.GetLayerIndex("Combat")));
        */
    }

    public void BeginChase()
    {
        soundManager.PlayChaseMusic();
        isInChase = true;  
    }

    IEnumerator DelayedCombatMusic()
    {
        yield return new WaitForSeconds(1);
        soundManager.PlayCombatMusic();
    }

  
    void Update()
    {
        if(Input.GetButtonDown("ChangeLeader"))
        {
            if(party.CurrentParty.Count > 1)
            {
                
            StartCoroutine(party.waitForChange());
            } else
            {
                Debug.Log("solo tienes un personaje en el grupo");
            }
        }
        party.SwapLeader();
    }
    public void AddPoints(int points)
    {
        this.score += points;
        
        txtScore.text = $"Score: {score} pts";
    }

    public void CountZombieKill(int kill)
    {
        score += enemy.KillPoints;
        kills += kill;
        Debug.Log(kills);
        Debug.Log("Tienes " + score + " Puntos");
    }

}
