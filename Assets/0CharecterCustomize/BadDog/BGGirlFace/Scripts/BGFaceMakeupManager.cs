using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BadDog
{
    [ExecuteInEditMode]
    public class BGFaceMakeupManager : MonoBehaviour
    {
        [Header("Face Color")]
        public List<Color> colorList = new List<Color>();

        [Header("Brow")]
        public Vector2 browStartPosition;
        public List<Texture2D> browTextureList = new List<Texture2D>();

        [Header("Eyeball")]
        public Vector2 eyeballStartPosition;
        public List<Texture2D> eyeballTextureList = new List<Texture2D>();

        [Header("EyeMakeup")]
        public Vector2 eyeMakeupStartPosition;
        public List<Texture2D> eyeMakeupTextureList = new List<Texture2D>();

        [Header("FaceMakeup")]
        public Vector2 cheekMakeupStartPosition;
        public List<Texture2D> cheekMakeupTextureList = new List<Texture2D>();

        [Header("Forehead")]
        public Vector2 foreheadStartPosition;
        public List<Texture2D> foreheadTextureList = new List<Texture2D>();

        [Header("Lip")]
        public Vector2 lipStartPosition;
        public List<Texture2D> lipTextureList = new List<Texture2D>();


        private static BGFaceMakeupManager m_Instance = null;

        public static BGFaceMakeupManager Instance
        {
            get
            {
                return m_Instance;
            }
        }

        private void Awake()
        {
            m_Instance = this; 
        }

        public Color GetColor(int index)
        {
            if(index < 0 || index >= colorList.Count)
            {
                return Color.white;
            }

            return colorList[index];
        }

        public Texture2D GetBrow(int index)
        {
            if(index < 0 || index >= browTextureList.Count)
            {
                return null;
            }

            return browTextureList[index];
        }

        public Texture2D GetEyeball(int index)
        {
            if(index < 0 || index >= eyeballTextureList.Count)
            {
                return null;
            }

            return eyeballTextureList[index];
        }

        public Texture2D GetEyeMakeup(int index)
        {
            if(index < 0 || index >= eyeMakeupTextureList.Count)
            {
                return null;
            }

            return eyeMakeupTextureList[index];
        }

        public Texture2D GetCheekMakeup(int index)
        {
            if(index < 0 || index >= cheekMakeupTextureList.Count)
            {
                return null;
            }

            return cheekMakeupTextureList[index];
        }

        public Texture2D GetForeheadMakeup(int index)
        {
            if(index < 0 || index >= foreheadTextureList.Count)
            {
                return null;
            }

            return foreheadTextureList[index];
        }

        public Texture2D GetLipMakeup(int index)
        {
            if(index < 0 || index >= lipTextureList.Count)
            {
                return null;
            }

            return lipTextureList[index];
        }
    }
}
