using System;
using UnityEngine;

namespace Framework.Utils
{
    public struct ScopedPhysicsQueriesRules : IDisposable
    {
        private bool _hitBackfaces;
        private bool _hitTriggers;
		
        public void Dispose()
        {
            Physics.queriesHitBackfaces = _hitBackfaces;
            Physics.queriesHitTriggers = _hitTriggers;
        }

        internal void UpdateHitBackfaces(bool hitBackfaces)
        {
            Physics.queriesHitBackfaces = hitBackfaces;
        }

        internal void UpdateHitTriggers(bool hitTriggers)
        {
            Physics.queriesHitTriggers = hitTriggers;
        }

        internal static ScopedPhysicsQueriesRules OverrideHitBackfacesAndTriggers(bool hitBackfaces, bool hitTriggers)
        {
            var rules = new ScopedPhysicsQueriesRules
            {
                _hitBackfaces = Physics.queriesHitBackfaces,
                _hitTriggers = Physics.queriesHitTriggers,
            };
            Physics.queriesHitBackfaces = hitBackfaces;
            Physics.queriesHitTriggers = hitTriggers;
            return rules;
        }

        internal static ScopedPhysicsQueriesRules OverrideHitBackfaces(bool hitBackfaces)
        {
            var rules = new ScopedPhysicsQueriesRules
            {
                _hitBackfaces = Physics.queriesHitBackfaces,
                _hitTriggers = Physics.queriesHitTriggers,
            };
            Physics.queriesHitBackfaces = hitBackfaces;
            return rules;
        }

        internal static ScopedPhysicsQueriesRules OverrideHitTriggers(bool hitTriggers)
        {
            var rules = new ScopedPhysicsQueriesRules
            {
                _hitBackfaces = Physics.queriesHitBackfaces,
                _hitTriggers = Physics.queriesHitTriggers,
            };
            Physics.queriesHitTriggers = hitTriggers;
            return rules;
        }
    }
}