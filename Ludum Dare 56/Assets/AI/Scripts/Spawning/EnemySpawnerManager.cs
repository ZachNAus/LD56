using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawnerManager : MonoBehaviour
{
	[SerializeField] Wave[] waves;

	[SerializeField] List<Transform> spawnPositions = new List<Transform>();

	[SerializeField] Transform enemyHolder;

	[SerializeField] int distFromPlayer;

	[SerializeField] TextMeshProUGUI waveTxt;
	[SerializeField] CanvasGroup waveArea;

	[Space]

	[SerializeField] GameObject deathStuff;
	[SerializeField] Button restartBtn;

	List<IHasHealth> ActiveEnemies = new List<IHasHealth>();

	public int CurrentWave { get; private set; }

	private void Start()
	{
		//StartWave();

		StartCoroutine(StartCutscene());
		IEnumerator StartCutscene()
		{
			PlayerStats.instance.playerDialogue.SaySomething("What happened to my cheese!?!?!?!?!", 0);
			yield return new WaitForSeconds(3);

			LadyBugDialogue.instance.SaySomething("Hey friend! No need to be so loud!", 0);
			yield return new WaitForSeconds(3);

			PlayerStats.instance.playerDialogue.SaySomething("Who did this, I need revenge!!", 0);
			yield return new WaitForSeconds(3);

			LadyBugDialogue.instance.SaySomething("Well, you're in luck, you have that shield right? You just need a sword.", 0);
			yield return new WaitForSeconds(4);
		
			LadyBugDialogue.instance.SaySomething("Grab that stick and get to work!", 5);
		}


		PlayerStats.instance.OnDeath.AddListener(OnPlayerDie);

		deathStuff.gameObject.SetActive(false);
		restartBtn.onClick.AddListener(() =>
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		});
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

		if (string.IsNullOrEmpty(waves[CurrentWave].playerMessage) == false)
		{
			PlayerStats.instance.playerDialogue.SaySomething(waves[CurrentWave].playerMessage, 0);
		}
	}

	void SpawnEnemy(Wave.EnemyStats enemy)
	{
		var inst = Instantiate(enemy.enemy, enemyHolder);

		//Pick a spawn point away from someone else
		var random = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];

		possibleSpawnPoints.Remove(random);

		if (enemy.permanantlyRemoveSpot)
		{
			spawnPositions.Remove(random);
		}

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

					deathStuff.gameObject.SetActive(true);
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
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
		deathStuff.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		float alpha = 0;
		DOTween.To(() => alpha, x => alpha = x, 1, 1).OnUpdate(() =>
		{
			waveArea.alpha = alpha;
		});
	}
}
