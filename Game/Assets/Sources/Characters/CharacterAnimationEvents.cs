using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Visuals
{
    public class CharacterAnimationEvents : MonoBehaviour
    {
        public UnityEvent OnShootEvent;

        public void TriggerShootEvent()
        {
            if (OnShootEvent != null)
            {
                OnShootEvent.Invoke();
            }
        }
    }

}
