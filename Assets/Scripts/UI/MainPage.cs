using UnityEngine;

public class MainPage : MonoBehaviour
{
    public Animator mainBG, backBtt, pageSlide, title, mainPage;

    private SceneLoader sl;
    private AudioSource audioSource;
    private AudioClip bttClk;

    private void Awake()
    {
        Time.timeScale = 1;
        sl = GetComponent<SceneLoader>();
        audioSource = GetComponent<AudioSource>();
        bttClk = (AudioClip)Resources.Load("Audios/ButtonClick", typeof(AudioClip));
    }

    private void slidePage(string bttName)
    {
        switch(bttName)
        {
            case "Experiment":
                {
                    mainBG.SetBool("SlideExperiment", true);
                    backBtt.SetBool("SlideExperiment", true);
                    pageSlide.SetBool("SlideExperiment", true);
                    title.SetBool("IsMainPage", false);
                    mainPage.SetBool("IsMainPage", false);
                    break;
                }

            case "Game":
                {
                    mainBG.SetBool("SlideGame", true);
                    backBtt.SetBool("SlideGame", true);
                    pageSlide.SetBool("SlideGame", true);
                    title.SetBool("IsMainPage", false);
                    mainPage.SetBool("IsMainPage", false);
                    break;
                }

            case "Back":
                {
                    if(mainBG.GetBool("SlideGame"))
                    {
                        mainBG.SetBool("SlideGame", false);
                        backBtt.SetBool("SlideGame", false);
                        pageSlide.SetBool("SlideGame", false);
                    }
                    else
                    {
                        mainBG.SetBool("SlideExperiment", false);
                        backBtt.SetBool("SlideExperiment", false);
                        pageSlide.SetBool("SlideExperiment", false);
                    }
                    title.SetBool("IsMainPage", true);
                    mainPage.SetBool("IsMainPage", true);
                    break;
                }
        }
    }

    public void bttClick(string bttName)
    {
        audioSource.PlayOneShot(bttClk);
        switch (bttName)
        {
            case "Experiment": slidePage(bttName); break;
            case "New": sl.loadNewScene(1); break;
            case "Load": break;
            case "Import": break;
            case "Game": slidePage(bttName); break;
            case "CannonShooter": break;
            case "BallRoller": break;
            case "Back": slidePage(bttName); break;
        }
    }
}
