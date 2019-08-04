using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public AudioSource source;
    public AudioClip musicWithIntro;
    public AudioClip musicNoIntro;
    public Animator bgAnim;
    public float lightningChance = 10f;

    void Start()
    {
        source.clip = musicWithIntro;
        source.Play();

        StartCoroutine(Lightning());
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            source.clip = musicNoIntro;
            source.loop = true;
            source.Play();
        }
    }

    IEnumerator Lightning()
    {
        print("Ges");
        bgAnim.ResetTrigger("lightning");
        yield return new WaitForSeconds(2f);
        int num = Random.Range(0, 100);
        if (num <= lightningChance)
        {
            bgAnim.SetTrigger("lightning");
        }
        StartCoroutine(Lightning());
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
