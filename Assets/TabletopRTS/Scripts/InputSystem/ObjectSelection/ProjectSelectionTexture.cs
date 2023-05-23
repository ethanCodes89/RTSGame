using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProjectSelectionTexture : MonoBehaviour
{
    public BaseUnit UnitRef;
    private DecalProjector selectionProjector;
    private void Awake()
    {
        selectionProjector = GetComponentInChildren<DecalProjector>();
    }

    private void Update()
    {
        selectionProjector.gameObject.SetActive(UnitRef.IsSelected);
    }
}
