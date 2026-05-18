mergeInto(LibraryManager.library, {

  InitSpeech: function() {
    window._ttsChosenVoice = null;

    function findVoice() {
      var voices = window.speechSynthesis.getVoices();
      if (voices.length === 0) return;

      var preferred = ["Microsoft David", "Microsoft Mark", "David", "Mark", "Fred", "Daniel"];
      var chosen = null;

      for (var i = 0; i < preferred.length; i++) {
        for (var j = 0; j < voices.length; j++) {
          if (voices[j].name.indexOf(preferred[i]) !== -1 && voices[j].lang.indexOf("en") === 0) {
            chosen = voices[j];
            break;
          }
        }
        if (chosen) break;
      }

      if (!chosen) {
        var excludeKeywords = ["female", "zira", "samantha", "hazel", "susan", "karen"];
        for (var j = 0; j < voices.length; j++) {
          var v = voices[j];
          if (v.lang.indexOf("en") === 0) {
            var nameLower = v.name.toLowerCase();
            var isFemale = false;
            for (var k = 0; k < excludeKeywords.length; k++) {
              if (nameLower.indexOf(excludeKeywords[k]) !== -1) {
                isFemale = true;
                break;
              }
            }
            if (!isFemale) { chosen = v; break; }
          }
        }
      }

      if (!chosen) {
        for (var j = 0; j < voices.length; j++) {
          if (voices[j].lang.indexOf("en") === 0) {
            chosen = voices[j];
            break;
          }
        }
      }

      window._ttsChosenVoice = chosen;
      console.log("[TTS] Voice ready: " + (chosen ? chosen.name : "default"));
    }

    findVoice();
    window.speechSynthesis.onvoiceschanged = function() {
      findVoice();
    };
  },

  SpeakText: function(textPtr, rateFloat, pitchFloat, volumeFloat) {
    var text = UTF8ToString(textPtr);
    if (!window.speechSynthesis) {
      console.warn("Web Speech API not supported.");
      return;
    }

    window.speechSynthesis.cancel();

    var utt = new SpeechSynthesisUtterance(text);
    utt.rate   = rateFloat;
    utt.pitch  = pitchFloat;
    utt.volume = volumeFloat;

    if (window._ttsChosenVoice) {
      utt.voice = window._ttsChosenVoice;
    }

    utt.onstart = function() { SendMessage("SpeechManager", "OnSpeechStart"); };
    utt.onend   = function() { SendMessage("SpeechManager", "OnSpeechEnd"); };
    utt.onerror = function(e) { SendMessage("SpeechManager", "OnSpeechError", e.error); };

    window.speechSynthesis.speak(utt);
  },

  StopSpeech: function() {
    if (window.speechSynthesis) window.speechSynthesis.cancel();
  },

  IsSpeaking: function() {
    if (!window.speechSynthesis) return 0;
    return window.speechSynthesis.speaking ? 1 : 0;
  }

});