using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject _panelEnemyHp;
    [SerializeField] private Image _hpBarEnemy;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private float _disableTime;
    [SerializeField] private TextMeshProUGUI _nameEnemy;
    [SerializeField] private Image _pictureEnemy;

    [Header("Player")]
    [SerializeField] private Image _hpBarPlayer;

    private void Start()
    {
        _hpBarEnemy.color = _gradient.Evaluate(1f);
        _panelEnemyHp.SetActive(false);
    }

    public void UpdateHpBar(float hp, string name, Sprite picture)
    {
        StopCoroutine(nameof(DisablePanel));
        StartCoroutine(nameof(DisablePanel));

        _pictureEnemy.sprite = picture;
        _nameEnemy.text = name;
        _hpBarEnemy.fillAmount = hp;
        _hpBarEnemy.color = _gradient.Evaluate(hp);
        _panelEnemyHp.SetActive(true);
    }

    public void UpdateHpBarPlayer(float hp)
    {
        _hpBarPlayer.fillAmount = hp;
        _hpBarPlayer.color = _gradient.Evaluate(hp);
    }

    private IEnumerator DisablePanel()
    {
        yield return new WaitForSeconds(_disableTime);
        _panelEnemyHp.SetActive(false);
    }
}
