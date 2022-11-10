using UnityEngine;
using UnityEngine.Events;

namespace InspectorProgramming
{
    public enum ColliderMoment { Enter, Stay, Exit }
    public enum ExecuteType { Components, Events, Both }
    public enum ExecuteMoment { OnCalled, OnStarted, OnEnable, OnUpdate, AfterTime, Trigger, Collision }

    public class Executor : MonoBehaviour
    {
        #region variables
        [Header("Executions")]
        public ExecuteType type = ExecuteType.Events;
        public ExecuteMoment moment = ExecuteMoment.OnStarted;
        public UnityEvent action;
        public ComponentAction[] components;


        // After Time settings.
        [Header("Settings")]
        public float timeValue = 2;

        [Header("Settings")]
        // Collider settings.
        public string colliderTag;
        public ColliderMoment colliderMoment;


        [Header("Others")]
        public bool destroyOnFinished;
        #endregion

        #region mono
        protected virtual void OnEnable()
        {
            if (moment == ExecuteMoment.OnEnable)
                Execute();
        }

        protected virtual void Start()
        {
            if (moment == ExecuteMoment.OnStarted)
                Execute();
            else
            if (moment == ExecuteMoment.AfterTime)
                StartCoroutine(ExecuteAfterTime());
        }

        protected virtual void Update()
        {
            if (moment == ExecuteMoment.OnUpdate)
                StartCoroutine(ExecuteAfterTime());
        }
        #endregion

        #region After time
        private System.Collections.IEnumerator ExecuteAfterTime()
        {
            yield return new WaitForSeconds(timeValue);
            Execute();
        }
        #endregion

        #region collider
        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (CompareTags(collision.gameObject.tag) && colliderMoment == ColliderMoment.Enter && moment == ExecuteMoment.Collision)
                Execute();
        }

        protected virtual void OnCollisionStay(Collision collision)
        {
            if (CompareTags(collision.gameObject.tag) && colliderMoment == ColliderMoment.Stay && moment == ExecuteMoment.Collision)
                Execute();
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            if (CompareTags(collision.gameObject.tag) && colliderMoment == ColliderMoment.Exit && moment == ExecuteMoment.Collision)
                Execute();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (CompareTags(other.tag) && colliderMoment == ColliderMoment.Enter && moment == ExecuteMoment.Trigger)
                Execute();
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (CompareTags(other.tag) && colliderMoment == ColliderMoment.Stay && moment == ExecuteMoment.Trigger)
                Execute();
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (CompareTags(other.tag) && colliderMoment == ColliderMoment.Exit && moment == ExecuteMoment.Trigger)
                Execute();
        }

        private bool CompareTags(string otherTag)
        {
            return otherTag == null || otherTag.CompareTo("Untagged") == 0 || colliderTag.CompareTo(otherTag) == 0;
        }
        #endregion

        #region execution
        public virtual void Execute()
        {
            if (type == ExecuteType.Both || type == ExecuteType.Components)
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] != null && !components[i].Equals(null))
                        components[i]?.Invoke();
                }

            if (type == ExecuteType.Both || type == ExecuteType.Events)
                action?.Invoke();

            if (destroyOnFinished == true)
                Destroy(this);
        }
        #endregion
    }
}