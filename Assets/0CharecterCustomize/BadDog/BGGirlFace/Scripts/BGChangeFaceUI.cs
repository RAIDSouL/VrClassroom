using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BadDog
{
    public class BGChangeFaceUI : MonoBehaviour
    {
        public BGFaceMakeup target;

        public void ChangeColor()
        {
            BGFaceMakeupManager makeupManager = BGFaceMakeupManager.Instance;

            if(makeupManager == null || target == null)
            {
                return;
            }

            int index = target.defaultColorIndex + 1;

            if(index >= makeupManager.colorList.Count)
            {
                index = 0;
            }

            target.defaultColorIndex = index;
            target.RefreshAll();
        }

        public void ChangeBrow()
        {
            BGFaceMakeupManager makeupManager = BGFaceMakeupManager.Instance;

            if(makeupManager == null || target == null)
            {
                return;
            }

            int index = target.defaultBrowIndex + 1;

            if(index >= makeupManager.browTextureList.Count)
            {
                index = 0;
            }

            target.defaultBrowIndex = index;
            target.RefreshAll();
        }

        public void ChangeEyeball()
        {
            BGFaceMakeupManager makeupManager = BGFaceMakeupManager.Instance;

            if(makeupManager == null || target == null)
            {
                return;
            }

            int index = target.defaultEyeballIndex + 1;

            if(index >= makeupManager.eyeballTextureList.Count)
            {
                index = 0;
            }

            target.defaultEyeballIndex = index;
            target.RefreshAll();
        }

        public void ChangeEyeMakeup()
        {
            BGFaceMakeupManager makeupManager = BGFaceMakeupManager.Instance;

            if(makeupManager == null || target == null)
            {
                return;
            }

            int index = target.defaultEyeMakeupIndex + 1;

            if(index >= makeupManager.eyeMakeupTextureList.Count)
            {
                index = 0;
            }

            target.defaultEyeMakeupIndex = index;
            target.RefreshAll();
        }

        public void ChangeCheekMakeup()
        {
            BGFaceMakeupManager makeupManager = BGFaceMakeupManager.Instance;

            if(makeupManager == null || target == null)
            {
                return;
            }

            int index = target.defaultCheekMakeupIndex + 1;

            if(index >= makeupManager.cheekMakeupTextureList.Count)
            {
                index = 0;
            }

            target.defaultCheekMakeupIndex = index;
            target.RefreshAll();
        }

        public void ChangeForehead()
        {
            BGFaceMakeupManager makeupManager = BGFaceMakeupManager.Instance;

            if(makeupManager == null || target == null)
            {
                return;
            }

            int index = target.defaultForeheadIndex + 1;

            if(index >= makeupManager.foreheadTextureList.Count)
            {
                index = 0;
            }

            target.defaultForeheadIndex = index;
            target.RefreshAll();
        }

        public void ChangeLip()
        {
            BGFaceMakeupManager makeupManager = BGFaceMakeupManager.Instance;

            if(makeupManager == null || target == null)
            {
                return;
            }

            int index = target.defaultLipIndex + 1;

            if(index >= makeupManager.lipTextureList.Count)
            {
                index = 0;
            }

            target.defaultLipIndex = index;
            target.RefreshAll();
        }
    }
}
