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
    private void TestMove()//�Ϲ����� ���� ������
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
    private void UseGravity()//������ �����ٰ� �����Ƽ� �����ϰ� ����
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
    //�÷��̾ �ȳѾ����� rigidBody Freezen�ص� 
    private void Jump()// ����ڰ� ����Ű ����
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
    private IEnumerator JumpUp()//������ ���� �ܰ躰�� ��� 
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
    private void CheckFlow()//ĳ���Ͱ� �ٴڿ� ��Ҵ��� üũ
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
    public void PlayerLose()//�÷��� �й�
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
        jumpRope.StopRotationRope();
        transform.position = JumpRopeSystem.Instance.SetStartPoint();
        winScore++;
        jumpRope.ChangeText(jumpRope.texts[0], winScore);
        jumpRope.RopeReSetting();
    }
    private void AnimationSpeedChange()//�ִϸ��̼� �ӵ� ����
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
