using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalDoor : MonoBehaviour
{
    // 是否接受传送
    public bool bAcceptTrans = true;
    // 允许被传送到的对象层（可在 inspector 设置）
    public LayerMask TransformTargetLayer;

    // 表示主相机（角色）的空物体，用于计算相对位置
    Transform m_MainCameraPos;
    // 主相机（角色）的 Transform
    Transform m_Camera;
    // 本门下表示门户相机的 Transform（通常是摄像机的 Transform）
    Transform m_ProtalCamera;
    // 用于传送位置的 Transform（空物体）
    Transform m_TargetPos;

    // 目标门（应为同类型的 PotalDoor，而非 GameObject）
    public PotalDoor TargetDoor;

    // 门摄像机渲染的纹理
    RenderTexture ProtalCameraTexture;
    // Start is called before the first frame update
    void Start()
    {
        // // 获取主相机（角色）的 Transform
        // m_Camera = Camera.main.transform;

        // // 创建一个空物体作为表示主相机的物体，并将其父物体设置为本门，这样它就会跟随门移动
        // GameObject mainCameraPosObj = new GameObject("MainCameraPos");
        // m_MainCameraPos = mainCameraPosObj.transform;
        // m_MainCameraPos.SetParent(transform);

        // // 获取门下表示门户相机的 Transform（通常是摄像机的 Transform）
        // m_ProtalCamera = this.GetComponentInChildren<Camera>().transform;

        // // 创建一个空物体作为传送位置，并将其父物体设置为目标门，这样它就会跟随目标门移动
        // GameObject targetPosObj = TargetDoor.transform.GetChild(2).gameObject;
        // m_TargetPos = targetPosObj.transform;
        // m_TargetPos.SetParent(TargetDoor.transform);

        m_TargetPos = this.transform.GetChild(2).gameObject.transform;
        // // 获取门底下的相机组件，设置渲染纹理
        // GetTexture();
    }

    // Update is called once per frame
    void Update()
    {
        // GetTexture();
        if (m_TargetPos ==null){
            // 创建一个空物体作为传送位置，并将其父物体设置为目标门，这样它就会跟随目标门移动
            // GameObject targetPosObj = TargetDoor.transform.GetChild(2).gameObject;
            // m_TargetPos = targetPosObj.transform;
            // m_TargetPos.SetParent(TargetDoor.transform);
            m_TargetPos = this.transform.GetChild(2).gameObject.transform;
        }
    }

    void GetTexture(){
        // 获取门底下的相机组件
        Camera cam = this.GetComponentInChildren<Camera>();
        
        // 根据屏幕尺寸建一张纹理
        ProtalCameraTexture = new RenderTexture(Screen.width, Screen.height, 32)
        {
            name = "ProtalCameraTargetTexture"
        };

        // 将门下相机的渲染目标设置为该纹理
        cam.targetTexture = ProtalCameraTexture;
        // 设置相机不要观察门本身
        cam.cullingMask &= ~(1 << gameObject.layer);
    }

    void setcamera(){
        // 将主相机（角色）的世界坐标和朝向赋予A门表示主相机的空物体
        m_MainCameraPos.position = m_Camera.position; 
        m_MainCameraPos.rotation = m_Camera.rotation;

        // 将A门下表示主相机的空物体相对于A门的相对位置，赋予B门的相机，使得B门相机相对于B的相对位置与主相机相对于A门的相对位置相同
        TargetDoor.m_ProtalCamera.localPosition = m_MainCameraPos.localPosition;
        TargetDoor.m_ProtalCamera.localRotation = m_MainCameraPos.localRotation;

    }
    void pass(){
        if( TargetDoor != null )
        {
            // 获取到门的渲染组件
            MeshRenderer renderer = TargetDoor.GetComponent<MeshRenderer>();
            foreach (var m in renderer.materials)
            {
                // 将本地门的材质纹理，设置为目标门摄像机的渲染纹理
                m.SetTexture("_MainTex", TargetDoor.ProtalCameraTexture);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 加一个接受传送的变量，只有在接受传送时，角色才能被传过去。
        // 另外，顺便判断一下目标物体是否允许被传送（通过层来判断）
        if (bAcceptTrans && (((1 << other.gameObject.layer) & TransformTargetLayer) != 0))
        {
            StartCoroutine(DelayDo());
            // 计算角色相对本地门的相对位置，保证传送到目标门后，与目标门的相对位置一致。
            m_TargetPos.position = other.transform.position;
            m_TargetPos.rotation = other.transform.rotation;
            TargetDoor.m_TargetPos.localPosition = m_TargetPos.localPosition;
            TargetDoor.m_TargetPos.localRotation = m_TargetPos.localRotation;

            // 如果是CharacterController组件控制的角色，先禁用一下，传送完了再打开
            // 如果你的代码不是用这个组件，可以删掉
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
                cc.enabled = false;
            other.transform.position = TargetDoor.m_TargetPos.position;
            other.transform.rotation = TargetDoor.m_TargetPos.rotation;
            if (cc != null)
                cc.enabled = true;
        }
    }
    IEnumerator DelayDo()
    {
        // 暂时关闭目标门传送触发
        TargetDoor.bAcceptTrans = false;
        Debug.Log("开始等待...");
        yield return new WaitForSeconds(2f); // 等待 2 秒
        Debug.Log("2 秒后执行这里");
        TargetDoor.bAcceptTrans = true;
    }

}
