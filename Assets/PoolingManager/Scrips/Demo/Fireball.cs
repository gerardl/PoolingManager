using UnityEngine;

namespace PoolingSystem.Demo
{
    public class Fireball : MonoBehaviour
    {
        public void Fire()
        {
            Debug.Log("Fire!!!!");
            transform.position = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);
        }
    }
}
