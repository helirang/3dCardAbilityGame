using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubCalculator : MonoBehaviour
{
    List<int> numArr = new List<int>();

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
        signArr.Clear();
        signBoolArr.Clear();
    }
    public int CalculationStart(List<int> numberList, List<int> signList)
    {
        Initialized();
        numArr = new List<int>(numberList);
        signArr = signList;
        signBoolArr = new List<bool>(Enumerable.Repeat(false, signArr.Count).ToArray());
        SetStart(true);
        SetStart(false);
        return answer;
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
                    result = Divide(numArr[i], numArr[rightNum]);
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
}
