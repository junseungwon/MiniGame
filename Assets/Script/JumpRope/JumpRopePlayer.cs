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
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        StartCoroutine(CheckFlow());
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        PlayerMove();
    }
    private void PlayerMove()
    {
        if (pv.IsMine)
        {
            float zPos = Input.GetAxis("Vertical");
            float xPos = Input.GetAxis("Horizontal");
            transform.Translate(new Vector3(zPos, 0f, xPos) * 2f * Time.deltaTime);

        }
    }
    //�÷��̾ �ȳѾ����� rigidBody Freezen�ص� 
    private void Jump()// ����ڰ� ����Ű ����
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isJump == true)
            {
                AnimationSpeedChange();
                animator.SetBool("Jump", true);
                StartCoroutine(AddForce());
                StartCoroutine(StopAnimation("Jump", 1f));
            }
        }
    }
    private IEnumerator AddForce()
    {
        float speed = 0.4f;
        yield return new WaitForSeconds(0.3f);
        transform.Translate(Vector3.up * speed);
        yield return new WaitForSeconds(0.1f);
        transform.Translate(Vector3.up * speed);
        yield return new WaitForSeconds(0.1f);

    }
    private IEnumerator StopAnimation(string text, float time)//�ִϸ��̼��� ���� ������ false���� raycast ���� 2�� ���
    {
        yield return new WaitForSeconds(time);
        animator.speed = 1.0f;
        animator.SetBool(text, false);
    }
    private IEnumerator CheckFlow()//ĳ���Ͱ� �ٴڿ� ��Ҵ��� üũ
    {
        while (true)
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
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator saveCorutine = null;
    private void SaveCorutine(IEnumerator corutine)//�ڷ�ƾ�� �����ϰ� �ش� �ڷ�ƾ ����
    {
        saveCorutine = corutine;
        StartCoroutine(saveCorutine);
    }
    public void PlayerLose()//�÷��� �й�
    {
        if (pv.IsMine)
        {
            Debug.Log("PlayerLose");
            JumpRope.Instance.StopRotationRope();
            transform.position = JumpRopeSystem.Instance.SetStartPoint();
            pv.RPC("RpcPlayerWin", RpcTarget.OthersBuffered);
            OpponentWinScore++;
            JumpRope.Instance.ChangeText(JumpRope.Instance.texts[1], OpponentWinScore);
            JumpRope.Instance.RopeReSetting();
        }
        //�ٸ��߰� ��ġ �ʱ�ȭ
        //�÷��̾� ��ġ �ʱ�ȭ
        //��뿡�� �¸��ߴٰ� ����
        //������� ���
        //UITEXT ����
        //�� ����ġ���� �����
    }
    [PunRPC]
    private void RpcPlayerWin()//�޾��� ��� ���κ��� �¸�
    {
        Debug.Log("rpcWin");
        JumpRope.Instance.StopRotationRope();
        transform.position = JumpRopeSystem.Instance.SetStartPoint();
        winScore++;
        JumpRope.Instance.ChangeText(JumpRope.Instance.texts[0], winScore);
        JumpRope.Instance.RopeReSetting();
    }
    private void AnimationSpeedChange()
    {
        animator.speed = 1.3f;
    }




    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
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
