using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ClipLibrary", menuName ="ClipLibrary")]
public class AudioClipLibrary : ScriptableObject
{
    [ReorderableList]
    [SerializeField] private List<AudioClip> AudioClips;

    public AudioClip Get(string soundName)
    {
        foreach ( AudioClip clip in AudioClips )
        {
            if ( clip.name == soundName )
            {
                return clip;
            }
        }
        return null;
    }
}
