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
        StartCoroutine(GetRequest("https://5a48-202-29-32-87.ngrok.io/api/gallery"));
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
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    var objects = JsonConvert.DeserializeObject<APIResponse[]>(webRequest.downloadHandler.text);
                    //Debug.Log(objects[0].url.ToString());
                    //StartCoroutine(DownloadImage(objects[0].url));
                    foreach (var item in objects)
                    {
                        StartCoroutine(DownloadImage(item.url));
                    }
                    //objects.ToList().ForEach(x => StartCoroutine(DownloadImage(x.ToString())));
                    break;
            }
        }
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        Debug.Log("downloading : " + MediaUrl);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            //StopAllCoroutines();
        }
        else
        {
            texture2Ds.Add(((DownloadHandlerTexture)request.downloadHandler).texture);
        }
    }

    public void OpenSlide()
    {
        rawImage.texture = texture2Ds[i];
    }
    public void LoadNextImage()
    {
        i++;
        rawImage.texture = texture2Ds[i];
    }

    public void LoadPrevImage()
    {
        i--;
        rawImage.texture = texture2Ds[i];
    }

}


[SerializeField]
public class APIResponse
{
    public string url;
}

