using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DisableGameObj());
    }

    IEnumerator DisableGameObj()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
