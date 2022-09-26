using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioWaveLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    
    private void Update()
    {
        Vector3[] timeDomainPositions = new Vector3[AudioWaveProvider.Instance.FFTbins];
        Vector3[] spectrumPositions = new Vector3[AudioWaveProvider.Instance.FFTbins];
        float[] outputTimeDomainWave = AudioWaveProvider.Instance.GetTimeDomainData();
        float[] outputSpectrum = AudioWaveProvider.Instance.GetSpectrumData();

#if UNITY_EDITOR
        
        for (int i = 0; i < spectrumPositions.Length; i++)
        {
            spectrumPositions[i] = new Vector3(-(outputSpectrum.Length / 2f) * 0.02f + i * 0.02f, outputSpectrum[i] * 10, 0);
        }
        
        /*
        for (int i = 0; i < timeDomainPositions.Length; i++)
        {
            timeDomainPositions[i] = new Vector3(-(outputTimeDomainWave.Length / 2f) * 0.02f + i * 0.02f, outputTimeDomainWave[i], 0);
        }
        */
        
#endif
#if UNITY_WEBGL && !UNITY_EDITOR

        for (int i = 0; i < spectrumPositions.Length; i++)
        {
            spectrumPositions[i] = new Vector3(-(outputSpectrum.Length / 2f) * 0.02f + i * 0.02f, Mathf.Max(AudioWaveProvider.Instance.Remap(outputSpectrum[i], 0, 0.01f, 0, 1), 0), 0);
        }
        
        /*
        for (int i = 0; i < timeDomainPositions.Length; i++)
        {
            timeDomainPositions[i] = new Vector3(-(outputTimeDomainWave.Length / 2f) * 0.02f + i * 0.02f, outputTimeDomainWave[i], 0);
        }
        */

#endif

        /*
        _lineRenderer.positionCount = outputTimeDomainWave.Length;
        _lineRenderer.SetPositions(timeDomainPositions);
        */
        
        _lineRenderer.positionCount = outputSpectrum.Length;
        _lineRenderer.SetPositions(spectrumPositions);
    }
}
