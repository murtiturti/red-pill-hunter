using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class Split : MonoBehaviour
    {
        public GameObject top;
        public GameObject bottom;
        public float splitForce = 10f;
        
        private Rigidbody _topRigidBody;

        private EnemyAI _enemyController;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _enemyController = GetComponent<EnemyAI>();
            _topRigidBody = GetComponent<Rigidbody>();
        }

        public void GenerateSplit()
        {
            top.transform.parent = null;
            bottom.transform.parent = null;
            
            top.SetActive(true);
            bottom.SetActive(true);

            StartCoroutine(AddSplitForce());
            //_topRigidBody.AddTorque();
        }

        private IEnumerator AddSplitForce()
        {
            yield return new WaitForSeconds(0.1f);
            _topRigidBody.AddForce(Vector3.right * splitForce, ForceMode.Impulse);
            gameObject.SetActive(false);
        }
    }
}
