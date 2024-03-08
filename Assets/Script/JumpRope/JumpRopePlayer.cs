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
    //�÷��̾ �ȳѾ����� rigidBody Freezen�ص� 
    private void Jump()// ����ڰ� ����Ű ����
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
    private IEnumerator CheckFlow()//ĳ���Ͱ� �ٴڿ� ��Ҵ��� üũ
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
    private void SaveCorutine(IEnumerator corutine)//�ڷ�ƾ�� �����ϰ� �ش� �ڷ�ƾ ����
    {
        saveCorutine = corutine;
        StartCoroutine(saveCorutine);
    }
    private void PlayerWin()//�÷��� �¸���
    {
        pv.RPC("RpcPlayerLose", RpcTarget.AllBuffered);
    }
    private void PlayerLose()//�÷��� �й��
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
    private void OnCollisionEnter(Collision collision)//�ٿ� �ɷ��� ���
    {
        if (collision.collider.name == "Rope")
        {
            Debug.Log("Fail");
            //if(pv.RPC("Talk",RpcTarget.AllBuffer, num))
        }
    }




    //�÷��� 1�� �÷��� 2�� �̰��. �÷��� 1�� �¸�ǥ���ϰ� ������״� �й��ڵ带 ������.
    [PunRPC]
    private void Talk(int num)
    {

    }
    private int text = -1;
    //������ ���̶� �޴� ���� ���� ���ƾ���
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
