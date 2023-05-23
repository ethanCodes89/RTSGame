using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GoldResourceNetBehavior : NetworkBehaviour
{
    public NetworkVariable<int> AmountOfGold { get; private set; } = new(MAX_GOLD);

    private Vector3 originalScale;
    private const int MAX_GOLD = 50;

    public override void OnNetworkSpawn()
    {
        originalScale = gameObject.transform.localScale;

        if (!IsServer)
            return;

        AmountOfGold.OnValueChanged += OnAmountOfGoldChanged;
    }

    public bool MineGold()
    {
        if (AmountOfGold.Value > 0)
        {
            AmountOfGold.Value -= 1;
            return true;
        }
        return false;
    }

    private void OnAmountOfGoldChanged(int oldAmount, int newAmount)
    {
        if (!IsServer)
            return;
        
        //inform all of the clients(and host) that some gold was mined
        UpdateXScaleBasedOnGoldAmountClientRpc();
    }

    [ClientRpc]
    private void UpdateXScaleBasedOnGoldAmountClientRpc()
    {
        float newXScale = originalScale.x * ((float)AmountOfGold.Value / MAX_GOLD);
    }
}
