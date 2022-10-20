using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAvirity : MonoBehaviour
{
    [SerializeField] Animator _am;

    [SerializeField] GameObject _katana;
    [SerializeField] GameObject _slashPos;


    [SerializeField] float _timeSpeed;
    [SerializeField] float _cooltimeCount = 5f;
    [SerializeField] float _cooltimeLimit = 5f;

    Vector3 _endPos;
    Vector3 _slashStartPos;
    Vector3 _slashEndPos;

    [SerializeField] float _slashPower = 10;

    [SerializeField] Text _t;

    Vector3 _saveVelo;

    Vector3 _angularVelocity;
    Vector3 _velocity;

    public bool _isSlashing;

    bool _isSlash;
    bool _isSlow;


    bool _isCount;

    Rigidbody _rb;


    void OnEnable()
    {
        SlashDelegate.OnBeginSlash += PreparationSlash;
        SlashDelegate.OnEndSlash += StartSlash;
    }

    void OnDisable()
    {
        SlashDelegate.OnBeginSlash -= PreparationSlash;
        SlashDelegate.OnEndSlash -= StartSlash;
    }

    void Start()
    {
        _am = _am.GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _cooltimeCount = _cooltimeLimit;
        _t.text = _cooltimeCount.ToString();
    }

    void Update()
    {
        CountTimeTimeAvirity();
        Slash();
        Slow();


        if (_isSlashing)
        {
            _slashPos.transform.position += new Vector3(0, 0.05f, 0);
        }
    }

    void Slow()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartSlow();
            _isCount = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopSlow();
            _isCount = false;
        }
    }

    void Slash()
    {
        //���𑕔����Ă���
        if (_katana.activeSelf)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (_isSlow)
                {
                    return;
                }
                Debug.Log("SlashStart");

                SlashDelegate.BeginSlash();
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (_isSlow)
                {
                    return;
                }

                SlashDelegate.EndSlash();
            }

        }
        SlashEndCheck(); //�X���b�V���̏I���𔻒f
    }

    void CountTimeTimeAvirity()
    {
        if (_isCount)
        {
            //���Ԃ�x�����Ă���ԁA�\�͂̃^�C�����炷
            if (_cooltimeCount >= 0) _cooltimeCount -= Time.deltaTime * 2;

            //�\�͂̃^�C����0�ɂȂ����狭���I��
            if (_cooltimeCount <= 0)
            {
                if (_isSlash) SlashDelegate.EndSlash();
                else StopSlow();
            }
        }

        //�\�͂̃^�C������
        if (_cooltimeCount <= _cooltimeLimit) _cooltimeCount += Time.deltaTime * 0.5f;
        _t.text = _cooltimeCount.ToString();
    }

    /// <summary>�X���b�V���̏���(�f���Q�[�g�ŌĂ΂��)</summary>
    void PreparationSlash()
    {
        Debug.Log("DelegateStart");
        Time.timeScale = 0.3f;
        _isSlashing = true;
        _isSlash = true;
        _isCount = true;

        transform.position += new Vector3(0, 0.3f, 0);
        _slashStartPos = transform.position;
    }

    /// <summary>�X���b�V��������(�f���Q�[�g�ŌĂ΂��)</summary>
    void StartSlash()
    {
        Debug.Log("DelegateEnd");

        Time.timeScale = 1f;
        _am.Play("Slash");
        _isCount = false;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(Camera.main.transform.forward * _slashPower, ForceMode.Impulse);
    }

    /// <summary>�X���b�V���̏I���𔻒f������</summary>
    void SlashEndCheck()
    {
        if (_isSlash)
        {
            var a = _am.GetCurrentAnimatorStateInfo(0).normalizedTime;

            var b = Vector3.Distance(transform.position, _slashStartPos);
            if (b>= 10 || a>=1)
            {
                _isSlashing = false;
                Debug.Log("MovedSlash");
                _isSlash = false;
                _rb.velocity = Vector3.zero;
            }
        }
    }

    void StartSlow()
    {
        //�\�͂̃^�C����0��������g���Ȃ�
        if (_cooltimeCount <= 0)
        {
            return;
        }
        Time.timeScale = 0.5f;
    }

    void StopSlow()
    {
        Time.timeScale = 1f;
    }

}
