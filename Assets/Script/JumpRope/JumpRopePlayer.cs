using Photon.Pun;
using System.Collections;
using UnityEngine;

public class JumpRopePlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    private int winScore = 0;
    private int OpponentWinScore = 0;
    public float JumpeSpeed = 1000000f;
    private PhotonView pv;
    private Animator animator;
    private bool isJump = false;
    private float isJumpTime = 1f;
    private Rigidbody rb;
    private JumpRope jumpRope;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        jumpRope =FindObjectOfType<JumpRope>();
    }
    void Update()
    {
        Jump();
        CheckFlow();
    }
    private void FixedUpdate()
    {
        UseGravity();
        TestMove();
    }
    private void TestMove()//일반적인 조작 움직임
    {
        if (pv.IsMine)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(h, 0, v);
            dir.Normalize();
            if (dir != Vector3.zero)
            {
                if (Mathf.Sign(dir.x) != Mathf.Sign(transform.position.x) || Mathf.Sign(dir.z) != Mathf.Sign(transform.position.z))
                {
                    transform.Rotate(0, 1, 0);
                }
                transform.forward = Vector3.Lerp(transform.forward, dir, 30f * Time.deltaTime);
            }
            rb.MovePosition(transform.position + dir * 5f * Time.deltaTime);

        }
    }
    private void UseGravity()//리지드 쓰려다가 개같아서 간단하게 만듬
    {
        Debug.Log(isJump);
        if (pv.IsMine && isJump == false)
        {
            isJumpTime += 0.1f; if (isJumpTime > 1.6f) isJumpTime = 1f;
            transform.Translate(Vector3.down * Time.deltaTime * 0.7f * isJumpTime);
        }
        else
        {
            isJumpTime = 1f;
        }
    }
    //플레이어가 안넘어지게 rigidBody Freezen해둠 
    private void Jump()// 사용자가 점프키 누름
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isJump == true)
            {
                isJump = false;
                AnimationSpeedChange();
                animator.SetBool("Jump", true);
                StartCoroutine(JumpUp());
                StartCoroutine(StopAnimation("Jump", 1f));
            }
        }
    }
    private IEnumerator JumpUp()//점프값 보정 단계별로 상승 
    {
        float speed = 0.4f;
        yield return new WaitForSeconds(0.3f);
        transform.Translate(Vector3.up * speed);
        yield return new WaitForSeconds(0.1f);
        transform.Translate(Vector3.up * speed);
        yield return new WaitForSeconds(0.1f);

    }
    private IEnumerator StopAnimation(string text, float time)//애니메이션을 몇초 지정후 false변경 raycast 오류 2번 재생
    {
        yield return new WaitForSeconds(time);
        animator.speed = 1.0f;
        animator.SetBool(text, false);
    }
    private void CheckFlow()//캐릭터가 바닥에 닿았는지 체크
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.04f))
        {
            if (hit.collider.name == "Plane")
            {
                isJump = true;
            }
            else
            {
                isJump = false;
            }
        }
        if (hit.collider == null) { isJump = false; }
    }
    public void PlayerLose()//플레이 패배
    {
        if (pv.IsMine)
        {
            Debug.Log("PlayerLose");
            jumpRope.StopRotationRope();
            transform.position = JumpRopeSystem.Instance.SetStartPoint();
            pv.RPC("RpcPlayerWin", RpcTarget.OthersBuffered);
            OpponentWinScore++;
            jumpRope.ChangeText(jumpRope.texts[1], OpponentWinScore);
            jumpRope.RopeReSetting();
        }
        //줄멈추고 위치 초기화
        //플레이어 위치 초기화
        //상대에게 승리했다고 전달
        //상대점수 상승
        //UITEXT 변경
        //겜 원위치에서 재시작
    }
    [PunRPC]
    private void RpcPlayerWin()//받았을 경우 상대로부터 승리
    {
        Debug.Log("rpcWin");
        jumpRope.StopRotationRope();
        transform.position = JumpRopeSystem.Instance.SetStartPoint();
        winScore++;
        jumpRope.ChangeText(jumpRope.texts[0], winScore);
        jumpRope.RopeReSetting();
    }
    private void AnimationSpeedChange()//애니메이션 속도 변경
    {
        animator.speed = 1.3f;
    }




    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    //플레이 1이 플레이 2를 이겼다. 플레이 1은 승리표시하고 상대한테는 패배코드를 날린다.
    [PunRPC]
    private void Talk(int num)
    {

    }
    private int text = -1;
    //보내준 줄이랑 받는 줄의 수가 같아야함
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(0);
        }
        else
        {
            text = (int)stream.ReceiveNext();
        }

    }
}
