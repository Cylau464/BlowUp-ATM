using UnityEngine;

public class Van : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private LeaveArea _leaveArea;
    [SerializeField] private CollectArea _collectArea;

    private int _openLeaveDoorParamID;
    private int _closeLeaveDoorParamID;
    private int _openBackDoorsParamID;
    private int _closeBackDoorsParamID;

    private void OnEnable()
    {
        _leaveArea.OnStartLeave += OpenLeaveDoor;
        _leaveArea.OnCancelLeave += CloseLeaveDoor;
        _collectArea.OnStartCollect += OpenBackDoors;
        _collectArea.OnStopCollect += CloseBackDoors;
    }

    private void OnDisable()
    {
        _leaveArea.OnStartLeave -= OpenLeaveDoor;
        _leaveArea.OnCancelLeave -= CloseLeaveDoor;
        _collectArea.OnStartCollect += OpenBackDoors;
        _collectArea.OnStopCollect += CloseBackDoors;
    }

    private void Start()
    {
        _openLeaveDoorParamID = Animator.StringToHash("open_leave");
        _closeLeaveDoorParamID = Animator.StringToHash("close_leave");
        _openBackDoorsParamID = Animator.StringToHash("open_back");
        _closeBackDoorsParamID = Animator.StringToHash("close_back");
    }

    private void OpenLeaveDoor()
    {
        _animator.SetTrigger(_openLeaveDoorParamID);
    }

    private void CloseLeaveDoor()
    {
        _animator.SetTrigger(_closeLeaveDoorParamID);
    }

    private void OpenBackDoors()
    {
        _animator.SetTrigger(_openBackDoorsParamID);
    }

    private void CloseBackDoors()
    {
        _animator.SetTrigger(_closeBackDoorsParamID);
    }
}