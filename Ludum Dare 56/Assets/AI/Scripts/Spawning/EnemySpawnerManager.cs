using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
	[SerializeField] Wave[] waves;

	[SerializeField] Transform[] spawnPositions;

	[SerializeField] Transform enemyHolder;

	[SerializeField] int distFromPlayer;

	[SerializeField] TextMeshProUGUI waveTxt;
	[SerializeField] CanvasGroup waveArea;

	List<IHasHealth> ActiveEnemies = new List<IHasHealth>();

	public int CurrentWave { get; private set; }

	private void Start()
	{
		StartWave();

		PlayerStats.instance.OnDeath.AddListener(OnPlayerDie);
	}

	List<Transform> possibleSpawnPoints = new List<Transform>();
	public void StartWave()
	{
		ActiveEnemies.Clear();

		waveTxt.SetText($"WAVE {CurrentWave + 1}");

		float alpha = 0;
		DOTween.To(() => alpha, x => alpha = x, 1, 1).OnUpdate(() =>
		{
			waveArea.alpha = alpha;
		}).OnComplete(() =>
		{
			DOTween.To(() => alpha, x => alpha = x, 0, 1).OnUpdate(() =>
			{
				waveArea.alpha = alpha;
			}).SetDelay(2);
		});

		possibleSpawnPoints.Clear();
		possibleSpawnPoints.AddRange(spawnPositions.Where(x => ((x.position - PlayerStats.instance.transform.position).sqrMagnitude) > distFromPlayer * distFromPlayer));

		foreach (var enemy in waves[CurrentWave].toSpawn)
		{
			for (int i = 0; i < enemy.Value; i++)
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

		if (health != null)
		{
			ActiveEnemies.Add(health);

			health.OnDeath.AddListener(() => OnEnemyKilled(health));
		}
	}

	void OnEnemyKilled(IHasHealth health)
	{
		ActiveEnemies.Remove(health);

		if (ActiveEnemies.Count() == 0)
		{
			StartCoroutine(NextWave());

			IEnumerator NextWave()
			{
				yield return new WaitForSeconds(2);

				CurrentWave++;

				if (CurrentWave >= waves.Count())
				{
					waveTxt.SetText($"YOU WIN!");

					float alpha = 0;
					DOTween.To(() => alpha, x => alpha = x, 1, 1).OnUpdate(() =>
					{
						waveArea.alpha = alpha;
					});
				}
				else
				{
					StartWave();
				}
			}
		}
	}

	void OnPlayerDie()
	{
		waveTxt.SetText($"YOU DIED...");

		float alpha = 0;
		DOTween.To(() => alpha, x => alpha = x, 1, 1).OnUpdate(() =>
		{
			waveArea.alpha = alpha;
		});
	}
}
