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

        // Used to preview colors
        //public Color color;
        //private void Update()
        //{
        //    SetCharacterColor(color);
        //}

        public void SetCharacterColor(Color color)
        {
            characterRenderer.materials[0].color = color;
            indicatorRenderer.material.color = color;
        }
    }

}
