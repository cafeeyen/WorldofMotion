using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingPrefeb;

    private Text loadingText;
    private bool loadScene = false;
    private float alpha;

    public void loadNewScene(int sceneIndex, int saveIndex=0)
    {
        if(!loadScene)
        {
            loadScene = true;
            loadingPrefeb.SetActive(true);
            loadingText = loadingPrefeb.GetComponentInChildren<Text>();
            if(saveIndex != 0)
            {
                // Do something
            }
            StartCoroutine(LoadScene(sceneIndex));
            loadScene = false;
        }
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneIndex);

        // While loading
        while (!async.isDone)
        {
            alpha = Mathf.PingPong(Time.unscaledTime, 1.0f);
            loadingText.color = loadingText.color * alpha;
            yield return null;
        }
    }
}
