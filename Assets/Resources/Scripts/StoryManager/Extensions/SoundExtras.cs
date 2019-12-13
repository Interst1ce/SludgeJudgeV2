using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundExtras : MonoBehaviour {
    public void PlaySoundEffects(SoundEffect sfx) {
        GameObject soundPlayer = GameObject.Find("Sound Player");
        if (soundPlayer == null) {
            soundPlayer = new GameObject("Sound Player");
            soundPlayer = Instantiate(soundPlayer,transform);
            foreach (SFX sound in sfx.soundEffects) {
                AudioSource aS = soundPlayer.AddComponent<AudioSource>();
                aS.clip = sound.soundEffect;
                aS.loop = sound.loop;
                aS.PlayDelayed(sound.delay);
            }
        } else {
            AudioSource[] sources = soundPlayer.GetComponents<AudioSource>();
            foreach (AudioSource source in sources) {
                if (source.loop != true) Destroy(source);
            }
        }
        foreach (SFX sound in sfx.soundEffects) {
            AudioSource aS = soundPlayer.AddComponent<AudioSource>();
            aS.clip = sound.soundEffect;
            aS.loop = sound.loop;
            aS.PlayDelayed(sound.delay);
        }
    }
}
