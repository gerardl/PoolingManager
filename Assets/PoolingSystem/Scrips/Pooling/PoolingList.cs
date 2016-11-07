using System.Collections.Generic;
using UnityEngine;

namespace PoolingSystem.Pooling
{
    public class PoolingList : MonoBehaviour
    {
        public GameObject pooledObject;
        public string poolListName;
        public int initialSize = 100;
        public int maxSize = 0;
        public bool autoCreate = false;
        public bool autoIncrease = true;
        public bool autoFill = true;

        private List<GameObject> pooledObjects;
        private List<GameObject> availableObjects;

        // Need a way to handle pre-created
        // ones setup from a UI
        private void Awake()
        {
            if (autoCreate)
            {
                CreatePool();
            }
        }

        public void SetupPool(GameObject gameObject, string name, int initialSize, int maxSize, bool fill, bool autoIncrease)
        {
            this.poolListName = name;
            this.initialSize = initialSize;
            this.maxSize = maxSize;
            this.autoIncrease = autoIncrease;
            this.autoFill = fill;
            this.pooledObject = gameObject;
            CreatePool();
        }

        private void CreatePool()
        {
            pooledObjects = new List<GameObject>();
            availableObjects = new List<GameObject>();
            if (autoFill) FillPool();
            PoolingManager.AddPoolingList(poolListName, this);
        }

        public void FillPool()
        {
            if (pooledObject == null) return;

            for (int i = 0; i < initialSize; i++)
            {
                GameObject newObject = CreatePooledObject();
                newObject.SetActive(false);
                pooledObjects.Add(newObject);
                availableObjects.Add(newObject);
            }
        }

        public bool TryGetPooledObject(out GameObject pooledObject)
        {
            pooledObject = GetPooledObject();
            return pooledObject != null;
        }

        public GameObject GetPooledObject()
        {
            foreach (var availableObject in availableObjects)
            {
                if (availableObject == null)
                {
                    Debug.LogWarning("Something externally destroyed an object from the pool: " + poolListName);
                    continue;
                }

                availableObject.SetActive(true);
                availableObjects.Remove(availableObject);
                return availableObject;
            }

            if (autoIncrease && (maxSize == 0 || pooledObjects.Count < maxSize))
            {
                var createdObj = CreatePooledObject();
                pooledObjects.Add(createdObj);
                return createdObj;
            }

            // couldn't find one and auto increase is off
            // or max size was not 0 and it has been reached
            return null;
        }

        private GameObject CreatePooledObject()
        {
            GameObject newPooledObject = Instantiate(pooledObject);
            newPooledObject.transform.SetParent(transform);

            PooledObject pooledObjectScript = newPooledObject.GetComponent<PooledObject>();
            if (pooledObjectScript == null) pooledObjectScript = newPooledObject.AddComponent<PooledObject>();
            pooledObjectScript.SourcePoolingList = this;

            return newPooledObject;
        }

        public void ReturnAvailablePooledObject(GameObject availableObject)
        {
            if (!availableObjects.Contains(availableObject))
                availableObjects.Add(availableObject);
        }

        public void EmptyPool()
        {
            pooledObjects.ForEach(f => Destroy(f));
            pooledObjects.Clear();
            availableObjects.Clear();
        }
    }
}
