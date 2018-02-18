using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using Vuforia;

public class TrackingObject : MonoBehaviour
{
    // Quad for recive rendered texture
    public GameObject quad;
    // Camera that show quad
    public Camera MainCamera;
    // Sphere for finger
    public GameObject finger1, finger2;

    private UnityEngine.Rect rect;
    private FingerColor blue, yellow;
    private Mat thresholdMat, hsvMat, outputMat, hierarchy;
    private Texture2D outputTexture;
    private bool useAR = false;
    private double maxArea = 0, area;
    private int contourIndex = -1;
    private Moments moment;
    private FingerColor fingerObject;

    void Start()
    {
        blue = new FingerColor("blue");
        yellow = new FingerColor("yellow");

        fingerObject = new FingerColor();

        rect = new UnityEngine.Rect(0, 0, Screen.width, Screen.height);
        outputTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        outputMat = new Mat(Screen.height, Screen.width, CvType.CV_8UC3);
        thresholdMat = new Mat();
        hsvMat = new Mat();
        hierarchy = new Mat();

        quad.GetComponent<Renderer>().material.mainTexture = outputTexture;
        quad.transform.localScale = new Vector3(Screen.width, Screen.height, 1);
        MainCamera.orthographicSize = Screen.height / 2;

        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.Stop();
    }

    private void OnPostRender()
    {
        if (useAR && Time.renderedFrameCount % 3 == 0) // Prevent from running when not use
        {
            outputTexture.ReadPixels(rect, 0, 0, true);
            outputTexture.Apply();
            Utils.texture2DToMat(outputTexture, outputMat);

            // Change from Texture(original) to HSV Mat
            // Convert color space from RGB(source) to HSV(recive)
            Imgproc.cvtColor(outputMat, hsvMat, Imgproc.COLOR_RGB2HSV);

            // Find blue finger
            Core.inRange(hsvMat, blue.HSVMin, blue.HSVMax, thresholdMat);
            morphOps(thresholdMat);
            trackingFinger(blue, thresholdMat);

            // Find yellow finger
            Core.inRange(hsvMat, yellow.HSVMin, yellow.HSVMax, thresholdMat);
            morphOps(thresholdMat);
            trackingFinger(yellow, thresholdMat);

            // Change from Mat(rendered) to Texture
            Utils.matToTexture2D(outputMat, outputTexture);
        }
    }

    private void morphOps(Mat threshold)
    {
        // Use opening to remove noise from threshold
        // Erode with 3px rectangle
        Mat erodeElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(3, 3));
        // Dilate with 8px rectangle to make object(finger) in good shape
        Mat dilateElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(8, 8));

        // Erode 2 times to make sure all noises delete
        Imgproc.erode(threshold, threshold, erodeElement);
        Imgproc.erode(threshold, threshold, erodeElement);

        // Dilate back 2 times
        Imgproc.dilate(threshold, threshold, dilateElement);
        Imgproc.dilate(threshold, threshold, dilateElement);
    }

    private void trackingFinger(FingerColor fingerColor, Mat threshold)
    {
        List<MatOfPoint> contours = new List<MatOfPoint>();
        hierarchy.release();

        /*** See more https://docs.opencv.org/3.3.1/d3/dc0/group__imgproc__shape.html ***/
        // findContours() change input image, we need it to find other finger later so we copy image to temp
        Imgproc.findContours(threshold, contours, hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);

        maxArea = 0;
        contourIndex = -1;

        if (hierarchy.rows() > 0)
        {
            for (int index = 0; index >= 0; index = (int)hierarchy.get(0, index)[0])
            {
                moment = Imgproc.moments(contours[index]);
                // (0, 0) is top-left
                area = moment.get_m00();

                // If the area is less than 40 * 40px then it is probably just noise
                // Store max area only
                if (area > 40 * 40 && area > maxArea)
                {
                    /*** See more https://docs.opencv.org/2.4/modules/imgproc/doc/structural_analysis_and_shape_descriptors.html ***/
                    fingerObject.XPos = (int)(moment.get_m10() / area);
                    fingerObject.YPos = (int)(moment.get_m01() / area);
                    fingerObject.Type = fingerColor.Type;
                    fingerObject.Color = fingerColor.Color;
                    maxArea = area;
                    contourIndex = index;
                }
            }
            if (contourIndex > -1)
            {
                drawObject(fingerObject, contourIndex, outputMat, contours, hierarchy);
                // Still find goood range
                float z = 20 * (20 - ((float)maxArea / 30000f));
                if (fingerColor.Type == "blue")
                {
                    //finger1.SetActive(true);
                    finger1.transform.position = new Vector3(fingerObject.XPos, fingerObject.YPos, (float)maxArea);

                }
                else
                {
                    //finger2.SetActive(true);
                    finger2.transform.position = new Vector3(fingerObject.XPos, fingerObject.YPos, (float)maxArea);
                }
            }
            else
            {
                if (fingerColor.Type == "blue")
                    finger1.SetActive(false);
                else
                    finger2.SetActive(false);
                /* Do something to find finger again */
            }
        }
    }

    private void drawObject(FingerColor fingerObject, int contourIndex, Mat frame, List<MatOfPoint> contours, Mat hierarchy)
    {
        // Draw contours line
        Imgproc.drawContours(frame, contours, contourIndex, fingerObject.Color, 3, 8, hierarchy, int.MaxValue, new Point());
        // Draw center of contours(centroid)
        Imgproc.circle(frame, new Point(fingerObject.XPos, fingerObject.YPos), 5, fingerObject.Color);
    }

    public bool UseAR
    {
        set
        {
            useAR = value;
            if (useAR)
                CameraDevice.Instance.Start();
            else
                CameraDevice.Instance.Stop();
        }
    }
}
