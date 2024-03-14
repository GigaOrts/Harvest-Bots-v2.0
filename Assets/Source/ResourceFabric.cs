using System.Collections;
using UnityEngine;

public class ResourceFabric : MonoBehaviour
{
    [SerializeField] Transform spawnPoints;
    [SerializeField] private float spawnDelay = 0.5f;
    [SerializeField] private int maxTotalCount = 10;
    [SerializeField] private Resource _resourcePrefab;

    private void Start()
    {
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        var waitSeconds = new WaitForSeconds(spawnDelay);
        
        while(true)
        {
            yield return waitSeconds;
            int totalCount = FindObjectsOfType<Resource>().Length;

            if(totalCount >= maxTotalCount)
            {
                continue;
            }

            int pointIndex = Random.Range(0, spawnPoints.childCount);
            Vector3 position = spawnPoints.GetChild(pointIndex).position;

            Instantiate(_resourcePrefab, position, Quaternion.identity, transform);
        }
    }
}
