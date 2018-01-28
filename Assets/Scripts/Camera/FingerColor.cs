using OpenCVForUnity;

public class FingerColor
{
    int xPos, yPos;
    string type;
    Scalar HSVmin, HSVmax;
    Scalar color;

    public FingerColor()
    {
        Type = "Object";
        color = new Scalar(0, 0, 0);
    }

    public FingerColor(string name)
    {
        if(name == "blue")
        {
            Type = name;
            HSVMin = new Scalar(92, 124, 123);
            HSVMax = new Scalar(124, 256, 256);
            // RGB
            Color = new Scalar(0, 0, 255);
        }
        if(name == "yellow")
        {
            Type = name;
            HSVMin = new Scalar(20, 124, 123);
            HSVMax = new Scalar(30, 255, 255);
            // RGB
            Color = new Scalar(255, 255, 0);
        }
    }

    public int XPos { get { return xPos; } set { xPos = value; } }
    public int YPos { get { return yPos; } set { yPos = value; } }
    public string Type { get { return type; } set { type = value; } }
    public Scalar HSVMin { get { return HSVmin; } set { HSVmin = value; } }
    public Scalar HSVMax { get { return HSVmax; } set { HSVmax = value; } }
    public Scalar Color { get { return color; } set { color = value; } }

}
