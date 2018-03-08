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
    }

    public FingerColor(string name)
    {
        if(name == "blue")
        {
            Type = name;
            HSVMin = new Scalar(92, 50, 0);
            HSVMax = new Scalar(124, 256, 256);
        }
        if(name == "yellow")
        {
            Type = name;
            HSVMin = new Scalar(20, 124, 124);
            HSVMax = new Scalar(30, 256, 256);
        }
    }

    public int XPos { get { return xPos; } set { xPos = value; } }
    public int YPos { get { return yPos; } set { yPos = value; } }
    public string Type { get { return type; } set { type = value; } }
    public Scalar HSVMin { get { return HSVmin; } set { HSVmin = value; } }
    public Scalar HSVMax { get { return HSVmax; } set { HSVmax = value; } }
    public Scalar Color { get { return color; } set { color = value; } }

}
