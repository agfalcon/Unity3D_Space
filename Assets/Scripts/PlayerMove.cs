using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class PlayerMove : MonoBehaviour
{

    public float moveSpeed = 7.0f;
    CharacterController cc;
    public GameObject oxygenUI;

    public Camera cam;
    //중력 변수
    float gravity = -1.63f;
    //수직 속력 변수
    float yVelocity = 0f;
    //점프력 변수
    public float jumpPower = 1.3f;
    //점프 상태 변수
    public bool isJumping = false;
    //플레이어 체력 변수
    public float oxygen = 100f;
    float maxOxygen = 100f;

    //UI
    public Slider oxygenSlider;
    public GameObject hitEffect;
    public GameObject dangerousEffect;
    public GameObject crossHair;
    public GameObject gameOverImage;
    public GameObject gameOver;
    public GameObject gameClear;

    bool cross = false;
    //플레이어 노즐 분사 파워
    public float nozzlePower = 2.2f;

    //플레이어 호흡량
    float conOx = 1f;

    //공기 분사 효과음
    private AudioSource audio;
    public AudioClip jumpSound;

    //숨소리 효과음
    private AudioSource audio2;
    public AudioClip breathSound;

    // 장애물에 위치해 있는 여부
    Vector3 distance;


    //위험 신호
    bool isStop = false;

    //사망 신호
    bool isDead = false;

    bool isDangerous = false;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.jumpSound;
        this.audio.loop = false;
        this.audio2 = this.gameObject.AddComponent<AudioSource>();
        this.audio2.clip = this.breathSound;
        this.audio2.loop = false;
    }



    // Update is called once per frame
    void Update()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;
        //메인 카메라를 기준으로 방향 변경
        dir = cam.transform.TransformDirection(dir);

        if(Input.GetKeyDown("c"))
        {
            if(cross)
            {
                crossHair.SetActive(false);
                cross = false;
            }
            else
            {
                crossHair.SetActive(true);
                cross = true;
            }
        }
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0f;
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        if (isJumping)
        {
            yVelocity += gravity * Time.deltaTime;
        }

        if(Input.GetMouseButtonDown(1))
        {
            this.audio.Play();
        }
        if(Input.GetMouseButton(1))
        {
            isJumping = true;

            if (yVelocity < 0.5f)
            {
                nozzlePower += 0.15f * Time.deltaTime;
                yVelocity += nozzlePower * Time.deltaTime;
            }
            oxygen -= 0.025f;
            print(yVelocity);
        }
        
        if(Input.GetMouseButtonUp(1))
        {
            nozzlePower = 2.2f;
            this.audio.Pause();
            print(nozzlePower);
        }
        dir.y = yVelocity;
        cc.Move(dir * moveSpeed * Time.deltaTime);
        oxygen -= conOx * Time.deltaTime;

        if (oxygen < 25)
        {
            dangerousEffect.SetActive(true);
            if (!isDangerous)
            {
                this.audio2.Play();
                isDangerous = true;
            }
        }
        oxygenSlider.value = oxygen / maxOxygen;

        if(oxygen <= 0)
        {
            oxygen = 0;
            if (!isDead)
            {
                cc.enabled = false;
                gameOverImage.SetActive(true);
                gameOver.SetActive(true);
                isDead = true;
            }
            if (isDead)
            {
                if (Input.anyKey)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
        oxygenUI.GetComponent<OxygenUI>().SetOxygen(oxygen);
    }

 
    private void OnCollisionEnter(Collision collision)
    {
        print("되나??");
        if(collision.transform.tag == "ground")
        {
            isJumping = false;
            print("테스트");
        }
        if (collision.transform.tag == "gggg")
        {
            distance = collision.transform.position - transform.position;
            print("접촉 시작");
        }
        if(collision.collider.gameObject.CompareTag("clear"))
        {
            gameClear.SetActive(true);
            isDead = true;
            if (Input.anyKey)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "gggg")
        {
            transform.position = collision.transform.position - distance;
        }
        print("접촉중");
    }

    public void DamageAction(int damage)
    {
        oxygen -= damage;
        if(oxygen > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
        if (oxygen < 0)
        {
            oxygen = 0;
        }
        print(oxygen);
    }

    IEnumerator PlayHitEffect()
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitEffect.SetActive(false);
    }
}
