using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace RokasDan.EstotyTestSurvivors.Runtime.Components.Triggers
{
    internal sealed class ColliderTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool isFilterByLayerMask;

        [ShowIf(nameof(isFilterByLayerMask))]
        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        private bool isInvokeUnityEvents;

        [ShowIf(nameof(isInvokeUnityEvents))]
        [SerializeField]
        private UnityEvent<ColliderEnteredArgs> onTriggerEntered;

        [ShowIf(nameof(isInvokeUnityEvents))]
        [SerializeField]
        private UnityEvent<ColliderExitedArgs> onTriggerExited;

        public event Action<ColliderEnteredArgs> OnTriggerEntered;

        public event Action<ColliderExitedArgs> OnTriggerExited;

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (IsApplicable(otherCollider))
            {
                InvokeEnteredEvents(otherCollider);
            }
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            if (IsApplicable(otherCollider))
            {
                InvokeExitedEvents(otherCollider);
            }
        }

        private bool IsApplicable(Collider2D otherCollider)
        {
            if (isFilterByLayerMask && IsApplicableLayer(otherCollider.gameObject) == false)
            {
                return false;
            }

            return true;
        }

        private bool IsApplicableLayer(GameObject otherGameObject)
        {
            return IsApplicableLayer(otherGameObject.layer);
        }

        private bool IsApplicableLayer(int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        private void InvokeEnteredEvents(Collider2D otherCollider)
        {
            var args = new ColliderEnteredArgs(otherCollider);
            OnTriggerEntered?.Invoke(args);

            if (isInvokeUnityEvents)
            {
                onTriggerEntered.Invoke(args);
            }
        }

        private void InvokeExitedEvents(Collider2D otherCollider)
        {
            var args = new ColliderExitedArgs(otherCollider);
            OnTriggerExited?.Invoke(args);

            if (isInvokeUnityEvents)
            {
                onTriggerExited.Invoke(args);
            }
        }
    }
}
