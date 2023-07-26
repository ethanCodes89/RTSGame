using TabletopRTS.UnitBehavior;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectSelectionTexture : MonoBehaviour
{
    private IUnit selectableRef;
    private DecalProjector selectionProjector;
    private void Awake()
    {
        selectionProjector = GetComponentInChildren<DecalProjector>();
        selectableRef = GetComponent<IUnit>();
    }

    private void Update()
    {
        selectionProjector.gameObject.SetActive(selectableRef.IsSelected);
    }
}
