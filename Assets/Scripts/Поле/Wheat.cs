using System.Collections;
using UnityEngine;

public class Wheat : MonoBehaviour
{
    [SerializeField] private ItemScriptableObject item;
    [SerializeField] private Transform point;
    [SerializeField] private float blockPower;

    private bool _cut;
    private GameObject _wheatObj;

    private void Awake()
    {
        _cut = true;
        _wheatObj = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "sickle" && _cut)
        {
            _wheatObj.SetActive(false);
            _cut = false;

            GameObject obj = Instantiate(item.boxPref, point.position, Quaternion.Euler(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
            obj.GetComponent<Rigidbody>().AddForce(obj.transform.up * blockPower, ForceMode.Impulse);
            obj.GetComponent<WheatBox>().SetMoney(item.money);

            StartCoroutine(ResetWheat());
        }
    }

    private IEnumerator ResetWheat()
    {
        yield return new WaitForSeconds(item.growthTime);
        _wheatObj.SetActive(true);
        _cut = true;
    }
}
