using System;
using System.Runtime.CompilerServices;
using GameControl;
using Settings.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    private FaderExample _fader;

    [SerializeField] private string nameOfScene;

    private void Start()
    {
        _fader = FindFirstObjectByType<FaderExample>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.PlaySfx("OpenDoor");
            _fader.LoadScene(nameOfScene);
        }
    }
}
