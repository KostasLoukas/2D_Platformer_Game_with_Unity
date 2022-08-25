using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;


    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives
    {
        get { return _remainingLives; }
    }

    [SerializeField]
    private int startingMoney;
    public static int Money;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }


    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;
    private KeyCode upgradeKey = KeyCode.U;
    private KeyCode exitGameKey = KeyCode.Escape;

    [SerializeField]
    private WaveSpawner waveSpawner;

    public delegate void UpgradeMenuCallback(bool active);  //this is a way to create a type so to store references to function and then call it to call all of the different function stored in it (like an event trigger)
    public UpgradeMenuCallback onToggleUpgradeMenu;

    //caching sounds
    private AudioManager audioManager;

    public bool gameIsOver = false;



    void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("GameMaster: No cameraShake referenced in GameMaster!");
        }

        _remainingLives = maxLives;

        Money = startingMoney;

        //caching sounds
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("GameMaster: No AudioManager found in the scene!");
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(upgradeKey) && gameIsOver != true)
        {
            ToggleUpgradeMenu();
        }

        if (Input.GetKeyDown(exitGameKey))
        {
            Application.Quit();
        }
    }


    private void ToggleUpgradeMenu()
    {
        //this seems to cause problems quite regularly...
        if (upgradeMenu != null && onToggleUpgradeMenu != null)
        {
            upgradeMenu.SetActive(!upgradeMenu.activeSelf);
            waveSpawner.enabled = !upgradeMenu.activeSelf;  //disable/enable the WaveSpawner when the upgrade menu is toggled
            onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
        }
    }


    public void EndGame()
    {
        audioManager.PlaySound(gameOverSoundName);

        Debug.Log("GAME OVER!");
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }
    

    public IEnumerator _RespawnPlayer()
    {
        audioManager.PlaySound(respawnCountdownSoundName);
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound(spawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(clone, 3f);  //destroy the particles after three seconds (they need two seconds to fade out anyway)
        
    }


    public static void KillPlayer (Player player)
    {
        Destroy(player.gameObject);
        _remainingLives--;

        if (_remainingLives <= 0)
        {
            //End game
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }


    public static void KillEnemy(Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }


    public void _KillEnemy(Enemy _enemy)
    {
        //Enemy death sound
        audioManager.PlaySound(_enemy.deathSoundName);

        //Gain some money
        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");

        //Particles
        GameObject _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity).gameObject;
        Destroy(_clone, 5f);

        //Camera shake
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
