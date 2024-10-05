using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] Wave[] waves;

    [SerializeField] Transform[] spawnPositions;

	[SerializeField] Transform enemyHolder;

	[SerializeField] int distFromPlayer;

    List<IHasHealth> ActiveEnemies = new List<IHasHealth>();

    public int CurrentWave { get; private set; }

	private void Start()
	{
		StartWave();
	}

	List<Transform> possibleSpawnPoints = new List<Transform>();
    public void StartWave()
	{
		ActiveEnemies.Clear();

		possibleSpawnPoints.Clear();
		possibleSpawnPoints.AddRange(spawnPositions.Where(x => ((x.position - PlayerStats.instance.transform.position).sqrMagnitude) > distFromPlayer * distFromPlayer));

		foreach (var enemy in waves[CurrentWave].toSpawn)
		{
            for(int i = 0; i < enemy.Value; i++)
			{
				SpawnEnemy(enemy.Key);
			}
		}
	}

    void SpawnEnemy(GameObject enemy)
	{
		var inst = Instantiate(enemy, enemyHolder);

		//Pick a spawn point away from someone else
		var random = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];

		possibleSpawnPoints.Remove(random);

		inst.transform.position = random.position;

		var health = inst.GetComponent<IHasHealth>();

		ActiveEnemies.Add(health);

		health.OnDeath.AddListener(() => OnEnemyKilled(health));
	}

	void OnEnemyKilled(IHasHealth health)
	{
		ActiveEnemies.Remove(health);

		if(ActiveEnemies.Count() == 0)
		{
			CurrentWave++;

			if(CurrentWave >= waves.Count())
			{
				//GAME FINISHED
			}
			else
			{
				StartWave();
			}
		}
	}
}
