using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class MoneyManager : MonoBehaviour
{
    public int money;
    [Space]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Transform IconMoney;
    [SerializeField] private float durationIconMoney = 1;
    [Space]
    [SerializeField] private GameObject animatedCoinPref;
    [SerializeField] private Transform pointCoin;
    [Space]
    [SerializeField] [Range(0.5f, 0.9f)] private float minAnimDuration;
    [SerializeField] [Range(0.9f, 2f)] private float maxAnimDuration;
    [SerializeField] private Ease easeType;

    private int _addMoney;
    private float _addTimeMoney;

    private void Awake()
    {
        text.text = money.ToString();
    }

    private void Update()
    {
        if(_addMoney > 0)
        {
            if (_addTimeMoney <= 0)
            {
                money += 1;
                _addMoney -= 1;
                text.text = money.ToString();
                StartCoroutine(ShakingIconMoney());
                _addTimeMoney = 0.04f;
            }
            else
                _addTimeMoney -= Time.deltaTime;
        }
    }

    public void Animate(Vector3 collectedCoinPosition, int amount)
    {
        GameObject coin;
        coin = Instantiate(animatedCoinPref);
        coin.transform.parent = transform;

        coin.transform.position = collectedCoinPosition;
        float duration = Random.Range(minAnimDuration, maxAnimDuration);
        coin.transform.DOMove(pointCoin.position, duration)
        .SetEase(easeType)
        .OnComplete(() => {
            AddMoney(amount);
            Destroy(coin);
        });
    }

    private void AddMoney(int amount)
    {
        _addMoney += amount;
        _addTimeMoney = 0.04f;
    }

    private IEnumerator ShakingIconMoney()
    {
        Vector3 startPosition = IconMoney.position;
        float elapsedTime = 0;

        while(elapsedTime < durationIconMoney)
        {
            elapsedTime += Time.deltaTime;
            IconMoney.position = startPosition + Random.insideUnitSphere;
            yield return null;
        }

        transform.position = startPosition;
    }
}
