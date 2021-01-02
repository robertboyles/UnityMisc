using System;
using UnityEngine;

/// <summary>
/// Camera retractor component for a camera that contains a FollowCamMovement 
/// component.
/// Acts as a virtual retractable camera boom that retracts when the line of soght (LOS) is broken 
/// between the camera and the target transform.
/// </summary>
public class CameraRetractor : MonoBehaviour
{
    /// <summary>
    /// The target transform.
    /// </summary>
    [SerializeField] private Transform target;

    /// <summary>
    /// Camera to retract.
    /// </summary>
    [SerializeField] private new Camera camera;

    /// <summary>
    /// Layer which contains all colliders to retract on.
    /// </summary>
    [SerializeField] private LayerMask retractLayer;

    /// <summary>
    /// Camera motion component extracted from the camera component on setting.
    /// </summary>
    private FollowCamMovement cameraMotion;

    /// <summary>
    /// Distance from the target to the current camera location.
    /// </summary>
    private float distanceToTarget;

    /// <summary>
    /// Ray object used for raycasting.
    /// </summary>
    private Ray ray;

    /// <summary>
    /// Raycast hit result object.
    /// </summary>
    private RaycastHit hit;

    /// <summary>
    /// Unity engine start method from mono behaviour.
    /// </summary>
    private void Start()
    {
        setCamera(camera);
    }

    /// <summary>
    /// Unity engine Late update method from mono behaviour.
    /// </summary>
    private void LateUpdate()
    {    
        Vector3 rtarget2camera = cameraMotion.transform.position - target.position;
    
        /// Set raycast parameters
        ray.origin = target.position;
        ray.direction = rtarget2camera.normalized;

        if (Physics.Raycast(ray, out hit, cameraMotion.getNaturalBoomMagnitude(), retractLayer.value))
        {
            distanceToTarget = (hit.point - target.position).magnitude;
        }
        else
        {
            /// Return default distance slightly larger that the minimum zoom length.
            distanceToTarget = cameraMotion.getNaturalBoomMagnitude() * 1.1f;
        }

        cameraMotion.setZoomAsDistanceFromTarget(distanceToTarget);
    }

    /// <summary>
    /// Set the target transform.
    /// </summary>
    /// <param name="target"></param>
    public void setTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// Set the camera, camera requires a FollowCamMovement component attached.
    /// Logs an error if failed to find component.
    /// </summary>
    /// <param name="camera">Camera game object with FollowCamMovement component.</param>
    public void setCamera(Camera camera)
    {
        /// Set the camera
        this.camera = camera;

        /// Attempt to extract and set the FollowCam component.
        if (camera.GetComponent<FollowCamMovement>() is FollowCamMovement movementComponent)
        {
            cameraMotion = movementComponent;
        }
        else
        {
            /// throw exception is this failed.
            Debug.LogError("Failed to detect a FollowCamMovement component on camera game object.");
            throw new ArgumentNullException("No FollowCamMovement found on camera.");
        }
        
    }
}
