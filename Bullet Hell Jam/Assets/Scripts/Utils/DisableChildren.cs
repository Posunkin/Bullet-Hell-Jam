using UnityEngine;

public class DisableChildren : MonoBehaviour
{
    [SerializeField] private int _childrenToStay;
    [SerializeField] private float _chance;
    
    private void Start()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        int count = children.Length - 1;
        int childrenDisabled = 0;
        
        foreach (Transform child in children)
        {
            if (child == this) continue;
            if (Random.value > _chance)
            {
                child.gameObject.SetActive(false);
                childrenDisabled++;
                if (count - childrenDisabled == _childrenToStay)
                {
                    break;
                }
            }
        }
    }
}
