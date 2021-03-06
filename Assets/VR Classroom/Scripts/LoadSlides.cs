using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Newtonsoft.Json;
using UnityEngine.UI;

namespace ChiliGames.VRClassroom
{
    //Class to handle loading slides in Oculus Quest data folder, more information in the Readme.pdf
    public class LoadSlides : MonoBehaviourPunCallbacks
    {
        public List<Texture2D> textureList;
        public int imgCount;

        int imgDownloaded;
        int currentSlide = 0;
        bool ableToNextSlide = true;

        public Dictionary<string, Texture2D> AllTextures = new Dictionary<string, Texture2D>(); //Store all Sprites created into this dictionary container

        [SerializeField] MeshRenderer quad;
        [SerializeField] MeshRenderer[] screens;
        [SerializeField] GameObject ImgScreen;

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        void Awake()
        {
            ImgScreen = GameObject.Find("TeacherScreen");
           // ImgScreen.SetActive(false);
            imgCount = 0;
            imgDownloaded = 0;
            textureList.Clear();
            StartCoroutine(GetRequest("http://d33b-202-29-32-87.ngrok.io/api/gallery"));
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(PropertiesKey.Slide))
            {
                int slideindex = (int)propertiesThatChanged[PropertiesKey.Slide];
                currentSlide = slideindex;
                IndexToImage(currentSlide);
            }
        }

        public void GetNextSlide()
        {
            if (PlatformManager.instance.mode != PlatformManager.Mode.Teacher) return;
            if (!ableToNextSlide) return;
            ableToNextSlide = false;
            StartCoroutine(NextSlideCooldown());
            currentSlide++;
            if (GetTexture(currentSlide.ToString()) == null)
            {
                currentSlide = 1;
            }
            Hashtable hash = new Hashtable();

            hash.Add(PropertiesKey.Slide, currentSlide);

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        void IndexToImage(int index)
        {
            quad.material.mainTexture = GetTexture(index.ToString());
            foreach (var item in screens)
            {
                item.material.mainTexture = GetTexture(index.ToString());
            }
        }

        IEnumerator NextSlideCooldown()
        {
            yield return new WaitForSeconds(0.5f);
            ableToNextSlide = true;
        }

        void LoadAllSpritesFromPngFilesInFolderAndAllSubFolders()
        {
            int i = 0;
            foreach (var img in textureList)
            {
                i++;
                img.filterMode = FilterMode.Trilinear;
                img.anisoLevel = 3;
                AllTextures.Add(i.ToString(), img); //Add the Sprite to the AllSprites Dictionary.  Key is the filenameWithoutExtension. Value is the Sprite.

            }
            Debug.Log("Finished Loading Sprites! Total Sprites Loaded: " + AllTextures.Count);
        }

        public Texture2D GetTexture(string imageName)
        {
            //Debug.Log("Getting Sprite:'" + imageName + "'");
            if (!AllTextures.ContainsKey(imageName))
            {
                //Trim excess Space
                imageName = imageName.Trim();
                //Try Again
                if (!AllTextures.ContainsKey(imageName))
                {
                    Debug.LogError("SPRITE NOT FOUND in AllSprites:[" + imageName + "]");
                    return null;
                }
            }
            return AllTextures[imageName];
        }

        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                int i = 0;
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        //Debug.Log(webRequest.downloadHandler.text);
                        var objects = JsonConvert.DeserializeObject<APIResponse[]>(webRequest.downloadHandler.text);
                        foreach (var item in objects)
                        {
                            textureList.Add(null);
                            imgCount++;
                            StartCoroutine(DownloadImage(item.url, i));
                            i++;
                        }
                        break;
                }
            }
        }
        IEnumerator DownloadImage(string MediaUrl, int i)
        {
            Debug.Log("downloading : " + MediaUrl);
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                textureList[i] = (((DownloadHandlerTexture)request.downloadHandler).texture);
                imgDownloaded++;
                if (imgDownloaded == imgCount)
                {
                    ImgScreen.SetActive(true);
                    LoadAllSpritesFromPngFilesInFolderAndAllSubFolders();
                }
            }
        }
    }

    [SerializeField]
    public class APIResponse
    {
        public string url;
    }
}