using System.Collections.Generic;
using UnityEngine;

public static class DijkstraAlgorithm
{
    private static List<DijkstraNode> exploredNodes = new List<DijkstraNode>();

    public static List<DijkstraNode> FindPath(DijkstraNode[,] mapNodes, DijkstraNode startNode, DijkstraNode endNode)
    {
        // �������нڵ�
        foreach (var node in mapNodes)
        {
            node.Reset();
        }

        // ��ʼ��
        exploredNodes.Clear();
        startNode.distance = 0;

        // ʹ�����ȶ��У�ȷ��ÿ������ѡȡ��Ŀǰ������̵Ľڵ�
        var priorityQueue = new SortedSet<DijkstraNode>(Comparer<DijkstraNode>.Create((a, b) =>
            a.distance == b.distance ? (a.x == b.x ? a.y.CompareTo(b.y) : a.x.CompareTo(b.x)) : a.distance.CompareTo(b.distance)
        ));
        priorityQueue.Add(startNode);

        while (priorityQueue.Count > 0)
        {
            // �����ȶ�����ѡȡ�����������Ľڵ�
            var currentNode = priorityQueue.Min;
            priorityQueue.Remove(currentNode);
            exploredNodes.Add(currentNode);

            // ����ҵ��յ㣬���˳�
            if (currentNode == endNode)
            {
                return ConstructPath(endNode);
            }

            // ������ǰ�ڵ���ھ�
            foreach (var neighbor in GetNeighbors(mapNodes, currentNode))
            {
                if (neighbor.type == ENodeType.Stop || neighbor.isVisited)
                    continue;

                // ����ӵ�ǰ�ڵ㵽�ھӽڵ�ľ���
                int newDist = currentNode.distance + 1; // �����������ڽڵ�ľ���Ϊ 1

                if (newDist < neighbor.distance)
                {
                    priorityQueue.Remove(neighbor); // �������ȶ����е��ھӽڵ�
                    neighbor.distance = newDist;
                    neighbor.previous = currentNode;
                    priorityQueue.Add(neighbor);
                }
            }

            currentNode.isVisited = true;
        }

        // ���û���ҵ�·�������ؿ�
        return null;
    }

    // ��ȡ�ھӽڵ�
    private static IEnumerable<DijkstraNode> GetNeighbors(DijkstraNode[,] mapNodes, DijkstraNode node)
    {
        int x = node.x;
        int y = node.y;

        if (x > 0) yield return mapNodes[x - 1, y];
        if (x < mapNodes.GetLength(0) - 1) yield return mapNodes[x + 1, y];
        if (y > 0) yield return mapNodes[x, y - 1];
        if (y < mapNodes.GetLength(1) - 1) yield return mapNodes[x, y + 1];
    }

    // �ؽ�·��
    private static List<DijkstraNode> ConstructPath(DijkstraNode endNode)
    {
        List<DijkstraNode> path = new List<DijkstraNode>();
        for (DijkstraNode node = endNode; node != null; node = node.previous)
        {
            path.Add(node);
        }
        path.Reverse(); // ����㵽�յ�
        return path;
    }

    // ��ȡ��̽���ڵ㣨���ڿ��ӻ���
    public static List<DijkstraNode> GetExploredNodes()
    {
        return exploredNodes;
    }
}
