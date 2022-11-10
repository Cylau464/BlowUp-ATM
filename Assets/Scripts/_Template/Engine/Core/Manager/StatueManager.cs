using UnityEngine;

namespace core
{
    public abstract class StatueManager : MonoBehaviour
    {
        protected IGameStatue m_GameStatue { get; private set; }

        protected void SwitchToStatue(IGameStatue statue)
        {
            if (statue == null) throw new System.ArgumentNullException();

            m_GameStatue?.End();

            m_GameStatue = statue;

            m_GameStatue.Start();
        }
    }
}
