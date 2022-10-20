using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMove : MonoBehaviour
{

    [Header("�����̑���")]
    [Tooltip("�����̑���")] [SerializeField] float _movingSpeed = 5f;

    [Header("�󒆂�Add���鑬�x")]
    [Tooltip("�󒆂�Add���鑬�x")] [SerializeField] float _airMoveSpeed = 1f;

    [Header("Jump�󒆂�Add���鑬�x")]
    [Tooltip("Jump�󒆂�Add���鑬�x")] [SerializeField] float _swingAirMoveSpeed = 1f;

    [Header("���Ⴊ�݂̈ړ����x")]
    [Tooltip("���Ⴊ�݂̈ړ����x")] [SerializeField] float _squatMoeSpeed = 4;

    [Header("�W�����v��")]
    [Tooltip("�W�����v��")] [SerializeField] float _jumpPower = 5f;

    [Header("�W�����v��ɉ�����ǉ��̏d��")]
    [Tooltip("�W�����v��ɉ�����ǉ��̏d��")] [SerializeField] float _gravity = 0.3f;


    [SerializeField] LayerMask _slope;

    /// <summary>���Ⴊ��ł��邩�ǂ���</summary>
    public bool _squat = false;
    /// <summary>�X���C�f�B���O�����Ă��邩�ǂ���</summary>
    public bool _isSliding = false;

    public bool _isSwing;

    P_Control _control;



    Vector3 angle;
    bool _isSlope;


    Vector3 _airVelo;

    /// <summary>�L�����N�^�[�� Animator</summary>
    [SerializeField] Animator _anim;
    [SerializeField] Animator _animKatana;
    //  [SerializeField] AudioSource m_aud;
    Animator m_anim;
    Rigidbody _rb;
    void Start()
    {
        Cursor.visible = false;
        _control = GetComponent<P_Control>();

        _rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();

        Physics.gravity = Physics.gravity * 2;
        _animKatana = _animKatana.GetComponent<Animator>();
        //// m_aud = m_aud.GetComponent<AudioSource>();
    }

    void Update()
    {
        Dir();
        Jump();

        if (_control.playerAction == P_Control.PlayerAction.OnGround)
        {
            Move();
            DownSpeed();
        }


    }

    private void FixedUpdate()
    {
        MoveAir(_airVelo);
    }

    void Dir()
    {
        Vector3 dir = Camera.main.transform.forward;
        dir.y = 0;  // y �������̓[���ɂ��Đ��������̃x�N�g���ɂ���
        transform.forward = dir;

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        // ���͕����̃x�N�g����g�ݗ��Ă�
        _airVelo = Vector3.forward * v + Vector3.right * h;
        _airVelo = Camera.main.transform.TransformDirection(_airVelo);    // ���C���J��������ɓ��͕����̃x�N�g����ϊ�����
        _airVelo.y = 0;  // y �������̓[���ɂ��Đ��������̃x�N�g���ɂ���
    }

    void Move()
    {
        // �����̓��͂��擾���A���������߂�
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // ���͕����̃x�N�g����g�ݗ��Ă�
        Vector3 dir = Vector3.forward * v + Vector3.right * h;

        if (dir == Vector3.zero && _control._isGround)
        {
            // �����̓��͂��j���[�g�����̎��́Ay �������̑��x��ێ����邾��
            _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
            //_airVelo = Vector3.zero;
            _animKatana.SetBool("Move", false);
        }
        else
        {
            _animKatana.SetBool("Move", true);
            // �J��������ɓ��͂��㉺=��/��O, ���E=���E�ɃL�����N�^�[��������
            dir = Camera.main.transform.TransformDirection(dir);    // ���C���J��������ɓ��͕����̃x�N�g����ϊ�����
            dir.y = 0;  // y �������̓[���ɂ��Đ��������̃x�N�g���ɂ���

            Vector3 velo;
            if (_control._isSquat)
            {
                velo = dir.normalized * _squatMoeSpeed; // ���͂��������Ɉړ�����
            }
            else
            {
                velo = dir.normalized * _movingSpeed; // ���͂��������Ɉړ�����
            }
            velo.y = _rb.velocity.y;   // �W�����v�������� y �������̑��x��ێ�����
            _rb.velocity = velo;   // �v�Z�������x�x�N�g�����Z�b�g����
        }

        // Animator Controller �̃p�����[�^���Z�b�g����
        if (_anim)
        {
            // �U���{�^���������ꂽ���̏���
            if (Input.GetButtonDown("Fire1") && _control._isGround)
            {
                _anim.SetTrigger("Attack");
            }

            // ���������̑��x�� Speed �ɃZ�b�g����
            Vector3 velocity = _rb.velocity;
            velocity.y = 0f;
            _anim.SetFloat("Speed", velocity.magnitude);

            // �n��/�󒆂̏󋵂ɉ����� IsGrounded ���Z�b�g����
            if (_rb.velocity.y <= 0f && _control._isGround)
            {
                _anim.SetBool("IsGrounded", true);
            }
            else if (!_control._isGround)
            {
                _anim.SetBool("IsGrounded", false);
            }
        }
    }





    //public void CheckSlope()
    //{
    //    RaycastHit hit;
    //    CapsuleCollider col = GetComponent<CapsuleCollider>();
    //    Vector3 start = this.transform.position + col.center + new Vector3(0, 0, -0.3f);   // start: �̂̒��S
    //    Vector3 end = start + Vector3.down*5;  // end: start ����^���̒n�_

    //    Debug.DrawLine(start, end, Color.red); // ����m�F�p�� Scene �E�B���h�E��Ő���\������
    //    if (Physics.Linecast(start, end,out hit,_slope))
    //    {
    //        Debug.Log("yes");
    //        Vector3 hitNomal = hit.normal;
    //        angle = Vector3.Cross(hitNomal, transform.up);
    //        _isSlope = true;
    //    }
    //    else
    //    {
    //        Debug.Log("no");
    //        _isSlope = false;
    //    }

    //}





    void MoveAir(Vector3 velo)
    {
        if (_control.playerAction == P_Control.PlayerAction.SwingAir)
        {
            _rb.AddForce(velo.normalized * _swingAirMoveSpeed, ForceMode.Force);
        }
        else if (_control.playerAction == P_Control.PlayerAction.Air)
        {
            _rb.AddForce(velo.normalized * _airMoveSpeed, ForceMode.Force);
        }

        //if (_control._isWapon == false)
        //{
        //    if (velo == Vector3.zero)
        //    {

        //    }
        //    else
        //    {
        //        _rb.AddForce(velo.normalized * _airMoveSpeed, ForceMode.Force);
        //    }
        //}
        //else if (_control._isWapon == true)
        //{
        //    Vector3 vlo = velo * _airMoveSpeed;

        //    vlo.y = _rb.velocity.y;

        //    _rb.velocity = vlo;
        //}

       // _rb.AddForce(-transform.up * _gravity);
    }




    void Jump()
    {
        if (!_control._isGrapple && !_control._isSwing && !_control._isWallRun && _control._isGround)
        {
            // �W�����v�̓��͂��擾���A�ڒn���Ă��鎞�ɉ�����Ă�����W�����v����
            if (Input.GetButtonDown("Jump") && _control._isGround)
            {
                FindObjectOfType<Sliding>().StopSquat();
                _control._isSliding = false;
                _control._isJump = true;
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpPower, _rb.velocity.z);
                // Animator Controller �̃p�����[�^���Z�b�g����
                if (_anim)
                {
                    _anim.SetBool("IsGrounded", false);
                }
            }
        }
    }


    void DownSpeed()
    {
        if (_control._isJump)
        {
            if (_rb.velocity.y < 4)
            {
                _rb.AddForce(-transform.up * _gravity, ForceMode.Force);
            }

            if (_rb.velocity.y == 0)
            {

                _control._isJump = false;
            }
        }
    }






}