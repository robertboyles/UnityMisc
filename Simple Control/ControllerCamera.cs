using UnityEngine;

/// <summary>
/// Camera controller for a camera with the follow camera component.
/// </summary>
public class ControllerCamera : MonoBehaviour
{
    /// <summary>
    /// Horizontal Rotation rate in rad/s
    /// </summary>
    [Range(0.01f, 20.0f)]
    [SerializeField] private float rotationRateHorizontal = 3.5f;

    /// <summary>
    /// Vertical Translation rate
    /// </summary>
    [Range(0.01f, 50.0f)]
    [SerializeField] private float translationRateVertical = 10.0f;

    /// <summary>
    /// Camera used to base inputs from.
    /// </summary>
    [SerializeField] private Camera activeCamera;

    /// <summary>
    /// Follow camera component from camera
    /// </summary>
    private FollowCamMovement cameraMotion;

    /// <summary>
    /// Initialise require components.
    /// </summary>
    void Start()
    {
        /// Attempt to extract and set the FollowCam component.
        if (GetComponent<FollowCamMovement>() is FollowCamMovement movementComponent)
        {
            cameraMotion = movementComponent;
        }
        else
        {
            /// throw exception if this failed.
            Debug.LogError("Failed to detect a FollowCamMovement component on camera game object.");
            throw new System.ArgumentNullException("No FollowCamMovement found on camera.");
        }
    }

    /// <summary>
    /// Update camera with control inputs.
    /// </summary>
    void Update()
    {
        float x = Input.GetAxis("Horizontal2");
        float y = Input.GetAxis("Vertical2");

        cameraMotion.rotate(
            x * Time.deltaTime * rotationRateHorizontal);

        cameraMotion.stepVerticalOffset(
            y * Time.deltaTime * translationRateVertical);
    }
}
