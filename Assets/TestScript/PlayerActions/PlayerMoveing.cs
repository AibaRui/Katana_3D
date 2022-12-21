using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveing : MonoBehaviour
{
    [Header("�����̑���")]
    [Tooltip("�����̑���")] [SerializeField] float _movingSpeed = 5f;

    private float _moveSpeed = 0;

    PlayerInput _playerInput;

    Rigidbody _rb;
    [SerializeField] Animator _anim;
    private void Awake()
    {
        _moveSpeed = _movingSpeed;
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move()
    {
        // �����̓��͂��擾���A���������߂�
        // ���͕����̃x�N�g����g�ݗ��Ă�
        Vector3 dir = Vector3.forward * _playerInput.VerticalInput + Vector3.right * _playerInput.HorizontalInput;

        if (dir == Vector3.zero)
        {
            // �����̓��͂��j���[�g�����̎��́Ay �������̑��x��ێ����邾��
            _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
            //_airVelo = Vector3.zero;
            //_animKatana.SetBool("Move", false);
        }
        else
        {
            //_animKatana.SetBool("Move", true);

            // �J��������ɓ��͂��㉺=��/��O, ���E=���E�ɃL�����N�^�[��������
            dir = Camera.main.transform.TransformDirection(dir);    // ���C���J��������ɓ��͕����̃x�N�g����ϊ�����
            dir.y = 0;  // y �������̓[���ɂ��Đ��������̃x�N�g���ɂ���

            Vector3 velo = dir.normalized * _moveSpeed; // ���͂��������Ɉړ�����
            velo.y = _rb.velocity.y;   // �W�����v�������� y �������̑��x��ێ�����
            _rb.velocity = velo;   // �v�Z�������x�x�N�g�����Z�b�g����
        }

        // ���������̑��x�� Speed �ɃZ�b�g����
        //Vector3 velocity = _rb.velocity;
       // velocity.y = 0f;
        //_anim.SetFloat("Speed", velocity.magnitude);

    }

    public void ChangeMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }

    public void ReSetMoveSpeed()
    {
        _moveSpeed = _movingSpeed;
    }

}
