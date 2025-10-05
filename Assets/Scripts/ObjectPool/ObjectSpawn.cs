using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class ObjectSpawn : MonoBehaviour
{
    [SerializeField]
    private float _spawnRate;

    [SerializeField]
    private bool _manualTrigger;

    private float _spawnTimer;
    private ObjectPool _poolComponent;

    private void Awake()
    {
        _poolComponent = GetComponent<ObjectPool>();
    }

    private void Update()
    {
        if (_manualTrigger)
        {
            return;
        }

        if (Time.time > _spawnTimer)
        {
            SpawnObject();
        }
    }

    public GameObject SpawnObject()
    {
        _spawnTimer = Time.time + _spawnRate;
        return _poolComponent.GetItem();
    }
}
