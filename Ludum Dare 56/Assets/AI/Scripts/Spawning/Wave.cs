using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave")]
public class Wave : ScriptableObject
{
    public EnemyDict toSpawn;

    public string playerMessage;

    [System.Serializable]
    public class EnemyStats
	{
        public GameObject enemy;

        public bool permanantlyRemoveSpot;
	}

    [System.Serializable]
    public class EnemyDict : SerializableDictionary<EnemyStats, int> { }
}
