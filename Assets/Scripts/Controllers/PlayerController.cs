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
    [SerializeField]
    private GameObject visiblePath;
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private float comparationTolerance = 0.1f;

    private NavMeshAgent agent;
    private Camera playerCamera;
    private Vector3 currentTapPoint;
    private LineController currentLine;
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
        if (currentLine != null && CompareVectors(agent.transform.position, currentLine.points[currentLine.points.Count - 1], comparationTolerance))
        {
            Destroy(currentLine.gameObject);
        }
    }

    private void SetNextDestination()
    {
        if (tapPoints.TryDequeue(out Vector3 result))
        {
            agent.SetDestination(result);
        }
    }

    private bool CompareVectors(Vector3 a, Vector3 b, float tolerance)
    {
        float distance = Vector3.Distance(a, b);
        if (distance < tolerance)
        {
            return true;
        }
        else
        {
            return false;
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
            Ray ray = playerCamera.ScreenPointToRay(currentTapPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHitInfo))
            {
                tapPoints.Enqueue(raycastHitInfo.point);
                if (currentLine == null)
                {
                    currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, visiblePath.transform).GetComponent<LineController>();
                }
                if (!agent.hasPath)
                {
                    currentLine.AddPoint(gameObject.transform.position);
                }
                currentLine.AddPoint(raycastHitInfo.point);
            }
        }
    }
}