using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _period = .5f;
    [SerializeField] private int _maxDots = 3;

    private float _currentPeriod;

    private const string LOADING_TEXT = "Loading";

    private void Update()
    {
        _currentPeriod += Time.deltaTime / _period;
        int dots = Mathf.RoundToInt(_maxDots * (_currentPeriod % 1));
        dots = Mathf.Clamp(dots, 1, _maxDots);
        _text.text = LOADING_TEXT + new string('.', dots);
    }
}