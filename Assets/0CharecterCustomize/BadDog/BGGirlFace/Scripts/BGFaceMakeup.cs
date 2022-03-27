using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BadDog
{
    public class BGFaceMakeup : MonoBehaviour
    {
        public int defaultColorIndex = -1;
        public int defaultBrowIndex = -1;
        public int defaultEyeballIndex = -1;
        public int defaultEyeMakeupIndex = -1;
        public int defaultCheekMakeupIndex = -1;
        public int defaultForeheadIndex = -1;
        public int defaultLipIndex = -1;

        private Texture2D m_FaceBornMainTexture;
        private Texture2D m_FaceBornMixTexture;
        private Texture2D m_FaceUpdatedTexture;

        private Material m_BornMaterial;
        private Material m_UpdatedMaterial;

        private Color m_CurrentColor;
        private Texture2D m_CurrentBrowTexture;
        private Texture2D m_CurrentEyeballTexture;
        private Texture2D m_CurrentEyeMakeupTexture;
        private Texture2D m_CurrentCheekMakeupTexture;
        private Texture2D m_CurrentForeheadTexture;
        private Texture2D m_CurrentLipTexture;

        private static readonly int MainTex = Shader.PropertyToID("_MainTex");
        private static readonly int MixTex = Shader.PropertyToID("_MixTex");


        private void OnEnable()
        {
            Renderer renderer = GetComponent<Renderer>();

            if (renderer != null)
            {
                m_BornMaterial = renderer.sharedMaterial;
                m_UpdatedMaterial = renderer.material;

                m_FaceBornMainTexture = m_BornMaterial.GetTexture(MainTex) as Texture2D;
                m_FaceBornMixTexture = m_BornMaterial.GetTexture(MixTex) as Texture2D;

                if (m_FaceUpdatedTexture == null)
                {
                    m_FaceUpdatedTexture = new Texture2D(m_FaceBornMainTexture.width, m_FaceBornMainTexture.height, m_FaceBornMainTexture.format, true);
                }
            }
        }

        private void OnDisable()
        {
            if(m_FaceUpdatedTexture != null)
            {
                Destroy(m_FaceUpdatedTexture);
                m_FaceUpdatedTexture = null;
            }

            if(m_UpdatedMaterial != null)
            {
                Destroy(m_UpdatedMaterial);
                m_UpdatedMaterial = null;

                GetComponent<Renderer>().sharedMaterial = m_BornMaterial;
            }

            defaultColorIndex = -1;
            defaultBrowIndex = -1;
            defaultEyeballIndex = -1;
            defaultEyeMakeupIndex = -1;
            defaultCheekMakeupIndex = -1;
            defaultForeheadIndex = -1;
            defaultLipIndex = -1;
        }

        private Color32[] ResetMakeupTextureColor(Color32[] mainTexturePixels, Color32[] mixTexturePixels)
        {
            int width = m_FaceBornMainTexture.width;
            int height = m_FaceBornMainTexture.height;

            Color32[] makeupPixels = new Color32[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int xy = x + y * width;

                    makeupPixels[xy] = mainTexturePixels[xy];

                    // only skin can change color
                    if (mixTexturePixels[xy].g > 0.5f)
                    {
                        makeupPixels[xy] *= m_CurrentColor;
                    }
                }
            }

            return makeupPixels;
        }

        private void ResetMakeupTextureSubpart(Vector2 targetPos, Texture2D targetTexture, Color32[] makeupPixels)
        {
            if(targetTexture == null)
            {
                return;
            }

            int width = targetTexture.width;
            int height = targetTexture.height;

            Color32[] targetPixels = targetTexture.GetPixels32();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color targetColor = targetPixels[x + y * width];

                    if (targetColor.a > 0)
                    {
                        int xy = ((int)targetPos.x + x) + ((int)targetPos.y + y) * m_FaceUpdatedTexture.width;

                        Color currentColor = makeupPixels[xy];
                        makeupPixels[xy] = Color.Lerp(currentColor, targetColor, targetColor.a);
                    }
                }
            }
        }

        public void RegenerateMakeupTexture()
        {
            if(m_FaceUpdatedTexture == null)
            {
                return;
            }

            BGFaceMakeupManager makeupManager = BGFaceMakeupManager.Instance;

            if(makeupManager == null)
            {
                return;
            }

            Color32[] mainTexturePixels = m_FaceBornMainTexture.GetPixels32();
            Color32[] mixTexturePixels = m_FaceBornMixTexture.GetPixels32();

            Color32[] makeupPixels = ResetMakeupTextureColor(mainTexturePixels, mixTexturePixels);
            ResetMakeupTextureSubpart(makeupManager.browStartPosition, m_CurrentBrowTexture, makeupPixels);
            ResetMakeupTextureSubpart(makeupManager.eyeballStartPosition, m_CurrentEyeballTexture, makeupPixels);
            ResetMakeupTextureSubpart(makeupManager.eyeMakeupStartPosition, m_CurrentEyeMakeupTexture, makeupPixels);
            ResetMakeupTextureSubpart(makeupManager.cheekMakeupStartPosition, m_CurrentCheekMakeupTexture, makeupPixels);
            ResetMakeupTextureSubpart(makeupManager.foreheadStartPosition, m_CurrentForeheadTexture, makeupPixels);
            ResetMakeupTextureSubpart(makeupManager.lipStartPosition, m_CurrentLipTexture, makeupPixels);

            m_FaceUpdatedTexture.SetPixels32(makeupPixels);
            m_FaceUpdatedTexture.Apply();

            m_UpdatedMaterial.SetTexture(MainTex, m_FaceUpdatedTexture);
        }

        public void ChangeColor(Color color)
        {
            if(m_CurrentColor == color)
            {
                return;
            }

            RegenerateMakeupTexture();
        }

        public void ChangeBrow(Texture2D targetTexture)
        {
            if (targetTexture == null)
            {
                return;
            }

            if(m_CurrentBrowTexture == targetTexture)
            {
                return;
            }

            RegenerateMakeupTexture();
        }

        public void ChangeEyeball(Texture2D targetTexture)
        {
            if (targetTexture == null)
            {
                return;
            }

            if(m_CurrentEyeballTexture == targetTexture)
            {
                return;
            }

            RegenerateMakeupTexture();
        }

        public void ChangeEyeMakeup(Texture2D targetTexture)
        {
            if (targetTexture == null)
            {
                return;
            }

            if(m_CurrentEyeMakeupTexture == targetTexture)
            {
                return;
            }

            RegenerateMakeupTexture();
        }

        public void ChangeCheekMakeup(Texture2D targetTexture)
        {
            if (targetTexture == null)
            {
                return;
            }

            if(m_CurrentCheekMakeupTexture == targetTexture)
            {
                return;
            }

            RegenerateMakeupTexture();
        }

        public void ChangeForeheadMakeup(Texture2D targetTexture)
        {
            if (targetTexture == null)
            {
                return;
            }

            if(m_CurrentForeheadTexture == targetTexture)
            {
                return;
            }

            RegenerateMakeupTexture();
        }

        public void ChangeLip(Texture2D targetTexture)
        {
            if (targetTexture == null)
            {
                return;
            }

            if(m_CurrentLipTexture == targetTexture)
            {
                return;
            }

            RegenerateMakeupTexture();
        }

        public void RefreshAll()
        {
            BGFaceMakeupManager makeupManager = BGFaceMakeupManager.Instance;

            if (makeupManager != null)
            {
                m_CurrentColor = makeupManager.GetColor(defaultColorIndex);
                m_CurrentBrowTexture = makeupManager.GetBrow(defaultBrowIndex);
                m_CurrentEyeballTexture = makeupManager.GetEyeball(defaultEyeballIndex);
                m_CurrentEyeMakeupTexture = makeupManager.GetEyeMakeup(defaultEyeMakeupIndex);
                m_CurrentCheekMakeupTexture = makeupManager.GetCheekMakeup(defaultCheekMakeupIndex);
                m_CurrentForeheadTexture = makeupManager.GetForeheadMakeup(defaultForeheadIndex);
                m_CurrentLipTexture = makeupManager.GetLipMakeup(defaultLipIndex);

                RegenerateMakeupTexture();
            }
        }

        private void OnValidate()
        {
            RefreshAll();
        }
    }
}
