using Photon.Pun;
using System.Collections;
using UnityEngine;
public class JumpRopePlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    public float JumpeSpeed = 0.1f;
    private Rigidbody rb;
    private PhotonView pv;
    private Animator animator;
    private bool isJump = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        PlayerMove();
    }
    private void PlayerMove()
    {
        float zPos = Input.GetAxis("Vertical");
        float xPos = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(zPos, 0f, xPos)*2f*Time.deltaTime);
    }
    //플레이어가 안넘어지게 rigidBody Freezen해둠 
    private void Jump()// 사용자가 점프키 누름
    {

        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space)&&isJump == true)
            {
                animator.SetBool("Jump", true);
                //transform.Translate(Vector3.up * 0.3f);
                rb.AddForce(Vector3.up * JumpeSpeed*Time.deltaTime, ForceMode.Impulse);
                // StartCoroutine(CheckFlow());
                //SaveCorutine(CheckFlow());
               StartCoroutine(StopAnimation("Jump", 1.3f));
            }
        }
    }
    private IEnumerator StopAnimation(string text ,float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool(text, false);
    }
    private IEnumerator CheckFlow()//캐릭터가 바닥에 닿았는지 체크
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position ,Vector3.down, out hit, 0.04f))
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
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator saveCorutine = null;
    private void SaveCorutine(IEnumerator corutine)//코루틴을 저장하고 해당 코루틴 실행
    {
        saveCorutine = corutine;
        StartCoroutine(saveCorutine);
    }
    private void PlayerWin()//플레이 승리시
    {
        pv.RPC("RpcPlayerLose", RpcTarget.AllBuffered);
    }
    private void PlayerLose()//플레이 패배시
    {
        pv.RPC("RpcPlayerWin", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void RpcPlayerWin()
    {

    }
    [PunRPC]
    private void RpcPlayerLose()
    {

    }
    private void OnCollisionEnter(Collision collision)//줄에 걸렸을 경우
    {
        if (collision.collider.name == "Rope")
        {
            Debug.Log("Fail");
            //if(pv.RPC("Talk",RpcTarget.AllBuffer, num))
        }
    }




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
