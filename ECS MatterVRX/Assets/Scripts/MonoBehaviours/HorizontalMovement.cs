using UnityEngine;
using Valve.VR;

public class HorizontalMovement : MonoBehaviour // left hand for horizontal movement
{
    [SerializeField] private SteamVR_Input_Sources handType;
    [SerializeField] private SteamVR_Action_Vector2 trackPad;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform dummyTumor;


    void Update()
    {
        Vector2 v = GetDirection();

        InputManager.direction.x = (playerCamera.transform.forward.x * v.y + playerCamera.transform.right.x * v.x) * InputManager.horizontalSpeed;
        InputManager.direction.z = (playerCamera.transform.forward.z * v.y + playerCamera.transform.right.z * v.x) * InputManager.horizontalSpeed;

        InputManager.globalPosition.x += InputManager.direction.x * Time.deltaTime;
        InputManager.globalPosition.z += InputManager.direction.z * Time.deltaTime;

        dummyTumor.position += new Vector3(InputManager.direction.x, 0, InputManager.direction.z) * Time.deltaTime;
    }

    public Vector2 GetDirection()
    {
        return trackPad.GetAxis(handType);
    }

}