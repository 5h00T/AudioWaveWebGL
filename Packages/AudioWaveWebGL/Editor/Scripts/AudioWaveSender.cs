using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioWaveSender : MonoBehaviour
{
    private Texture2D _audioWaveMap;
    private Texture2D _audioSpectrumMap;
    
    private int _outputWaveLength;

    private Color[] _outputColors;
    private Color[] _outputSpectrumColors;

    private Material _material;
    
    private void Start()
    {
        _outputWaveLength = AudioWaveProvider.Instance.FFTbins;
        
        _outputColors = new Color[_outputWaveLength];
        _outputSpectrumColors = new Color[_outputWaveLength];
        
        _audioWaveMap = new Texture2D(_outputWaveLength, 1, TextureFormat.RFloat, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        _audioWaveMap.Apply();
        
        _audioSpectrumMap = new Texture2D(_outputWaveLength, 1, TextureFormat.RFloat, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        _audioSpectrumMap.Apply();
        
        _material = GetComponent<MeshRenderer>().material;
        
        _material.SetTexture("_TimeDomainWave", _audioWaveMap);
        _material.SetTexture("_SpectrumWave", _audioSpectrumMap);
    }
    
    private void Update()
    {
        float[] outputTimeDomainData = AudioWaveProvider.Instance.GetTimeDomainData();
        for (int i = 0; i < _outputWaveLength; i++)
        {
            _outputColors[i].r = outputTimeDomainData[i];
        }
        _audioWaveMap.SetPixels(_outputColors);
        _audioWaveMap.Apply();
        
        float[] outputSpectrumData = AudioWaveProvider.Instance.GetSpectrumData();
        for (int i = 0; i < _outputWaveLength; i++)
        {
            _outputSpectrumColors[i].r = outputSpectrumData[i];
        }
        _audioSpectrumMap.SetPixels(_outputSpectrumColors);
        _audioSpectrumMap.Apply();
    }
}
