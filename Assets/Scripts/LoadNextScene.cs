using GameControl;
using Settings.Audio;
using UnityEngine;

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
            if (PlayerPrefs.HasKey("SecondPhaseStart"))
            {
                PlayerPrefs.SetInt("SecondPhaseStart", 0);
            }
            _fader.LoadScene(nameOfScene);
        }
    }
}
