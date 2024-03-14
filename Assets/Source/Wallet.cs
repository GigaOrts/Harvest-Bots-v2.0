using UnityEngine;

internal class Wallet : MonoBehaviour
{
    [SerializeField] private int money = 9;

    public bool CanPay(int amount)
    {
        return money >= amount;
    }

    public void Add(int value)
    {
        money += value;
    }

    public void Remove(int amount)
    {
        money -= amount;
    }
}
