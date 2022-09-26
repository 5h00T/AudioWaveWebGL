var AudioAnalyzer = {

    $analyzer : {
        FFTSize: 512,
    },

    $GetSource : function(duration) {

        var acceptableDisantce = 0.05;

        if (typeof WEBAudio != 'undefined') {
            if (Array.isArray(WEBAudio.audioInstances)) {
                if (WEBAudio.audioInstances.length > 1) {
                    for (var i = WEBAudio.audioInstances.length - 1; i >= 0; i--) {
                        if (WEBAudio.audioInstances[i] != null) {
                            var tmpSource = WEBAudio.audioInstances[i].source;
                            if (tmpSource != null && tmpSource.buffer != null) {
                                if(Math.abs(tmpSource.buffer.duration - duration) < acceptableDisantce){
                                    analyzer["source"] = tmpSource;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else {
                if (Object.keys(WEBAudio.audioInstances).length > 1) {
                    for (var key in WEBAudio.audioInstances) {
                        if (WEBAudio.audioInstances.hasOwnProperty(key)) {
                            var tmpSource = WEBAudio.audioInstances[key].source;
                            if (tmpSource != null && tmpSource.buffer != null) {
                                if(Math.abs(tmpSource.buffer.duration - duration) < acceptableDisantce){
                                    analyzer["source"] = tmpSource;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    },

    $GetAnalyzer : function(duration) {
        GetSource(duration);

        if (analyzer["source"] == undefined) {
            return;
        }

        analyzer["analyzer"] = analyzer["source"].context.createAnalyser();
        analyzer["source"].connect(analyzer["analyzer"]);

    },

    SetFFTSize : function(size) {

        analyzer["FFTSize"] = size;
    },

    StartAudio: function (duration) {

        GetAnalyzer(duration);
    },

    GetSpectrumWave : function (bufferPtr, bufferSize, duration) {
        if (analyzer["analyzer"] == undefined) {
            GetAnalyzer(duration);
        }
        else {
            analyzer["analyzer"].fftSize = analyzer["FFTSize"];

            var buffer = new Float32Array(Module.HEAPF32.buffer, bufferPtr, bufferSize);

            analyzer["analyzer"].getFloatFrequencyData(buffer);
        }
    },

    GetTimeDomainWave : function (bufferPtr, bufferSize, duration) {

        if (analyzer["analyzer"] == undefined) {
            GetAnalyzer(duration);
        }
        else {
            analyzer["analyzer"].fftSize = analyzer["FFTSize"];

            var buffer = new Float32Array(Module.HEAPF32.buffer, bufferPtr, bufferSize);

            analyzer["analyzer"].getFloatTimeDomainData(buffer);
        }
    }
};

autoAddDeps(AudioAnalyzer, '$GetSource');
autoAddDeps(AudioAnalyzer, '$GetAnalyzer');
autoAddDeps(AudioAnalyzer, '$analyzer');
mergeInto(LibraryManager.library, AudioAnalyzer);