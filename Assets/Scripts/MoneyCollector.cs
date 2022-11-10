using main.level;
using UnityEngine;

public class MoneyCollector : MonoBehaviour
{
    [SerializeField] private AnimationCurve _moveVerticalTrajectory;
    [SerializeField] private AnimationCurve _moveHorizontalTrajectory;
    [SerializeField] private Vector3 _minMoneyPoint;
    [SerializeField] private Vector3 _maxMoneyPoint;
    private Vector3 _curMoneyPoint;
    private bool _limitExceeded;

    private void Start()
    {
        _curMoneyPoint = _minMoneyPoint;
    }

    public void Collect(Money money)
    {
        money.MoveToCollector(transform, _curMoneyPoint, Collected, _moveVerticalTrajectory, _moveHorizontalTrajectory, _limitExceeded);

        Vector3 boundsSize = money.GetBoundsSize() / money.transform.lossyScale.x;
        Vector3 localPos = _curMoneyPoint + Vector3.right * boundsSize.x;

        if(localPos.x > _maxMoneyPoint.x)
        {
            localPos = _curMoneyPoint + Vector3.forward * boundsSize.z;
            localPos.x = _minMoneyPoint.x;
        }

        if(localPos.z > _maxMoneyPoint.z)
        {
            localPos = new Vector3(_minMoneyPoint.x, _curMoneyPoint.y + boundsSize.y, _minMoneyPoint.z);
        }

        if (localPos.y > _maxMoneyPoint.y)
        {
            localPos = _minMoneyPoint;
            _limitExceeded = true;
        }

        _curMoneyPoint = localPos;
    }

    private void Collected(int count)
    {
        LevelsManager.currentLevel.CollectMoney(count);
    }
}
