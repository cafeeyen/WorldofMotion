using UnityEngine;
using UnityEngine.UI;

public class ProblemGenerator : MonoBehaviour
{
    public LessonItem item;
    public Text distanceText;

    private float distance, time;

	public void newProblem()
    {
        distance = Random.Range(10, 100);
        time = Random.Range(5, 20);

        item.setTime(time);
        transform.position = new Vector3(0, 0, distance);
        distanceText.text = distance.ToString();
    }
}
