using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class main : MonoBehaviour {
    public Text[] Num;
    public int CurrentNumber;
    public int[] Answer;
    public int Ball;
    public int Strike;

    //public Text BallNumber;
    //public Text StrikeNumber;
    public Text PrevNumber;
    public GameObject RecordObj;
    public int tryNumber;
    public Transform List;
    public Transform MyCanvas;
    public int Difficulty;
    public GameObject NumOrigin;
    public Text Message;

    public GameObject OpeningPanel;
    public GameObject MainPanel;
    public GameObject ClearPanel;
    public Text Title;
    public Text DifficultyText;
    public Transform InputParent;

    //public Text ClearTitle;
    public Text ClearAnswer;
    public Text ClearNum;
    public Text ClearGrade;
    public Text ClearTime;

    public Flash myFlash;

    private float playTime;
    public Text TimeText;
    //private float Scroll = 0;

    public enum eGameMode
    {
        Opening,
        Main,
        Clear
    }
    internal eGameMode gameMode;

    private float ScreenRate = 1f;

#if UNITY_IPHONE
    private Vector2 TouchStart;
    private Vector2 TouchRecent;
#endif



    // Use this for initialization
    void Start()
    {
        OpeningPanel.SetActive(true);
        MainPanel.SetActive(false);
        ClearPanel.SetActive(false);
        CurrentNumber = 0;
        tryNumber = 0;

        Difficulty = 4;
        gameMode = eGameMode.Opening;

        ScreenRate = 800f / (float)Screen.currentResolution.width;
    }


    void InitializeGame()
    {
        CurrentNumber = 0;
        tryNumber = 0;
        playTime = 0;
        TimeText.text = "00:00:00";

        System.Array.Clear(Num, 0, Num.Length);
        Num = new Text[Difficulty];
        for (int i = 0; i < Difficulty; i++)
        {
            GameObject obj = Instantiate(NumOrigin);
            obj.transform.SetParent(InputParent);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector2(-320 + i * 60, -40);
            obj.name = i.ToString();
            int num = i;
            obj.transform.GetComponent<Button>().onClick.AddListener(delegate { SetCurrentNumber(num); });


            Num[i] = obj.GetComponent<Text>();

            obj.SetActive(true);

            //Num[i] = GameObject.Find("Num" + i).GetComponent<Text>();
            Num[i].text ="?";
        }


        System.Array.Clear(Answer, 0, Answer.Length);
        Answer = new int[Difficulty];
        /*
        for (int i = 0; i < Answer.Length; i++)
        {
            Answer[i] = -1;
        }
        */

        switch (Difficulty)
        {
            case 2:
                Title.text = "효나의 바보를 위한 숫자야구";
                break;
            case 3:
                Title.text = "효나의 개쉬운 숫자야구";
                break;
            case 4:
                Title.text = "효나의 숫자야구";
                break;
            case 5:
                Title.text = "효나의 약간 어려운 숫자야구";
                break;
            case 6:
                Title.text = "효나의 개어려운 숫자야구";
                break;
            case 7:
                Title.text = "효나의 슈우우퍼 개어려운 숫자야구";
                break;

        }




        GetRandom();

    }


    void GetRandom()
    {
        for (int i = 0; i < Answer.Length; i++) // 4 자리의 숫자를 하나씩 돌리는 루프 시작. i 는 몇번째 숫자인지를 의미함.
        {
            bool check = false; // 이전거랑 겹치는 숫자인지 체크하기 위한 참/거짓 불린 함수. 일단 거짓이라고 해놓자.
            int ran = 0; // 주사위를 굴려 산정될 최종 숫자를 담기 위한 임시 변수

            //Debug.Log("Making Answer " + i + " : Start"); //자 i 번째 숫자를 만들어보자.

            while (check == false) //이 체크 값이 참이 될때까지 무한히 돌려봅시다. 시작은 거짓이었으니 무조건 여기 진입합니다.
            {

                ran = Random.Range(0, 10); // 일단 ran 에다가 주사위를 굴려 넣어봅니다. 0 - 9 까지 나올겁니다.

                //Debug.Log("Got Ramdom Number " + ran);

                check = true;  // 이제 이 값이 괜찮은 값인지 확인합시다. 일단은 무죄 라고 가정하죠.

                for (int j = 0; j < i; j++) //현재까지 지정한 숫자까지 다시 한번 반복합시다. 3번째 숫자를 하는거라면 1,2 번째까지 반복합니다.
                {
                    if (ran == Answer[j]) //지금 만든 값이 이전 값과 같습니까? 같다면 유죄입니다
                    {
                        //Debug.Log("Damn!!! This Number " + ran + " is same as Answer " + j);
                        check = false;
                        break; //이미 유죄는 증명되었으니 나는 이 루프에서 탈출하겠어
                    }
                    //하지만 모든 숫자를 다 확인해봤는데도 끝까지 걸리지 않는다면 이놈은 무죄가 맞습니다.
                }

            }//무죄가 맞다면 여기서 탈출합니다.

            //Debug.Log("Answer " + i + " = " + ran);
            Answer[i] = ran;
        }
    }



    public void CheckNumber()
    {
        Debug.Log(Num[0].text + Num[1].text + Num[2].text + Num[3].text);
        for (int i = 0; i < Num.Length; i++)
        {
            Num[i].color = Color.white;
        }

        Ball = 0;
        Strike = 0;
        for (int i = 0; i < Num.Length; i++)
        {
            if (Num[i].text == "?")
            {
                Message.text = "숫자를 전부 입력해야해";
                myFlash.Play(true);
                Debug.Log("No No No No You not entered whole Number!!");
                return;
            }
            for (int j = 0; j < i; j++)
            {
                if (Num[j].text == Num[i].text)
                {
                    Message.text = "슷자 겹치면 안돼요";
                    myFlash.Play(true);
                    Debug.Log("same number");
                    return;
                }
            }
        }

        Debug.Log("Your Entered Number is OK");
        myFlash.Play(false);



        for (int i = 0; i < Answer.Length; i++)
        {
            for (int j = 0; j < Num.Length; j++)
            {
                if (Answer[i].ToString() == Num[j].text)
                {
                    if (i == j)
                    {
                        Strike++;
                    }
                    else
                    {
                        Ball++;
                    }
                }

            }
        }
        Debug.Log("Strike = " + Strike + " / Ball = " + Ball);

        PrevNumber.text = "";
        for (int i = 0; i < Num.Length; i++)
        {
            PrevNumber.text = PrevNumber.text + Num[i].text;
        }

        //StrikeNumber.text = Strike.ToString();
        //BallNumber.text = Ball.ToString();

        GameObject obj = Instantiate(RecordObj);
        obj.transform.SetParent(List);
        obj.transform.localScale = new Vector2(1, 1);

        List.transform.localPosition = new Vector2(0, tryNumber * 40);
        obj.transform.position = new Vector2(0, 280);

        obj.transform.localPosition = new Vector2(180, obj.transform.localPosition.y);

        obj.SetActive(true);
        tryNumber++;

        Message.text = "이제 " + (tryNumber + 1) + "번째 시도야!";
        /*
        if (tryNumber < 10)
            Message.text = "이제 " + tryNumber + "번째 시도야!";
        else if (tryNumber < 20)
            Message.text = "좀 더 해봐 " + tryNumber + "번째 시도야";
        else
            Message.text = "어려워? " + tryNumber + "번째 시도야";
        */

        if (Strike == Difficulty)
        {
            myFlash.Play(false);
            gameMode = eGameMode.Clear;
            OpeningPanel.SetActive(false);
            MainPanel.SetActive(false);
            ClearPanel.SetActive(true);

            ClearAnswer.text = "";

            ClearTime.text = TimeText.text;
            for (int i = 0; i < Answer.Length; i++)
            {
                ClearAnswer.text += Answer[i] + " ";
            }


            if (tryNumber < 6 + Difficulty)
                ClearGrade.text = "님 좀 천재인듯?";
            else
                ClearGrade.text = "수고했어요!";

            for (int i = 0; i < Num.Length; i++)
            {
                Destroy(Num[i].gameObject);
            }



            int childCount = List.childCount;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    Destroy(List.GetChild(i).gameObject);
                }
            }

        }



        for (int i = 0; i < Num.Length; i++)
        {
            //Num[i] = GameObject.Find("Num" + i).GetComponent<Text>();
            Num[i].text = "?";
        }


        CurrentNumber = 0;


    }


    // Update is called once per frame
    void Update()
    {
        switch (gameMode)
        {
            case eGameMode.Opening:
                Opening();
                break;
            case eGameMode.Main:
                Main();
                break;
            case eGameMode.Clear:
                Clear();
                break;
        }
    }


    void Opening()
    {
        if (gameMode != eGameMode.Opening)
            return;

        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            Difficulty = 2;
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            Difficulty = 3;
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            Difficulty = 4;
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
            Difficulty = 5;
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
            Difficulty = 6;
        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
            Difficulty = 7;

        DifficultyText.text = Difficulty.ToString();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            gameMode = eGameMode.Main;
            OpeningPanel.SetActive(false);
            MainPanel.SetActive(true);
            ClearPanel.SetActive(false);
            InitializeGame();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    void Main()
    {
        if (gameMode != eGameMode.Main)
            return;

        playTime += Time.deltaTime;
        int sec = (int)playTime % 60; //60으로 나눈 나머지 
        int min = (int)(playTime / 60f);
        int hour = (int)(playTime / 3600f);


        TimeText.text = hour.ToString("00") + ":" +  min.ToString("00") + ":" + sec.ToString("00"); //ToString 뒤에 "00"붙이는건 강제 자리수 형식임


        int nowNum = -1;

        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
            nowNum = 0;
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            nowNum = 1;
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            nowNum = 2;
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            nowNum = 3;
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            nowNum = 4;
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
            nowNum = 5;
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
            nowNum = 6;
        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
            nowNum = 7;
        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
            nowNum = 8;
        if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
            nowNum = 9;


        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.DownArrow) || wheel < 0)
        {
            List.transform.localPosition = new Vector2(0, List.transform.localPosition.y + 40);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || wheel > 0)
        {
            List.transform.localPosition = new Vector2(0, List.transform.localPosition.y - 40);
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Num[CurrentNumber].color = Color.white;
            CurrentNumber -= 1;
            if (CurrentNumber < 0)
                CurrentNumber = Num.Length - 1;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Num[CurrentNumber].color = Color.white;
            CurrentNumber += 1;
            if (CurrentNumber >= Num.Length)
                CurrentNumber = 0;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameMode = eGameMode.Opening;
            OpeningPanel.SetActive(true);
            MainPanel.SetActive(false);
            ClearPanel.SetActive(false);

            for (int i = 0; i < Num.Length; i++)
            {
                Destroy(Num[i].gameObject);
            }

            int childCount = List.childCount;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    Destroy(List.GetChild(i).gameObject);
                }
            }
        }


#if UNITY_IPHONE

        if (Input.touches.Length > 0)
        {
            //Debug.Log("Touch");
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //Debug.Log("Touch Start");
                TouchStart = t.position;
                TouchRecent = TouchStart;
            }
            else if (t.phase == TouchPhase.Moved)
            {
                //Debug.Log("Touch Move");
                float gap = t.position.y - TouchRecent.y;
                TouchRecent = t.position;
                List.transform.localPosition = new Vector2(0, List.transform.localPosition.y + gap * ScreenRate); 
            }
        }
#endif




        Color currentColor = new Color(1.0f, Mathf.Abs(Mathf.Sin(Time.time * 10)), Mathf.Abs(Mathf.Sin(Time.time * 10)));
        Num[CurrentNumber].color = currentColor;

        if (nowNum >= 0)
        {
            for (int i = 0; i < Num.Length; i++)
            {
                Num[i].color = Color.white;
            }

            Num[CurrentNumber].text = "" + nowNum;//nowNum.ToString();

            CurrentNumber++;

            if (CurrentNumber >= Num.Length)
            {
                CurrentNumber = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckNumber();
        }
    }

    void Clear()
    {
        ClearNum.text = tryNumber + "번째 시도";

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape))
        {

            gameMode = eGameMode.Opening;
            OpeningPanel.SetActive(true);
            MainPanel.SetActive(false);
            ClearPanel.SetActive(false);


        }
    }

    public void SetNumber(int num)
    {
        EventSystem.current.SetSelectedGameObject(gameObject); // 커서 위치가 남아서 엔터 누르면 같은걸 찍어주는걸 방지하기 위해 이 윈도우로 포커스를 옮김

        Debug.Log("Button " + num);


        for (int i = 0; i < Num.Length; i++)
        {
            Num[i].color = Color.white;
        }

        Num[CurrentNumber].text = num.ToString();

        CurrentNumber++;

        if (CurrentNumber > Num.Length - 1)
        {
            CurrentNumber = 0;
        }
    }

    public void ClickEnter()
    {
        EventSystem.current.SetSelectedGameObject(gameObject); // 커서 위치가 남아서 엔터 누르면 같은걸 찍어주는걸 방지하기 위해 이 윈도우로 포커스를 옮김
        Debug.Log("Enter Button ");
        CheckNumber();
    }


    public void SetCurrentNumber(int num)
    {
        EventSystem.current.SetSelectedGameObject(gameObject); // 커서 위치가 남아서 엔터 누르면 같은걸 찍어주는걸 방지하기 위해 이 윈도우로 포커스를 옮김
        Debug.Log(num);
        Num[CurrentNumber].color = Color.white;
        CurrentNumber = num;
    }

    public void SetDifficulty(int num)
    {
        EventSystem.current.SetSelectedGameObject(gameObject); // 커서 위치가 남아서 엔터 누르면 같은걸 찍어주는걸 방지하기 위해 이 윈도우로 포커스를 옮김
        Difficulty = num;

        DifficultyText.text = Difficulty.ToString();

        
    }

    public void EnterDifficulty()
    {
        EventSystem.current.SetSelectedGameObject(gameObject); // 커서 위치가 남아서 엔터 누르면 같은걸 찍어주는걸 방지하기 위해 이 윈도우로 포커스를 옮김
        gameMode = eGameMode.Main;
        OpeningPanel.SetActive(false);
        MainPanel.SetActive(true);
        ClearPanel.SetActive(false);
        InitializeGame();
    }

    public void EnterRestart()
    {
        EventSystem.current.SetSelectedGameObject(gameObject); // 커서 위치가 남아서 엔터 누르면 같은걸 찍어주는걸 방지하기 위해 이 윈도우로 포커스를 옮김
        gameMode = eGameMode.Opening;
        OpeningPanel.SetActive(true);
        MainPanel.SetActive(false);
        ClearPanel.SetActive(false);
    }

}

