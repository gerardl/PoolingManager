using UnityEngine;
using System.Collections;
using PoolingSystem.Pooling;

namespace PoolingSystem.Demo
{
    public class CreatePoolsFromCode : MonoBehaviour
    {

        public GameObject objectToPool;
        public string poolName = "TestPrefab1";
        private GameObject objectToReturn;

        public GameObject fireballObjectToPool;
        public string fireballPoolName = "Fireball";

        private void Start()
        {
            // Create a pool, this also returns the pool so we can save a reference
            PoolingManager.CreatePoolingList(objectToPool, poolName, 100);
            // but lets make sure that getting it again works
            PoolingList testPrefabPool = PoolingManager.GetPoolByName(poolName);
            // now get an object directly from our pooled list. I left methods here because I 
            // could see wanting to directly manipulate lists, like emptying them or filling them on-demand
            if (testPrefabPool != null)
            {
                GameObject pooledObjectInstance;
                if (testPrefabPool.TryGetPooledObject(out pooledObjectInstance))
                {
                    Debug.Log("got prefab from specific PoolingList: " + testPrefabPool.name);
                }
            }

            // but for most cases you would probably just want to grab an object easily from the manager by name
            objectToReturn = PoolingManager.GetPooledObjectByName(poolName);
            if (objectToReturn != null)
            {
                Debug.Log("Got Prefab directly from PoolingManager: " + testPrefabPool.name);
                // manipulate it a bit
                objectToReturn.transform.position = new Vector3(Mathf.PingPong(Time.time, 8) - 4, transform.position.y, transform.position.z);
                // now lets return an object to the pool after a delay.
                // The code to return an object lives on the object itself in
                // a class called PooledObject.  This is added to each object
                // in the pool during pool creation
                Invoke("ReturnToPoolingList", 5);
            }

            // create a new fireball pool, get an object from it
            // and then use a user created script on that object
            CreateFireballPool();
        }

        private void ReturnToPoolingList()
        {
            Debug.Log("Returning object to pool");
            objectToReturn.SetActive(false);
        }

        public void CreateFireballPool()
        {
            // create a new pool, start it at 0 to force to it to grow only when we need it
            PoolingManager.CreatePoolingList(fireballObjectToPool, fireballPoolName, 0);
            // get a fireball and use it
            var fireball = PoolingManager.GetPooledObjectByName(fireballPoolName);
            if (fireball != null) fireball.GetComponent<Fireball>().Fire();
        }
    }
}

