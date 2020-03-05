using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BoardGame : MonoBehaviour
{
    public DotSlot bgPref;
    public Dot dotPref;
    public const int MAX_COL = 20;
    public const int MAX_COL_USE = MAX_COL / 4;
    public const int MAX_ROW = 20;
    public const int MAX_ROW_USE = MAX_ROW / 4;
    public bool touching;
    public Transform BackgroundContainer;
    public Transform DotContainer;
    private Dot[,] dots;
    private DotSlot[,] bgSlots;
    public int countDown;
    public bool IsSpecial;
    public Text timeCountDown;
    public bool allowClicking;
    IAlogism alogism;
    private void Awake()
    {
        DIContainer.SetModule<IAlogism, NormalAlogism>();
        alogism = DIContainer.GetModule<IAlogism>();
        dots = new Dot[MAX_COL, MAX_ROW];
        bgSlots = new DotSlot[MAX_COL, MAX_ROW];
        Setup();
        allowClicking = true;
    }
    public void Setup()
    {
        int count = 0;
        for (int i = 0; i < MAX_COL; i++)
        {
            for (int j = 0; j < MAX_ROW; j++)
            {
                Vector2 pos = new Vector2(i-2, j-2);
                GameObject initBG = Instantiate(bgPref.gameObject, pos, Quaternion.identity);
                bgSlots[i, j] = initBG.GetComponent<DotSlot>();
                bgSlots[i, j].transform.SetParent(BackgroundContainer);
                GameObject initDot = Instantiate(dotPref.gameObject, pos, Quaternion.identity);
                Dot dot = initDot.GetComponent<Dot>();
                dot.transform.SetParent(DotContainer);
                dot.Setup(i,j);
                dot.index = count;
                dots[i, j] = initDot.GetComponent<Dot>();
                count++;
                if (j >= MAX_ROW_USE || i >= MAX_COL_USE)
                {
                    bgSlots[i, j].gameObject.SetActive(false);
                    dots[i, j].gameObject.SetActive(false);
                }
            }
        }
    }

    Dot DotClicked(Vector2 screenPosition)
    {
        //Converting Mouse Pos to 2D (vector2) World Pos
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector2 rayPos = new Vector2(worldPos.x, worldPos.y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
        if (hit)
        {
            Dot dot = hit.transform.GetComponent<Dot>();
            if (dot != null)
                return dot;
        }
        return null;
    }

    private void touchEnd(Vector3 mousePosition)
    {
    }

    private void touchBegin(Vector3 mousePosition)
    {
        Dot dot = DotClicked(mousePosition);
        if (dot != null)
        {
            alogism.SetupAlogism(dot, dots, MAX_ROW_USE, MAX_COL_USE);
            StartCoroutine(Arranging());
        }
    }

    private void touchHold(Vector3 mousePosition)
    {
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        // Proces click
        #region
        if (allowClicking)
        {
            if (touching)
            {
                touchHold(Input.mousePosition);
            }
            if (Input.GetMouseButtonDown(0) && !touching)
            {
                touching = true;
                touchBegin(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                touching = false;
                touchEnd(Input.mousePosition);
            }
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    touching = true;
                    touchBegin(Input.GetTouch(0).position);
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    touchHold(Input.GetTouch(0).position);
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    touching = false;
                    touchEnd(Input.GetTouch(0).position);
                }
            }
        }
        #endregion

        if (IsSpecial)
        {
            Special();
        }
    }
    public void Special()
    {
        timeCountDown.gameObject.SetActive(true);
        IsSpecial = true;
        StartCoroutine(_special());
        IsSpecial = false;
    }
    IEnumerator _special()
    {
        countDown = 10;
        DIContainer.SetModule<IAlogism, Loang>();
        alogism = DIContainer.GetModule<IAlogism>();
        Debug.Log("Loang");
        TimeCountDown();
        yield return new WaitUntil(()=> countDown == 0);
        DIContainer.SetModule<IAlogism, NormalAlogism>();
        alogism = DIContainer.GetModule<IAlogism>();
        Debug.Log("Normal");
    }
   
    public void TimeCountDown()
    {
        StartCoroutine(_TimeCountDown());
    }
    IEnumerator _TimeCountDown()
    {
        timeCountDown.text = countDown.ToString();
        if (countDown <= 0)
        {
            countDown = 0;
            timeCountDown.gameObject.SetActive(false);
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            countDown--;
            TimeCountDown();
        }
    }
    public void Refill()
    {
        for (int col = 0; col < MAX_COL; col++)
        {
            for (int row = 0; row < MAX_ROW; row++)
            {
                if (dots[col, row].fill == 0)
                {
                    dots[col,row].Setup(col, row);
                }
            }
        }
    }

    public IEnumerator Arranging()
    {
        allowClicking = false;
        //yield return new WaitForSeconds(0.1f);
        int nullCount = 0;
        for (int col = 0; col < MAX_COL; col++)
        {
            for (int row = 0; row < MAX_ROW; row++)
            {
                //Debug.Log("col: " + col + ", row: " + row + " : " + dots[col, row].dot.enabled);
                if (dots[col, row].fill == 0)
                {

                    //Debug.Log("col: " + col+", row: " + row);
                    nullCount++;
                }
                else if(nullCount > 0)
                {
                    dots[col, row].Arranging(dots[col, row - nullCount], allowClicking);
                   // yield return new WaitUntil(() => !dots[_col, _row].dot.enabled);
                    //yield return new WaitForSeconds
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.2f);
        allowClicking = true;
        Refill();
    }
}
