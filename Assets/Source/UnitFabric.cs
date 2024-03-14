using UnityEngine;

internal class UnitFabric : MonoBehaviour
{
    [SerializeField] private Unit unitPrefab;
    [SerializeField] private Transform unitsContainer;
    [SerializeField] private int maxTotalUnits;

    public bool IsFull => unitsContainer.childCount >= maxTotalUnits;

    public Unit CreateUnit()
    {
        return Instantiate(unitPrefab, unitsContainer);
    }
}
