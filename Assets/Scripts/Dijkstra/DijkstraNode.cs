using UnityEngine;

public enum ENodeType
{
    Normal,   // ��ͨ�ڵ�
    Stop      // �ϰ��ڵ�
}

public class DijkstraNode
{
    public int x;                   // �ڵ��X����
    public int y;                   // �ڵ��Y����
    public int distance = int.MaxValue; // ����㵽��ǰ�ڵ�ľ��룬��ʼΪ���޴�
    public DijkstraNode previous;   // ǰ���ڵ㣬�����ؽ�·��
    public ENodeType type;          // �ڵ����ͣ���ͨ���ϰ���
    public bool isVisited;          // �Ƿ��ѷ��ʹ�

    public DijkstraNode(int x, int y, ENodeType type = ENodeType.Normal)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }

    // ���ýڵ�����
    public void Reset()
    {
        distance = int.MaxValue;
        previous = null;
        isVisited = false;
    }
}
