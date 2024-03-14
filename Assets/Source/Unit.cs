using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public event Action<Unit, Resource> HarvestCompleted;

    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform bag;
    [SerializeField] private DepartmentFabric departmentFabric;

    private Department department;

    public void SetHome(Department department)
    {
        this.department = department;
    }
    
    public void Work(Resource resource)
    {
        StartCoroutine(Harvest(resource));
    }

    public void Work(Flag flag)
    {
        StartCoroutine(Build(flag));
    }
    
    private void Build(Vector3 position)
    {
        Department newDepartment = departmentFabric.Create(position);
        newDepartment.Init(this);
    }

    private IEnumerator Build(Flag flag)
    {
        yield return MoveTo(flag.transform);
        Build(flag.transform.position);
        flag.Hide();
    }

    private IEnumerator Harvest(Resource resource)
    {
        Transform target = resource.transform;
        yield return MoveTo(target);

        PickUp(target);

        yield return MoveTo(department.transform);

        Drop(resource);
        HarvestCompleted?.Invoke(this, resource);
    }
    
    private IEnumerator MoveTo(Transform target)
    {
        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void PickUp(Transform target)
    {
        target.parent = transform;
        target.position = bag.position;
    }

    private void Drop(Resource resource)
    {
        Destroy(resource.gameObject);
    }
}
