using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubPanel : MonoBehaviour
{
    MainPanel mainPanel;
    public TextMeshProUGUI texX;
    public TextMeshProUGUI texY;

    public Button UpBtn;
    public Button DownBtn;
    public Button DeleteBtn;
    public Vector3 posData;
    GameObject PosObj;

    //초기화
    //AddNewData에서 생성한 서브판넬에 넣을 함수 이러헥 하면 자기자신이 리스트의 누구인지 명명해진다. 
    public void PanelInit(Vector2 pos, MainPanel mainPanel)
    {
        this.mainPanel = mainPanel;
        posData = pos;
        if (mainPanel.PosObj != null) // 위치 표시용 오브젝트 생성
            PosObj = Instantiate(mainPanel.PosObj, pos, Quaternion.identity);

        texX.text = pos.x.ToString();
        texY.text = pos.y.ToString();

        btnInit();
    }

    #region ButtonInit
    private void btnInit()
    {
        //버튼들의 기능을 초기화 한다.
        UpBtn?.onClick.RemoveAllListeners();
        DownBtn?.onClick.RemoveAllListeners();
        DeleteBtn?.onClick.RemoveAllListeners();

        UpBtn?.onClick.AddListener(OnUpBtnClick);
        DownBtn?.onClick.AddListener(OnDownBtnClick);
        DeleteBtn?.onClick.AddListener(OnDeleteBtnClick);
    }

    void OnUpBtnClick()
    {
        if (mainPanel == null)
        {
            Debug.LogError("설정에러 메인판넬이 등록되지 않았습니다.");
            return;
        }
        mainPanel.UpSubPanel(this);
    }
    void OnDownBtnClick()
    {
        if (mainPanel == null)
        {
            Debug.LogError("설정에러 메인판넬이 등록되지 않았습니다.");
            return;
        }
        mainPanel.DownSubPanel(this);
    }
    void OnDeleteBtnClick()
    {
        if (mainPanel == null)
        {
            Debug.LogError("설정에러 메인판넬이 등록되지 않았습니다.");
            return;
        }
        mainPanel.RemoveSubPanel(this);
    }

    #endregion


    private void OnDestroy()
    {
        if (PosObj)
            Destroy(PosObj);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
