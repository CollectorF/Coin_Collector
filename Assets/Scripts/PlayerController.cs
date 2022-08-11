using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputAction clickInputAction;

    private NavMeshAgent agent;
    private Camera playerCamera;
    private Vector3 currentTapPoint;
    private Vector3 currentWayPoint;
    private Queue<Vector3> tapPoints;
    private LineRenderer line;
    private int pointsCount = 0;

    private void Awake()
    {
        clickInputAction.Enable();
        clickInputAction.performed += OnClick;

        agent = GetComponent<NavMeshAgent>();
        tapPoints = new Queue<Vector3>();
        agent.enabled = true;
        playerCamera = Camera.main;

        line = gameObject.AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.widthMultiplier = 0.2f;
    }

    private void OnDisable()
    {
        clickInputAction.performed -= OnClick;
        clickInputAction.Disable();
    }

    private void Update()
    {
        if (agent.enabled && !agent.hasPath)
        {
            SetNextDestination();
            if (pointsCount > 0)
            {
                pointsCount--;
            }
        }
    }

    private void SetNextDestination()
    {
        if (tapPoints.TryDequeue(out Vector3 result))
        {
            Ray ray = playerCamera.ScreenPointToRay(result);
            if (Physics.Raycast(ray, out RaycastHit raycastHitInfo))
            {
                currentWayPoint = raycastHitInfo.point;
                agent.SetDestination(currentWayPoint);
                //Invoke("CheckPointOnPath", 0.2f);
            }
        }
    }

    private void CheckPointOnPath()
    {
        line.positionCount = agent.path.corners.Length;
        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            line.SetPosition(i, agent.path.corners[i]);
        }
    }


    public void OnClick(InputAction.CallbackContext callback)
    {
        if (agent.enabled)
        {
#if UNITY_ANDROID || UNITY_IOS
            currentTapPoint = Touchscreen.current.primaryTouch.position.ReadValue();
#elif UNITY_STANDALONE
            currentTapPoint = Mouse.current.position.ReadValue();
#endif
            tapPoints.Enqueue(currentTapPoint);
            pointsCount++;
            line.positionCount = pointsCount;
            for (int i = 0; i < pointsCount; i++)
            {
                line.SetPosition(i, currentTapPoint);
            }
        }
    }
}