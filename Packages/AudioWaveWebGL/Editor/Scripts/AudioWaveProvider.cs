using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AudioWaveProvider : MonoBehaviour
{
    private static AudioWaveProvider instance;
    public static AudioWaveProvider Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioWaveProvider>();

                if (instance == null)
                {
                    Debug.LogError("AudioWaveProvider not found in scene");
                }
            }
            return instance;
        }
    }
    
    [SerializeField]
    private int FFTSize = 256;
    public int FFTbins => FFTSize / 2;

    [SerializeField]
    private AudioSource _audioSource;

    [Header("Remap Spectrum(WebGL only)")]
    [SerializeField]
    private float _SpectrumInMin = -100f;
    [SerializeField]
    private float _SpectrumInMax = -30f;
    [SerializeField]
    private float _SpectrumOutMin = 0f;
    [SerializeField]
    private float _SpectrumOutMax = 0.01f;
    
    private float[] _outputTimeDomainWave;
    private float[] _outputSpectrum;

    [DllImport("__Internal")]
    private static extern void GetTimeDomainWave(float[] bufferPtr, int bufferSize, float duration);

    [DllImport("__Internal")]
    private static extern void GetSpectrumWave(float[] bufferPtr, int bufferSize, float duration);
    
    [DllImport("__Internal")]
    private static extern void StartAudio(float duration);

    [DllImport("__Internal")]
    private static extern void SetFFTSize(int size);
    
    void Awake()
    {
        _outputTimeDomainWave = new float[FFTbins];
        _outputSpectrum = new float[FFTbins];

#if UNITY_WEBGL && !UNITY_EDITOR
        
        SetFFTSize(FFTSize);

#endif
    }

    
    void Update()
    {
        if (_audioSource != null && _audioSource.clip != null)
        {
            UpdateWaveform();
        }
    }
    
    private void UpdateWaveform()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        
        _audioSource.GetSpectrumData(_outputSpectrum, 0, FFTWindow.BlackmanHarris);
        _audioSource.GetOutputData(_outputTimeDomainWave, 0);
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
        
        GetTimeDomainWave(_outputTimeDomainWave, FFTbins, _audioSource.clip.length);
        GetSpectrumWave(_outputSpectrum, FFTbins, _audioSource.clip.length);

#endif
    }

    public float[] GetSpectrumData()
    {
        float[] output = new float[FFTbins];
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        for (int i = 0; i < _outputSpectrum.Length; i++)
        {
            output[i] = _outputSpectrum[i];
        }
#endif
        
#if UNITY_WEBGL && !UNITY_EDITOR
        for (int i = 0; i < _outputSpectrum.Length; i++)
        {
            output[i] = Mathf.Max(Remap(_outputSpectrum[i], _SpectrumInMin, _SpectrumInMax, _SpectrumOutMin, _SpectrumOutMax), 0);
        }
#endif
            return output;
        
    }
    
    public float[] GetTimeDomainData()
    {
        float[] output = new float[FFTbins];
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        for (int i = 0; i < _outputTimeDomainWave.Length; i++)
        {
            output[i] = _outputTimeDomainWave[i];
        }
#endif
        
#if UNITY_WEBGL && !UNITY_EDITOR
        for (int i = 0; i < _outputSpectrum.Length; i++)
        {
            output[i] = _outputTimeDomainWave[i];
        }
#endif
            return output;
        
    }
    
    public void SetAudioSource(AudioSource audioSource)
    {
        _audioSource = audioSource;
#if UNITY_WEBGL && !UNITY_EDITOR
        if (_audioSource.clip != null)
        {
            StartAudio(_audioSource.clip.length);
        }
#endif
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
