using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class WaveSpawner : MonoBehaviour {

	public enum SpawnState { SPAWNING, WAITING, COUNTING };

	[System.Serializable]
	public class Wave
	{
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}

	public Wave[] waves;
	private int nextWave = 0;
	public int NextWave
	{
		get { return nextWave + 1; }
	}

	public Transform[] spawnPoints;

	public float timeBetweenWaves = 5f;
	private float waveCountdown;
	public float WaveCountdown
	{
		get { return waveCountdown; }
	}

	private float searchCountdown = 1f;

	private SpawnState state = SpawnState.COUNTING;
	public SpawnState State
	{
		get { return state; }
	}

    private AudioManager audioManager;



	void Start()
	{
		//caching sounds
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("WaveSpawner: No AudioManager found in the scene!");
        }

		if (spawnPoints.Length == 0)
		{
			Debug.LogError("WaveSpawner: No spawn points referenced!");
		}

		waveCountdown = timeBetweenWaves;
	}

	void Update()
	{
		if (state == SpawnState.WAITING)
		{
            //Check if enemies are still alive
			if (!EnemyIsAlive())
			{
                //Complete the current round and begin a new round
				WaveCompleted();
			}
			else
			{
				return;
			}
		}

		if (waveCountdown <= 0)
		{
			if (state != SpawnState.SPAWNING)
			{
                //Start spawning wave
				StartCoroutine( SpawnWave (waves[nextWave]));
			}
		}
		else
		{
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted()
	{
		Debug.Log("Wave Completed!");

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

        //IF WE EXCEED THE NUMBER OF WAVES WE HAVE SET UP IN THE INSPECTOR, LOOP BACK FROM THE FIRST WAVE
		if (nextWave + 1 > waves.Length - 1)
		{
			//nextWave = 0;
			Debug.Log("ALL WAVES COMPLETED!");
			audioManager.StopSound("RespawnCountdown");  //in case the player dies (not out of lives) when the game ends..
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //this will load the endgame scene
		}
		else
		{
			nextWave++;
		}
	}

	bool EnemyIsAlive()
	{
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f) //we only check if there are enemies once a second
		{
			searchCountdown = 1f;  //reset the timer
			if (GameObject.FindGameObjectWithTag("Enemy") == null) //if you can't find an enemy on the scene, return false
			{
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave(Wave _wave)
	{
		Debug.Log("Spawning Wave: " + _wave.name);

		state = SpawnState.SPAWNING;

        //Spawn enemies
		for (int i = 0; i < _wave.count; i++)
		{
			SpawnEnemy(_wave.enemy);
			yield return new WaitForSeconds(1f/_wave.rate);
		}

		state = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy(Transform _enemy)
	{
		Debug.Log("Spawning Enemy: " + _enemy.name);

        //Spawn enemy at random spawn point
		Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
		Instantiate(_enemy, _sp.position, _sp.rotation);
	}

}

