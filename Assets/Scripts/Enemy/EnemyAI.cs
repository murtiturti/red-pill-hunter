using UnityEngine;
using UnityEngine.AI;
using Util;
using Math = System.Math;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Debug")]
        public bool passive = false;
        
        // Internal states for the enemy behaviour
        public enum AIState
        {
            Idle, Reposition, Engage, Dead
        }
        
        public AIState state = AIState.Idle;
        public Transform playerTransform;
        
        [Header("Agent Settings")]
        public float engageDistance = 15f;
        public float repositionDistance = 30f;
        private NavMeshAgent _agent;
        public GameObject gun;
        public GameObject projectilePrefab;
        private Rigidbody _gunRigidbody;
        private SphereCollider _gunSphereCollider;
        
        private Rigidbody _rb;
        private Split _split;
        private Aim _aimHelper;
        
        private Cooldown _cooldown;
        private bool _canFire = true;
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
            _gunRigidbody = gun.GetComponent<Rigidbody>();
            _gunSphereCollider = gun.GetComponent<SphereCollider>();
            _split = GetComponent<Split>();
            _aimHelper = GetComponent<Aim>();
            _aimHelper.SetTarget(playerTransform);
            _cooldown = GetComponent<Cooldown>();
            _cooldown.OnCooldownOver += () =>
            {
                _canFire = true;
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (state == AIState.Dead)
            {
                return;
            }
            
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
            _agent.isStopped = true;
            if (!_aimHelper.Fire(engageDistance))
            {
                _aimHelper.LookAtTarget();
            }
            else
            {
                if (!_canFire)
                {
                    return;
                }
                var directionToPlayer = (playerTransform.position - transform.position).normalized;
                var projectile = Instantiate(projectilePrefab, gun.transform.position, Quaternion.LookRotation(directionToPlayer));
                projectile.GetComponent<Projectile>().Shoot(directionToPlayer);
                _canFire = false;
                _cooldown.StartCooldown();
            }
            
        }

        private void RepositionBehavior()
        {
            _agent.isStopped = false;
            var directionToPlayer = (playerTransform.position - transform.position).normalized;
            // reposition without passing the player
            var offset = -directionToPlayer * Random.Range(4.5f, 10f) + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            var destination = playerTransform.position + offset;
            _agent.SetDestination(destination);
        }

        private void IdleBehavior()
        {
            // Maybe trigger animation?
            return;
        }

        private void UpdateState()
        {
            var distance = Vector3.Distance(transform.position, playerTransform.position);
            // Is the enemy close enough to attack
            if (distance <= engageDistance)
            {
                // Check for line of sight
                if (HasLineOfSight())
                {
                    // if yes, state is engage (unless in passive mode)
                    state = !passive ? AIState.Engage : AIState.Idle;

                    return;
                }

                state = AIState.Reposition;
                return;
            }

            if (distance <= repositionDistance)
            {
                state = AIState.Reposition;
                return;
            }
            // else player is too far to the agent
            state = AIState.Idle;
        }
        
        private bool HasLineOfSight()
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
            _agent.enabled = false;
            _rb.isKinematic = false;
            _rb.AddForce(hitDirection * hitForce, ForceMode.Impulse);
            gun.transform.parent = null;
            _gunRigidbody.isKinematic = false;
            _gunSphereCollider.enabled = true;
            Destroy(this);
        }

        public void Split()
        {
            state = AIState.Dead;
            _agent.enabled = false;
            gun.transform.parent = null;
            _gunRigidbody.isKinematic = false;
            _gunSphereCollider.enabled = true;
            _split.GenerateSplit();
        }
    }
}
