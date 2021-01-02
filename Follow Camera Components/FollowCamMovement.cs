using UnityEngine;
using static UnityEngine.Mathf;

/// <summary>
/// Camera motion component to be attached to a camera game object.
/// Acts as a follow cam where the camera has a target transform to track at a defined offset.
/// The camera is always pointed at the target in terms of rotation.
/// Angle relative to the target's vertical (y) axis can be set.
/// </summary>
public class FollowCamMovement : MonoBehaviour
{
    /// <summary>
    ///  Unzoomed offset between the camera and the target transform.
    /// </summary>
    [SerializeField] private Vector3 positionOffet0 = new Vector3(0.0f, 8.0f, -20.0f);

    /// <summary>
    /// The target transform
    /// </summary>
    [SerializeField] private Transform target;

    /// <summary>
    /// Vertical unzoomed limits in world scale.
    /// </summary>
    private static readonly float[] zLimits = { 0.0f, 30.0f };

    /// <summary>
    /// Manual zoom value.
    /// Constrained within the Unity editor to be within range 0:1, 
    /// where 0 is the length of the position offset and 0 means the camera and target transforms coincide.
    /// </summary>
    [Range(0.0f, 1.0f)]
    [SerializeField] private float zoom0 = 0.0f;

    /// <summary>
    /// Dynamic zoom level.
    /// </summary>
    private float _zoom;

    /// <summary>
    /// Current zoom level property.
    /// This is property returns the minimum between the manual zoom level and
    /// dynamic zoom values.
    /// </summary>
    public float zoom
    {
        get
        {
            if (_zoom < zoom0)
            {
                return zoom0;
            } else
            {
                return _zoom;
            }
        }

        set
        {
            _zoom = value;
        }
    }

    /// <summary>
    /// Camera's rotation about the target's vertical axis (Y).
    /// </summary>
    public float rotationAboutTarget = 0.0f;

    /// <summary>
    /// Unity late update from mono behaviour.
    /// </summary>
    void LateUpdate()
    {
        setCameraTranslation(
            CalculateOffset());
    }

    /// <summary>
    /// Calculate the current offset based on offset0, camera rotation about the axis and zoom level.
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateOffset()
    {
        Vector3 rCamera = target.position + getPositionOffsetTotal();
        Vector3 rPivotOnAxis = new Vector3(target.position.x, rCamera.y, target.position.z);
        Vector3 V = rCamera - rPivotOnAxis;

        Vector3 Vrot = Quaternion.AngleAxis(rotationAboutTarget * 180 / PI, Vector3.up) * V;

        return rPivotOnAxis - target.position + Vrot;
    }

    /// <summary>
    /// Set the camera's transform position to an offset from the target transform.
    /// </summary>
    /// <param name="offset">Offset between the target transform and the camera transform</param>
    private void setCameraTranslation(Vector3 offset)
    {
        transform.position = target.position + offset;
        Quaternion lookRotation = Quaternion.LookRotation(-offset);
        transform.rotation = lookRotation;
    }

    /// <summary>
    /// Set the target transform to a new transform.
    /// </summary>
    /// <param name="target">A new target transform</param>
    public void setTargetTransform(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// Returns the zoom adjusted offset vector
    /// </summary>
    /// <returns>A zoom adjusted Vector3 offset vector</returns>
    private Vector3 getPositionOffsetTotal()
    {
        return positionOffet0 + (-positionOffet0 * zoom);
    }

    /// <summary>
    /// Adds a delta value to the vertical offset vector. 
    /// </summary>
    /// <param name="delta">Delta to add on to the vertical offset</param>
    public void stepVerticalOffset(float delta)
    {
        if (positionOffet0.y + delta > zLimits[1])
        {
            positionOffet0.y = zLimits[1];
            return;
        }

        if (positionOffet0.y + delta < zLimits[0])
        {
            positionOffet0.y = zLimits[0];
            return;
        }

        positionOffet0.y += delta;
    }

    /// <summary>
    /// Adds a delta value to the rotation anlge of the camera relative to the target.
    /// </summary>
    /// <param name="dtheta">Delta to add to the rotation about the target [rad]</param>
    public void rotate(float dtheta)
    {
        rotationAboutTarget += dtheta;
    }

    /// <summary>
    /// Set the current zoom as a distance from the target transform.
    /// </summary>
    /// <param name="distance">Distance to set from the target along the vector target to camera.</param>
    public void setZoomAsDistanceFromTarget(float distance)
    {
        _zoom = (positionOffet0.magnitude - distance) / positionOffet0.magnitude;
    }

    /// <summary>
    /// Returns the natural unzoomed distance between the target and camera.
    /// </summary>
    /// <returns>Natural unzoomed distance between the target and the camera.</returns>
    public float getNaturalBoomMagnitude()
    {
        return (positionOffet0 + (-positionOffet0 * zoom0)).magnitude;
    }
}
