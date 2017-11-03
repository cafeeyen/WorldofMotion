using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAxisLine : MonoBehaviour
{
    public Material AxisX, AxisY, AxisZ;

    private void DrawAxis()
    {
        DrawLine(AxisX, new Vector3(-2000, 0, 0), new Vector3(2000, 0, 0));
        DrawLine(AxisY, new Vector3(0, -2000, 0), new Vector3(0, 2000, 0));
        DrawLine(AxisZ, new Vector3(0, 0, -2000), new Vector3(0, 0, 2000));
    }

    private void DrawLine(Material colorMat, Vector3 start, Vector3 end)
    {
        colorMat.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(new Color(colorMat.color.r, colorMat.color.g, colorMat.color.b, colorMat.color.a));
        GL.Vertex3(start.x, start.y, start.z);
        GL.Vertex3(end.x, end.y, end.z);
        GL.End();
    }

    // Show line in application
    private void OnPostRender()
    {
        DrawAxis();
    }

    // Show line in Unity editor mode
    private void OnDrawGizmos()
    {
        DrawAxis();
    }

}
