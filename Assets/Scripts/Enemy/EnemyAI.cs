using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        public bool passive = false;
        
        public enum AIState
        {
            Idle, Reposition, Engage
        }
        
        public AIState state = AIState.Idle;
        public Transform playerTransform;

        public float engageDistance = 15f;
        public float repositionDistance = 15f;
        public NavMeshAgent agent;

        private Rigidbody _rb;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateState();

            switch (state)
            {
                case AIState.Idle:
                    IdleBehavior();
                    break;
                case AIState.Reposition:
                    RepositionBehavior();
                    break;
                case AIState.Engage:
                    EngageBehavior();
                    break;
            }
        }

        private void EngageBehavior()
        {
            agent.isStopped = true;
            Debug.Log("Engaging: Stopping and shooting.");
        }

        private void RepositionBehavior()
        {
            agent.isStopped = false;
            var offset = -playerTransform.forward * 10f; // example offset
            var destination = playerTransform.position + offset;
            agent.SetDestination(destination);
            Debug.Log("Repositioning: Moving to get a better view.");
        }

        private void IdleBehavior()
        {
            // Maybe trigger animation?
            return;
        }

        private void UpdateState()
        {
            var distance = Vector3.Distance(transform.position, playerTransform.position);
            // Is the enemy close to the player?
            if (distance <= repositionDistance)
            {
                // Check for line of sight
                if (HasLineOfSight())
                {
                    // if yes, state is engage
                    if (!passive)
                    {
                        state = AIState.Engage;
                    }
                    else
                    {
                        state = AIState.Idle;
                    }

                    return;
                }
                // else
                state = AIState.Reposition;
                return;
            }
            // else
            state = AIState.Idle;
        }
        
        bool HasLineOfSight()
        {
            // RaycastHit hit;
            var direction = (playerTransform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position + Vector3.up, direction, out var hit, engageDistance))
            {
                return hit.transform == playerTransform;
            }
            return false;
        }

        public void Die(Vector3 hitDirection, float hitForce)
        {
            agent.enabled = false;
            _rb.isKinematic = false;
            _rb.AddForce(hitDirection * hitForce, ForceMode.Impulse);
            // Drop gun
            Destroy(this);
        }
    }
}
