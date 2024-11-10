using System.Collections.Generic;
using UnityEngine;

public class DijkstraManager
{
    private static DijkstraManager instance;
    public static DijkstraManager GetInstance()
    {
        if (instance == null)
        {
            instance = new DijkstraManager();
        }
        return instance;
    }

    public DijkstraNode[,] mapNodes;
    public List<DijkstraNode> openList = new List<DijkstraNode>();

    public void InitMap(int width, int height, Vector2[] stops, int stopNum)
    {
        mapNodes = new DijkstraNode[width, height];

        // 初始化节点
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapNodes[x, y] = new DijkstraNode(x, y);
            }
        }

        // 设置随机阻挡
        for (int i = 0; i < stopNum; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            mapNodes[x, y].type = ENodeType.Stop;
        }

        // 设置固定阻挡
        foreach (var stop in stops)
        {
            mapNodes[(int)stop.x, (int)stop.y].type = ENodeType.Stop;
        }
    }

    public List<DijkstraNode> FindPath(Vector2 start, Vector2 end)
    {
        DijkstraNode startNode = mapNodes[(int)start.x, (int)start.y];
        DijkstraNode endNode = mapNodes[(int)end.x, (int)end.y];

        List<DijkstraNode> path = DijkstraAlgorithm.FindPath(mapNodes, startNode, endNode);
        openList = DijkstraAlgorithm.GetExploredNodes(); // 可用于显示搜索过程
        return path;
    }
}
