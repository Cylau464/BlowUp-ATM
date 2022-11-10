using System;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHolder : MonoBehaviour
{
    [SerializeField] private int _maxMoneyHeight = 40;
    [SerializeField] private float _scatterForce = 2f;

    private Stack<Money> _money;
    private Vector3 _offset;

    public static Action OnFill;
    public static Action OnEmpty;

    private void Start()
    {
        _money = new Stack<Money>();
    }

    public void PickUp(Money money)
    {
        money.MoveToCollector(transform, _offset);
        _money.Push(money);

        if(_money.Count > 0 && _money.Count % _maxMoneyHeight == 0)
        {
            _offset.x += money.GetBoundsSize().x;
            _offset.y = 0f;
        }
        else
        {
            _offset.y += money.GetBoundsSize().y;
        }
        
        OnFill?.Invoke();
    }

    public Money GetMoney()
    {
        if(_money.Count > 0)
        {
            Money money = _money.Pop();

            if((_money.Count + 1) % _maxMoneyHeight == 0)
            {
                Vector3 size = money.GetBoundsSize();
                _offset.x -= size.x;
                _offset.y = size.y * (_maxMoneyHeight - 1);
            }
            else
            {
                _offset -= Vector3.up * money.GetBoundsSize().y;
            }

            if (_money.Count <= 0)
                OnEmpty?.Invoke();

            return money;
        }
        else
        {
            OnEmpty?.Invoke();
            return null;
        }
    }

    public void ScatterMoney(Vector3 direction)
    {
        if (_money.Count <= 0) return;

        foreach(Money money in _money)
            money.Scatter(_scatterForce, direction);

        _money.Clear();
        _offset = Vector3.zero;
        OnEmpty?.Invoke();
    }
}