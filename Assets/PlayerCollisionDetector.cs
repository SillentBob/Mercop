using Mercop.Core;
using Mercop.Core.Events;
using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    [SerializeField] private string helipadTag = "Helipad";

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"OnCollisionEnter {other}");
        if (other.gameObject.CompareTag(helipadTag))
        {
            EventManager.Invoke(new LandingEvent(other.gameObject));
        }
    }
}