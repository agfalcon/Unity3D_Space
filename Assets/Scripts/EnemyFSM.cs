using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState m_State;
    Transform player;

    //�÷��̾� �߰� ����
    public float findDistance = 8f;

    //�÷��̾� ���� ����
    public float attackDistance = 2f;

    //���� �ӵ�
    public float moveSpeed = 4f;

    CharacterController cc;


    float currentTime = 0f;
    float attackDelay = 1.5f;
    public int attackPower = 3;

    // ���ʹ� �ʱ� ��ġ
    Vector3 originPos;

    // ���� ����
    public float moveDistance = 20f;

    //���ʹ� ü��
    public int hp;
    public int maxhp = 15;

    //�ִϸ��̼�
    Animator anim;

    // ���ʹ� �ʱ� �����̼�
    Quaternion originRot;

    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle;
        player = GameObject.Find("Player").transform;
        cc = GetComponent<CharacterController>();
        originPos = transform.position;
        originRot = transform.rotation;
        hp = maxhp;
        anim = transform.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:

                break;
        }
    }

    void Idle()
    {
        if (Vector3.Distance(player.position, transform.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Idle -> Move");
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDistance) 
        {
            m_State = EnemyState.Return;
            print("���� ��ȯ : Move -> Return");
        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.forward = dir;
        }
        else
        {
            m_State = EnemyState.Attack;
            print("���� ��ȯ : Move -> Attack");
            currentTime = attackDelay;
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0;
                anim.SetTrigger("StartAttack");
            }
        }
        else
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Attack -> Move");
            anim.SetTrigger("AttackToMove");
            currentTime = 0;
        }
    }

    void Return()
    {
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
            transform.forward = dir;
        }
        else
        {
            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxhp;
            m_State = EnemyState.Idle;
            print("���� ��ȯ : Return -> Idle");
            anim.SetTrigger("MoveToIdle");
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    // ������ ó�� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        print("start coroutine");
        yield return new WaitForSeconds(0.5f);
        m_State = EnemyState.Move;
        print("end coroutine");
    }

    public void HitEnemy(int hitPower)
    {
        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }
        hp -= hitPower;
        if(hp > 0)
        {
            m_State = EnemyState.Damaged;
            Damaged();
            print("���� ��ȯ : Any Stae -> Damaged");
        }
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ : Any State -> Die");
            Die();
        }
    }

    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        cc.enabled = false;
        anim.SetTrigger("AnyToDie");
        yield return new WaitForSeconds(1.5f);
        print("�Ҹ�!");
        Destroy(gameObject);
    }
}
