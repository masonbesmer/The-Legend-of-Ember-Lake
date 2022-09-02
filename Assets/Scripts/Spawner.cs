using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    [SerializeField] SpawnerScriptableObject enemyDataContainer;
    [SerializeField] GameObject enemyHolder;
    [SerializeField] GameObject player;
    [SerializeField] float activateSpawnRange;
    [SerializeField] EnemyTypes enemyType;
    [SerializeField] int numberOfEnemies;
    [SerializeField] float offsetBetweenEachOther;
    [SerializeField] int distance;

    [SerializeField] Terrain terrain;

    [Header("Dont mind this")]
    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask groundMask;

    bool spawnActivated;
    GameObject enemyToSpawnObject;
    // Start is called before the first frame update
    void Start()
    {
        spawnActivated = false;
        enemyHolder.SetActive(false);
        enemyToSpawnObject = enemyDataContainer.GetEnemyInstance(enemyType);
        enemyToSpawnObject.GetComponent<BehaviourTree.Tree>().SetTarget(player,terrain);
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        bool isWithinAttackRange = ExtensionMethodsBT.GetDistance(ExtensionMethodsBT.GetXZVector(transform.position), ExtensionMethodsBT.GetXZVector(player.transform.position)) <= activateSpawnRange;
        if (isWithinAttackRange && !spawnActivated)
        {
            spawnActivated = true;
            enemyHolder.SetActive(true);
        }
        else if(!isWithinAttackRange && spawnActivated)
        {
            spawnActivated = false;
            enemyHolder.SetActive(false);
        }

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
                yield return new WaitUntil(() => enemyHolder.transform.childCount < numberOfEnemies);

                GameObject enemyObject = Instantiate(enemyToSpawnObject, enemyHolder.transform);
                enemyObject.transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
              //  enemyObject.transform.localScale *= 3;
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, activateSpawnRange);
    }

}
