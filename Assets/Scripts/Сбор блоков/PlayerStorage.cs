using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerInput))]
public class PlayerStorage : MonoBehaviour
{
    public int limited;
    [SerializeField] private Transform storage;
    [SerializeField] private GameObject counterBlocks;
    [SerializeField] private float unloadingTime = 0.1f;

    public List<WheatBox> Wheats { get; private set; }

    private Transform _parentPickup;
    private float _timer;
    private PlayerInput _playerInput;
    private Vector2 _input;

    private void Start()
    {
        Wheats = new List<WheatBox>();
        _playerInput = GetComponent<PlayerInput>();
    }

    public void AddItem(GameObject item)
    {
        item.GetComponent<BoxCollider>().enabled = false;

        Transform objTransform = item.transform;
        objTransform.rotation = storage.rotation;

        if(_parentPickup == null)
        {
            _parentPickup = objTransform;
            _parentPickup.position = storage.position;
            _parentPickup.parent = storage;
        }
        else
        {
            objTransform.position = _parentPickup.position;
            objTransform.position += Vector3.up * (_parentPickup.localScale.y);
            _parentPickup = objTransform;
            _parentPickup.parent = storage;
        }

        UpdateList();
    }

    private void Update()
    {
        
        _input.x = _playerInput.Horizontal;
        _input.y = _playerInput.Vertical;
        
        if (_input != Vector2.zero)
        {
            _timer += Time.deltaTime * gameObject.GetComponent<PlayerController>().SpeedPlayer;
            storage.localRotation = Quaternion.Euler(new Vector3(0, 0, 0 + Mathf.Sin(_timer) * 4));
        }
        else
            storage.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void UpdateList()
    {
        Wheats.Clear();

        for (int i = 0; i < storage.childCount; i++)
        {
            Wheats.Add(storage.GetChild(i).GetComponent<WheatBox>());
            counterBlocks.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Wheats.Count.ToString() + "/" + limited;
        }

        counterBlocks.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Wheats.Count.ToString() + "/" + limited;

        if (Wheats.Count >= 1)
            counterBlocks.GetComponent<Animator>().SetBool("BoolCounterBlocks", true);
        else
            counterBlocks.GetComponent<Animator>().SetBool("BoolCounterBlocks", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Barn>() && Wheats.Count >= 1)
        {
            float timeBox = 0;
            for (int i = 0; i < Wheats.Count; i++)
            {
                timeBox += unloadingTime;
                Wheats[i].ActivateMoveToBarn(other.GetComponent<Barn>(), timeBox);
            }
        }
    }
}
