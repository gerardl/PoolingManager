using UnityEngine;
using System.Collections.Generic;
using PoolingSystem.Pooling;

namespace PoolingSystem.Demo
{
    public class CreatePoolsFromCode : MonoBehaviour
    {

        public GameObject objectToPool;
        public string poolName = "TestPrefab1";
        private List<GameObject> objectsToReturn;


        public GameObject fireballObjectToPool;
        public string fireballPoolName = "Fireball";

        private void Start()
        {
            // Create a pool, this also returns the pool so we can save a reference
            PoolingManager.CreatePoolingList(objectToPool, poolName, 1000);
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
                    pooledObjectInstance.transform.position = new Vector3(Mathf.PingPong(Time.time, 8) - 4, transform.position.y, transform.position.z);
                }
            }

            // but for most cases you would probably just want to grab an object easily from the manager by name
            objectsToReturn = new List<GameObject>();
            Debug.Log("Got Prefabs directly from PoolingManager: " + testPrefabPool.name);
            for (int i = 0; i < 100; i++)
            {
                var newObject = PoolingManager.GetPooledObjectByName(poolName);
                if (newObject != null)
                {
                    objectsToReturn.Add(newObject);
                    // manipulate it a bit
                    newObject.transform.position = new Vector3(Mathf.PingPong(Time.time, 8) - 4, transform.position.y, transform.position.z);
                    
                }
            }
            // now lets the objects to the pool after a delay.
            // The code to return an object lives on the object itself in
            // a class called PooledObject.  This is added to each object
            // in the pool during pool creation
            Invoke("ReturnToPoolingList", 5);

            // create a new fireball pool, get an object from it
            // and then use a user created script on that object
            CreateFireballPool();
        }

        private void ReturnToPoolingList()
        {
            Debug.Log("Returning objects to pool");
            objectsToReturn.ForEach(f => f.SetActive(false));
        }

        public void CreateFireballPool()
        {
            // create a new pool
            PoolingManager.CreatePoolingList(fireballObjectToPool, fireballPoolName, 10);
            // get a fireball and use it
            var fireball = PoolingManager.GetPooledObjectByName(fireballPoolName);
            if (fireball != null) fireball.GetComponent<Fireball>().Fire();
        }
    }
}

