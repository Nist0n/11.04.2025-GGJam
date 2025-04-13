using System.Collections;
using System.Collections.Generic;
using Settings.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameControl
{
    public class CutSceneManager : MonoBehaviour
    {
        [System.Serializable]
    public class CutsceneSlide
    {
        public Sprite image;
        [TextArea(3, 10)]
        public string text;
        public float duration = 5f; // Общее время слайда
        public float textSpeed = 0.05f; // Задержка между буквами
        public AudioClip clip;
    }

    public List<CutsceneSlide> slides;
    public Image displayImage;
    public TextMeshProUGUI displayText;
    public Button skipButton;
    public float fadeDuration = 1f; // Длительность затемнения

    private bool isPlaying;
    private Coroutine cutsceneRoutine;

    void Start()
    {
        skipButton.onClick.AddListener(SkipCutscene);
        StartCutscene();
    }

    void StartCutscene()
    {
        isPlaying = true;
        cutsceneRoutine = StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        foreach (var slide in slides)
        {
            // Плавное появление нового слайда
            displayImage.sprite = slide.image;
            displayText.text = "";

            if (slide.clip)
            {
                AudioManager.instance.PlaySfx(slide.clip.name);
            }

            // Постепенный вывод текста
            float timePerChar = slide.textSpeed;
            float timeElapsed = 0;
            int visibleChars = 0;

            while (visibleChars < slide.text.Length && isPlaying)
            {
                timeElapsed += Time.deltaTime;
                visibleChars = Mathf.FloorToInt(timeElapsed / timePerChar);
                visibleChars = Mathf.Clamp(visibleChars, 0, slide.text.Length);
                displayText.text = slide.text.Substring(0, visibleChars);
                yield return null;
            }

            // Ожидание оставшегося времени слайда
            float remainingTime = slide.duration - (visibleChars * timePerChar);
            if (remainingTime > 0 && isPlaying)
                yield return new WaitForSeconds(remainingTime);
        }

        EndCutscene();
    }

    void SkipCutscene()
    {
        if (!isPlaying) return;
        isPlaying = false;
        StopCoroutine(cutsceneRoutine);
        EndCutscene();
    }

    void EndCutscene()
    {
        // Переход на другую сцену или отключение Canvas
        Debug.Log("Cutscene ended!");
        gameObject.SetActive(false);
        AudioManager.instance.musicSource.Stop();
        // SceneManager.LoadScene("GameScene");
    }
    }
}
