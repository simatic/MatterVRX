using UnityEngine;
using Valve.VR;

public class Zoom : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private Camera userHead;
    [SerializeField] private SteamVR_Input_Sources leftHandType;
    [SerializeField] private SteamVR_Input_Sources rightHandType;
    [SerializeField] private SteamVR_Action_Single squeeze;

    // parameter to export in config file
    [SerializeField] private float zoomFactor = 1.0f;
    [SerializeField] private float zoomCenterOffset = 1.5f;

    private Vector3 offset;

    void Start()
    {
        InputManager.zoomPivotX = 0;
        InputManager.zoomPivotY = 0;
        InputManager.zoomPivotZ = 0;
        InputManager.zoomFactor = 1;
    }

    // Update is called once per frame
    void Update()
    {
        bool zooming = GetLeftTrigger() && GetRightTrigger();

        if (zooming)
        {
            Vector3 newOffset = leftHand.transform.position - rightHand.transform.position;

            // if we start zooming, offset will be zero and if we don't have this test,
            // there will be a violent first zoom
            if (offset == Vector3.zero) offset = newOffset;

            if (offset.sqrMagnitude == 0) return;

            float scale = ((newOffset.sqrMagnitude / offset.sqrMagnitude) - 1) * zoomFactor + 1;

            Vector3 pivot = userHead.transform.position + zoomCenterOffset * userHead.transform.forward;

            InputManager.zoomPivotX = pivot.x;
            InputManager.zoomPivotY = pivot.y;
            InputManager.zoomPivotZ = pivot.z;
            InputManager.zoomFactor =  scale;

            offset = newOffset;
        }
        else offset = Vector3.zero;
    }


    public bool GetLeftTrigger()
    {
        return squeeze.GetAxis(leftHandType) > 0.7f;
    }

    public bool GetRightTrigger()
    {
        return squeeze.GetAxis(rightHandType) > 0.7f;
    }
}