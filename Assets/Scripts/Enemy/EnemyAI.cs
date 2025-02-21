using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Util;


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
        public GameObject bombPrefab;
        public GameObject projectilePrefab;
        private Rigidbody _gunRigidbody;
        private SphereCollider _gunSphereCollider;
        
        private Rigidbody _rb;
        private Split _split;
        private Aim _aimHelper;
        
        [SerializeField]
        private float cooldownTime = 0.4f;
        private float _cooldownTimer = 0;
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
        }
        

        // Update is called once per frame
        void Update()
        {
            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer >= cooldownTime)
            {
                _canFire = true;
                _cooldownTimer = 0f;
            }
            
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
            _aimHelper.LookAtTarget();
            var threshold = 0.1f;
            var value = Random.Range(0f, 1f);
            
            if (_aimHelper.Fire(engageDistance) && _canFire)
            {
                var directionToPlayer = (playerTransform.position - transform.position).normalized;
                
                if (value < threshold)
                {
                    // Bomb attack
                    var bomb = Instantiate(bombPrefab, transform.position + transform.forward * 0.75f, Quaternion.identity);
                    bomb.GetComponent<Rigidbody>().AddForce(directionToPlayer * 5f + Vector3.up * 0.75f, ForceMode.Impulse);
                    return;
                }
                
                var projectile = Instantiate(projectilePrefab, gun.transform.position + gun.transform.forward * 0.5f, Quaternion.identity);
                projectile.GetComponent<Projectile>().Shoot(directionToPlayer);
                _canFire = false;
            }
        }

        private void RepositionBehavior()
        {
            _agent.isStopped = false;
            var directionToPlayer = (playerTransform.position - transform.position).normalized;
            // reposition without passing the player
            var offset = -directionToPlayer * Random.Range(4.5f, 10f);
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
