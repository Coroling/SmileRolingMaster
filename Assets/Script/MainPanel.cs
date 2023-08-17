using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public Transform PanelParnt;

    public TMP_InputField InputX;
    public TMP_InputField InputY;
    public TMP_InputField InputSpeed;
    public Button AddBtn;
    public Button PlayBtn;
    public Button StopBtn;


    [Header("프리팹연결")]
    public SubPanel DefsubPanel;
    public GameObject PosObj;
    public GameObject MoveObj;
    [SerializeField]
    List<SubPanel> SubPanels = new List<SubPanel>();

    //필드변수
    float moveSpeed = 10f;

    //프로퍼티
    public float MoveSpeed { get { return moveSpeed * Time.deltaTime; } set { moveSpeed = value; } }

    void BtnInit()
    {
        if(InputSpeed)
            InputSpeed.text = MoveSpeed.ToString();
        AddBtn?.onClick.RemoveAllListeners();
        PlayBtn?.onClick.RemoveAllListeners();
        StopBtn?.onClick.RemoveAllListeners();
        InputSpeed?.onEndEdit.RemoveAllListeners();

        AddBtn?.onClick.AddListener(AddNewData);
        PlayBtn?.onClick.AddListener(StartMove);
        InputSpeed?.onEndEdit.AddListener(t=> MoveSpeed = float.Parse(t));
    }

    void AddNewData()
    {
        if (DefsubPanel == null)
        { 
            Debug.LogError("설정에러 서브판넬이 등록되지 않았습니다.");
            return;
        }
        //변수선언
        float X;
        float Y;
        //파싱 후 할당
        if (!float.TryParse(InputX.text, out X)) { Debug.LogError("올바른 좌표를 입력하세요"); return; }
        if (!float.TryParse(InputY.text, out Y)) { Debug.LogError("올바른 좌표를 입력하세요"); return; }
        //판넬생성
        SubPanel subPanel = Instantiate(DefsubPanel, PanelParnt).GetComponent<SubPanel>();
        //초기화
        subPanel.PanelInit(new Vector2(X,Y), this);
        //등록
        SubPanels.Add(subPanel);
    }
    #region 서브판넬 위치 변경

    public void RemoveSubPanel(SubPanel subPanel)
    {
        // 리스트에 존재하고있으면 리스트에서 제거후 오브젝트 삭제
        if (SubPanels.Contains(subPanel))
        {
            SubPanels.Remove(subPanel);
            Destroy(subPanel.gameObject);
        }
    }
    public void UpSubPanel(SubPanel subPanel)
    {
        //목표 인덱스
        int index = SubPanels.IndexOf(subPanel) - 1;
        if (index == -1)
            return;
        ChangePanelPos(subPanel, index);
    }
    public void DownSubPanel(SubPanel subPanel)
    {
        //목표 인덱스
        int index = SubPanels.IndexOf(subPanel) + 1;
        if (index == SubPanels.Count)
            return;
        ChangePanelPos(subPanel, index);
    }
    void ChangePanelPos(SubPanel subPanel, int index)
    {
        //현재 리스트에서 제거
        SubPanels.Remove(subPanel);
        //위치 변경
        SubPanels.Insert(index, subPanel);
        //물리적 위치 변경
        subPanel.transform.SetSiblingIndex(index);
    }

    #endregion

    IEnumerator MoveCor;
    IEnumerator _MoveCor()
    {
        if(SubPanels.Count <= 1)
            yield break;

        MoveObj.transform.position = SubPanels[0].posData;
        int NextIndex = 1;
        while (true)
        {
            //만일 전체 갯수가 2개 이하일경우. 이동정지.
            if (SubPanels.Count <= 1)
                yield break;
            //거리 검사 && 인덱스 변경
            if (closePos(MoveObj.transform.position, SubPanels[NextIndex].posData))
            {
                // 일정 거리내에 들어왔을경우. 다음 인덱스로. 
                NextIndex ++;
                if (NextIndex == SubPanels.Count)
                    NextIndex = 0;
            }

            MoveObj.transform.position = Vector3.MoveTowards(MoveObj.transform.position, SubPanels[NextIndex].posData, MoveSpeed);
            yield return null;
        }
        //거리 검사 내부 함수
        bool closePos(Vector3 pos1, Vector3 pos2)
        {
            float dist = Vector3.Distance(pos1, pos2);
            if(dist < 0.1f)
                return true;
            else
                return false;
        }
    }

    void StartMove()
    {
        if (MoveCor != null)
            StopCoroutine(MoveCor);
        MoveCor = _MoveCor();
        StartCoroutine(MoveCor);
    }

    // Start is called before the first frame update
    void Start()
    {
        BtnInit();
    }
}
