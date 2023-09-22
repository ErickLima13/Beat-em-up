using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _panelEnemyHp;
    [SerializeField] private Image _hpBar;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private float _disableTime;
    [SerializeField] private TextMeshProUGUI _nameEnemy;
    [SerializeField] private Image _pictureEnemy;

    private void Start()
    {
        _hpBar.color = _gradient.Evaluate(1f);
        _panelEnemyHp.SetActive(false);
    }

    public void UpdateHpBar(float hp, string name)
    {
        StopCoroutine(nameof(DisablePanel));
        StartCoroutine(nameof(DisablePanel));

        _nameEnemy.text = name;
        _hpBar.fillAmount = hp;
        _hpBar.color = _gradient.Evaluate(hp);
        _panelEnemyHp.SetActive(true);
    }

    private IEnumerator DisablePanel()
    {
        yield return new WaitForSeconds(_disableTime);
        _panelEnemyHp.SetActive(false);
    }
}
