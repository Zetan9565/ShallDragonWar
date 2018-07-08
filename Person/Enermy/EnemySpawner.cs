using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public List<EnemyInfoAgent> enemyList;
    public GameObject enemyPrafab;
    public LayerMask terrainLayer = ~0;
    //public GameObject wayPointPrefab;
    public float spawnTime = 30f;
    public float spawnRange;
    float realSpawnRange;
    //public int MinSpawn;
    public int maxSpawn = 10;
    public int maxWayPoint;
    public List<GameObject> wayPoints;
    public float rayPointHeight;
    bool isSpawn;

    public bool testMode;
    public bool gizmos;
    bool isInit;

    private void OnEnable()
    {
        //Debug.Log("SpawnerEnable");
        if (!isInit)
        {
            if(!testMode) StartCoroutine(WaitPlayerInit());
            else
            {
                isInit = true;
                GetWayPoint();
                GetEenemys();
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        realSpawnRange = spawnRange / Mathf.Sqrt(2);
        if (!testMode) StartCoroutine(WaitPlayerInit());
        else
        {
            GetWayPoint();
            GetEenemys();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (EnemyInfoAgent enemy in enemyList)
        {
            if (!enemy.IsAlive)
            {
                enemy.timeAfterDeath += Time.deltaTime;
                if (enemy.timeAfterDeath > spawnTime) Spawn(enemy);
            }
        }
    }

    public void Spawn(EnemyInfoAgent enemy)
    {
        if (!isSpawn) GetEenemys();
        enemy.transform.position = wayPoints[Random.Range(0, wayPoints.Count)].transform.position;
        enemy.Relive();
        MyTools.SetActive(enemy.gameObject, true);
    }

    IEnumerator WaitPlayerInit()
    {
        yield return new WaitUntil(() => PlayerInfoManager.Instance && PlayerInfoManager.Instance.isInit);
        isInit = true;
        GetWayPoint();
        GetEenemys();
    }

    public void GetEenemys()
    {
        //if (wayPoints == null || wayPoints.Count <= 0) return;
        if (isSpawn) return;
        enemyList = new List<EnemyInfoAgent>();
        for (int i = 0; i < maxSpawn; i++)
        {
            GameObject enemy = ObjectPoolManager.Instance.Get(enemyPrafab, wayPoints[Random.Range(0, wayPoints.Count)].transform.position,
                transform.rotation);
            enemy.transform.SetParent(transform, true);
            EnemyInfoAgent enemyInfoAgent = enemy.GetComponent<EnemyInfoAgent>();
            enemyInfoAgent.Relive();
            BehaviorDesigner.Runtime.BehaviorTree btree = enemy.GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
            btree.SetVariableValue("Player", GameObject.FindWithTag("Player"));
            btree.SetVariableValue("SpawnPoint", gameObject);
            btree.SetVariableValue("ResetPoints", wayPoints);
            btree.SetVariableValue("SpawnRange", spawnRange);
            MyTools.SetActive(enemy, true);
            enemyList.Add(enemyInfoAgent);
        }
        isSpawn = true;
    }

    public void GetWayPoint()
    {
        List<GameObject> points = new List<GameObject>();
        Vector3 rayPoint = transform.position;
        RaycastHit hit;
        while (points.Count < maxWayPoint)
        {
            rayPoint = new Vector3(transform.position.x + Random.Range(-realSpawnRange, realSpawnRange), transform.position.y + rayPointHeight, transform.position.z + Random.Range(-realSpawnRange, realSpawnRange));
            //if ((rayPoint - transform.position).sqrMagnitude < sqrSpawnRange)
            if (Physics.Raycast(rayPoint, Vector3.down, out hit, Mathf.Infinity, terrainLayer, QueryTriggerInteraction.Ignore))
            {
                GameObject point = new GameObject("WayPoint (" + points.Count + ")");
                point.transform.SetParent(transform);
#if UNITY_EDITOR
                GizmoVisualizer gv = point.AddComponent<GizmoVisualizer>();
                gv.gizmoType = GizmoVisualizer.GizmoType.Sphere;
                gv.debugSize = 0.3f;
#endif
                point.transform.position = hit.point;
                points.Add(point);
            }
        }
        wayPoints = points;
    }

    public Color color = Color.yellow;
    private void OnDrawGizmos()
    {
        if (!gizmos) return;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
        Gizmos.DrawSphere(transform.position + new Vector3(0, rayPointHeight, 0), 0.3f);
        Gizmos.DrawLine(transform.position + new Vector3(0, rayPointHeight, 0), transform.position);
    }
}
