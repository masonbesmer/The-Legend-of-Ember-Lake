using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes { FROG, RACCOON, MOSQUITO, SQUIRREL, OWL }

[CreateAssetMenu(fileName = "New Enemy",menuName = "Enemy")]
public class SpawnerScriptableObject : ScriptableObject
{
    //Script hasn't been implemented for enemy yet so I'm just using GO here.
    public GameObject frogEnemy;
    public GameObject raccoonEnemy;
    public GameObject mosquitoEnemy;
    public GameObject squirrelEnemy;
    public GameObject owlEnemy;

    public GameObject GetEnemyInstance(EnemyTypes enemyType)
    {
        GameObject enemy = null;

        switch (enemyType)
        {
            case EnemyTypes.FROG:
                enemy = frogEnemy;
                break;
            case EnemyTypes.RACCOON:
                enemy = raccoonEnemy;
                break;
            case EnemyTypes.MOSQUITO:
                enemy = mosquitoEnemy;
                break;
            case EnemyTypes.SQUIRREL:
                enemy = squirrelEnemy;
                break;
            case EnemyTypes.OWL:
                enemy = owlEnemy;
                break;
        }

        return enemy;
    }
}
