using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    [Header("�W�����v��")]
    [Tooltip("�W�����v��")] [SerializeField] float _jumpPower = 5f;

    [Header("�W�����v��")]
    [Tooltip("�W�����v��")] [SerializeField] int _jumpNum = 1;

    [SerializeField] bool _isMultiJump = false;

    private int _jumpCount = 0;

    GroundCheck _groundCheck;
    PlayerInput _playerInput;

    Rigidbody _rb;
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _groundCheck = GetComponent<GroundCheck>();
        _rb = GetComponent<Rigidbody>();
    }

    public void Jump()
    {
        if (_playerInput.IsJumping && _jumpCount<_jumpNum)
        {
            if(_groundCheck.IsGround)
            {
                //�W�����v�������ɒn�ʂɂ��Ă�������񐔂�0�B
                //�ŁA�P��W�����v���邩��1
                _jumpCount = 1;
            }
            else  if(_isMultiJump)
            {
                _jumpCount++;
            }
            
            Vector3 velo = new Vector3(_rb.velocity.x, _jumpPower, _rb.velocity.z);
            _rb.velocity = velo;
        }
    }

}
