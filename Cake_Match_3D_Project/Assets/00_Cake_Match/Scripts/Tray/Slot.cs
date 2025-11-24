using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private CakeBox _currentCakeInSlot;
    public CakeBox CurrentCakeInSlot => _currentCakeInSlot;



    public void ClearCakeBox()
    {
        _currentCakeInSlot = null;
    }

    public bool isHaveCakeInTray()
    {
        if (_currentCakeInSlot) return true;

        return false;
    }
}
