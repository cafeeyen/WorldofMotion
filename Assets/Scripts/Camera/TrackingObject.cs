using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;

public class TrackingObject : MonoBehaviour
{
    // Quad for recive rendered texture
    public GameObject quad;
    // Camera that show quad
    public Camera MainCamera;
    // Render Texture from camera that get background only
    public RenderTexture RT;

    FingerColor blue, yellow;
    Mat BGMat, rgbMat, thresholdMat, hsvMat, outputMat;
    Texture2D cameraTexture, outputTexture;
    Color32[] colors;

    void Start()
    {
        blue = new FingerColor("blue");
        yellow = new FingerColor("yellow");

        BGMat = new Mat(Screen.height, Screen.width, CvType.CV_8UC4);
        RT.height = Screen.height;
        RT.width = Screen.width;
        cameraTexture = new Texture2D(BGMat.cols(), BGMat.rows(), TextureFormat.ARGB32, false);
        outputTexture = new Texture2D(BGMat.cols(), BGMat.rows(), TextureFormat.ARGB32, false);
        colors = new Color32[outputTexture.width * outputTexture.height];
        MainCamera.orthographicSize = cameraTexture.height / 2;
        
        rgbMat = new Mat(cameraTexture.height, cameraTexture.width, CvType.CV_8UC3);
        outputMat = new Mat(cameraTexture.height, cameraTexture.width, CvType.CV_8UC3);
        hsvMat = new Mat();
        thresholdMat = new Mat();

        quad.transform.localScale = new Vector3(cameraTexture.width, cameraTexture.height, quad.transform.localScale.z);
        quad.GetComponent<Renderer>().material.mainTexture = outputTexture;
    }

    private void OnPostRender()
    {
        // Get texture from camera
        UnityEngine.Rect rect = new UnityEngine.Rect(0, 0, cameraTexture.width, cameraTexture.height);
        cameraTexture.ReadPixels(rect, 0, 0, true);

        // Change from Texture(original) to Mat
        Utils.texture2DToMat(GetRTPixels(RT), BGMat);
        Utils.texture2DToMat(cameraTexture, outputMat);

        // Convert color space from rgba(source) to rgb(recive)
        Imgproc.cvtColor(BGMat, rgbMat, Imgproc.COLOR_RGBA2RGB);

        // Find blue finger
        Imgproc.cvtColor(rgbMat, hsvMat, Imgproc.COLOR_RGB2HSV);
        Core.inRange(hsvMat, blue.HSVMin, blue.HSVMax, thresholdMat);
        morphOps(thresholdMat);
        trackingFinger(blue, thresholdMat, hsvMat, rgbMat);

        // Find yellow finger
        Imgproc.cvtColor(rgbMat, hsvMat, Imgproc.COLOR_RGB2HSV);
        Core.inRange(hsvMat, yellow.HSVMin, yellow.HSVMax, thresholdMat);
        morphOps(thresholdMat);
        trackingFinger(yellow, thresholdMat, hsvMat, rgbMat);

        // Change from Mat(rendered) to Texture
        Utils.matToTexture2D(outputMat, outputTexture, colors);
    }

    public Texture2D GetRTPixels(RenderTexture rt)
    {
        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set the supplied RenderTexture as the active one
        RenderTexture.active = rt;

        // Create a new Texture2D and read the RenderTexture image into it
        Texture2D tex = new Texture2D(rt.width, rt.height);

        // Read pixels one by one and add image to texture
        tex.ReadPixels(new UnityEngine.Rect(0, 0, tex.width, tex.height), 0, 0);

        // Restorie previously active render texture
        RenderTexture.active = currentActiveRT;
        return tex;
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

    private void trackingFinger(FingerColor fingerColor, Mat threshold, Mat hsv, Mat cameraFeed)
    {
        List<MatOfPoint> contours = new List<MatOfPoint>();
        Mat hierarchy = new Mat();

        /*** See more https://docs.opencv.org/3.3.1/d3/dc0/group__imgproc__shape.html ***/
        // findContours() change input image, we need it to find other finger later so we copy image to temp
        Mat temp = threshold.clone();
        Imgproc.findContours(temp, contours, hierarchy, Imgproc.RETR_EXTERNAL, Imgproc.CHAIN_APPROX_SIMPLE);

        FingerColor fingerObject = new FingerColor();
        double maxArea = 0;
        int contourIndex = -1;

        if (hierarchy.rows() > 0)
        {
            for (int index = 0; index >= 0; index = (int)hierarchy.get(0, index)[0])
            {
                Moments moment = Imgproc.moments(contours[index]);
                // (0, 0) is top-left
                double area = moment.get_m00();

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
                drawObject(fingerObject, contourIndex, outputMat, contours, hierarchy);
            else
            {
                /* Do something to fing finger again */
            }
        }
    }

    private void drawObject(FingerColor fingerObject, int contourIndex, Mat frame, List<MatOfPoint> contours, Mat hierarchy)
    {
        // Draw contours line
        Imgproc.drawContours(frame, contours, contourIndex, fingerObject.Color, 3, 8, hierarchy, int.MaxValue, new Point());

        // Draw center of contours(centroid)
        Imgproc.circle(frame, new Point(fingerObject.XPos, fingerObject.YPos), 5, fingerObject.Color);

        // Show postion of centroid
        Imgproc.putText(frame, fingerObject.XPos + " , " + fingerObject.YPos, new Point(fingerObject.XPos, fingerObject.YPos + 20), 1, 1, fingerObject.Color, 2);

        // Show type(Color) of contour
        Imgproc.putText(frame, fingerObject.Type, new Point(fingerObject.XPos, fingerObject.YPos - 20), 1, 2, fingerObject.Color, 2);
    }
}
