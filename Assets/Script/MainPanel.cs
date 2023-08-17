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


    [Header("�����տ���")]
    public SubPanel DefsubPanel;
    public GameObject PosObj;
    public GameObject MoveObj;
    [SerializeField]
    List<SubPanel> SubPanels = new List<SubPanel>();

    //�ʵ庯��
    float moveSpeed = 10f;

    //������Ƽ
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
            Debug.LogError("�������� �����ǳ��� ��ϵ��� �ʾҽ��ϴ�.");
            return;
        }
        //��������
        float X;
        float Y;
        //�Ľ� �� �Ҵ�
        if (!float.TryParse(InputX.text, out X)) { Debug.LogError("�ùٸ� ��ǥ�� �Է��ϼ���"); return; }
        if (!float.TryParse(InputY.text, out Y)) { Debug.LogError("�ùٸ� ��ǥ�� �Է��ϼ���"); return; }
        //�ǳڻ���
        SubPanel subPanel = Instantiate(DefsubPanel, PanelParnt).GetComponent<SubPanel>();
        //�ʱ�ȭ
        subPanel.PanelInit(new Vector2(X,Y), this);
        //���
        SubPanels.Add(subPanel);
    }
    #region �����ǳ� ��ġ ����

    public void RemoveSubPanel(SubPanel subPanel)
    {
        // ����Ʈ�� �����ϰ������� ����Ʈ���� ������ ������Ʈ ����
        if (SubPanels.Contains(subPanel))
        {
            SubPanels.Remove(subPanel);
            Destroy(subPanel.gameObject);
        }
    }
    public void UpSubPanel(SubPanel subPanel)
    {
        //��ǥ �ε���
        int index = SubPanels.IndexOf(subPanel) - 1;
        if (index == -1)
            return;
        ChangePanelPos(subPanel, index);
    }
    public void DownSubPanel(SubPanel subPanel)
    {
        //��ǥ �ε���
        int index = SubPanels.IndexOf(subPanel) + 1;
        if (index == SubPanels.Count)
            return;
        ChangePanelPos(subPanel, index);
    }
    void ChangePanelPos(SubPanel subPanel, int index)
    {
        //���� ����Ʈ���� ����
        SubPanels.Remove(subPanel);
        //��ġ ����
        SubPanels.Insert(index, subPanel);
        //������ ��ġ ����
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
            //���� ��ü ������ 2�� �����ϰ��. �̵�����.
            if (SubPanels.Count <= 1)
                yield break;
            //�Ÿ� �˻� && �ε��� ����
            if (closePos(MoveObj.transform.position, SubPanels[NextIndex].posData))
            {
                // ���� �Ÿ����� ���������. ���� �ε�����. 
                NextIndex ++;
                if (NextIndex == SubPanels.Count)
                    NextIndex = 0;
            }

            MoveObj.transform.position = Vector3.MoveTowards(MoveObj.transform.position, SubPanels[NextIndex].posData, MoveSpeed);
            yield return null;
        }
        //�Ÿ� �˻� ���� �Լ�
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
