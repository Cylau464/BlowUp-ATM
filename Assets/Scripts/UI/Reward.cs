using UnityEngine;
using TMPro;
using main.level;

public class Reward : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _moneyCountText;
    [SerializeField] private float _moneyCalculateTime = 1f;

    private int _bonusOneStar = 100;
    private int _bonusTwoStar = 250;
    private int _bonusThreeStar = 500;

    private int _showParamID;

    public void Show()
    {
        gameObject.SetActive(true);
        _showParamID = Animator.StringToHash("show");
        _animator.SetTrigger(_showParamID);
        int money = LevelsManager.currentLevel.CurrentMoney +
            (LevelsManager.currentLevel.LevelProgress >= .9f ? _bonusThreeStar
            : (LevelsManager.currentLevel.LevelProgress >= .7f ? _bonusTwoStar
            : _bonusOneStar));

        this.LerpCoroutine(
            time: _moneyCalculateTime,
            from: 0f,
            to: money,
            action: a => _moneyCountText.text = "+ " + Mathf.Min(Mathf.RoundToInt(a), money)
        );
    }
}