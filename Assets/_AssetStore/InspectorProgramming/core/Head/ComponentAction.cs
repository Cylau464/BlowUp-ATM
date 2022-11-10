namespace InspectorProgramming
{
    public abstract class ComponentAction : UnityEngine.MonoBehaviour, IAction
    {
        public abstract void Invoke();
    }
}
