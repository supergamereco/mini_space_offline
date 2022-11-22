using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    private SoundManagerComponent m_Component;

    /// <summary>
    /// 
    /// </summary>
    private void Initialize()
    {
        SoundManagerComponent prefab = Resources.Load<SoundManagerComponent>("SoundManagerComponent");
        m_Component = Instantiate(prefab, transform);
        if (PlayerPrefs.HasKey("BGMVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            ToggleBGMVolume(PlayerPrefs.GetInt("BGMVolume") != 0);
            ToggleSFXVolume(PlayerPrefs.GetInt("SFXVolume") != 0);
        }
        else
        {
            PlayerPrefs.SetInt("BGMVolume", 1);
            PlayerPrefs.SetInt("SFXVolume", 1);
        }
        m_Component.gameObject.SetActive(true);
    }
    /// <summary>
    /// 
    /// </summary>
    public void MixMainMenu()
    {
        if (null == m_Component)
            Initialize();

        m_Component.MixMainMenu();
    }
    /// <summary>
    /// 
    /// </summary>
    public void MixGameplay()
    {
        if (null == m_Component)
            Initialize();

        m_Component.MixGameplay();
    }
    /// <summary>
    /// 
    /// </summary>
    public void MixGameOver()
    {
        if (null == m_Component)
            Initialize();

        m_Component.MixGameOver();
    }
    /// <summary>
    /// Mute and UnMute Background music 
    /// </summary>
    public void ToggleBGMVolume(bool isOn)
    {
        if (null == m_Component)
            Initialize();

        m_Component.ToggleBGMVolume(isOn);
    }
    /// <summary>
    /// Mute and UnMute SFX
    /// </summary>
    public void ToggleSFXVolume(bool isOn)
    {
        if (null == m_Component)
            Initialize();

        m_Component.ToggleSFXVolume(isOn);
    }
}
