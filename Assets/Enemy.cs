using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("HP")]
    [Tooltip("HP")] [SerializeField] int _hp;

    [Header("�ړ����x")]
    [Tooltip("�ړ����x")] [SerializeField] float _moveSpeed = 3;

    [Header("�U���̃N�[���^�C��")]
    [Tooltip("�U���̃N�[���^�C��")] [SerializeField] float _attackCoolTime = 3;


    [Header("�������U�����ɏo���G�t�F�N�g")]
    [Tooltip("�������U�����ɏo���G�t�F�N�g")] [SerializeField] GameObject _longAttackEffect;

    [Header("����")]
    [Tooltip("����")] [SerializeField] GameObject _weapon;

    [Header("�f�X���ɏo���I�u�W�F�N�g")]
    [Tooltip("�f�X���ɏo���I�u�W�F�N�g")] [SerializeField] GameObject _deathBody;

    [Header("�ߋ����U���̉�")]
    [Tooltip("�ߋ����U���̉�")] [SerializeField] AudioClip _attackCloseAudioClip;

    [Header("�������U���̉�")]
    [Tooltip("�������U���̉�")] [SerializeField] AudioClip _attackFarAudioClip;

    [Header("��_�����̉�")]
    [Tooltip("��_�����̉�")] [SerializeField] AudioClip _damage;


    AudioSource _aud;

    /// <summary>true��������v�l</summary>
    bool _thinkNow = false;

    /// <summary>����̍s�������Ă���Ƃ��͍s�����Ȃ��B�U�����͑����Ă΂�邩��</summary>
    bool _isActionNow = false;

    /// <summary>�^�[�Q�b�g�U���������Ă邩�ǂ���</summary>
    public bool _isDamagedTargetAttack = false;

    int _countDamagedTargetAttack = 0;


    Vector3 _angularVelocity;
    Vector3 _velocity;

    public bool _isJustKaihi = false;



    Rigidbody _rb;
    Animator _anim;
    GameObject _player;
    public EnemyAction _enemyAction = EnemyAction.Wait;
    Animator _weaponAnim;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        _thinkNow = true;
        _rb = gameObject.GetComponent<Rigidbody>();
        _anim = gameObject.GetComponent<Animator>();
        _weaponAnim = _weapon.GetComponent<Animator>();
        _aud = gameObject.GetComponent<AudioSource>();
    }






    void Update()
    {
        SetAi();

        //���݂̏󋵂ɉ������s��
        switch (_enemyAction)
        {
            case EnemyAction.Wait:
                Wait();
                break;
            case EnemyAction.Move:
                Move();
                break;

            case EnemyAction.Follow:
                Follow();
                break;

            case EnemyAction.Attack:
                Attack();
                break;

            case EnemyAction.Stop:
                Stop();
                break;

            case EnemyAction.LongAttack:
                LongAttck();
                break;

            case EnemyAction.Back:
                Back();
                break;

            case EnemyAction.TargetAttackDamaged:
                TargetAttackDamaged();
                break;
        }
    }

    /// <summary>�s������</summary>
    void SetAi()
    {
        //�^�C�}�[���񂵂Ă�Ԃ͎v�l�����Ȃ�
        if (!_thinkNow || _isActionNow)
        {
            return;
        }
        MainRoutine();

        StartCoroutine(AiTimer());
    }

    void MainRoutine()
    {
        float dir = Vector2.Distance(_player.transform.position, transform.position);

        if (dir < 5)
        {
            var r = Random.Range(0, 4);
            if (r == 0)
            {
                _enemyAction = EnemyAction.Attack;
            }
            else if (r == 1)
            {
                _enemyAction = EnemyAction.Stop;
            }
            else if (r == 2)
            {
                _enemyAction = EnemyAction.LongAttack;
            }
            else
            {
                _enemyAction = EnemyAction.Back;
            }
            return;
        }

        if (dir < 10)
        {
            _enemyAction = EnemyAction.Follow;
            return;
        }
        if (dir < 15)
        {
            _enemyAction = EnemyAction.Move;
            return;
        }


        if (dir < 20)
        {
            _enemyAction = EnemyAction.Wait;
            return;
        }

    }

    IEnumerator AiTimer()
    {
        _thinkNow = false;
        yield return new WaitForSeconds(0.1f);
        _thinkNow = true;
    }

    void Wait()
    {
        //  Debug.Log("wait");
    }

    void Damaged()
    {
        _isActionNow = true;
        float h = GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x;
        _rb.velocity = Vector3.zero;
        if (h >= 0)
        {
            _rb.AddForce(transform.right * -2, ForceMode.Impulse);
        }
        else
        {
            _rb.AddForce(1 * transform.right * 2, ForceMode.Impulse);
        }

        StartCoroutine(Damagedd());
    }
    IEnumerator Damagedd()
    {
        _hp--;
        _aud.PlayOneShot(_damage);
        Debug.Log(_hp);

        if (_hp <= 0)
        {
            var go = Instantiate(_deathBody);
            go.transform.position = transform.position;
            Destroy(gameObject);
            yield break;
        }
        yield return new WaitForSeconds(1);
        _isActionNow = false;
    }

    /// <summary>���̏�ɗ��܂�s�� </summary>
    void Stop()
    {
        if (_isActionNow)
        {
            return;
        }
        _isActionNow = true;
        Dirction();
        StartCoroutine(StopC());
    }
    IEnumerator StopC()
    {
        yield return new WaitForSeconds(2);
        _isActionNow = false;
    }

    /// <summary>�v���C���[����ジ����s�� </summary>
    void Back()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Dirction();
        if (player)
        {
            Vector2 v = player.transform.position - transform.position;

            _rb.velocity = new Vector3(-2 * v.normalized.x, _rb.velocity.y, 0);
        }

        if (_isActionNow)
        {
            return;
        }
        _isActionNow = true;
        StartCoroutine(BackC());
    }
    IEnumerator BackC()
    {
        yield return new WaitForSeconds(2);

        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        _isActionNow = false;
    }


    void Move()
    {
        // Debug.Log("move");
    }
    /// <summary>�v���C���[��ǂ�������s�� </summary>
    void Follow()
    {
        Vector2 dir = _player.transform.position - transform.position;
        _rb.velocity = new Vector2(dir.normalized.x * 3, _rb.velocity.y);
        Dirction();
    }

    /// <summary>�ߋ����U���̍s�� </summary>
    void Attack()
    {
        if (_isActionNow)
        {
            return;
        }
        // Debug.Log("Attack");

        _isActionNow = true;
        Dirction();
        StartCoroutine(AttackC());
    }

    IEnumerator AttackC()
    {
        _aud.PlayOneShot(_attackCloseAudioClip);

        yield return new WaitForSeconds(0.5f);

        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        _weaponAnim.Play("ArrmerEnemyWeaponAttackClose");

        yield return new WaitForSeconds(1.1f + _attackCoolTime);
        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        _isActionNow = false;
    }

    void LongAttck()
    {
        if (_isActionNow)
        {
            return;
        }
        _isActionNow = true;
        Dirction();
        StartCoroutine(LongAttackC());

    }

    IEnumerator LongAttackC()
    {
        _weaponAnim.Play("ArmerEnemyWeaponFar1");
        _aud.PlayOneShot(_attackFarAudioClip);

        yield return new WaitForSeconds(0.5f);

        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            var go = Instantiate(_longAttackEffect);
            go.transform.position = player.transform.position + new Vector3(0, 1, -1.5f);
        }

        yield return new WaitForSeconds(1.1f);
        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        yield return new WaitForSeconds(_attackCoolTime);
        _isActionNow = false;
    }

    void TargetAttackDamaged()
    {
        _isActionNow = true;
        Dirction();

        if (_countDamagedTargetAttack > 0 && !_isDamagedTargetAttack)
        {
            StartCoroutine(TargetAttackDamagedC());

        }
    }

    IEnumerator TargetAttackDamagedC()
    {
        Debug.Log("AAAD");
        _rb.velocity = Vector3.zero;
        _countDamagedTargetAttack = 0;
        yield return new WaitForSeconds(1);

        _isActionNow = false;
        _isDamagedTargetAttack = false;
        _enemyAction = EnemyAction.Damaged;
        Damaged();
    }


    void Dirction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 v = player.transform.position - transform.position;
        if (v.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (v.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public enum EnemyAction
    {
        Next,
        Wait,
        Move,
        Follow,
        Attack,
        Evation,
        Stop,
        LongAttack,
        Back,
        Damaged,
        TargetAttackDamaged,
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "P_Attack")
        {
            _enemyAction = EnemyAction.Damaged;
            Damaged();
        }

        if (other.gameObject.tag == "RaisingAttack")
        {

        }

        if (other.gameObject.tag == "TargetAttack")
        {
            _isDamagedTargetAttack = true;
            _countDamagedTargetAttack++;
            _hp--;
        }
    }
}

