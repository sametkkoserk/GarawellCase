using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class ReactionController : MonoBehaviour
{
  public static ReactionController instance;
  [SerializeField] private GameObject parentObj;

  [SerializeField]
  private List<TextMeshProUGUI> countTexts;
  public int comboCounter { get; private set; }
  private bool isAnimating = false;
  public Action<int> onComboMade=delegate(int i) {  };

  private void Awake()
  {
    instance = this;
    comboCounter = -1;
  }

  public void OnSquareMade()
  {
    comboCounter++;
    onComboMade.Invoke(comboCounter+1);

    if (comboCounter<=0)return;
    StartCoroutine(AnimateCombo());
  }

  private IEnumerator AnimateCombo()
  {
    if (isAnimating)yield break;

    isAnimating = true;

    for (int i = 0; i < countTexts.Count; i++)
    {
      countTexts[i].text = comboCounter.ToString();
    }

    LeanTween.moveY(parentObj, parentObj.transform.position.y + 150, 0.3f);
    LeanTween.scale(parentObj, Vector3.one, 0.3f);
    yield return new WaitForSeconds(1f);
    LeanTween.moveY(parentObj, parentObj.transform.position.y - 150, 0.3f);
    LeanTween.scale(parentObj, Vector3.zero, 0.3f).setOnComplete(o1 => { isAnimating = false;});
  }

  public void OnSquareCouldntMade()
  {
    comboCounter = -1;
  }
}
