using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{
    public Animator mainBG, backBtt, title, mainPage, expPage, lessonPage, gamePage, CSPage;
    public Sprite[] frameGal;
    public Image[] displayFrame;
    public Button csLv2, csLv3;
    public GameObject ARmode, loadBtt;

    private SceneLoader sceneLoader;
    private AudioSource audioSource;
    private AudioClip bttClk;
    private int UnlockAR;

    private void Awake()
    {
        // For Debug / Test only, DON'T TOUCH THIS!
        // PlayerPrefs.DeleteAll();
        // PlayerPrefs.SetInt("CannonShooterLv1Star", 3);
        // PlayerPrefs.SetInt("CannonShooterLv2Star", 3);
        // PlayerPrefs.SetInt("CSLv2", 1);
        // PlayerPrefs.SetInt("CannonShooterLv3Star", 3);
        // PlayerPrefs.SetInt("CSLv3", 1);


        // ***************************** Set defaut setting here *****************************
        PlayerPrefs.SetString("CannonShooterMode", "None");
        // ************************************************************************************

        Time.timeScale = 1;
        sceneLoader = GetComponent<SceneLoader>();
        audioSource = GetComponent<AudioSource>();
        bttClk = (AudioClip)Resources.Load("Audios/ButtonClick", typeof(AudioClip));

        /*****************************
         * Cannon Shooter
        *****************************/
        // Unlock level
        if (PlayerPrefs.GetInt("CSLv2") == 1)
            csLv2.interactable = true;
        if (PlayerPrefs.GetInt("CSLv3") == 1)
            csLv3.interactable = true;

        // Change border
        for (int i = 1; i <= 3; i++)
        {
            switch (PlayerPrefs.GetInt("CannonShooterLv" + i + "Star"))
            {
                case 1:
                    displayFrame[i - 1].sprite = frameGal[0];
                    UnlockAR += 1;
                    break;
                case 2:
                    displayFrame[i - 1].sprite = frameGal[1];
                    UnlockAR += 2;
                    break;
                case 3:
                    displayFrame[i - 1].sprite = frameGal[2];
                    UnlockAR += 3;
                    break;
            }
        }

        if (UnlockAR >= 7) // Unlock after get total 7 stars from any level
        {
            ARmode.SetActive(true);
        }
        UnlockAR = 0; //reset star count

        if (PlayerPrefs.GetInt("CSLvSelect") == 1)
        {
            slidePage("Game");
            slidePage("CannonShooter");
            PlayerPrefs.SetInt("CSLvSelect", 0);
        }

        if (PlayerPrefs.GetInt("LessonSelect") == 1)
        {
            slidePage("Experiment");
            slidePage("Lesson");
            PlayerPrefs.SetInt("LessonSelect", 0);
        }

        /*****************************
         * Experiment
        *****************************/
        PlayerPrefs.SetInt("World", 0);
        if (PlayerPrefs.GetInt("HaveWorldSaved") == 1)
            loadBtt.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void slidePage(string bttName)
    {
        switch (bttName)
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
            case "Lesson":
                {
                    mainBG.SetBool("SlideLesson", true);
                    backBtt.SetBool("SlideExperiment", true);
                    lessonPage.SetBool("IsLessonPage", true);

                    expPage.SetBool("IsExpPage", false);
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
                    CSPage.SetBool("IsCSPage", true);

                    gamePage.SetBool("IsGamePage", false);
                    title.SetBool("IsMainPage", false);
                    mainPage.SetBool("IsMainPage", false);
                    break;
                }

            case "Back":
                {
                    if (mainBG.GetBool("SlideGame") && !mainBG.GetBool("SlideCS"))
                    {
                        mainBG.SetBool("SlideGame", false);
                        backBtt.SetBool("SlideGame", false);
                        gamePage.SetBool("IsGamePage", false);

                        title.SetBool("IsMainPage", true);
                        mainPage.SetBool("IsMainPage", true);
                    }
                    else if (mainBG.GetBool("SlideCS"))
                    {
                        mainBG.SetBool("SlideCS", false);
                        CSPage.SetBool("IsCSPage", false);

                        gamePage.SetBool("IsGamePage", true);
                    }
                    else if (mainBG.GetBool("SlideExperiment") && mainBG.GetBool("SlideLesson"))
                    {
                        expPage.SetBool("IsExpPage", true);
                        mainBG.SetBool("SlideLesson", false);
                        lessonPage.SetBool("IsLessonPage", false);
                    }
                    else
                    {
                        mainBG.SetBool("SlideExperiment", false);
                        backBtt.SetBool("SlideExperiment", false);
                        expPage.SetBool("IsExpPage", false);

                        title.SetBool("IsMainPage", true);
                        mainPage.SetBool("IsMainPage", true);
                    }
                    break;
                }
        }
    }

    public void bttClick(string bttName)
    {
        audioSource.PlayOneShot(bttClk);
        switch (bttName)
        {
            /* Menu */
            case "CannonShooter": slidePage(bttName); break;
            case "Experiment": slidePage(bttName); break;
            case "Back": slidePage(bttName); break;

            /* Experiment */
            case "New":
                Time.timeScale = 0;
                PlayerPrefs.SetInt("LessonTask", 0);
                PlayerPrefs.SetInt("Lesson", 0);
                PlayerPrefs.SetInt("World", 0);
                sceneLoader.loadNewScene(1);
                break;
            case "Load":
                Time.timeScale = 0;
                PlayerPrefs.SetInt("LessonTask", 0);
                PlayerPrefs.SetInt("Lesson", 0);
                PlayerPrefs.SetInt("World", 1);
                sceneLoader.loadNewScene(1);
                break;

            case "Lesson":
                slidePage(bttName);
                break;
                /* Lessom*/
            case "Force":
                PlayerPrefs.SetInt("LessonTask", 1);
                PlayerPrefs.SetInt("Lesson", 0);
                sceneLoader.loadNewScene(2);
                break;
            case "Friction":
                PlayerPrefs.SetInt("LessonTask", 2);
                PlayerPrefs.SetInt("Lesson", 0);
                sceneLoader.loadNewScene(2);
                break;
            case "Gravity":
                PlayerPrefs.SetInt("LessonTask", 3);
                PlayerPrefs.SetInt("Lesson", 0);
                sceneLoader.loadNewScene(2);
                break;
            case "Momentum":
                PlayerPrefs.SetInt("LessonTask", 4);
                PlayerPrefs.SetInt("Lesson", 0);
                sceneLoader.loadNewScene(2);
                break;

            case "Game": slidePage(bttName); break;
            /* CannonShooter*/
            case "CSLv1":
                PlayerPrefs.SetString("CannonShooterMode", "Lv1");
                sceneLoader.loadNewScene(3);
                break;
            case "CSLv2":
                PlayerPrefs.SetString("CannonShooterMode", "Lv2");
                sceneLoader.loadNewScene(3);
                break;
            case "CSLv3":
                PlayerPrefs.SetString("CannonShooterMode", "Lv3");
                sceneLoader.loadNewScene(3);
                break;
            case "ARmode":
                PlayerPrefs.SetString("CannonShooterMode", "AR");
                sceneLoader.loadNewScene(4); break;
        }
    }
}
