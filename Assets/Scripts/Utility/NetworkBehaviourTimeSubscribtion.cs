using FishNet.Managing.Timing;
using FishNet.Object;
using Unity.Collections;
using UnityEngine;

namespace AlexMod.Utility {

    public abstract class NetworkBehaviourTimeSubscribtion : NetworkBehaviour {
        

        private bool _subscribed = false;

        /// <summary> Subscribe or unsubscribe to the TimeManager for Tick events. </summary>
        private void SubscribeToTimeManager(bool subscribe) {

            //TimeManager could be null if exiting the application or not yet initialized.
            if (TimeManager == null) return;

            // If already subscribed/unsubscribed there is no need to do it again.
            if (subscribe == _subscribed) return;
            _subscribed = subscribe;

            HandleSubscription(TimeManager, _subscribed);
        }

        protected abstract void HandleSubscription(TimeManager manager, bool subscribe);

        // we dont want the events to go to null objects
        private void OnDestroy() => SubscribeToTimeManager(false);

        /* The TimeManager won't be set until at least
         * OnStartClient or OnStartServer, so do not
         * try to subscribe before these events. */
        public override void OnStartClient() {
            base.OnStartClient();
            SubscribeToTimeManager(true);
        }

        public override void OnStartServer() {
            base.OnStartServer();
            SubscribeToTimeManager(true);
        }
    }
}