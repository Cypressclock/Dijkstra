using System.Collections.Generic;
using UnityEngine;

public static class DijkstraAlgorithm
{
    private static List<DijkstraNode> exploredNodes = new List<DijkstraNode>();

    public static List<DijkstraNode> FindPath(DijkstraNode[,] mapNodes, DijkstraNode startNode, DijkstraNode endNode)
    {
        // 重置所有节点
        foreach (var node in mapNodes)
        {
            node.Reset();
        }

        // 初始化
        exploredNodes.Clear();
        startNode.distance = 0;

        // 使用优先队列，确保每次总是选取到目前距离最短的节点
        var priorityQueue = new SortedSet<DijkstraNode>(Comparer<DijkstraNode>.Create((a, b) =>
            a.distance == b.distance ? (a.x == b.x ? a.y.CompareTo(b.y) : a.x.CompareTo(b.x)) : a.distance.CompareTo(b.distance)
        ));
        priorityQueue.Add(startNode);

        while (priorityQueue.Count > 0)
        {
            // 从优先队列中选取距离起点最近的节点
            var currentNode = priorityQueue.Min;
            priorityQueue.Remove(currentNode);
            exploredNodes.Add(currentNode);

            // 如果找到终点，则退出
            if (currentNode == endNode)
            {
                return ConstructPath(endNode);
            }

            // 遍历当前节点的邻居
            foreach (var neighbor in GetNeighbors(mapNodes, currentNode))
            {
                if (neighbor.type == ENodeType.Stop || neighbor.isVisited)
                    continue;

                // 计算从当前节点到邻居节点的距离
                int newDist = currentNode.distance + 1; // 假设所有相邻节点的距离为 1

                if (newDist < neighbor.distance)
                {
                    priorityQueue.Remove(neighbor); // 更新优先队列中的邻居节点
                    neighbor.distance = newDist;
                    neighbor.previous = currentNode;
                    priorityQueue.Add(neighbor);
                }
            }

            currentNode.isVisited = true;
        }

        // 如果没有找到路径，返回空
        return null;
    }

    // 获取邻居节点
    private static IEnumerable<DijkstraNode> GetNeighbors(DijkstraNode[,] mapNodes, DijkstraNode node)
    {
        int x = node.x;
        int y = node.y;

        if (x > 0) yield return mapNodes[x - 1, y];
        if (x < mapNodes.GetLength(0) - 1) yield return mapNodes[x + 1, y];
        if (y > 0) yield return mapNodes[x, y - 1];
        if (y < mapNodes.GetLength(1) - 1) yield return mapNodes[x, y + 1];
    }

    // 重建路径
    private static List<DijkstraNode> ConstructPath(DijkstraNode endNode)
    {
        List<DijkstraNode> path = new List<DijkstraNode>();
        for (DijkstraNode node = endNode; node != null; node = node.previous)
        {
            path.Add(node);
        }
        path.Reverse(); // 从起点到终点
        return path;
    }

    // 获取已探索节点（用于可视化）
    public static List<DijkstraNode> GetExploredNodes()
    {
        return exploredNodes;
    }
}
