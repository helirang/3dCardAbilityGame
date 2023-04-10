using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    List<int> numArr = new List<int>();
    [HideInInspector]
    public List<int> BackUpnumArr = new List<int>();
    [HideInInspector]
    public List<int> signArr = new List<int>();
    List<bool> signBoolArr = new List<bool>();
    [HideInInspector]
    public int numCount = 4;
    int answer = 0;

    /// <summary>
    /// 계산 결과 값
    /// </summary>
    public int Answer
    {
        get { return answer; }
        set 
        { 
            answer = value;
        }
    }

    private void Initialized()
    {
        numArr.Clear();
        BackUpnumArr.Clear();
        signArr.Clear();
        signBoolArr.Clear();
    }
    public void CalculationStart()
    {
        Initialized();
        for (int i = 0; i < numCount; i++)
        {
            numArr.Add(Random.Range(1, 10));
            BackUpnumArr.Add(numArr[i]);
            if (i < numCount - 2) SetSign(true);
            if (i == numCount - 2) SetSign(false);
        }
        SetStart(true);
        SetStart(false);
        //Debug.Log(BackUpnumArr.Count);
        //Debug.Log(signArr.Count);
        string debug = "";
        for (int i = 0; i < numCount; i++)
        {
            debug += " " + BackUpnumArr[i];
            if (i < numCount - 1) debug += " " + ChangeSign(signArr[i]);
            else debug += " = " + answer;
        }
        //@정답 로그
        Debug.Log(debug);
    }

    string ChangeSign(int num)
    {
        string result = "";
        switch (num)
        {
            case 0:
                result = "+";
                break;
            case 1:
                result = "-";
                break;
            case 2:
                result = "x";
                break;
            case 3:
                result = "%";
                break;
        }
        return result;
    }

    void SetSign(bool isLast)
    {
        if (isLast)
        {
            signArr.Add(Random.Range(0, 3));
            signBoolArr.Add(false);
        }
        else
        {
            signArr.Add(Random.Range(0, 4));
            signBoolArr.Add(false);
        }
    }

    void SetStart(bool isFirst)
    {
        for(int i=0; i< signArr.Count; i++)
        {
            if (signBoolArr[i]) continue;
            int sign = signArr[i];
            if (isFirst && sign < 2) continue;
            if (!isFirst && sign >= 2) continue;
            int rightNum = GetRightNum(i+1);
            int result = 0;
            //Debug.Log($"i : {numArr[i]} || right : {numArr[rightNum]}");
            //Debug.Log($"{numArr[i]} {ChangeSign(sign)} {numArr[rightNum]}");
            switch (sign)
            {
                case 0:
                    result = Plus(numArr[i], numArr[rightNum]);
                    break;
                case 1:
                    result = Subtract(numArr[i], numArr[rightNum]);
                    break;
                case 2:
                    result = Multiply(numArr[i], numArr[rightNum]);
                    break;
                case 3:
                    int dividNum = Divid(numArr[i]);
                    if (dividNum == 0)
                    {
                        signArr[i] = 2;
                        result = Multiply(numArr[i], numArr[rightNum]);
                    }
                    else
                    {
                        numArr[rightNum] = dividNum;
                        BackUpnumArr[rightNum] = dividNum;
                        //Debug.Log($"fix Divided: {numArr[rightNum]}");
                        result = Divide(numArr[i], numArr[rightNum]);
                    }
                    break;
            }
            numArr[rightNum] = result;
            signBoolArr[i] = true;
            //Debug.Log($" = {result}");
            Answer = result;
        }
    }

    int Plus(int left, int right)
    {
        return left + right;
    }

    int Subtract(int left, int right)
    {
        return left - right;
    }

    int Multiply(int left, int right)
    {
        return left * right;
    }

    int Divide(int left, int right)
    {
        return left / right;
    }

    int GetRightNum(int idx)
    {
        int result = 0;
        //Debug.Log("요청 idx :" + idx);

        if (idx >= signBoolArr.Count) 
        {
            return signBoolArr.Count;
        }

        result = signBoolArr[idx] ? idx + 1 : idx;
        //if (signBoolArr[idx])
        //{
            //@todo 오류 발견 7+9*6*7 =61 || 정상 : 387
            for (int i = idx; i < signBoolArr.Count; i++)
            {
                if (!signBoolArr[i])
                {
                    result = i;
                    break;
                }
                else
                {
                    result = i + 1;
                }
            }
        //}
        //Debug.Log("결과 idx :" +result);
        return result;
    }

    int Divid(int num)
    {
        int result = 0;
        
        for(int i=9; 2 < i; i--)
        {
            if (num % i == 0)
            {
                result = i;
                break;
            }
        }
        return result;
    }
}
