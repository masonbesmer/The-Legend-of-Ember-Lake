using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    [SerializeField] SpawnerScriptableObject enemyDataContainer;
    [SerializeField] GameObject player;
    [SerializeField] EnemyTypes enemyType;
    [SerializeField] int numberOfEnemies;
    [SerializeField] float offsetBetweenEachOther;
    [SerializeField] int distance;

    [Header("Dont mind this")]
    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask groundMask;
   
    GameObject enemyToSpawnObject;
    // Start is called before the first frame update
    void Start()
    {
        enemyToSpawnObject = enemyDataContainer.GetEnemyInstance(enemyType);
        enemyToSpawnObject.GetComponent<BehaviourTree.Tree>().SetTarget(player);
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnEnemyDeath(Enemy enemy)
    {
       // enemy.onDeathAction -= OnEnemyDeath;
    }

    IEnumerator Spawn()
    {
        while (true)
        {

            for (int i = 0; i < numberOfEnemies; i++)
            {
                yield return new WaitUntil(() => transform.childCount < numberOfEnemies);

                GameObject enemyObject = Instantiate(enemyToSpawnObject, transform);
                enemyObject.transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
                enemyObject.transform.localScale *= 3;
                bool hasFoundSpawnPoint = false;

                while (!hasFoundSpawnPoint)
                {
                    var offsetPosition = Random.insideUnitSphere * distance;
                    Ray ray = new Ray(transform.position + offsetPosition + (Vector3.up * distance), Vector3.down);

                    if (!Physics.SphereCast(ray, offsetBetweenEachOther, 100, enemyMask))
                    {
                        if (Physics.Raycast(ray, out RaycastHit hit, 100, groundMask))
                        {
                            Debug.Log("Spawning");
                            enemyObject.transform.position = hit.point + Vector3.up * 2.0f;
                            hasFoundSpawnPoint = true;
                            break;
                        }
                        //  yield return null;
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return new WaitForSeconds(10.0f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distance);
    }

}
