using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    public Animator mainBG, backBtt, title, mainPage, expPage, gamePage, CSPage;
    public Button csLv2, csLv3;

    private SceneLoader sceneLoader;
    private AudioSource audioSource;
    private AudioClip bttClk;

    private void Awake()
    {
        Time.timeScale = 1;
        sceneLoader = GetComponent<SceneLoader>();
        audioSource = GetComponent<AudioSource>();
        bttClk = (AudioClip)Resources.Load("Audios/ButtonClick", typeof(AudioClip));

        // Unlock level
        if (PlayerPrefs.GetInt("csLv2") == 1)
            csLv2.interactable = true;
        if (PlayerPrefs.GetInt("csLv3") == 1)
            csLv3.interactable = true;
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
            case "CannonShooter":
                {
                    mainBG.SetBool("SlideCS", true);
                    mainBG.SetBool("SlideGame", false);
                    CSPage.SetBool("IsCSPage", true);
                    gamePage.SetBool("IsGamePage", false);
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
                        title.SetBool("IsMainPage", true);
                        mainPage.SetBool("IsMainPage", true);
                    }
                    else if(mainBG.GetBool("SlideCS"))
                    {
                        mainBG.SetBool("SlideCS", false);
                        CSPage.SetBool("IsCSPage", false);
                        backBtt.SetBool("SlideGame", false);

                        mainBG.SetBool("SlideGame", true);
                        backBtt.SetBool("SlideGame", true);
                        gamePage.SetBool("IsGamePage", true);
                    }
                    else
                    {
                        mainBG.SetBool("SlideExperiment", false);
                        backBtt.SetBool("SlideExperiment", false);
                        expPage.SetBool("IsExpPage", false);
                        title.SetBool("IsMainPage", true);
                        mainPage.SetBool("IsMainPage", true);
                    }

                    //title.SetBool("IsMainPage", true);
                    //mainPage.SetBool("IsMainPage", true);
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
            case "CannonShooter": slidePage(bttName); break;
            case "CSLv1":
                PlayerPrefs.SetInt("CannonShooterMode", 1);
                sceneLoader.loadNewScene(3);
                break;
            case "Back": slidePage(bttName); break;
        }
    }
}
