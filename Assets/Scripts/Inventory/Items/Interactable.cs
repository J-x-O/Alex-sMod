using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.CodeAnalysis.Annotations;
using FishNet.Object;
using UnityEngine;

namespace AlexMod.Inventory.Items {
    public abstract class Interactable : NetworkBehaviour {

        public static IReadOnlyList<Interactable> Interactables => _interactables;
        private static readonly List<Interactable> _interactables = new List<Interactable>();

        [OverrideMustCallBase]
        protected virtual void Awake() => _interactables.Add(this);

        [OverrideMustCallBase]
        protected void OnDestroy() => _interactables.Remove(this);

        public abstract void Interact(GameObject user);
    }
}