using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DrawPastWay : MonoBehaviour
{
    public GameObject player;
    public Material defaultMaterial;

    private LineRenderer curLine;
    private int positionCount = 2;
    private Vector2 PrevPos = Vector2.zero;

    private LineRenderer lineRenderer;
    private Vector3[] positions;
    private Light2D light2D;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        positions = new Vector3[0];
        light2D = GetComponent<Light2D>();

        // Set the LineRenderer material to Sprite-Lit-Default
        Material material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = material;

        lineRenderer.startColor = new Color(1f, 1f, 1f, 0f);
        lineRenderer.endColor = new Color(1f, 1f, 1f, 0f);
    }


    void Update()
    {
        Vector3 position = transform.position;
        Array.Resize(ref positions, positions.Length + 1);
        positions[positions.Length - 1] = position;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
    /*private void Draw()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

        createLine(playerPos);
        connectLine(playerPos);
    }
    private void createLine(Vector2 playerPos)
    {
        positionCount = 2;
        GameObject line = new GameObject("Line");
        LineRenderer lineRend = line.AddComponent<LineRenderer>();

        line.transform.parent = player.transform;
        line.transform.position = playerPos;
        lineRend.numCapVertices = 1;
        lineRend.numCornerVertices = 1;
        lineRend.startWidth = 2f;
        lineRend.endWidth = 2f;
        lineRend.material = defaultMaterial;
        lineRend.SetPosition(0, playerPos);
        lineRend.SetPosition(1, playerPos);

        curLine = lineRend;
    }
    private void connectLine(Vector2 playerPos)
    {
        if (PrevPos != null && Mathf.Abs(Vector2.Distance(PrevPos, playerPos)) >= 0.01f)
        {
            PrevPos = playerPos;
            positionCount++;
            curLine.positionCount = positionCount;
            curLine.SetPosition(positionCount - 1, playerPos);
        }
    }*/
}
