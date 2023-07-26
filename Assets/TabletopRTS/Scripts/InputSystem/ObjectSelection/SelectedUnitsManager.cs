using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TabletopRTS.Flocking;

public class SelectedUnitsManager : MonoBehaviour
{
    public List<GameObject> CurrentSelection;
    [SerializeField] private GameObject FlockPrefab;
    private void Start()
    {
        CurrentSelection = new List<GameObject>();
    }

    private void InstantiateFlock(List<GameObject> currentSelection, FlockBehavior behavior)
    {
        GameObject flockGameObject = Instantiate(FlockPrefab);
        Flock flock = flockGameObject.GetComponent<Flock>();

        if (flock)
        {
            flock.Behavior = behavior;
            List<FlockAgent> flockList = CurrentSelection.Cast<FlockAgent>().ToList();
            flock.Agents = flockList;
            foreach(FlockAgent agent in flockList)
            {
                agent.SetFlock(flock);
            }
        }
    }
}
