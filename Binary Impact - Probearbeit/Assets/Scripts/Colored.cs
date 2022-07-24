using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Colored", order = 1)]
public class Colored : ScriptableObject
{
    public Color color = Color.red;


    public bool IsSame(Colored colored)
    {
        return ReferenceEquals(colored, this);
    }
}