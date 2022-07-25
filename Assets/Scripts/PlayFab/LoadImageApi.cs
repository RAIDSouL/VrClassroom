using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoadImageApi : MonoBehaviour
{
    // Start is called before the first frame update
    public RawImage rawImage;
    public List<Texture2D> texture2Ds;
    int i = 0;
    void Start()
    {
        texture2Ds.Clear();
        StartCoroutine(GetRequest("http://183.88.227.207:81/vr-api/public/api/gallery"));
       // StartCoroutine(GetRequest("http://d33b-202-29-32-87.ngrok.io/api/gallery"));
        //StartCoroutine(DownloadImage("https://5a48-202-29-32-87.ngrok.io/storage/files/evAjLpSQxgjidws5NWzCPjgzsVM81vdz24IhWeY3.jpg"));
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
                    var objects = JsonConvert.DeserializeObject<APIResponse[]>(webRequest.downloadHandler.text);
                    foreach (var item in objects)
                    {
                        texture2Ds.Add(null);
                        StartCoroutine(DownloadImage(item.url, i));
                        i++;
                    }
                    break;
            }
        }
    }

    IEnumerator DownloadImage(string MediaUrl,int i)
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
            texture2Ds[i] = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    public void OpenSlide()
    {
        rawImage.texture = texture2Ds[i];
    }
    public void LoadNextImage()
    {
        if (i + 1 < texture2Ds.Count)
        {
            i++;
            rawImage.texture = texture2Ds[i];
        }
       
    }

    public void LoadPrevImage()
    {
        if (i - 1 > 0)
        {
            i--;
            rawImage.texture = texture2Ds[i];
        }
    }

}


[SerializeField]
public class APIResponse
{
    public string url;
}

