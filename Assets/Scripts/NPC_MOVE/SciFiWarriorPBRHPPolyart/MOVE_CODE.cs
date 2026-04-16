using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(�������))]�������ǣ�
//Ϊ��ǰ���ظýű�����Ϸ����������Ҫ�����(�����ڱ�������)
//�ò�������Ҫ�������������ռ�
[RequireComponent(typeof(CharacterController))]

//���벥�Ŷ��������
[RequireComponent(typeof(Animator))]

public class MOVE_CODE : MonoBehaviour
{ 
    CharacterController controller;
    Animator animator;

    //�����ƶ��ٶȣ������е��ڣ�
    public float MoveSpeed;
    public float RotatSpeed;

    //��ȡ����ˮƽ����ʹ�ֱ����ֵ�ã�GetAxis()������
    public float horizontal;
    public float vertical;

    //���ڸı䶯��״̬�ı���
    public int move_var;
    public int move_dir;

    //Ŀ�곯��
    public Vector3 target_dir = Vector3.zero;  //��ʼ��Ϊ(0,0,0)�������е���
    public Vector3 now_dir = Vector3.zero;

    //���������
    public enum MouseState
    {
        None,
        MidMouseBtn,
        LeftMouseBtn
    }

    private MouseState mMouseState = MouseState.None;
    private Camera mCamera;

    // Start is called before the first frame update
    void Start()
    {
        //��ȡ����ɫ������������������Ҫ����������ײ�Ȳ������ͱ�������������
        controller = GetComponent<CharacterController>();

        //��ȡ�����������������
        animator = GetComponent<Animator>();

        //��ʼ�������ƶ��ٶ�
        MoveSpeed = 0;
        RotatSpeed = 1;

        //��ʼ������״̬����Ϊ0�����ﾲֹ�������ţ�
        move_var = 0;

        //У׼����ɫ���������Ľ��������(������ײ���)
        controller.center = new Vector3(0, 1, 0);
        controller.radius = 0.5f;
        controller.height = 2;

        //���������
        //Cursor.visible = false;
        mCamera = transform.Find("Player_Camera").GetComponent<Camera>();
        if (mCamera == null)
        {
            Debug.LogError(GetType() + "camera Get Error ����");
        }

        GetDefaultFov();
    }

    bool Mouse_Staus_Hide = true;
    bool Mouse_Staus_can_change = true;
    // Update is called once per frame
    void Update()
    {
        HandControl_Move();

        CameraRotate();

        CameraFOV();

        CameraMove();

        CameraLeft();

        Mouse_Hide();
    }

    public void Mouse_Hide()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Mouse_Staus_can_change)
        {
            Mouse_Staus_can_change = false;
            if (Mouse_Staus_Hide)
            {
                Cursor.lockState = CursorLockMode.Locked;//鼠标锁定并隐藏
                StartCoroutine(MouseHideBreak());
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;//鼠标解除锁定并显形
                StartCoroutine(MouseHideBreak());
            }
            Mouse_Staus_Hide = !Mouse_Staus_Hide;
        }
    }

    IEnumerator MouseHideBreak()
    {
        yield return new WaitForSeconds(1f);
        Mouse_Staus_can_change = true;
    }

    public void HandControl_Move()
    {
        //GetAxis("Horizontal");��Ӧ���Ǽ����ϵ�A��D����ˮƽ����
        horizontal = Input.GetAxis("Horizontal");
        //GetAxis("Vertical");��Ӧ���Ǽ����ϵ�W��S������ֱ����    
        vertical = Input.GetAxis("Vertical");

        //ע��������ġ�ˮƽ��ֱ������ӳ��ļ�λʵ���Ͽ��Ը��ģ�
        //����ֻ������Ĭ�Ϲ���,���ǵ����ֵ��Ϊ1
        if (horizontal != 0 || vertical != 0) //����(Ĭ�ϣ�WASD)����ʱ�ͽ����ж�
        {
            //ǰ��
            if (Input.GetKey(KeyCode.W))
            {
                move_var = 1;
                move_dir = 0;
                MoveSpeed = 1.5f;
                //transform.rotation = Quaternion.LookRotation(target_dir);
                //Quaternion.LookRotation()��
                //����һ������ֵʹ���峯����������
                //ʹ���峯����һ������ֻ��Ҫ������������Position֮���Vector3��ֵ����


                //�����ж�
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
                {
                    move_var = 2;
                    MoveSpeed = 3.5f;
                }

            }

            //�����
            else if (Input.GetKey(KeyCode.S))
            {
                move_var = 1;
                move_dir = 3;
                MoveSpeed = 1.5f;
                //transform.rotation = Quaternion.LookRotation(target_dir);

                //�����ж�
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S))
                {
                    move_var = 2;
                    MoveSpeed = 3.5f;
                }
            }

            //������
            else if (Input.GetKey(KeyCode.A))
            {
                move_var = 1;
                move_dir = 1;
                MoveSpeed = 1.5f;
                //transform.rotation = Quaternion.LookRotation(target_dir);

                //�����ж�
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A))
                {
                    move_var = 2;
                    MoveSpeed = 3.5f;
                }
                //transform.Translate(Vector3.forward * Time.deltaTime);
            }

            //������
            else if (Input.GetKey(KeyCode.D))
            {
                move_var = 1;
                move_dir = 2;
                MoveSpeed = 1.5f;
                //transform.rotation = Quaternion.LookRotation(target_dir);

                //�����ж�
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D))
                {
                    move_var = 2;
                    MoveSpeed = 3.5f;
                }
            }


            //��������
            //���"BasicMotion"�����֣�������ǵ�Parameter���ֲ�����һ��
            //���Ǿ͵øģ�
            animator.SetInteger("BaseMotion", move_var);
            animator.SetInteger("MoveDir", move_dir);
            //��ά���귽������ֵ����
            //��������ֵ�������������
            target_dir = new Vector3(horizontal, 0, vertical);
            //now_dir = mCamera.transform.rotation.eulerAngles;
            now_dir = this.transform.rotation.eulerAngles;
            //��Ӧdir��ˮƽ����ת��y;x������,z��ǰ��

            //����ûд�ã�����ԭ����д���Ǿֲ�����ϵ�ķ��򣬶�controller.Move����������ϵ����δ���
            //Vector3 dir_right =
            //    new Vector3(Mathf.Sin(now_dir.y + Mathf.PI / 2), 0, Mathf.Cos(now_dir.y + Mathf.PI / 2));
            //Vector3 dir_for =
            //    new Vector3(Mathf.Sin(now_dir.y), 0, Mathf.Cos(now_dir.y));
            //Debug.Log(now_dir);
            //Debug.Log(dir_right);
            //Debug.Log(dir_for);
            //target_dir = dir_right * horizontal + dir_for * vertical;

            target_dir = transform.right * horizontal + transform.forward * vertical;
            //Debug.Log(now_dir);
            //Debug.Log(transform.right);
            //Debug.Log(transform.forward);
            //�����ƶ�����
            controller.Move(target_dir * MoveSpeed * Time.deltaTime * 10);

            //controller.Move()��������ʵ��������ƶ��������ڸ����ķ������ƶ���Ϸ����
            //����������Ҫ�������˶�����ֵ���������ٶȣ���֡ˢ��ʱ�䣨Time.deltaTime��
            //ע��controller.Move()�ǲ�ʹ�á��������ģ�����Ѿ����£��ٻ���ʱ�޷����£�
            //    �����Ҫʹ�������ͱ����Լ��ֶ�д����ģ���������Ĵ��롣

            #region
            //Ҳ����ʹ�ø÷�������controller.Move();
            //this.transform.Translate(dir * MoveSpeed * Time.deltaTime);
            #endregion
        }

        else
        {
            //Ĭ��Ϊ��ֹ����
            move_var = 0;

            //����״̬����
            //���"BasicMotion"�����֣�������ǵ�Parameter���ֲ�����һ��
            //���Ǿ͵øģ�
            animator.SetInteger("BaseMotion", 0);
            MoveSpeed = 0;
        }
    }

    //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    //���������

    #region Camera Rotation

    //��ת���Ƕ�
    public int yRotationMinLimit = -20;
    public int yRotationMaxLimit = 80;
    //��ת�ٶ�
    public float xRotationSpeed = 250.0f;
    public float yRotationSpeed = 120.0f;
    //��ת�Ƕ�
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    //���ڸı�shoot����״̬�ı���
    public int shoot_var;

    /// <summary>
    /// ����ƶ�������ת
    /// </summary>
    void CameraRotate()
    {
        if (mMouseState == MouseState.None)
        {

            //Input.GetAxis("MouseX")��ȡ����ƶ���X��ľ���
            xRotation -= Input.GetAxis("Mouse X") * xRotationSpeed * Time.deltaTime;
            yRotation += Input.GetAxis("Mouse Y") * yRotationSpeed * Time.deltaTime;

            yRotation = ClampValue(yRotation, yRotationMinLimit, yRotationMaxLimit);//��������ڽ�β
                                                                                    //ŷ����ת��Ϊ��Ԫ��
            // z�����ϱ�ʾ�������ƫ����ƽ�������¿�ƽ�У�
            // y�����ϱ�ʾ����ת��x�����ϱ�ʾ�������ת
            Quaternion rotation = Quaternion.Euler(-yRotation, 0, 0);
            mCamera.transform.localRotation = rotation;
            rotation = Quaternion.Euler(0, -xRotation, 0);
            transform.rotation = rotation;

        }
    }


    #endregion

    #region Camera fov

    //fov �����С�Ƕ�
    public int fovMinLimit = 25;
    public int fovMaxLimit = 75;
    //fov �仯�ٶ�
    public float fovSpeed = 50.0f;
    //fov �Ƕ�
    private float fov = 0.0f;

    void GetDefaultFov()
    {
        fov = mCamera.fieldOfView;
    }

    /// <summary>
    /// ���ֿ�������ӽ�����
    /// </summary>
    public void CameraFOV()
    {
        //��ȡ�����ֵĻ�����
        fov += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 100 * fovSpeed;

        // fov ��������
        fov = ClampValue(fov, fovMinLimit, fovMaxLimit);

        //�ı������ fov
        mCamera.fieldOfView = (fov);
    }

    #endregion


    #region Camera Move

    float _mouseX = 0;
    float _mouseY = 0;
    public float moveSpeed = 1;
    /// <summary>
    /// �м������϶�
    /// </summary>
    public void CameraMove()
    {
        if (Input.GetMouseButton(2))
        {
            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");

            //���λ�õ�ƫ������Vector3���ͣ�ʵ��ԭ���ǣ������ļӷ���
            Vector3 moveDir = (_mouseX * -transform.right + _mouseY * -transform.forward);

            //����y���ƫ����
            moveDir.y = 0;
            transform.position += moveDir * 0.5f * moveSpeed;
        }
        else if (Input.GetMouseButtonDown(2))
        {
            mMouseState = MouseState.MidMouseBtn;
            Debug.Log(GetType() + "mMouseState = " + mMouseState.ToString());
        }
        else if (Input.GetMouseButtonUp(2))
        {
            mMouseState = MouseState.None;
            Debug.Log(GetType() + "mMouseState = " + mMouseState.ToString());
        }

    }

    #endregion

    #region tools ClampValue

    //ֵ��Χֵ�޶�
    float ClampValue(float value, float min, float max)//������ת�ĽǶ�
    {
        if (value < -360)
            value += 360;
        if (value > 360)
            value -= 360;
        return Mathf.Clamp(value, min, max);//����value��ֵ��min��max֮�䣬 ���valueС��min������min�� ���value����max������max�����򷵻�value
    }


    void CameraLeft()
    {

        if (Input.GetMouseButton(0)) { 
            shoot_var = 1;
            //animator.SetLayerWeight(animator.GetLayerIndex("Shoot Layer"), 1);
        } else { 
            shoot_var = 0;
            //animator.SetLayerWeight(animator.GetLayerIndex("Shoot Layer"), 0);
        }

        animator.SetInteger("ShootMode", shoot_var);
    }

    #endregion

}
