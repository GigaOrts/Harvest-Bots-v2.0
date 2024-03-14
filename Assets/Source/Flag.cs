using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public event Action Placed;

    public bool IsPlaced { get; private set; }

    public void SetPlaced()
    {
        IsPlaced = true;
        Placed?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        IsPlaced = false;
    }
}