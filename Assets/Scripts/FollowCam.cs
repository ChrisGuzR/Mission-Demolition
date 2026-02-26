using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;   // The static point of Interest, used to pass the projectile to follow

    [Header("Inscribed")] 
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Dynamic")] 
    public float camZ;              // The desired Z pos of the camera

    void Awake()
    {
        // Hold the initial Z position for the camera
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        /*// If there is no POI, then return
        if (POI == null) return;
        
        // Get the position of the POI (projectile)
        Vector3 destination = POI.transform.position;*/
        
        // After the projectile has settled, the camera
        // should move back to focus on the slingshot again.
        Vector3 destination = Vector3.zero;
        
        // Wait until the rigidbody of the projectile is sleeping, then return the camera to [0, 0, 0]
        if (POI != null)
        {
            // If the POI has a Rigidbody, check to see if it is sleeping
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if (poiRigid != null && poiRigid.IsSleeping()) POI = null;
        }

        if (POI != null)
        {
            destination = POI.transform.position;
        }
        
        //Limit the minimum values of destination.x & destination.y
        //to prevent camera from moving below the ground.
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        
        // Interpolate from the current Camera position toward destination
        // Vector3.Lerp() method uses linear interpolation to find a point between
        // the two Vector3s that are passed in, returning a weighted average of the two
        destination = Vector3.Lerp(transform.position, destination, easing);
        
        // Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;
        
        // Set the camera to the destination
        transform.position = destination;
        
        // Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y += 10;
    }
    
}
