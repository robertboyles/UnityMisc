using UnityEngine;
using static UnityEngine.Mathf;

/// <summary>
/// Player controller component that transforms the input to world coordinates from
/// an active camera view frame.
/// </summary>
public class ControllerCharacterCamRelative : MonoBehaviour
{
    /// <summary>
    /// Maximum translational speed in the x,z plane
    /// </summary>
    [Range(0.01f, 20.0f)]
    [SerializeField] private float maxTranslationSpeed = 3.0f;

    /// <summary>
    /// Character controller extracted from the parent game object
    /// </summary>
    private CharacterController controller;

    /// <summary>
    /// Camera used to base inputs from.
    /// </summary>
    [SerializeField] private Camera activeCamera;

    /// <summary>
    /// Relative angle between view and world heading
    /// </summary>
    private float angle
    {
        get
        {
            if (activeCamera)
            {
                return Atan2(
                    activeCamera.transform.forward.x, 
                    activeCamera.transform.forward.z) * 180 / PI;
            } 
            else
            {
                return 0.0f;
            }
        }
    }

    /// <summary>
    /// Initialise require components.
    /// </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Update the character controller transform and rotation.
    /// </summary>
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 velocityVector_CAM = new Vector3(x, 0.0f, z);
        float inputScale = velocityVector_CAM.magnitude < 1.0f ? velocityVector_CAM.magnitude : 1.0f;
        
        Vector3 velocityVector_WORLD = transform_CAM2World_aboutY(
            velocityVector_CAM,
            angle);

        controller.SimpleMove(
            velocityVector_WORLD.normalized * maxTranslationSpeed * inputScale);

        controller.transform.rotation = 
            velocityVector_CAM.magnitude > 0.00001 ? Quaternion.LookRotation(velocityVector_WORLD) : 
            controller.transform.rotation;
    }


    /// <summary>
    /// Transform control input velocity vector in to world coordinates from camera view orientation.
    /// </summary>
    /// <param name="cam">Vector in camera view cooridinates</param>
    /// <param name="angle">delta angle about the y axis between camera and world heading anlge</param>
    /// <returns></returns>
    private Vector3 transform_CAM2World_aboutY(Vector3 cam, float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.up) * cam;
    }

    /// <summary>
    /// Set the active camera
    /// </summary>
    /// <param name="camera"></param>
    public void setActiveCamera(Camera camera)
    {
        activeCamera = camera;
    }
}
