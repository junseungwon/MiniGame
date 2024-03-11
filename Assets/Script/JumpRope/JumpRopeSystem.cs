using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class JumpRopeSystem : MonoBehaviourPunCallbacks
{
    private static JumpRopeSystem instance = null;
    public static JumpRopeSystem Instance
    {
        get {  if (instance == null) instance = FindObjectOfType<JumpRopeSystem>(); return instance; }
    }
    private string nickName = "";
    [SerializeField] private Text nickNameText = null;
    private void Awake()//처음 세팅
    {
        DontDestroyOnLoad(this);
        Screen.SetResolution(600, 600, false);
    }
    public void NextScene()//버튼 누르면 겜씬으로 이동한다.
    {
        GetNickName();
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.LoadScene(1);
    }
    private void GetNickName()//닉넴생성
    {
        nickName = nickNameText.text;
    }
    public override void OnConnectedToMaster()//마스터가 연결되면 방만들기
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
    }
    public override void OnJoinedRoom()//방에 참가했을 경우 플레이어 생성
    {
        GameObject player = PhotonNetwork.Instantiate("Player", SetStartPoint(), Quaternion.identity);
        player.name = nickName;
        Debug.Log(player.transform.position);
        FindObjectOfType<JumpRope>().RpcStartGame();
    }
    public Vector3 SetStartPoint()//초기위치 설정
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return new Vector3(1,0.3f,0);
        }
        else
        {
            return new Vector3(-1, 0.3f, 0);
        }     
    }
    [PunRPC]
    public void UpIntMaxPlayer()
    {
        
    }
}
