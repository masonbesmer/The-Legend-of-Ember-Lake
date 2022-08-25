using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes { FROG }

[CreateAssetMenu(fileName = "New Enemy",menuName = "Enemy")]
public class SpawnerScriptableObject : ScriptableObject
{
    //Script hasn't been implemented for enemy yet so I'm just using GO here.
    public GameObject frogEnemy;

    public GameObject GetEnemyInstance(EnemyTypes enemyType)
    {
        GameObject enemy = null;

        switch (enemyType)
        {
            case EnemyTypes.FROG:
                enemy = frogEnemy;
                break;
        }

        return enemy;
    }
}
