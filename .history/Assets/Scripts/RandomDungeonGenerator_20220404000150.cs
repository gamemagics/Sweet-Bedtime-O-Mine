﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomDungeonGenerator : MonoBehaviour
{

    enum wallIndex
    {
        topLeft = 0,
        top = 1,
        topRight = 5,
        left = 6,
        right = 7,
        bottomLeft = 8,
        bottom = 9,
        bottomRight = 10,
        cornerTopLeft = 11,
        cornerTopRight = 12,
        cornerBottomLeft = 13,
        cornerBottomRight = 14

    }
    [SerializeField]
    private Tile[] groundTile;
    [SerializeField]
    private Tile[] pitTile;
    [SerializeField]
    private Tile[] wallTile;
    [SerializeField]
    private Tilemap groundMap;
    [SerializeField]
    private Tilemap pitMap;
    [SerializeField]
    private Tilemap wallMap;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private int deviationRate = 10;
    [SerializeField]
    private int roomRate = 15;
    [SerializeField]
    private int maxRouteLength;
    [SerializeField]
    private int maxRoutes = 15;


    private int routeCount = 0;

    private void Start()
    {
        int x = 0;
        int y = 0;
        int routeLength = 0;
        GenerateSquare(x, y, 1);
        Vector2Int previousPos = new Vector2Int(x, y);
        y += 3;
        GenerateSquare(x, y, 1);
        NewRoute(x, y, routeLength, previousPos);

        SetTiles();
    }

    private void SetTiles()
    {
        BoundsInt bounds = groundMap.cellBounds;
        for (int xMap = bounds.xMin; xMap <= bounds.xMax; xMap++)
        {
            for (int yMap = bounds.yMin; yMap <= bounds.yMax; yMap++)
            {
                Vector3Int pos = new Vector3Int(xMap, yMap, 0);
                Vector3Int posBelow = new Vector3Int(xMap, yMap - 1, 0);
                Vector3Int posAbove = new Vector3Int(xMap, yMap + 1, 0);
                Vector3Int posLeft = new Vector3Int(xMap - 1, yMap, 0);
                Vector3Int posRight = new Vector3Int(xMap + 1, yMap, 0);
                Vector3Int posBottomLeft = new Vector3Int(xMap - 1, yMap - 1, 0);
                Vector3Int posBottomRight = new Vector3Int(xMap + 1, yMap - 1, 0);
                Vector3Int posTopLeft = new Vector3Int(xMap - 1, yMap + 1, 0);
                Vector3Int posTopRight = new Vector3Int(xMap + 1, yMap + 1, 0);
                TileBase tile = groundMap.GetTile(pos);
                TileBase tileBelow = groundMap.GetTile(posBelow);
                TileBase tileAbove = groundMap.GetTile(posAbove);
                TileBase tileLeft = groundMap.GetTile(posLeft);
                TileBase tileRight = groundMap.GetTile(posRight);
                TileBase tileBottomLeft = groundMap.GetTile(posBottomLeft);
                TileBase tileBottomRight = groundMap.GetTile(posBottomRight);
                TileBase tileTopLeft = groundMap.GetTile(posTopLeft);
                TileBase tileTopRight = groundMap.GetTile(posTopRight);
                if (tile != null)
                {
                    //corner top left
                    if (tileTopLeft == null && tileLeft != null && tileAbove != null)
                    {
                        wallMap.SetTile(posTopLeft, wallTile[(int)wallIndex.cornerTopLeft]);
                    }
                    //corner top right
                    else if (tileTopRight == null && tileRight != null && tileAbove != null)
                    {
                        wallMap.SetTile(posTopRight, wallTile[(int)wallIndex.cornerTopRight]);
                    }
                    //corner bottom left
                    else if (tileBottomLeft == null && tileLeft != null && tileBelow != null)
                    {
                        wallMap.SetTile(posBottomLeft, wallTile[(int)wallIndex.cornerBottomLeft]);
                    }
                    //corner bottom right
                    else if (tileBottomRight == null && tileRight != null && tileBelow != null)
                    {
                        wallMap.SetTile(posBottomRight, wallTile[(int)wallIndex.cornerBottomRight]);
                    }

                    //bottom tiles
                    if (tileBelow == null && tileAbove != null)
                    {
                        //bottom left tile
                        if (tileLeft == null && tileRight != null)
                        {
                            wallMap.SetTile(posBottomLeft, wallTile[(int)wallIndex.bottomLeft]);
                        }
                        //bottom right tile
                        else if (tileRight == null && tileLeft != null)
                        {
                            wallMap.SetTile(posBottomRight, wallTile[(int)wallIndex.bottomRight]);

                        }
                        //bottom tile
                        wallMap.SetTile(posBelow, wallTile[(int)wallIndex.bottom]);
                    }
                    //top tiles
                    else if (tileAbove == null && tileBelow != null)
                    {
                        //top left tile
                        if (tileLeft == null && tileRight != null)
                        {
                            wallMap.SetTile(posTopLeft, wallTile[(int)wallIndex.topLeft]);
                        }
                        //top right tile
                        else if (tileRight == null && tileLeft != null)
                        {
                            wallMap.SetTile(posTopRight, wallTile[(int)wallIndex.topRight]);
                        }
                        //top tile
                        if (Random.Range(1, 100) >= deviationRate)
                        {
                            wallMap.SetTile(posAbove, wallTile[(int)wallIndex.top]);
                        }
                        else
                        {
                            wallMap.SetTile(posAbove, wallTile[Random.Range((int)wallIndex.top + 1, (int)wallIndex.topRight - 1)]);
                        }
                    }
                    //left tiles
                    if (tileLeft == null && tileRight != null)
                    {
                        wallMap.SetTile(posLeft, wallTile[(int)wallIndex.left]);
                    }
                    //right tiles
                    else if (tileRight == null && tileLeft != null)
                    {
                        wallMap.SetTile(posRight, wallTile[(int)wallIndex.right]);
                    }
                    // pitMap.SetTile(pos, pitTile);
                }
            }
        }
    }

    private void NewRoute(int x, int y, int routeLength, Vector2Int previousPos)
    {
        if (routeCount < maxRoutes)
        {
            routeCount++;
            while (++routeLength < maxRouteLength)
            {
                //Initialize
                bool routeUsed = false;
                int xOffset = x - previousPos.x;
                int yOffset = y - previousPos.y;
                int roomSize = 1; //Hallway size
                                  // if (Random.Range(1, 100) <= roomRate)
                                  // roomSize = Random.Range(3, 6);
                previousPos = new Vector2Int(x, y);

                //Go Straight
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        GenerateSquare(previousPos.x + xOffset, previousPos.y + yOffset, roomSize);
                        NewRoute(previousPos.x + xOffset, previousPos.y + yOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                    }
                    else
                    {
                        x = previousPos.x + xOffset;
                        y = previousPos.y + yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                    }
                }

                //Go left
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        GenerateSquare(previousPos.x - yOffset, previousPos.y + xOffset, roomSize);
                        NewRoute(previousPos.x - yOffset, previousPos.y + xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                    }
                    else
                    {
                        y = previousPos.y + xOffset;
                        x = previousPos.x - yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                    }
                }
                //Go right
                if (Random.Range(1, 100) <= deviationRate)
                {
                    if (routeUsed)
                    {
                        GenerateSquare(previousPos.x + yOffset, previousPos.y - xOffset, roomSize);
                        NewRoute(previousPos.x + yOffset, previousPos.y - xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                    }
                    else
                    {
                        y = previousPos.y - xOffset;
                        x = previousPos.x + yOffset;
                        GenerateSquare(x, y, roomSize);
                        routeUsed = true;
                    }
                }

                if (!routeUsed)
                {
                    x = previousPos.x + xOffset;
                    y = previousPos.y + yOffset;
                    GenerateSquare(x, y, roomSize);
                }
            }
        }
    }

    private void GenerateSquare(int x, int y, int radius)
    {
        for (int tileX = x - radius; tileX <= x + radius; tileX++)
        {
            for (int tileY = y - radius; tileY <= y + radius; tileY++)
            {
                Vector3Int tilePos = new Vector3Int(tileX, tileY, 0);
                if (Random.Range(1, 100) >= deviationRate)
                {
                    groundMap.SetTile(tilePos, groundTile[0]);
                }
                else
                {
                    groundMap.SetTile(tilePos, groundTile[Random.Range(1, groundTile.Length)]);
                }
            }
        }
    }
}