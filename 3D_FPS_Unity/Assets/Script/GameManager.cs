using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public int killPlayer;
    public int killNpc1;
    public int killNpc2;
    public int killNpc3;

    public int deadPlayer;
    public int deadNpc1;
    public int deadNpc2;
    public int deadNpc3;

    private int enemyCount;
    private bool gameOver;

    [Header("勝利/失敗")]
    public Text winOrLose;
    [Header("資料 - 玩家")]
    public Text textPlayer;
    [Header("資料 - 電腦1")]
    public Text textNpc1;
    [Header("資料 - 電腦2")]
    public Text textNpc2;
    [Header("資料 - 電腦3")]
    public Text textNpc3;
    [Header("結算畫面顯示")]
    public CanvasGroup group;

    private void Update()
    {
        Replay();
    }
    public void UpdateDataKill(ref int kill, Text textKill, string content, int dead)
    {
        kill++;
        textKill.text = content + kill + "           " + dead;
    }

    public void UpdateDataDead(int kill, Text textDead, string content, ref int dead)
    {
        dead++;
        textDead.text = content + kill + "           " + dead;

        if (content == "") StartCoroutine(ShowFinal());
        else if (content.Contains(" "))
        {
            enemyCount++;

            if (enemyCount == 3) StartCoroutine(ShowFinal());
        }

    }

    /// <summary>
    /// 顯示結束畫面
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowFinal()
    {
        float a = group.alpha;

        while (a < 1)
        {
            a += 0.1f;
            group.alpha = a;
            yield return new WaitForSeconds(0.1f);

        }
        gameOver = true;
    }

    private void Replay()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameOver) SceneManager.LoadScene("遊戲場景");
    }
}


