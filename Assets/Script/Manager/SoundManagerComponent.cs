using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerComponent : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private AudioMixerSnapshot m_SnapshotMenu;
    [SerializeField] private AudioMixerSnapshot m_SnapshotGameplay;
    [SerializeField] private AudioMixerSnapshot m_SnapshotGameOver;
    [SerializeField] private float m_TransitionTime = 1f;

    private const float DEFULT_VOLUME = 0f;
    private const float MUTE_VOLUME = -80f;
    private const string MIXER_BGM = "BGMVolum";
    private const string MIXER_SFX = "SFXVolum";
    /// <summary>
    /// 
    /// </summary>
    public void MixMainMenu()
    {
        m_SnapshotMenu.TransitionTo(m_TransitionTime);
    }
    /// <summary>
    /// 
    /// </summary>
    public void MixGameplay()
    {
        m_SnapshotGameplay.TransitionTo(m_TransitionTime);
    }
    /// <summary>
    /// 
    /// </summary>
    public void MixGameOver()
    {
        m_SnapshotGameOver.TransitionTo(0.01f);
    }
    /// <summary>
    /// 
    /// </summary>
    public void ToggleBGMVolume(bool isOn)
    {
        m_AudioMixer.SetFloat(MIXER_BGM, isOn ? DEFULT_VOLUME : MUTE_VOLUME);
    }
    /// <summary>
    /// 
    /// </summary>
    public void ToggleSFXVolume(bool isOn)
    {
        m_AudioMixer.SetFloat(MIXER_SFX, isOn ? DEFULT_VOLUME : MUTE_VOLUME);
    }
}
