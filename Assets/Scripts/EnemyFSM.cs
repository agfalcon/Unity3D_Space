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

    //플레이어 발견 범위
    public float findDistance = 8f;

    //플레이어 공격 범위
    public float attackDistance = 2f;

    //적군 속도
    public float moveSpeed = 4f;

    CharacterController cc;


    float currentTime = 0f;
    float attackDelay = 1.5f;
    public int attackPower = 3;

    // 에너미 초기 위치
    Vector3 originPos;

    // 리턴 범위
    public float moveDistance = 20f;

    //에너미 체력
    public int hp;
    public int maxhp = 15;

    //애니메이션
    Animator anim;

    // 에너미 초기 로테이션
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
            print("상태 전환 : Idle -> Move");
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDistance) 
        {
            m_State = EnemyState.Return;
            print("상태 전환 : Move -> Return");
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
            print("상태 전환 : Move -> Attack");
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
                print("공격");
                currentTime = 0;
                anim.SetTrigger("StartAttack");
            }
        }
        else
        {
            m_State = EnemyState.Move;
            print("상태 변환 : Attack -> Move");
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
            print("상태 변환 : Return -> Idle");
            anim.SetTrigger("MoveToIdle");
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리 코루틴 함수
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
            print("상태 전환 : Any Stae -> Damaged");
        }
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환 : Any State -> Die");
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
        print("소멸!");
        Destroy(gameObject);
    }
}
