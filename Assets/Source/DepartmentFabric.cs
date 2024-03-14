using UnityEngine;

public class DepartmentFabric : MonoBehaviour
{
    [SerializeField] Department departmentPrefab;

    public Department Create(Vector3 position)
    {
        return Instantiate(departmentPrefab, position, Quaternion.identity);
    }
}
