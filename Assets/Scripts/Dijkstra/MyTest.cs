using System.Collections.Generic;
using UnityEngine;

public class MyTest : MonoBehaviour
{
    public Color normalColor, stopColor, pathColor, findColor, startColor, endColor;
    public int mapW = 10;
    public int mapH = 10;
    public float cubeOffset = 0.1f;
    public int stopNum = 20;
    public Vector2[] stops;

    private Dictionary<string, GameObject> goDic;
    private Vector2 mousePos = Vector2.one * -1;
    private List<DijkstraNode> pathList;
    private bool setStart = false;
    private int step = 0;
    [SerializeField] private bool openStep;

    void Start()
    {
        DijkstraManager.GetInstance().InitMap(mapW, mapH, stops, stopNum);
        StartCoroutine(CreateCube());
    }

    IEnumerator<GameObject> CreateCube()
    {
        goDic = new Dictionary<string, GameObject>();
        var nodes = DijkstraManager.GetInstance().mapNodes;

        for (int i = 0; i < mapW; i++)
        {
            for (int j = 0; j < mapH; j++)
            {
                if (j % 6 == 0)
                    yield return null;

                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = new Vector3(i + cubeOffset * i, 0, j + cubeOffset * j);
                go.name = i + "_" + j;
                goDic.Add(go.name, go);

                var node = nodes[i, j];
                go.GetComponent<MeshRenderer>().material.color = node.type == ENodeType.Stop ? stopColor : normalColor;
            }
        }
    }

    public void ShowNextStep()
    {
        var openList = DijkstraManager.GetInstance().openList;
        if (step >= openList.Count)
        {
            DisplayPath();
            return;
        }

        var node = openList[step];
        var goName = node.x + "_" + node.y;
        var pathGo = goDic[goName];
        pathGo.GetComponent<MeshRenderer>().material.color = findColor;

        step++;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                var go = hit.collider.gameObject;
                string[] names = go.name.Split('_');
                int x = int.Parse(names[0]);
                int y = int.Parse(names[1]);
                var pos = new Vector2(x, y);

                if (!setStart)
                {
                    ClearMap();
                    mousePos = pos;
                    setStart = true;
                }
                else
                {
                    pathList = DijkstraManager.GetInstance().FindPath(mousePos, pos);
                    step = 0;
                    DisplayPath();
                    setStart = false;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ClearMap();
        }
        else if (openStep && Input.GetKeyUp(KeyCode.Space))
        {
            ShowNextStep();
        }
    }

    private void DisplayPath()
    {
        if (pathList == null) return;

        foreach (var node in pathList)
        {
            var goName = node.x + "_" + node.y;
            goDic[goName].GetComponent<MeshRenderer>().material.color = pathColor;
        }
    }

    private void ClearMap()
    {
        if (pathList != null)
        {
            foreach (var node in pathList)
            {
                var goName = node.x + "_" + node.y;
                goDic[goName].GetComponent<MeshRenderer>().material.color = normalColor;
            }
            pathList = null;
        }

        var openList = DijkstraManager.GetInstance().openList;
        foreach (var node in openList)
        {
            var goName = node.x + "_" + node.y;
            goDic[goName].GetComponent<MeshRenderer>().material.color = normalColor;
        }
        openList.Clear();
    }
}

