using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;

public class CannonUIController : MonoBehaviour
{
    /* Input Component */
    public InputField powField;
    public Slider powSlider;
    public TapGesture AngleButtonUp, AngleButtonDown;

    /* Output Component */
    public Text angleText, tarDistText, tarHeightText, calDistText, calHeightText, calTimeText, countText;
    public LineRenderer arcLine, groundLine;

    /* AR Camera Component */
    public RawImage MiniRawImage, FullScreenRawImage;
    public Image imageOverlay;
    public RenderTexture cutsceneCamRT, sideCamRT;

    /* Image and Sprite */
    public Image lessonDisplay;
    public Sprite activeDot, nonActiveDot;
    public Sprite[] lessonGallery;
    public GameObject[] tutorialGallery;
    public Image[] dots;
    public GameObject BadgeGroup, Lv1Badge, Lv2Badge, Lv3Badge;

    /* Game Parameter */
    private int angle = 0, power, shootCnt = 0;
    private float tarDist, tarHeight;
    private Vector3 cannonPos = new Vector3(0, -1.8f, 8), canyonPos = new Vector3(0, 27, 8);

    /* UI Parameter / Group */
    public GameObject MenuSection, LessonSection, TableDisplay, TutorialSection, TutorialAsk, Lv1Dot, Lv2Dot, Lv3Dot;
    public Animator CalculateTab;
    private int page, tempPage;

    /* Script */
    public CannonGameController GameController;

    /* BGs */
    public GameObject canyon, ground;

    /* Unity Function */
    private void OnEnable()
    {
        switch (PlayerPrefs.GetString("CannonShooterMode"))
        {
            case "Lv1":
                page = 0;
                canyon.SetActive(false);
                ground.SetActive(true);
                Lv1Dot.SetActive(true);
                if(PlayerPrefs.GetInt("CannonTutorialLv1") == 0)
                {
                    GameController.pauseGame(true);
                    askTutorial();
                    PlayerPrefs.SetInt("CannonTutorialLv1", 1);
                }
                break;
            case "Lv2":
                page = 12;
                Lv2Dot.SetActive(true);
                if (PlayerPrefs.GetInt("CannonTutorialLv2") == 0)
                {
                    GameController.pauseGame(true);
                    openMenu("LessonSection");
                    PlayerPrefs.SetInt("CannonTutorialLv2", 1);
                }
                    break;
            case "Lv3":
                page = 16;
                Lv3Dot.SetActive(true);
                if (PlayerPrefs.GetInt("CannonTutorialLv3") == 0)
                {
                    GameController.pauseGame(true);
                    openMenu("LessonSection");
                    PlayerPrefs.SetInt("CannonTutorialLv3", 1);
                }
                break;
            case "AR":
                page = 0;
                Lv1Dot.SetActive(true); // Use same variable
                if (PlayerPrefs.GetInt("CannonTutorialAR") == 0)
                {
                    openMenu("LessonSection");
                    PlayerPrefs.SetInt("CannonTutorialAR", 1);
                }
                break;
        }
    }
    private void FixedUpdate()
    {
        if (AngleButtonUp.State == Gesture.GestureState.Possible && angle > -90)
            angle--;
        else if (AngleButtonDown.State == Gesture.GestureState.Possible&& angle < 0)
            angle++;
        GameController.rotateAngle(angle);
        angleText.text = string.Format("{0} \u00B0", Mathf.Round(-angle));
    }

    /* Input-Output-Feedback */
    public void changePowSlider()
    {
        powField.text = powSlider.value.ToString();
        power = (int)powSlider.value;
    }
    public void changePowText()
    {
        if (string.IsNullOrEmpty(powField.text))
            power = 0;
        else
        {
            try
            {
                power = Mathf.Clamp(int.Parse(powField.text), 0, 30);
                powField.text = power.ToString();
                powSlider.value = power;
            }
            catch { Debug.Log("Power input error."); }
        }
    }
    public void shoot()
    {
        /* Don't care about air resistance
        * Use G = 9.81 m/s^2
            * Start point height is cannon
            * End point height is target
            * Lowest between two will be height for ground
            * 
            * -------------------------------------------------------------
            * 
            * *** Start point and end point have SAME height ***
            * 
            * Max height = (initial velocity^2 * sin^2(angle)) / 2g 
            * Time of flight = 2initial velocity * sin(angle) / g
            * Distance = (initial velocity^2 * sin2(angle)) / g
            * 
            * -------------------------------------------------------------
            * 
            * *** Start point and end point can be DIFFERENCE height *** <<< Use this one
            * 
            * Max height = h + (vy0 * Rise) - (0.5 * g * Rise^2)
            * Time of flight = Rise(vy0 / g) + Fall(sqrt( 2Max height / g))
            * Distance = vx0 * time
            * 
            * vx0 = V0 * Cos(angle) <- ucos
            * vy0 = V0 * Sin(angle) <- usin
            * h = height difference between cannon and ground/target
           */
        if (power > 0)
        {
            if(GameController.shoot(-angle, power))
            {
                var rise = power * Mathf.Sin(-angle * Mathf.Deg2Rad) / 9.81f;
                var maxHeight = tarHeight + (power * Mathf.Sin(-angle * Mathf.Deg2Rad) * rise) - (0.5f * 9.81f * Mathf.Pow(rise, 2));
                var fall = Mathf.Sqrt(2 * maxHeight / 9.81f);
                var maxTime = rise + fall;

                calDistText.text = "ระยะทางในแนวราบ " + (power * Mathf.Cos(-angle * Mathf.Deg2Rad) * maxTime).ToString("F2") + " m";
                calHeightText.text = "ความสูงจากจุดเริ่ม " + maxHeight.ToString("F2") + " m";
                calTimeText.text = "เวลาที่ลอยกลางอากาศ " + maxTime.ToString("F2") + " s";

                if(PlayerPrefs.GetString("CannonShooterMode") != "AR")
                {
                    shootCnt++;
                    countText.text = shootCnt.ToString();
                }
                drawCurve(maxTime);
            }
        }
    }
    public void showCalculateTab()
    {
        CalculateTab.SetBool("IsDisplayed", !CalculateTab.GetBool("IsDisplayed"));
    }
    private void drawCurve(float maxTime)
    {
        if (PlayerPrefs.GetString("CannonShooterMode") == "AR")
        {
            if (!MiniRawImage.enabled)
                MiniRawImage.enabled = true;

            viewToSideCam();
            groundLine.positionCount = 2;
            groundLine.SetPosition(0, new Vector3(0, -1.8f, 0));
            groundLine.SetPosition(1, new Vector3(0, -1.8f, 100));
        }

         if (!arcLine.enabled)
            arcLine.enabled = true;

        int maxIndex = Mathf.RoundToInt(maxTime / Time.fixedDeltaTime);
        arcLine.positionCount = maxIndex;

        Vector3 curPos;
        if (PlayerPrefs.GetString("CannonShooterMode") == "Lv2" || PlayerPrefs.GetString("CannonShooterMode") == "Lv3")
            curPos = canyonPos;
        else
            curPos = cannonPos;

        Vector3 curVel = GameController.getCannonForward() * power;

        for (int i = 0; i < maxIndex; i++)
        {
            arcLine.SetPosition(i, curPos);

            curVel += Physics.gravity * Time.fixedDeltaTime;
            curPos += curVel * Time.fixedDeltaTime;
        }
    }

    /* Game Tutorial */
    private void askTutorial()
    {
        TutorialSection.SetActive(true);
    }
    public void ansTutorial(bool ans)
    {
        if(ans)
        {
            powSlider.gameObject.SetActive(true);
            MenuSection.SetActive(false);
            LessonSection.SetActive(false);
            tempPage = page;
            page = 0;
            tutorialGallery[page].SetActive(true);
        }
        else
        {
            TutorialSection.SetActive(false);
            GameController.pauseGame(false);
        }
        TutorialAsk.SetActive(false);
    }
    public void nextTutorialPage()
    {
        tutorialGallery[page].SetActive(false);
        page++;

        if (page != tutorialGallery.Length)
            tutorialGallery[page].SetActive(true);
        else
        {
            LessonSection.SetActive(true);
            TutorialSection.SetActive(false);
            powSlider.gameObject.SetActive(false);
            GameController.pauseGame(true);
            page = tempPage;
        }
    }

    /* Game Lesson */
    public void changeLessonPage(bool isNext)
    {
        dots[page].sprite = nonActiveDot;
        switch (PlayerPrefs.GetString("CannonShooterMode"))
        {
            case "Lv1":
                page = isNext ? (page + 1) % 12 : page - 1 < 0 ? 11 : page - 1;
                break;
            case "Lv2":
                page = isNext ? (page + 1 > 15 ? 12 : page + 1) : (page - 1 < 12 ? 15 : page - 1);
                break;
            case "Lv3":
                page = isNext ? (page + 1 > 20 ? 16 : page + 1) : (page - 1 < 16 ? 20 : page - 1);
                break;
            case "AR":
                page = isNext ? (page + 1) % 8 : page - 1 < 0 ? 7 : page - 1;
                break;
        }
        dots[page].sprite = activeDot;
        lessonDisplay.sprite = lessonGallery[page];
    }

    /* AR Mode Camera */
    public void viewToCutCam()
    {
        MiniRawImage.texture = cutsceneCamRT;
        FullScreenRawImage.texture = cutsceneCamRT;
    }
    public void viewToSideCam()
    {
        MiniRawImage.texture = sideCamRT;
        FullScreenRawImage.texture = sideCamRT;
    }
    public void openFullScreenImage()
    {
        if (MiniRawImage.enabled)
        {
            FullScreenRawImage.gameObject.SetActive(true);
            imageOverlay.enabled = true;
        }
    }
    public void closeFullScreenImage()
    {
        FullScreenRawImage.gameObject.SetActive(false);
        imageOverlay.enabled = false;
    }

    /* Shared UI - Game Functions */
    public void openMenu(string window)
    {
        switch (window)
        {
            case "MenuSection": MenuSection.SetActive(true); GameController.pauseGame(true); powSlider.gameObject.SetActive(false); break;
            case "LessonSection": LessonSection.SetActive(true); powSlider.gameObject.SetActive(false); break;
            case "TableDisplay": TableDisplay.SetActive(true); break;
            case "TutorialSection": TutorialSection.SetActive(true); ansTutorial(true); break;
        }
    }
    public void closeMenu(string window)
    {
        switch(window)
        {
            case "MenuSection": MenuSection.SetActive(false); GameController.pauseGame(false); powSlider.gameObject.SetActive(true); break;
            case "LessonSection":
                {
                    LessonSection.SetActive(false);
                    if (MenuSection.activeSelf == false)
                        GameController.pauseGame(false);
                    powSlider.gameObject.SetActive(true);
                    break;
                }
            case "TableDisplay": TableDisplay.SetActive(false); break;
        }
    }
    public void backToMainMenu(bool selectLV)
    {
        if(selectLV)
            PlayerPrefs.SetInt("CSLvSelect", 1);
        PlayerPrefs.SetString("CannonShooterMode", "None");
        GetComponent<SceneLoader>().loadNewScene(0);
    }
    public void setTargetDetail(float dist, float height)
    {
        if(dist == -1 && height == -1)
        {
            tarDist = 0;
            tarHeight = -1.8f;

            tarDistText.text = "----";
            tarHeightText.text = "----";
        }
        else
        {
            tarDist = dist;
            tarHeight = height;

            tarDistText.text = tarDist.ToString("F2") + " m";
            tarHeightText.text = tarHeight.ToString("F2") + " m";
        }
    }
    public void endStage()
    {
        BadgeGroup.SetActive(true);
        GameController.pauseGame(true);
        powSlider.gameObject.SetActive(false);
        if (PlayerPrefs.GetString("CannonShooterMode") == "Lv1")
            PlayerPrefs.SetInt("CSLv2", 1);
        if (PlayerPrefs.GetString("CannonShooterMode") == "Lv2")
            PlayerPrefs.SetInt("CSLv3", 1);

        if (shootCnt == 5)
        {
            Lv3Badge.SetActive(true);
            Lv3Badge.GetComponent<Animator>().SetBool("Show", true);
            PlayerPrefs.SetInt("CannonShooter" + PlayerPrefs.GetString("CannonShooterMode") + "Star", 3);
        }
        else if (shootCnt <= 10)
        {
            Lv2Badge.SetActive(true);
            Lv2Badge.GetComponent<Animator>().SetBool("Show", true);
            if (PlayerPrefs.GetInt("CannonShooter" + PlayerPrefs.GetString("CannonShooterMode") + "Star") < 3)
                PlayerPrefs.SetInt("CannonShooter" + PlayerPrefs.GetString("CannonShooterMode") + "Star", 2);
        }
        else
        {
            Lv1Badge.SetActive(true);
            Lv1Badge.GetComponent<Animator>().SetBool("Show", true);
            if (PlayerPrefs.GetInt("CannonShooter" + PlayerPrefs.GetString("CannonShooterMode") + "Star") < 2)
                PlayerPrefs.SetInt("CannonShooter" + PlayerPrefs.GetString("CannonShooterMode") + "Star", 1);
        }
    }
}
