using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PoolingSystem.Pooling
{
    public class PoolingManager : MonoBehaviour
    {
        public const string POOLING_GAMEOBJECT_CONTAINER_NAME = "PoolingManager";
        public const string POOLING_GAMEOBJECT_PREFIX = "PoolingList_";
        private static Dictionary<string, PoolingList> poolingLists = new Dictionary<string, PoolingList>();
        private static GameObject poolingContainer;

        public static GameObject PoolingSceneContainer
        {
            get
            {
                if (poolingContainer == null)
                {
                    poolingContainer = GameObject.Find(POOLING_GAMEOBJECT_CONTAINER_NAME);
                    if (poolingContainer == null) poolingContainer = new GameObject(POOLING_GAMEOBJECT_CONTAINER_NAME);
                }
                return poolingContainer;
            }
        }

        public static void AddPoolingList(string listName, PoolingList list)
        {
            poolingLists.Add(listName, list);
            list.gameObject.transform.SetParent(PoolingSceneContainer.transform);
            list.gameObject.name = POOLING_GAMEOBJECT_PREFIX + listName;
        }

        /// <summary>
        /// Allow direct pool access for pool manipulation
        /// </summary>
        /// <param name="poolingListName"></param>
        /// <returns></returns>
        public static PoolingList GetPoolByName(string poolingListName)
        {
            if (!poolingLists.ContainsKey(poolingListName))
            {
                Debug.LogWarning("You are asking for a pool that does not exist. PoolListName: " + poolingListName);
                return null;
            }

            return poolingLists[poolingListName];
        }

        /// <summary>
        /// Get pooled GameObject by the name of a PoolingList.
        /// Returns null if no List is found with this name, or if the
        /// list is empty or not initilaized
        /// </summary>
        /// <param name="poolingListName"></param>
        /// <returns></returns>
        public static GameObject GetPooledObjectByName(string poolingListName)
        {
            GameObject pooledGameObject = null;

            var poolingList = GetPoolByName(poolingListName);
            if (poolingList == null)
            {
                return pooledGameObject;
            }

            poolingList.TryGetPooledObject(out pooledGameObject);
            return pooledGameObject;
        }

        /// <summary>
        /// Create a new pool from code
        /// </summary>
        /// <param name="poolingObject"></param>
        /// <param name="groupName"></param>
        /// <param name="startingSize"></param>
        /// <param name="autoResize"></param>
        /// <param name="maxSize"></param>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        public static PoolingList CreatePoolingList(GameObject poolingObject, string groupName, int startingSize, bool autoResize = true, int maxSize = 0, bool fill = true)
        {
            if (!poolingLists.ContainsKey(groupName))
            {
                GameObject poolingListObject = new GameObject(POOLING_GAMEOBJECT_PREFIX + groupName);
                PoolingList poolingList = poolingListObject.AddComponent<PoolingList>();
                poolingList.SetupPool(poolingObject, groupName, startingSize, maxSize, fill, autoResize);
                return poolingList;
            }
            else
            {
                Debug.LogWarning("You are trying to create a pool that already exists, we returned the existing list. PoolingList name: " + groupName);
                return poolingLists[groupName];
            }
        }
    }
}
