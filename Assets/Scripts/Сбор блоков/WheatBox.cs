using System.Collections;
using UnityEngine;

public class WheatBox : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distanceToPlayer = 10;
    [SerializeField] private float timeToTake = 5;
    
    public int Money { get; private set; }

    private Transform _storage;
    private PlayerStorage _playerStorage;
    private float _distancePlayer;
    private bool _isTake;

    private Barn _barn;
    private bool _isMoveToBarn;
    private float _distanceBarnPoint;
    private float _timeMoveToBarn;

    private void Awake()
    {
        _storage = GameObject.Find("Storage").transform;
        _playerStorage = GameObject.Find("Player").GetComponent<PlayerStorage>();
        StartCoroutine(UpWheat());
    }

    private void Update()
    {
        if (_isTake && _playerStorage.Wheats.Count < _playerStorage.limited)
            MoveToStorage();

        if (_isMoveToBarn)
            MoveToBarn();
    }

    public void SetMoney(int money) => Money = money; 

    private void MoveToStorage()
    {
        _distancePlayer = Vector3.Distance(_storage.position, transform.position);

        if (_distancePlayer <= distanceToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, _storage.position, speed * Time.deltaTime);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            if (_distancePlayer <= 1)
            {
                _playerStorage.GetComponent<PlayerStorage>().AddItem(gameObject);
                _isTake = false;
            }
        }
        else
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void MoveToBarn()
    {
        if (_timeMoveToBarn > 0)
            _timeMoveToBarn -= Time.deltaTime;

        if (_timeMoveToBarn <= 0)
        {
            gameObject.transform.parent = null;
            _playerStorage.GetComponent<PlayerStorage>().UpdateList();
            _distanceBarnPoint = Vector3.Distance(_barn.point.position, transform.position);

            transform.position = Vector3.MoveTowards(transform.position, _barn.point.position, speed * Time.deltaTime);

            if (_distanceBarnPoint < 1)
                _barn.BlockSale(gameObject.GetComponent<WheatBox>());
        }
    }

    public void ActivateMoveToBarn(Barn barn, float time)
    {
        _barn = barn;
        _timeMoveToBarn = time;
        _isMoveToBarn = true;
    }

    private IEnumerator UpWheat()
    {
        yield return new WaitForSeconds(timeToTake);
        _isTake = true;
    }
}
