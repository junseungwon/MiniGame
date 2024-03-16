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
    public string nickName = "";
    public string opperNickName = "";
    [SerializeField] private Text nickNameText = null;
    public Player[] players = new Player[2];
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
        Debug.Log("hello");
    }
    private void SettingPlayer(GameObject player)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            players[0] = new Player(player, nickName, 0,0);
        }
        else
        {
           // if (player[0] == null)
            players[1] = new Player(player, nickName, 0,0);
        }
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
public class Player
{
    public GameObject playerObj = null;
    public string nickName = "";
    public int winScore = 0;
    public int opperScore =0;
    public Player(GameObject playerObj, string nickName,int winScore, int opperScore) {
        this.playerObj = playerObj;
        this.nickName = nickName;
        this.winScore = winScore;
        this.opperScore = opperScore;    
    }

}
