mergeInto(LibraryManager.library, {

  InitSpeech: function () {
    window._ttsChosenVoice = null;
    window._ttsCurrentUtterance = null;  
    window._ttsWatchdog = null;

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
    window.speechSynthesis.onvoiceschanged = function () { findVoice(); };
  },

  SpeakText: function (textPtr, rateFloat, pitchFloat, volumeFloat) {
    var text = UTF8ToString(textPtr);
    if (!window.speechSynthesis) {
      console.warn("[TTS] Web Speech API not supported.");
      return;
    }

    if (window._ttsCurrentUtterance) {
      window.speechSynthesis.cancel();
    }

    if (window._ttsWatchdog) {
      clearInterval(window._ttsWatchdog);
      window._ttsWatchdog = null;
    }

    var utt = new SpeechSynthesisUtterance(text);
    utt.rate = rateFloat;
    utt.pitch = pitchFloat;
    utt.volume = volumeFloat;
    if (window._ttsChosenVoice) utt.voice = window._ttsChosenVoice;

    var self = this;

    utt.onstart = function () {
      if (window._ttsCurrentUtterance === utt) {
        SendMessage("GameManager", "OnSpeechStart");
      }
    };

    utt.onend = function () {
      if (window._ttsCurrentUtterance === utt) {
        window._ttsCurrentUtterance = null;
        if (window._ttsWatchdog) {
          clearInterval(window._ttsWatchdog);
          window._ttsWatchdog = null;
        }
        SendMessage("GameManager", "OnSpeechEnd");
      }
    };

    utt.onerror = function (e) {
      if (window._ttsCurrentUtterance === utt) {
        window._ttsCurrentUtterance = null;
        if (window._ttsWatchdog) {
          clearInterval(window._ttsWatchdog);
          window._ttsWatchdog = null;
        }
        SendMessage("GameManager", "OnSpeechError", e.error);
      }
    };

    window._ttsCurrentUtterance = utt;

    window._ttsWatchdog = setInterval(function () {
      if (window._ttsCurrentUtterance && window.speechSynthesis.speaking) {
        window.speechSynthesis.pause();
        window.speechSynthesis.resume();
      } else if (!window.speechSynthesis.speaking && window._ttsCurrentUtterance) {
        console.warn("[TTS] Watchdog: forcing end");
        var deadUtt = window._ttsCurrentUtterance;
        window._ttsCurrentUtterance = null;
        clearInterval(window._ttsWatchdog);
        window._ttsWatchdog = null;
        SendMessage("GameManager", "OnSpeechEnd");
      }
    }, 10000);

    window.speechSynthesis.speak(utt);
  },

  StopSpeech: function () {
    if (window._ttsWatchdog) {
      clearInterval(window._ttsWatchdog);
      window._ttsWatchdog = null;
    }
    if (window.speechSynthesis) {
      window.speechSynthesis.cancel();
    }
    if (window._ttsCurrentUtterance) {
      window._ttsCurrentUtterance = null;
      SendMessage("GameManager", "OnSpeechEnd");
    }
  },

  IsSpeaking: function () {
    if (!window.speechSynthesis) return 0;
    return (window.speechSynthesis.speaking || window._ttsCurrentUtterance !== null) ? 1 : 0;
  }
});