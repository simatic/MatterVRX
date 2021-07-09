using UnityEngine;
using Valve.VR;

public class HorizontalMovement : MonoBehaviour // left hand for horizontal movement
{
    [SerializeField] private SteamVR_Input_Sources handType;
    [SerializeField] private SteamVR_Action_Vector2 trackPad;
    [SerializeField] private Camera playerCamera;

    // parameter to export in config file
    [SerializeField] private float horizontalSpeed = 1;


    void Update()
    {
        Vector2 v = GetDirection();

        InputManager.xAxis = (playerCamera.transform.forward.x * v.y + playerCamera.transform.right.x * v.x) * horizontalSpeed;
        InputManager.zAxis = (playerCamera.transform.forward.z * v.y + playerCamera.transform.right.z * v.x) * horizontalSpeed;
    }

    public Vector2 GetDirection()
    {
        return trackPad.GetAxis(handType);
    }

}