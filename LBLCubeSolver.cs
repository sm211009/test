using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LBLCubeSolver : MonoBehaviour
{
    private readonly List<string> Moves = new List<string>
    {
        "U","D","A","L","F","B",
        "U2","D2","L2","R2","F2","B2",
        "U'","D'","L'","R'","F'","B'"
    };

    private CubeState cubeState;
    public static List<string> moveList = new List<string>();
    public static List<string> firstMoveList = new List<string>();
    private bool isProcessingMove = false;// 現在移動中かどうかのフラグ
    //private bool isCubeRotated = false; 

    private Renderer[] upRenderers = new Renderer[9];//白
    private Renderer[] downRenderers = new Renderer[9];//オレンジ
    private Renderer[] frontRenderers = new Renderer[9];//赤
    private Renderer[] leftRenderers = new Renderer[9];//緑
    private Renderer[] rightRenderers = new Renderer[9];//青
    private Renderer[] backRenderers = new Renderer[9];//黄色

    Color orange = new Color(1.0f, 0.5f, 0.0f);
    private readonly Color white = Color.white;
    private readonly Color red = Color.red;
    private readonly Color blue = Color.blue;
    private readonly Color green = Color.green;
    private readonly Color yellow = Color.yellow;

    void Start()
    {
        cubeState = FindObjectOfType<CubeState>(); // CubeStateのインスタンスを取得
    }

    public void OnSolveButtonPressed()
    {
        StartAutoSolve(); // 自動解決を開始
    }

    void Update()
    {
        if (moveList.Count > 0 && !isProcessingMove)
        {
            Debug.Log("MoveList Count: " + moveList.Count);
            Debug.Log("Current Move: " + moveList[0]);
            isProcessingMove = true;
            StartCoroutine(ProcessMove(moveList[0]));

        }
    }

    private IEnumerator ProcessMove(string move)
    {
        //isProcessingMove = true;

        DoMove(move);
        firstMoveList.Add(move);
        moveList.RemoveAt(0);

        // 回転が終わるのを待つ
        yield return new WaitForSeconds(1.0f); // 必要な時間を待つ

        isProcessingMove = false;
    }

    public void StartAutoSolve()
    {
        for (int i = 0; i < 9; i++)
        {
            upRenderers[i] = cubeState.up[i].GetComponent<Renderer>();
            downRenderers[i] = cubeState.down[i].GetComponent<Renderer>();
            frontRenderers[i] = cubeState.front[i].GetComponent<Renderer>();
            leftRenderers[i] = cubeState.left[i].GetComponent<Renderer>();
            rightRenderers[i] = cubeState.right[i].GetComponent<Renderer>();
            backRenderers[i] = cubeState.back[i].GetComponent<Renderer>();
        }
        moveList.Clear(); // 前回の動きをクリア
        SolveCrossLayer();
        //SolveSecondLayer();
        //SolveLastLayer();
    }

    private void SolveCrossLayer()
    {
        //while (backRenderers[1].material.color != white && backRenderers[3].material.color != white &&
        //       backRenderers[5].material.color != white && backRenderers[7].material.color != white)
        //{
        //while (backRenderers[7].material.color != white)
        //{
        //    if (frontRenderers[5].material.color == white)
        //    {
        //        moveList.Add("R'");
        //    }
        //    if (downRenderers[1].material.color == white)
        //    {
        //        moveList.Add("D'");
        //        moveList.Add("R");
        //    }
        //    if (downRenderers[3].material.color == white)
        //    {
        //        moveList.Add("R'");
        //    }
        //    if (downRenderers[7].material.color == white)
        //    {
        //        moveList.Add("D");
        //        moveList.Add("R");
        //    }
        //    if (upRenderers[5].material.color == white)
        //    {
        //        moveList.Add("R2");
        //    }

        //}

        //白エッジが上側にある場合
        if (frontRenderers[1].material.color == white)
        {
            moveList.Add("F'");
            moveList.Add("L");
        }
        if (leftRenderers[1].material.color == white)
        {
            moveList.Add("L'");
            moveList.Add("D");
        }
        if (rightRenderers[1].material.color == white)
        {
            moveList.Add("R'");
            moveList.Add("F");
        }
        if (downRenderers[1].material.color == white)
        {
            moveList.Add("D'");
            moveList.Add("R");
        }

        //白エッジが下側にある場合
        if (frontRenderers[7].material.color == white)
        {
            moveList.Add("F");
            moveList.Add("L");
        }
        if (leftRenderers[7].material.color == white)
        {
            moveList.Add("L");
            moveList.Add("D");
        }
        if (rightRenderers[7].material.color == white)
        {
            moveList.Add("R");
            moveList.Add("F");
        }
        if (downRenderers[7].material.color == white)
        {
            moveList.Add("D");
            moveList.Add("R");
        }

        //白エッジが真ん中の段の右側にある場合
        if (frontRenderers[5].material.color == white)
        {
            moveList.Add("R'");
        }
        if (leftRenderers[5].material.color == white)
        {
            moveList.Add("F'");
        }
        if (rightRenderers[5].material.color == white)
        {
            moveList.Add("D'");
        }
        if (downRenderers[5].material.color == white)
        {
            moveList.Add("L'");//ばつ
        }


        ////白エッジが真ん中の段の左側にある場合
        if (frontRenderers[3].material.color == white)
        {
            moveList.Add("L");
        }
        if (leftRenderers[3].material.color == white)
        {
            moveList.Add("D");
        }
        if (rightRenderers[3].material.color == white)
        {
            moveList.Add("F");
        }
        if (downRenderers[3].material.color == white)
        {
            moveList.Add("R'");
        }

        //白エッジが下側にある場合
        if (frontRenderers[7].material.color == white)
        {
            moveList.Add("F");
            moveList.Add("L");
        }
        if (leftRenderers[7].material.color == white)
        {
            moveList.Add("L");
            moveList.Add("D");
        }
        if (rightRenderers[7].material.color == white)
        {
            moveList.Add("R");
            moveList.Add("F");
        }
        if (downRenderers[7].material.color == white)
        {
            moveList.Add("D");
            moveList.Add("R");
        }

        ////白エッジが下の面にある場合(白の面)
        if (upRenderers[1].material.color == white)
        {
            moveList.Add("D2");
        }
        if (upRenderers[3].material.color == white)
        {
            moveList.Add("L2");
        }
        if (upRenderers[5].material.color == white)
        {
            moveList.Add("R2");
        }
        if (upRenderers[7].material.color == white)
        {
            moveList.Add("F2");
        }
        Debug.Log("Current moveList: " + string.Join(", ", moveList));
        //}
    }




    void DoMove(string move)
    {
        // 回転命令を実行
        //isProcessingMove = true;
        //CubeState.autoRotating = true;
        switch (move)
        {
            case "U": RotateSide(cubeState.up, -90); break;
            case "U'": RotateSide(cubeState.up, 90); break;
            case "U2": RotateSide(cubeState.up, -180); break;

            case "D": RotateSide(cubeState.down, -90); break;
            case "D'": RotateSide(cubeState.down, 90); break;
            case "D2": RotateSide(cubeState.down, -180); break;

            case "L": RotateSide(cubeState.left, -90); break;
            case "L'": RotateSide(cubeState.left, 90); break;
            case "L2": RotateSide(cubeState.left, -180); break;

            case "R": RotateSide(cubeState.right, -90); break;
            case "R'": RotateSide(cubeState.right, 90); break;
            case "R2": RotateSide(cubeState.right, -180); break;

            case "F": RotateSide(cubeState.front, -90); break;
            case "F'": RotateSide(cubeState.front, 90); break;
            case "F2": RotateSide(cubeState.front, -180); break;

            case "B": RotateSide(cubeState.back, -90); break;
            case "B'": RotateSide(cubeState.back, 90); break;
            case "B2": RotateSide(cubeState.back, -180); break;
        }

    }

    void RotateSide(List<GameObject> side, float angle)
    {
        //角度だけ自動的に側面を回転させる
        Debug.Log("Rotating side by angle: " + angle);
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);
    }
}
