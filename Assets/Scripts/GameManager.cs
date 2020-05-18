using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] Player player;

    public Player Player { get => player; }

    [SerializeField] int score = 0;

    public int Score { get => score; set => score = value; }
    [SerializeField] Text txtScore;
    public Text TxtScore { get => txtScore; set => txtScore = value; }

    int kills = 0;

    bool isInCombat = false;
    public bool IsInCombat { get => isInCombat; set => isInCombat = value; }
    bool isInChase = false;
    public bool IsInChase { get => isInChase; set => isInChase = value; }
    public int Kills { get => kills; }

    [SerializeField] public const float originalBulletDamage = 5f;
    public float bulletDamage = 5f;

    [SerializeField] public const float originalZombieDamage = 10f;
    public float zombieDamage = 10f;

    [SerializeField] public bool instakillBuff = false;

    [SerializeField] public float enemySpeed = 3.5f;

    [SerializeField] SoundManager soundManager;
    AudioSource audioSource;

    [SerializeField] public Party party;

    [SerializeField] protected GameObject pauseMenu;
    [SerializeField] GameObject cantChange;
    public GameObject CantChange { get => cantChange; set => cantChange = value; }

    [SerializeField] GameObject invencible;
    public GameObject Invencible { get => invencible; set => invencible = value; }

    bool invencibility = false;
    public bool Invencibility { get => invencibility; set => invencibility = value; }

    [SerializeField] GameObject life;

    [SerializeField] public GameObject gameOver;

    [SerializeField] public GameObject gameWin;

    [SerializeField] public GameObject roundOver;

    public GameObject Life { get => life; set => life = value; }

    [SerializeField] Text txtRound;

    int round = 1;

    float scale;

    [SerializeField] int vaccineCount = 0;
    public int VaccineCount { get => vaccineCount; }
    [SerializeField] int winCount = 10;
    public int WinCount { get => winCount; }

    public float Scale { get => scale; set => scale = value; }

    bool start = false;

    public bool isShooting = false;

    [SerializeField] Image blkImage;

    private Vector3 lifeSize;
    public Vector3 LifeSize { get => lifeSize; set => lifeSize = value; }

    public int Round { get => round; set => round = value; }

    void Awake()
    {
        lifeSize = life.transform.localScale;
        if (!instance)
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

        gameOver.SetActive(false);
        gameWin.SetActive(false);
        roundOver.SetActive(false);
    }

    public void StartCombat()
    {
        soundManager.WeaponDrawn();
        //StartCoroutine(DelayedCombatMusic());
        isInCombat = true;
        // player.Animator.SetLayerWeight(1, 1);
        // player.WeaponVisibility(true);
        isInCombat = true;
    }

    public void EscapeCombatAndChase()
    {
        if (isInCombat || isInChase)
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
        //soundManager.PlayChaseMusic();
        isInChase = true;
    }

    IEnumerator DelayedCombatMusic()
    {
        yield return new WaitForSeconds(1);
        //soundManager.PlayCombatMusic();
    }

    void Update()
    {
        if (Input.GetButtonDown("ChangeLeader"))
        {
            if (party.CurrentParty.Count > 1)
            {
                cantChange.SetActive(true);
                StartCoroutine(party.waitForChange());
            }
            else
            {
                cantChange.SetActive(true);
            }
            party.SwapLeader();
        }
        else if (Input.GetButtonDown("Submit") && !Win && !party.PartyDeath)
        {
            if (!start)
            {
                Pause();
                start = true;
            }
            else
            {
                Unpause();
                start = false;
            }
        }
    }

    public void CountZombieKill(int kill, int killPoints)
    {
        score += killPoints;
        kills += kill;
        txtScore.text = $"{score}";
    }

    public void ChangeRound()
    {
        if (kills > round * 5)
        {
            roundOver.SetActive(true);

            round++;
            txtRound.text = $"{round}";

            // Fill the pool
            if (round <= 3)
            {
                ObjectPooler.Instance.AddEnemiesToPool("Enemy");
            }
            if (enemySpeed <= player.moveSpeed)
                enemySpeed *= 1.01f;
            else
                zombieDamage *= 1.05f;
        }
    }

    // Reset Buff stats after 12 seconds
    public IEnumerator ResetBuffs(string buff, float buffDuration, Player playerBuffed)
    {
        yield return new WaitForSeconds(buffDuration);

        switch (buff)
        {
            case "damage":
                bulletDamage = originalBulletDamage;
                break;

            case "defense":
                zombieDamage = originalZombieDamage;
                Invencible.SetActive(false);
                invencibility = false;
                break;

            case "instakill":
                instakillBuff = false;
                break;
        }
    }

    public void AddPoints(int points)
    {
        vaccineCount += points;
    }

    public void Pause()
    {
        StartCoroutine(FadeIn(blkImage));
    }

    public void Unpause()
    {
        // Resume time
        Time.timeScale = 1;

        StartCoroutine(FadeOut(blkImage));
    }

    IEnumerator FadeIn(MaskableGraphic element)
    {
        for (double i = 0; i <= 0.75; i += 0.1)
        {
            Color tmp = element.color;
            tmp.a = (float)i;
            element.color = tmp;
            yield return new WaitForSeconds(0.001f);
        }

        // Stop time
        Time.timeScale = 0;

        pauseMenu.gameObject.SetActive(true);
    }

    IEnumerator FadeOut(MaskableGraphic element)
    {
        for (double i = 0.75; i >= 0.0; i -= 0.1)
        {
            Color tmp = element.color;
            tmp.a = (float)i;
            element.color = tmp;
            yield return new WaitForSeconds(0.001f);
        }
        pauseMenu.gameObject.SetActive(false);
    }

    public bool Win
    {
        get => VaccineCount >= WinCount;
    }
}