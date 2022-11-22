using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NFT1Forge.OSY.JsonModel;
using NFT1Forge.OSY.System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssetCacheManager : Singleton<AssetCacheManager>
{
    public bool IsBusy { get; private set; } = false;
    private readonly Dictionary<string, Texture> m_TextureDict = new Dictionary<string, Texture>();

    //TODO
    //Not a good solution , we need better preload
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator PreLoadTextureFromUrl(string url)
    {
        if (m_TextureDict.ContainsKey(url))
            yield break;

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            m_TextureDict.Add(url, texture);
        }
        request.Dispose();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator LoadTextureFromURL(string url, Action<Texture> callback)
    {
        if (m_TextureDict.ContainsKey(url))
        {
            callback?.Invoke(m_TextureDict[url]);
        }
        else
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                yield return new WaitUntil(() => request.downloadHandler.isDone);
                if (m_TextureDict.ContainsKey(url))
                {
                    callback?.Invoke(m_TextureDict[url]);
                }
                else
                {
                    Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    m_TextureDict.Add(url, texture);
                    callback?.Invoke(texture);
                }
            }
            request.Dispose();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public Texture GetTextureImmediately(string url)
    {
        if (m_TextureDict.ContainsKey(url))
            return m_TextureDict[url];
        else
            return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rawImage"></param>
    /// <param name="url"></param>
    public void SetTextureToRawImage(RawImage rawImage, string url)
    {
        if (m_TextureDict.ContainsKey(url))
        {
            rawImage.texture = m_TextureDict[url];
            rawImage.enabled = true;
        }
        else
        {
            Texture myTexture = Resources.Load <Texture> ("UI/loading-item");
            rawImage.texture = myTexture;
            StartCoroutine(
                LoadTextureFromURL(url, (texture) =>
                {
                    rawImage.enabled = true;
                    rawImage.texture = texture;
                }
            ));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void SetUISprite(string key, string type, Image uiImage)
    {
        int foundIndex = DatabaseSystem.I.GetMetadata<ImagePath>().image_path.FindIndex(
                a => a.key.Equals(key)
            );
        if (-1 < foundIndex)
        {
            string imageUrl = $"{SystemConfig.BaseAssetPath}{type}/{DatabaseSystem.I.GetMetadata<ImagePath>().image_path[foundIndex].path}";
            StartCoroutine(LoadTextureFromURL(imageUrl, (texture) =>
            {
                uiImage.sprite = CreateSprite(texture);
                uiImage.preserveAspect = true;
                uiImage.enabled = true;
            }));

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void SetSprite(Image uiImage, string url)
    {
        if (string.IsNullOrEmpty(url)) return;
        if (m_TextureDict.ContainsKey(url))
        {
            uiImage.sprite = CreateSprite(m_TextureDict[url]);
            uiImage.preserveAspect = true;
            uiImage.enabled = true;
        }
        else
        {
            Texture myTexture = Resources.Load <Texture> ("UI/loading-item");
            uiImage.sprite = CreateSprite(myTexture);
            uiImage.preserveAspect = true;
            
            StartCoroutine(
                LoadTextureFromURL(url, (texture) =>
                {
                    uiImage.sprite = CreateSprite(texture);
                    uiImage.preserveAspect = true;
                    uiImage.enabled = true;
                }
            ));
        }
    }
    /// <summary>
    /// Create sprite from texture
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    private Sprite CreateSprite(Texture texture)
    {
        if (null == texture) return null;
        Texture2D tex2D = (Texture2D)texture;
        return Sprite.Create(tex2D, new Rect(0.0f, 0.0f, texture.width, texture.height)
            , new Vector2(0.5f, 0.5f), 100.0f);
    }

}
