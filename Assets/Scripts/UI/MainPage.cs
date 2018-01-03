using UnityEngine;

public class MainPage : MonoBehaviour
{
    public Animator mainBG, backBtt, title, mainPage, expPage, gamePage;

    private SceneLoader sceneLoader;
    private AudioSource audioSource;
    private AudioClip bttClk;

    private void Awake()
    {
        Time.timeScale = 1;
        sceneLoader = GetComponent<SceneLoader>();
        audioSource = GetComponent<AudioSource>();
        bttClk = (AudioClip)Resources.Load("Audios/ButtonClick", typeof(AudioClip));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void slidePage(string bttName)
    {
        switch(bttName)
        {
            case "Experiment":
                {
                    mainBG.SetBool("SlideExperiment", true);
                    backBtt.SetBool("SlideExperiment", true);
                    expPage.SetBool("IsExpPage", true);
                    title.SetBool("IsMainPage", false);
                    mainPage.SetBool("IsMainPage", false);
                    break;
                }

            case "Game":
                {
                    mainBG.SetBool("SlideGame", true);
                    backBtt.SetBool("SlideGame", true);
                    gamePage.SetBool("IsGamePage", true);
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
                        gamePage.SetBool("IsGamePage", false);
                    }
                    else
                    {
                        mainBG.SetBool("SlideExperiment", false);
                        backBtt.SetBool("SlideExperiment", false);
                        expPage.SetBool("IsExpPage", false);
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
            case "New": sceneLoader.loadNewScene(1); break;
            case "Load": break;
            case "Import": break;
            case "Game": slidePage(bttName); break;
            case "BallRoller": sceneLoader.loadNewScene(2); break;
            case "CannonShooter": sceneLoader.loadNewScene(3); break;
            case "Back": slidePage(bttName); break;
        }
    }
}
