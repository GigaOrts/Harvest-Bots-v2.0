using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask mapLayer;
    [SerializeField] private LayerMask departmentLayer;

    private const int LeftMouse = 0;
    private Camera mainCamera;
    private FlagHolder currentFlagHolder;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouse))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            ProcessDepartmentCollision(raycastHit2D);
            ProcessMapCollision(raycastHit2D);
        }
    }

    private void ProcessMapCollision(RaycastHit2D raycastHit2D)
    {
        int objectLayer = 1 << raycastHit2D.collider.gameObject.layer;

        if (objectLayer == mapLayer.value && currentFlagHolder != null)
        {
            currentFlagHolder.Place(raycastHit2D.point);
            currentFlagHolder = null;
        }
    }

    private void ProcessDepartmentCollision(RaycastHit2D raycastHit2D)
    {
        int objectLayer = 1 << raycastHit2D.collider.gameObject.layer;

        if (objectLayer == departmentLayer)
        {
            FlagHolder flagHolder = raycastHit2D.collider.GetComponent<FlagHolder>();
            currentFlagHolder = flagHolder;
        }
    }
}
