using UnityEngine;

namespace Enemy
{
    public class Aim : MonoBehaviour
    {
        private Transform _target;
        public Transform projectileExitPosition;
        
        public LayerMask layerMask;
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void LookAtTarget()
        {
            var directionToTarget = (_target.position - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(directionToTarget, transform.up);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        public bool Fire(float maxDistance)
        {
            var directionToTarget = (_target.position - projectileExitPosition.position).normalized;
            var ray = new Ray(projectileExitPosition.position, directionToTarget);

            if (Physics.Raycast(ray, out var hit, maxDistance, layerMask))
            {
                if (hit.transform == _target)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
