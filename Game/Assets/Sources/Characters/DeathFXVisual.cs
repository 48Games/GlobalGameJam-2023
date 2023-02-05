using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    public class DeathFXVisual : MonoBehaviour
    {

        [Header("Setup")]
        [SerializeField] private ParticleSystemRenderer[] particleSystemRenderers;

        public void SetColor(Color color)
        {
            foreach (var particleRenderer in particleSystemRenderers)
            {
                particleRenderer.material.color = color;
            }
        }
    }

}
