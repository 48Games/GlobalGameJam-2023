using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    public class CharacterVisual : MonoBehaviour
    {

        [Header("Setup")]
        [SerializeField] private SkinnedMeshRenderer characterRenderer;
        [SerializeField] private MeshRenderer indicatorRenderer;
        [SerializeField] private TrailRenderer trailRenderer;

        // Used to preview colors
        //public Color color;
        //private void Update()
        //{
        //    SetCharacterColor(color);
        //}

        public Color Color { get; private set; }

        public void SetCharacterColor(Color color)
        {
            Color = color;
            characterRenderer.materials[0].color = color;
            indicatorRenderer.material.color = color;
            trailRenderer.startColor = color;
            trailRenderer.endColor = new Color(color.r, color.g, color.b, 0.0f);
        }

        public void ShowTrail(bool show)
        {
            trailRenderer.enabled = show;
        }

    }

}
