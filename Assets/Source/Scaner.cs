using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Scaner : MonoBehaviour
{
    [SerializeField] private float radius = 10f;
    [SerializeField] private float scanDelay = 1f;
    [SerializeField] private LayerMask layerMask;

    private readonly Queue<Resource> resources = new();

    private void Start()
    {
        StartCoroutine(Scan());
    }

    public bool TryDequeue(out Resource resource)
    {
        return resources.TryDequeue(out resource);
    }

    private IEnumerator Scan()
    {
        var waitSeconds = new WaitForSeconds(scanDelay);

        while (true)
        {
            yield return waitSeconds;

            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius, layerMask.value);
            foreach (var collider in collider2Ds)
            {
                collider.enabled = false;

                Resource resource = collider.GetComponent<Resource>();
                resources.Enqueue(resource);
            }
        }
    }
}
