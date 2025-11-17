using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesSpawner : MonoBehaviour
{
    private readonly float _circleInDegree = 360f;
    private readonly float _spawningObjectScale = 3;
    private readonly int _collidersCount = 1;

    [SerializeField] private ResourceObjectPool _pool;
    [SerializeField] private List<Vector3> _freePlaces;
    [SerializeField] private LayerMask _checkingLayerMask;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private int _spawnObjectCount;

    private Collider[] _checkingColliders;
    private Vector3 _originPoint;
    private Vector3 _spawnPosition;
    private float _possibleObjectCountInRadius;

    private void Start()
    {
        _originPoint = transform.position;
        _freePlaces = new List<Vector3>();
        _checkingColliders = new Collider[_collidersCount];

        FindPlace();
        StartCoroutine(Spawn());
    }

    private void FindPlace()
    {
        Vector3 spawnPoint = _originPoint;

        float piNumber = 3.14f;
        float piMultiplier = 2;
        float circleLength = piMultiplier * piNumber * _spawnRadius;

        var angle = _circleInDegree * Mathf.Deg2Rad;

        for (float i = _spawnRadius; i >= 0; i--)
        {
            _possibleObjectCountInRadius = circleLength / _spawningObjectScale;

            for (int j = 1; j <= _possibleObjectCountInRadius; j++)
            {
                float positionX = _originPoint.x + Mathf.Sin(angle / _possibleObjectCountInRadius * j) * _spawnRadius;
                float positionY = 1.5f;
                float positionZ = _originPoint.z + Mathf.Cos(angle / _possibleObjectCountInRadius * j) * _spawnRadius;

                spawnPoint.x = positionX;
                spawnPoint.y = positionY;
                spawnPoint.z = positionZ;

                _freePlaces.Add(spawnPoint);
            }

            _spawnRadius -= _spawningObjectScale;
            circleLength = piMultiplier * piNumber * _spawnRadius;
        }
    }

    private void CreateObject()
    {
        if (_spawnObjectCount > 0 && _freePlaces.Count > 0)
        {
            float facingDirection = Random.Range(0f, _circleInDegree + 1f);

            _spawnPosition = _freePlaces[Random.Range(0, _freePlaces.Count - 1)];

            _freePlaces.Remove(_spawnPosition);

            if (CheckSpawnPosition(_spawnPosition))
            {
                Resource resource = _pool.GetResource();

                resource.gameObject.SetActive(true);
                resource.transform.SetPositionAndRotation(_spawnPosition, Quaternion.Euler(new Vector3(0f, facingDirection, 0f)));

                _spawnObjectCount--;
            }
            else
            {
                Debug.Log("Changing Spawn Position");
            }
        }
        else
        {
            StopCoroutine(Spawn());
        }
    }

    private bool CheckSpawnPosition(Vector3 spawnPosition)
    {
        int foundColliders = Physics.OverlapSphereNonAlloc(spawnPosition, _spawningObjectScale, _checkingColliders, _checkingLayerMask);

        if (foundColliders != 0)
        {
            return false;
        }

        return true;
    }

    private IEnumerator Spawn()
    {
        var wait = new WaitForSeconds(_spawnDelay);

        CreateObject();

        yield return wait;

        StartCoroutine(Spawn());
    }
}