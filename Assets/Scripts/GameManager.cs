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
    
    public int Score { get => score; set => score = value; }
    [SerializeField] Text txtScore;

    int kills = 0;

    bool isInCombat = false;
    public bool IsInCombat { get => isInCombat; set => isInCombat = value; }
    bool isInChase = false;
    public bool IsInChase { get => isInChase; set => isInChase = value; }
    public int Kills { get => kills; }

    [SerializeField]
    public const float originalBulletDamage = 5f;
    public float bulletDamage = 5f;
    
    [SerializeField]
    public const float originalZombieDamage = 10f;
    public float zombieDamage = 10f;
    
    [SerializeField]
    public bool instakillBuff = false;

    [SerializeField] SoundManager soundManager;
    AudioSource audioSource;

    [SerializeField]
    public Party party; 

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
        // player.Animator.SetLayerWeight(player.Animator.GetLayerIndex("Combat"), 0);
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

    public void CountZombieKill(int kill,int killPoints)
    {
        score += killPoints;
        kills += kill;
        Debug.Log(kills);
        Debug.Log("Tienes " + score + " Puntos");
    }
    
    // Reset Buff stats after 12 seconds
    public IEnumerator ResetBuffs(string buff)
    {
        Debug.Log("Se quiere resetear");
        yield return new WaitForSeconds(12);

        switch(buff)
        {
            case "damage":
                Debug.Log(buff + " reset");
                bulletDamage = originalBulletDamage;
            break;

            case "defense":
                Debug.Log(buff + " reset");
                zombieDamage = originalZombieDamage;  
            break;

            case "instakill":
                Debug.Log(buff + " reset");
                instakillBuff = false;
            break;
        }
    }
}