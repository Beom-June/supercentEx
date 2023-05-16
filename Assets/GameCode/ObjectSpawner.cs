using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // 생성할 오브젝트 프리팹
    public Transform spawnPoint; // 오브젝트 생성 위치
    [SerializeField] Vector3 throwPower = new Vector3(100f, 0f, 0f);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (objectPrefab != null && spawnPoint != null)
        {
            GameObject spawnedObject = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);

            Rigidbody objectRigidbody = spawnedObject.GetComponent<Rigidbody>();
            if (objectRigidbody != null)
            {
                objectRigidbody.velocity = throwPower;
                GetComponent<Collider>().isTrigger = false; // Collider의 isTrigger 비활성화
            }
        }
    }
}
