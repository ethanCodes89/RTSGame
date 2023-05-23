using Unity.Netcode;
using UnityEngine;

public class BaseNetBehavior : NetworkBehaviour
{
    [SerializeField] 
    private GameObject workerPrefabToSpawn;

    private GoldResourceNetBehavior goldResourceTarget = null;
    private float currentSpawnTime = 0f;
    private int workersSpawned = 0;
    private const int MAX_WORKERS = 10;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        currentSpawnTime = Random.Range(1f, 3.5f);
    }

    public void SetGoldResourceTarget(GoldResourceNetBehavior goldResourceTarget)
    {
        this.goldResourceTarget = goldResourceTarget;
    }

    private void Update()
    {
        if (!IsServer || workersSpawned >= MAX_WORKERS)
            return;
        currentSpawnTime -= Time.deltaTime;

        if (currentSpawnTime >= 0f)
            return;
        SpawnNewBaseWorker();
    }

    private void SpawnNewBaseWorker()
    {
        var newInstanceOfBaseWorker = Instantiate(workerPrefabToSpawn, transform.position, Quaternion.identity);
        var networkObject = newInstanceOfBaseWorker.GetComponent<NetworkObject>();
        networkObject.Spawn(true);

        var baseWorkerNetBehavior = newInstanceOfBaseWorker.GetComponent<BaseWorkerNetBehavior>();

        baseWorkerNetBehavior.SetGoldResourceTarget(this, goldResourceTarget);
        currentSpawnTime = Random.Range(1f, 5.5f);
        workersSpawned++;
    }
}
