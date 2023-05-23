using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Operation: This script is put on the main camera and is a 3rd person follow camera. It does not take user input to rotate and should be setup to follow over the should or farther
 behind the player.
 SETUP:
    1. Attach script to main camera
    2. Set PlayerHeightOffset - this should aim the camera up from the player characters pivot at their feet
    3. Set the Target gameobject. - If you want this camera to aim straight at the main player, the Target object would be the player, if you want it to follow at a point in front,
        add an empty gameobject at your desired focus position, and add that as the Target.
        NOTE: If Target is an empty gameobject, camera should be positioned at matching X value to empty gameobject, creating a straight line of site, otherwise the look at angle is curved from Player's
        position, causing the player to always be turning when we try to walk. 
    4. Adjust the springConst value to get the desired spring amount for the camera
    TODO future development: 
    a. manage turn arounds
    b. manage building/object collision - shortening spring, moving camera around objects vs through them, etc.
 */
public class ThirdPersonCamera : MonoBehaviour
{
    [Tooltip("Offset of the camera from the Target's pivot ex. move camera up from players feet")]
    public Vector3 PlayerHeightOffset = new Vector3(-25, 0, 0);
    public GameObject Target;
    [SerializeField]
    private float springConst = 30f;
    private float dampingConst;

    private float hDist;
    private float vDist;

    private Vector3 velocity;
    private Vector3 actualPosition;


    private void Start()
    {
        hDist = Target.transform.position.z - transform.position.z; //Horizontal offset from target
        vDist = transform.position.y - Target.transform.position.y; //Vertical offset from target

        dampingConst = 2.0f * Mathf.Sqrt(springConst);
        actualPosition = Target.transform.position - Target.transform.forward * hDist + Target.transform.up * vDist;
        velocity = Vector3.zero;
    }

    private void Update()
    {
        Vector3 idealPosition = Target.transform.position - Target.transform.forward * hDist + Target.transform.up * vDist;
        Vector3 displacement = actualPosition - idealPosition;
        Vector3 springAcceleration = (-springConst * displacement) - (dampingConst * velocity);
        velocity += springAcceleration * Time.deltaTime;
        actualPosition += velocity * Time.deltaTime;
        transform.position = actualPosition;
        transform.LookAt(Target.transform);
        transform.rotation *= Quaternion.Euler(PlayerHeightOffset); //offset the camera angle to not be aimed at the players feet
    }
}
