using UnityEngine;

/// <summary>
/// Very basic gravity componentused to ground a characater controller controlled game object. 
/// </summary>
public class BasicGravity : MonoBehaviour
{
    /// <summary>
    /// Character controller found on game object
    /// </summary>
    [SerializeField] private CharacterController controller;

    /// <summary>
    /// Free fall rate
    /// </summary>
    [Range(0.0f, 100.0f)]
    [SerializeField] private float constant_free_fall_velocity = 9.81f;

    /// <summary>
    /// Initialise the character controller.
    /// </summary>
    void Start()
    {

        /// Attempt to set the character controller
        if (GetComponent<CharacterController>() is CharacterController characterController)
        {
            controller = characterController;
        }
        else
        {
            Debug.LogError("Failed to find character controller on the game object attached to.");
        }
    }

    /// <summary>
    /// Fixed update from the mono behaviour
    /// </summary>
    void FixedUpdate()
    {
        /// Set the vertical velocity to free fall rate if the controller was not grounded in during the last update
        controller.SimpleMove(
            new Vector3(
                0.0f, 
                controller.isGrounded ? 0.0f : -constant_free_fall_velocity, 
                0.0f));
    }
}
