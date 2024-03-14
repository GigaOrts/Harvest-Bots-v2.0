using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Department : MonoBehaviour
{
    [SerializeField] private float workDelay = 1f;

    [SerializeField] private UnitFabric unitFabric;
    [SerializeField] private Scaner scaner;
    [SerializeField] private Wallet wallet;
    
    private FlagHolder flagHolder;

    private Flag flag => flagHolder.Flag;

    private readonly Queue<Unit> units = new();
    private readonly int unitCost = 3;
    private readonly int departmentCost = 5;

    private Status currentStatus;

    enum Status
    {
        Harvest,
        Build
    }

    private void Awake()
    {
        flagHolder = GetComponent<FlagHolder>();
    }

    private void Start()
    {
        currentStatus = Status.Harvest;

        flag.Placed += OnFlagPlaced;

        StartCoroutine(RunWork());
        StartCoroutine(RunShop());
    }

    private void OnFlagPlaced()
    {
        currentStatus = Status.Build;
    }

    private void BuyDepartment()
    {
        if (wallet.CanPay(departmentCost))
        {
            wallet.Remove(departmentCost);
        }
    }

    private void BuyUnit()
    {
        if (wallet.CanPay(unitCost) && unitFabric.IsFull == false)
            AddUnit();
    }

    private void AddUnit()
    {
        wallet.Remove(unitCost);

        Unit unit = unitFabric.CreateUnit();
        SetUpUnit(unit);
    }

    private void SetUpUnit(Unit unit)
    {
        units.Enqueue(unit);

        unit.HarvestCompleted += OnHarvestCompleted;
        unit.SetHome(this);
    }

    private void OnHarvestCompleted(Unit unit, Resource resource)
    {
        wallet.Add(resource.Value);
        units.Enqueue(unit);
    }

    private IEnumerator RunShop()
    {
        var waitSeconds = new WaitForSeconds(workDelay);

        while (true)
        {
            yield return waitSeconds;

            switch (currentStatus)
            {
                case Status.Harvest:
                    BuyUnit();
                    break;
                case Status.Build:
                    BuyDepartment();
                    break;
            }
        }
    }

    private IEnumerator RunWork()
    {
        var waitSeconds = new WaitForSeconds(workDelay);

        while (true)
        {
            yield return waitSeconds;

            while (units.TryPeek(out Unit _) && scaner.TryDequeue(out Resource resource))
            {
                Unit unit = units.Dequeue();

                switch (currentStatus)
                {
                    case Status.Harvest:
                        unit.Work(resource);
                        break;

                    case Status.Build:
                        Build(unit);
                        break;
                }
            }
        }
    }

    private void Build(Unit unit)
    {
        unit.Work(flag);
        unit.HarvestCompleted -= OnHarvestCompleted;
        currentStatus = Status.Harvest;
    }

    public void Init(Unit unit)
    {
        SetUpUnit(unit);
    }

    public void Init(IEnumerable<Unit> newUnits)
    {
        foreach (var unit in newUnits)
        {
            SetUpUnit(unit);
        }
    }
}
