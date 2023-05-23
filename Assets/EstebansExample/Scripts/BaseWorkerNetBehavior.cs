using Unity.Netcode;
using UnityEngine;

public enum BaseWorkerState
{
    IDLE,
    CHASING_GOLD,
    MINING_GOLD,
    RETURNING_GOLD
}

public class BaseWorkerNetBehavior : NetworkBehaviour
{
    public BaseWorkerState CurrentBaseWorkerState { get; private set; } = BaseWorkerState.IDLE;

    [SerializeField] 
    private SpriteRenderer resourceSprite;

    private BaseNetBehavior owningBase = null;
    private GoldResourceNetBehavior goldResourceTarget = null;

    private float miningGoldTime = 3f;
    private float moveSpeed = 0f;
    private Vector3 moveDirection = Vector3.zero;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        moveSpeed = Random.Range(.35f, 1.2f);
    }

    public void SetGoldResourceTarget(BaseNetBehavior ownerBase, GoldResourceNetBehavior goldResourceReference)
    {
        owningBase = ownerBase;
        goldResourceTarget = goldResourceReference;
        UpdateMovementDirection(goldResourceReference.transform.position);
        CurrentBaseWorkerState = BaseWorkerState.CHASING_GOLD;
    }

    private void Update()
    {
        if (!IsServer || goldResourceTarget == null)
            return;
        
        if (CurrentBaseWorkerState == BaseWorkerState.CHASING_GOLD)
        {
            transform.Translate(Time.deltaTime * moveSpeed * moveDirection);
        }
        else if (CurrentBaseWorkerState == BaseWorkerState.MINING_GOLD)
        {
            miningGoldTime -= Time.deltaTime;

            if (miningGoldTime <= 0.0f)
            {
                miningGoldTime = 3f;
                goldResourceTarget.MineGold();
                UpdateMovementDirection(owningBase.transform.position);

                moveSpeed *= .75f; //reduce move speed to simulate carrying gold
                ToggleResourceSpriteClientRpc();

                CurrentBaseWorkerState = BaseWorkerState.RETURNING_GOLD;
            }
        }
        else if (CurrentBaseWorkerState == BaseWorkerState.RETURNING_GOLD)
        {
            transform.Translate(Time.deltaTime * moveSpeed * moveDirection);

            float distanceOfWorkerToBase = Vector3.Distance(owningBase.transform.position, transform.position);

            if (Mathf.Abs(distanceOfWorkerToBase) <= .002f)
            {
                //set target back to gold
                UpdateMovementDirection(goldResourceTarget.transform.position);
                moveSpeed = Random.Range(.35f, 1.2f);
                
                //inform that worker is NOT carrying gold anymore
                ToggleResourceSpriteClientRpc();
                CurrentBaseWorkerState = BaseWorkerState.CHASING_GOLD;
            }
        }
    }

    [ClientRpc]
    private void ToggleResourceSpriteClientRpc()
    {
        resourceSprite.enabled = !resourceSprite.enabled;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
            return;

        if (CurrentBaseWorkerState == BaseWorkerState.CHASING_GOLD)
        {
            CurrentBaseWorkerState = BaseWorkerState.MINING_GOLD;
        }
    }

    private void UpdateMovementDirection(Vector3 newTargetPosition)
    {
        moveDirection = newTargetPosition - transform.position;
        moveDirection.Normalize();
    }
}
