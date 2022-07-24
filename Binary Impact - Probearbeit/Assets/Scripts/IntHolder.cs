using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/IntHolder", order = 1)]
public class IntHolder : ScriptableObject
{
    [SerializeField]
    private int _value;

    public int value
    {
        get
        {
            return _value;
        }
        set
        {
            if (value != _value)
                OnValueChanged?.Invoke(value);
            
            _value = value;
            
        }
    }

    public UnityEvent<int> OnValueChanged;
    
}