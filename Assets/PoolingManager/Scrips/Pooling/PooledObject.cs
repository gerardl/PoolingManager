using UnityEngine;

namespace PoolingSystem.Pooling
{
    public class PooledObject : MonoBehaviour
    {
        public PoolingList SourcePoolingList { get; set; }

        private void OnDisable()
        {
            if (SourcePoolingList == null)
            {
                Debug.LogWarning("Could not find destination pool for " + name);
                return;
            }

            transform.SetParent(SourcePoolingList.transform);
            transform.localPosition = Vector3.zero;
            SourcePoolingList.ReturnAvailablePooledObject(gameObject);
        }
    }
}
