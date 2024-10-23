using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;
    private bool autoRotating = false;
    private float sensitivity = 0.4f;
    private float speed = 300f;
    private Vector3 rotation;

    private Quaternion targrtQuaternion;

    private ReadCube readCube;
    private CubeState cubeState;


    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {

        if (dragging)
        {
            SpinSide(activeSide);
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                RotateToRightAngle();
            }
        }
        if (autoRotating)
        {
            AutoRotate();
        }
    }

    private void SpinSide(List<GameObject> side)
    {
        //回転をリセットする
        rotation = Vector3.zero;

        //現在のマウスの位置から最後のマウス位置を引いた値
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);

        if (side == cubeState.front)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.down)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.back)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        //回転
        transform.Rotate(rotation, Space.Self);

        //マウスの参照
        mouseRef = Input.mousePosition;


    }

    //回転時に1回呼び出されるメゾット
    public void Rotate(List<GameObject> side)
    {
        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;
        //回転するベクトルを作成する
        //x,y,zのローカル前方で0であることを確認した立方体の中心に基づいて回転するベクトルを作成する

        //回転が開始する方法
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
    }

    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);
        Vector3 localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        targrtQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
        activeSide = side;
        autoRotating = true;
    }

    //自動正しい位置に回転させる
    public void RotateToRightAngle()
    {
        Vector3 vec = transform.localEulerAngles;
        //vecを最も近い90度に止める
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targrtQuaternion.eulerAngles = vec;
        autoRotating = true;
    }

    private void AutoRotate()
    {
        dragging = false;
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targrtQuaternion, step);

        //一度以内の場合は、目標の角度を設定し、回転を終了する
        if (Quaternion.Angle(transform.localRotation, targrtQuaternion) <= 1)
        {
            transform.localRotation = targrtQuaternion;
            //小さなキューブの親を外す
            cubeState.PutDown(activeSide, transform.parent);
            readCube.ReadState();
            CubeState.autoRotating = false;
            autoRotating = false;
            dragging = false;
            
        }
    }

}
