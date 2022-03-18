using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ChiliGames.VRClassroom {
    //Class to handle loading slides in Oculus Quest data folder, more information in the Readme.pdf
    public class LoadSlides : MonoBehaviour {
        GameObject[] gameObj;
        Texture2D[] textureList;

        string[] files;
        string pathPreFix;

        int currentSlide = 1;
        bool ableToNextSlide = true;

        public Dictionary<string, Texture2D> AllTextures = new Dictionary<string, Texture2D>(); //Store all Sprites created into this dictionary container

        [SerializeField] MeshRenderer quad;

        void Awake() {
            if (PlatformManager.instance.mode == PlatformManager.Mode.Teacher && Application.platform == RuntimePlatform.Android) {
                //Change this to change pictures folder

                pathPreFix = Application.persistentDataPath;

                LoadAllSpritesFromPngFilesInFolderAndAllSubFolders();
            }
        }

        private void Start() {
            if (PlatformManager.instance.mode == PlatformManager.Mode.Teacher && Application.platform == RuntimePlatform.Android) {
                if (GetTexture("1") != null) {
                    quad.enabled = true;
                    quad.material.mainTexture = GetTexture("1");
                }
            }
        }

        public void GetNextSlide() {
            if(PlatformManager.instance.mode != PlatformManager.Mode.Teacher) return;
            if (!ableToNextSlide) return;
            ableToNextSlide = false;
            StartCoroutine(NextSlideCooldown());
            currentSlide++;
            if (GetTexture(currentSlide.ToString()) != null) {
                quad.material.mainTexture = GetTexture(currentSlide.ToString());
            } else {
                currentSlide = 1;
                quad.material.mainTexture = GetTexture(currentSlide.ToString());
            }
        }

        IEnumerator NextSlideCooldown() {
            yield return new WaitForSeconds(0.5f);
            ableToNextSlide = true;
        }

        void LoadAllSpritesFromPngFilesInFolderAndAllSubFolders() {
            //Get all files PNG in "ART" directory
            string[] allFilePaths = Directory.GetFiles(pathPreFix, "*.png");
            //SearchOptions.AllDirectories is what gets you all subfolders too. Change this to not use subfolders
            //Loop through allFilePaths 
            foreach (string filePath in allFilePaths) {
                //Ready the PNG file from the harddrive
                byte[] newPngFileData;
                newPngFileData = File.ReadAllBytes(filePath); //Read the PNG file's bytes. This loads the PNG file into memory.
                                                              //Create a Unity TEXTURE from PNG file
                Texture2D newTexture2D = new Texture2D(2048, 2048); //Create a new Texture. Size doesn't matter!
                newTexture2D.LoadImage(newPngFileData); //Load the PNG file into a Texture2D.       PngData ---> Texture2D
                string textureName = Path.GetFileNameWithoutExtension(filePath); //Get the filename of the png image             //example: "C:/GameFolder/Art/heroCharacter.png" ---> "heroCharacter"
                newTexture2D.filterMode = FilterMode.Trilinear;
                newTexture2D.anisoLevel = 3;
                AllTextures.Add(textureName, newTexture2D); //Add the Sprite to the AllSprites Dictionary.  Key is the filenameWithoutExtension. Value is the Sprite.

            }
            Debug.Log("Finished Loading Sprites! Total Sprites Loaded: " + AllTextures.Count);
        }

        public Texture2D GetTexture(string imageName) {
            //Debug.Log("Getting Sprite:'" + imageName + "'");
            if (!AllTextures.ContainsKey(imageName)) {
                //Trim excess Space
                imageName = imageName.Trim();
                //Try Again
                if (!AllTextures.ContainsKey(imageName)) {
                    Debug.LogError("SPRITE NOT FOUND in AllSprites:[" + imageName + "]");
                    return null;
                }
            }
            return AllTextures[imageName];
        }
    }
}