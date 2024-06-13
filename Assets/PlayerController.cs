using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class PlayerController : MonoBehaviour
{
    const float shortNoLabelPropertyWidth = 50;
    const float shortLabelWidth = 25;

    Rigidbody rigid;
    [SerializeField] Transform center;

    [SerializeField] Camera cam;
    [SerializeField] float camMove;

    [SerializeField] float speed = 10.0f;
    [SerializeField] float moveDrag = 0;
    [SerializeField] float stopDrag = 10.0f;

    [SerializeField] Transform arm;
    [SerializeField] Transform hand;
    [SerializeField] Transform arg;
    [SerializeField] float inputDead = 0.1f;

    [SerializeField] float armSpinSpeed = 1;
    [SerializeField] float armRevertSpinSpeed = 1;

    [FoldoutGroup("Hand")]
    #region Foldout Hand

        [BoxGroup("Hand/ArmPos")]
        #region Box ArmPos
    
            [HorizontalGroup("Hand/ArmPos/X", Width = shortNoLabelPropertyWidth + shortLabelWidth)]
            #region Horizontal X  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
            
                [SerializeField][DelayedProperty][PropertyOrder(0)]
                [LabelText("X")][LabelWidth(shortLabelWidth)]
                float armPosXMin = -30;
                                                                                               [HorizontalGroup("Hand/ArmPos/X")]
                [ShowInInspector]
                [HideLabel]
                [PropertyRange(nameof(armPosXMin), nameof(armPosXMax))]
                float showArmPosX { get => armPos.x; set => armPos.x = value; }
                                                                                               [HorizontalGroup("Hand/ArmPos/X", Width = shortNoLabelPropertyWidth)]
                [SerializeField][DelayedProperty][PropertyOrder(2)]
                [HideLabel][LabelWidth(shortNoLabelPropertyWidth)]
                float armPosXMax = 10;

            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     
            
            [HorizontalGroup("Hand/ArmPos/Y", Width = shortNoLabelPropertyWidth + shortLabelWidth)]
            #region Horizontal Y  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
            
                [SerializeField][DelayedProperty][PropertyOrder(0)]
                [LabelText("Y")][LabelWidth(shortLabelWidth)]
                float armPosYMin = -30;
                                                                                               [HorizontalGroup("Hand/ArmPos/Y")]
                [ShowInInspector]
                [HideLabel]
                [PropertyRange(nameof(armPosYMin), nameof(armPosYMax))]
                float showArmPosY { get => armPos.y; set => armPos.y = value; }
                                                                                               [HorizontalGroup("Hand/ArmPos/Y", Width = shortNoLabelPropertyWidth)]
                [SerializeField][DelayedProperty][PropertyOrder(2)]
                [HideLabel][LabelWidth(shortNoLabelPropertyWidth)]
                float armPosYMax = 10;

            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     

            [HorizontalGroup("Hand/ArmPos/Z", Width = shortNoLabelPropertyWidth + shortLabelWidth)]
            #region Horizontal Z  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
            
                [SerializeField][DelayedProperty][PropertyOrder(0)]
                [LabelText("Z")][LabelWidth(shortLabelWidth)]
                float armPosZMin = 0.25f;
                                                                                               [HorizontalGroup("Hand/ArmPos/Z")]
                [ShowInInspector]
                [HideLabel]
                [PropertyRange(nameof(armPosZMin), nameof(armPosZMax))]
                float showArmPosZ { get => armPos.z; set => armPos.z = value; }
                                                                                               [HorizontalGroup("Hand/ArmPos/Z", Width = shortNoLabelPropertyWidth)]
                [SerializeField][DelayedProperty][PropertyOrder(2)]
                [HideLabel][LabelWidth(shortNoLabelPropertyWidth)]
                float armPosZMax = 1;

            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     

            Vector3 armPos;

        #endregion

        [BoxGroup("Hand/Rotation")]
        #region Box Rotation - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|

            [HorizontalGroup("Hand/Rotation/Hori", Width = shortNoLabelPropertyWidth + shortLabelWidth)]
            #region Horizontal Hori  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
            
                [SerializeField][DelayedProperty][PropertyOrder(0)]
                [LabelText("Horizontal")][LabelWidth(shortLabelWidth)]
                float handRotHorizontalMin = -30;
                                                                                               [HorizontalGroup("Hand/Rotation/Hori")]
                [ShowInInspector]
                [HideLabel]
                [PropertyRange(nameof(handRotHorizontalMin), nameof(handRotHorizontalMax))]
                float handRotHorizontal;
                                                                                               [HorizontalGroup("Hand/Rotation/Hori", Width = shortNoLabelPropertyWidth)]
                [SerializeField][DelayedProperty][PropertyOrder(2)]
                [HideLabel][LabelWidth(shortNoLabelPropertyWidth)]
                float handRotHorizontalMax = 10;

            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|     

            [HorizontalGroup("Hand/Rotation/Y", Width = shortNoLabelPropertyWidth + shortLabelWidth)]
            #region Horizontal Y  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
            
                [SerializeField][DelayedProperty][PropertyOrder(0)]
                [LabelText("Y")][LabelWidth(shortLabelWidth)]
                float handRotYMin = -30;
                                                                                               [HorizontalGroup("Hand/Rotation/Y")]
                [ShowInInspector]
                [HideLabel]
                [PropertyRange(nameof(handRotYMin), nameof(handRotYMax))]
                float handRotY;
                                                                                               [HorizontalGroup("Hand/Rotation/Y", Width = shortNoLabelPropertyWidth)]
                [SerializeField][DelayedProperty][PropertyOrder(2)]
                [HideLabel][LabelWidth(shortNoLabelPropertyWidth)]
                float handRotYMax = 10;

            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|      
    
            [HorizontalGroup("Hand/Rotation/Vert", Width = shortNoLabelPropertyWidth + shortLabelWidth)]
            #region Horizontal Vert  - - - - - - - - - - - - - - - - - - - - - - - - - - - -|  
            
                [SerializeField][DelayedProperty][PropertyOrder(0)]
                [LabelText("Vertical")][LabelWidth(shortLabelWidth)]
                float handRotVerticalMin = -30;
                                                                                               [HorizontalGroup("Hand/Rotation/Vert")]
                [ShowInInspector]
                [HideLabel]
                [PropertyRange(nameof(handRotVerticalMin), nameof(handRotVerticalMax))]
                float handRotVertical;
                                                                                               [HorizontalGroup("Hand/Rotation/Vert", Width = shortNoLabelPropertyWidth)]
                [SerializeField][DelayedProperty][PropertyOrder(2)]
                [HideLabel][LabelWidth(shortNoLabelPropertyWidth)]
                float handRotVerticalMax = 10;

            #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|   
    
        #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|             

    #endregion - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    [SerializeField] Vector3 armVelocity;
    [SerializeField] Vector3 armAcceleration = Vector3.one;
    [SerializeField] float handVerticalSpeed = 100;
    [SerializeField] float spearCharge = 1;

    [SerializeField] Vector2 armResist;
    [SerializeField] float armDead = 0.1f;
    [SerializeField] Vector2 armDeadResist;

    [SerializeField] Transform target;
    Vector3 targetDir;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        center.transform.LookAt(target);
        cam.transform.localEulerAngles = new Vector3(-armPos.y, armPos.x, 0) * camMove;

        Arm();

        if (EditorApplication.isPlaying == false) return;

        Move();
    }
    void Move()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            rigid.drag = moveDrag;
         
            targetDir = (target.position - transform.position - Vector3.up * (target.position.y - transform.position.y)).normalized;
            Vector3 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            rigid.AddForce(
                new Vector3(
                    input.x * (targetDir.z) + input.y * (targetDir.x), 
                    0, 
                    input.x * (-targetDir.x) + input.y * (targetDir.z)
                ) * speed * Time.deltaTime, ForceMode.Force);
        }
        else
        {
            rigid.drag = stopDrag;
        }
    }
    void Arm()
    {
        ArmPhisics();


        hand.LookAt(arm);
        //if(armVelocity != Vector2.zero) hand.Rotate(0, 0, Mathf.Atan2(armVelocity.y, armVelocity.x) * Mathf.Rad2Deg);
        
        float armSpinFactor = armSpinSpeed * Mathf.Abs(handRotHorizontalMax - handRotHorizontalMin) * Time.deltaTime;
        float armRevertFactor = armRevertSpinSpeed * Mathf.Abs(handRotHorizontalMax - handRotHorizontalMin) * Time.deltaTime;
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (Input.GetMouseButton(0))
            {
                handRotHorizontal -= armSpinFactor;
                if (handRotHorizontal < handRotHorizontalMin) handRotHorizontal = handRotHorizontalMin;
            }
            if (Input.GetMouseButton(1))
            {
                handRotHorizontal += armSpinFactor;
                if (handRotHorizontal > handRotHorizontalMax) handRotHorizontal = handRotHorizontalMax;
            }
        }
        else
        {
            if (handRotHorizontal > 0)
            {
                handRotHorizontal -= armRevertFactor;
                if (handRotHorizontal < 0) handRotHorizontal = 0;
            }
            if (handRotHorizontal < 0)
            {
                handRotHorizontal += armRevertFactor;
                if (handRotHorizontal > 0) handRotHorizontal = 0;
            }
        }

        handRotVertical = Mathf.Clamp(handRotVertical - Input.GetAxis("Mouse ScrollWheel") * handVerticalSpeed, handRotVerticalMin, handRotVerticalMax);

        hand.Rotate(handRotVertical, handRotY, handRotHorizontal);
    }
    void ArmPhisics()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //입력 무시
        if (input.x > inputDead || input.x < -inputDead) input.x = 0;
        if (input.y > inputDead || input.y < -inputDead) input.y = 0;

        //속도
        armVelocity += new Vector3(input.x * armAcceleration.x, input.y * armAcceleration.y, 0);

        //저항
        if (armVelocity.magnitude < armDead)
        {
            AddToZero(ref armVelocity.x, armDeadResist.x);
            AddToZero(ref armVelocity.y, armDeadResist.y);
        }
        else
        {
            AddToZero(ref armVelocity.x, armResist.x);
            AddToZero(ref armVelocity.y, armResist.y);
        }

        //찌르기
        if (Input.GetKey(KeyCode.Mouse2))
        {
            armVelocity.z -= spearCharge * Time.deltaTime;
        }
        else armVelocity.z += armAcceleration.z * Time.deltaTime;

        //이동
        armPos += armVelocity * Time.deltaTime;

        Lock(ref armPos.x, ref armVelocity.x, armPosXMin, armPosXMax);
        Lock(ref armPos.y, ref armVelocity.y, armPosYMin, armPosYMax);
        Lock(ref armPos.z, ref armVelocity.z, armPosZMin, armPosZMax);

        //적용
        arg.localEulerAngles = new Vector3(-armPos.y, armPos.x, 0);
        hand.localPosition = new Vector3(0, 0, armPos.z);
    }
    void AddToZero(ref float value, float speed)
    {
        if (value > 0)
        {
            value -= speed * Time.deltaTime;
            if (value < 0)
            {
                value = 0;
            }
        }
        else if (value < 0)
        {
            value += speed * Time.deltaTime;
            if (value > 0)
            {
                value = 0;
            }
        }
    }
    void Lock(ref float pos, ref float velocity, float min, float max)
    {
        if (pos > max)
        {
            pos = max;
            velocity = 0;
        }
        if (pos < min)
        {
            pos = min;
            velocity = 0;
        }
    }
}
