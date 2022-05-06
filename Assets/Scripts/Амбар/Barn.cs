using UnityEngine;

public class Barn : MonoBehaviour
{
    public Transform point;
    [SerializeField] private MoneyManager moneyManager;

    public void BlockSale(WheatBox obj)
    {
        moneyManager.Animate(point.position, obj.Money);
        Destroy(obj.gameObject);
    }
}
