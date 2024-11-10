using UnityEngine;

public enum ENodeType
{
    Normal,   // 普通节点
    Stop      // 障碍节点
}

public class DijkstraNode
{
    public int x;                   // 节点的X坐标
    public int y;                   // 节点的Y坐标
    public int distance = int.MaxValue; // 从起点到当前节点的距离，初始为无限大
    public DijkstraNode previous;   // 前驱节点，用于重建路径
    public ENodeType type;          // 节点类型（普通或障碍）
    public bool isVisited;          // 是否已访问过

    public DijkstraNode(int x, int y, ENodeType type = ENodeType.Normal)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

    // 重置节点属性
    public void Reset()
    {
        distance = int.MaxValue;
        previous = null;
        isVisited = false;
    }
}
