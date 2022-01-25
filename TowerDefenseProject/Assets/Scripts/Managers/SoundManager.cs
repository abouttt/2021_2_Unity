using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        Bgm,
        Effect,
        MaxCount,
    }

    private static SoundManager s_instance = null;
    public static SoundManager Instance { get { Init(); return s_instance; } }

    private static AudioSource[] _audioSources = new AudioSource[(int)SoundType.MaxCount];
    private static Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private void Start()
    {
        Init();
    }

    public void Play(string path, SoundType type = SoundType.Effect, float pich = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pich);
    }

    public void Play(AudioClip audioClip, SoundType type = SoundType.Effect, float pich = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == SoundType.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)SoundType.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pich;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)SoundType.Effect];
            audioSource.pitch = pich;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string path, SoundType type = SoundType.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == SoundType.Bgm)
        {
            audioClip = ResourceManager.Instance.Load<AudioClip>(path);
        }
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = ResourceManager.Instance.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("SoundManager");
            if (go == null)
            {
                go = new GameObject { name = "SoundManager" };
                go.AddComponent<SoundManager>();
            }

            s_instance = go.GetComponent<SoundManager>();
            Util.SetChildAsParent("Managers", s_instance.transform);

            string[] soundNames = System.Enum.GetNames(typeof(SoundType));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject sound = new GameObject { name = soundNames[i] };
                _audioSources[i] = sound.AddComponent<AudioSource>();
                sound.transform.parent = s_instance.transform;
            }

            _audioSources[(int)SoundType.Bgm].loop = true;
        }
    }
}
