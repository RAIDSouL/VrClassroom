using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Platform
{
    VR,
    ANDROID
}
[CreateAssetMenu(fileName = "PlatformSetting", menuName = "ScriptableObjects/PlatformSetting", order = 1)]
public class PlatformSetting : ScriptableObject
{
    public Platform platform;
    private static PlatformSetting setting;
    public static PlatformSetting Instance  { get { return setting ?? (setting = Resources.Load<PlatformSetting>("PlatformSetting")); } }

}
