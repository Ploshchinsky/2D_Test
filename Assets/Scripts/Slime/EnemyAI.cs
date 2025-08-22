using UnityEngine;
using UnityEngine.AI;
using UtilsCollection;
public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming
    }

    [SerializeField] State startState = State.Roaming;
    [SerializeField] float roamingDistanceMax = 8.0f;
    [SerializeField] float roamingDistanceMin = 1.0f;
    [SerializeField] float roamingTimerMax = 2.0f;

    private NavMeshAgent navMeshAgent;
    private State currentState;
    private float currentRoamingTime;
    private Vector3 targetRoamingPosition;

    private void Awake()
    {
        currentState = startState;
        currentRoamingTime = 0;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void Start()
    {
    }

    private void Update()
    {
        switch (currentState)
        {
            default:
            case State.Roaming:
                currentRoamingTime += Time.deltaTime;
                if (currentRoamingTime >= roamingTimerMax)
                {
                    Roaming();
                    currentRoamingTime = 0;
                }
                break;
        }
    }

    private void Roaming()
    {
        float randomDistance = UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
        targetRoamingPosition = transform.position + (Utils.GetRandomDirection() * randomDistance);
        ChangeFacingDirection(transform.position, targetRoamingPosition);
        navMeshAgent.SetDestination(targetRoamingPosition);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
