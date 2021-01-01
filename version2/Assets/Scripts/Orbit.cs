using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Orbit : MonoBehaviour
{
    //-------------Rotation Variables--------------------
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 5.0f;
    public float ySpeed = 5.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public float zoomRate = 10.0f;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5.0f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;

    private Vector3 FirstPosition;
    private Vector3 SecondPosition;
    private Vector3 delta;
    private Vector3 lastOffset;
    private Vector3 lastOffsettemp;
    
    bool OrbitWork = false;
    public float targetTime = 1f;

    public float moveSpeed = 20.0f;
    private GameObject targetObject;
    private bool movingTowardsTarget = false;
    private float lerpSpeed = 8f;
    private Transform fromRot;
    private Transform toRot;
    private bool inPosition = false;
    private bool msgReceived = false;


    void Start()
    {
        Init();
    }
    void OnEnable() { Init(); }

    public void CamMsg(string ReceivedMessage)
    {
        Debug.Log("ReceivedMessage   " + ReceivedMessage);
        if (ReceivedMessage == "LooKAtTheObject")
        {
            OrbitWork = true;
        }
        if (ReceivedMessage == "Cancel")
        {
            OrbitWork = false;
        }
    }

    public void msgController(string MsgData)
    {
        Debug.Log("ReceivedMessagedata  " + MsgData);
        targetObject = GameObject.Find(MsgData);
        Debug.Log(targetObject);
        msgReceived = true;
        if(MsgData == "Cancel")
        {
            msgReceived = false;
        }
    }

    //private bool IsPointerOverUIObject()
    //{
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //    return results.Count > 0;
    //}

    public void Init()
    {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }

        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
    }

    /*
      * Camera logic on LateUpdate to only update after all character movement logic has been handled.
      */
    void LateUpdate()
    {
        //foreach (Touch touch in Input.touches)
        //{
        //    int pointerID = touch.fingerId;
        //    if (EventSystem.current.IsPointerOverGameObject(pointerID))
        //    {
        //        return;
        //    }

        //    if (touch.phase == TouchPhase.Ended)
        //    {
        //        // here we don't know if the touch was over an canvas UI
        //        return;
        //    }
        //}


        if (OrbitWork == false)
        {

                orbitFunction();
        }

        if (msgReceived == true)
        {
            if (movingTowardsTarget == true)
            {
                movingTowardsTarget = false;
            }
            else
            {
                fromRot = gameObject.transform;
                toRot = targetObject.transform;
                movingTowardsTarget = true;
            }
        }
        if (movingTowardsTarget)
        {
            MoveTowardsTarget(targetObject);
        }
    }

    public void orbitFunction()
    {
        // If Control and Alt and Middle button? ZOOM!
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
            float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;

            desiredDistance += deltaMagDiff * Time.deltaTime * zoomRate * 0.0025f * Mathf.Abs(desiredDistance);
        }

        // If middle mouse and left alt are selected? ORBIT
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Vector2 touchposition = Input.GetTouch(0).deltaPosition;
            xDeg += touchposition.x * xSpeed * 0.002f;
            yDeg -= touchposition.y * ySpeed * 0.002f;
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

        }

        desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
        currentRotation = transform.rotation;
        rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        transform.rotation = rotation;


        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            FirstPosition = Input.mousePosition;
            lastOffset = targetOffset;
        }

        if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            SecondPosition = Input.mousePosition;
            delta = SecondPosition - FirstPosition;
            targetOffset = lastOffset + transform.right * delta.x * 0.003f + transform.up * delta.y * 0.003f;

        }

        ////////Orbit Position

        // affect the desired Zoom distance if we roll the scrollwheel
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

        position = target.position - (rotation * Vector3.forward * currentDistance);

        position = position - targetOffset;

        transform.position = position;
    }

    public void MoveTowardsTarget(GameObject target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1)
        {
            transform.position = target.transform.position;
            inPosition = true;
            //movingTowardsTarget = false;
        }

        if (inPosition)
        {
            //Debug.Log(Mathf.Abs(fromRot.localEulerAngles.y - toRot.localEulerAngles.y));
            if (Mathf.Abs(fromRot.localEulerAngles.y - toRot.localEulerAngles.y) < 3)
            {
                transform.rotation = target.transform.rotation;
                movingTowardsTarget = false;
                inPosition = false;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(fromRot.rotation, toRot.rotation, Time.deltaTime * lerpSpeed);
            }
        }
    }
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

}
