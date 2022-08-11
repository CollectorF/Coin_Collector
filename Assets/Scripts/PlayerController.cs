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

    private void Awake()
    {
        clickInputAction.Enable();
        clickInputAction.performed += OnClick;

        agent = GetComponent<NavMeshAgent>();
        tapPoints = new Queue<Vector3>();
        agent.enabled = true;
        playerCamera = Camera.main;
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
            }
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
        }
    }
}