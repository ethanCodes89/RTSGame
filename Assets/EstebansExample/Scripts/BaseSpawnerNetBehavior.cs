using Unity.Netcode;
using UnityEngine;


public class BaseSpawnerNetBehavior : NetworkBehaviour
{
    [SerializeField] 
    private BaseData blackBaseData;
    [SerializeField] 
    private BaseData whiteBaseData;
    [SerializeField] 
    private GoldResourceNetBehavior goldResourceTarget = null;
    private NetworkVariable<byte> amountOfBasesSpawned = new(0);

    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += CheckIfBothBasesAreSpawned;
        NetworkManager.Singleton.OnClientConnectedCallback += OnNewClientConnect;
    }

    private void CheckIfBothBasesAreSpawned(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = amountOfBasesSpawned.Value < 2;
    }

    private void OnNewClientConnect(ulong newClientId)
    {
        if (!IsServer)
            return;

        if (amountOfBasesSpawned.Value == 0)
        {
            SpawnNewBase(blackBaseData);
        }
        else
        {
            SpawnNewBase(whiteBaseData);
        }
    }

    private void SpawnNewBase(BaseData baseData)
    {
        GameObject newInstanceOfBase = Instantiate(baseData.BasePrefab, baseData.BaseSpawnPoint);
        NetworkObject networkObject = newInstanceOfBase.GetComponent<NetworkObject>();
        networkObject.Spawn(true);

        BaseNetBehavior baseNetBehavior = newInstanceOfBase.GetComponent<BaseNetBehavior>();
        baseNetBehavior.SetGoldResourceTarget(goldResourceTarget);
        amountOfBasesSpawned.Value += 1;
    }
}