# AudioWaveWebGL
UnityのWebGLビルドでオーディオ波形を取得するためのツール。  
![Animation](https://user-images.githubusercontent.com/25942568/192350896-50ebe114-95e0-4a09-a501-a3fab583bdae.gif)

## Requirements
- Unity 2020.3.0 or higher
- Universal RP 10.3.2 (Use sample)
- Shader Graph 10.3.2 (Use sample)

## Installation 
PackageManagerの`Add package from git url`に  
`https://github.com/5h00T/AudioWaveWebGL.git?path=Packages/AudioWaveWebGL`を入力

## Usage
GameObjectにAudioWaveProviderコンポーネントをアタッチします。  
AudioWaveProviderにAudioSourceをセットします。  
![キャプチャ](https://user-images.githubusercontent.com/25942568/192354517-958ece6d-bc75-46d9-9b26-ff1762197510.PNG)

波形は次のようにして取得できます。  
```
float[] output = AudioWaveProvider.Instance.GetTimeDomainData();
float[] output = AudioWaveProvider.Instance.GetSpectrumData();
```

AudioSourceまたはAudioClipの差し替えをした時は`SetAudioSource(audioSource)`を呼び出してください。

## License
Sampleに含まれる音声ファイルは[Yosshy氏](https://twitter.com/natadekokosuper)から許可を得て収録しています。
Sampleをインポートした場合、サンプルに含まれる音声ファイルの2次配布を禁止します。
