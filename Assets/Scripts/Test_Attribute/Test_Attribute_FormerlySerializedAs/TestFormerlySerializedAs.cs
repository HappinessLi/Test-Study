using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TestFormerlySerializedAs : MonoBehaviour
{
    [FormerlySerializedAs("m_oldArgs")]
    [SerializeField]
    private Button m_NewArgs;

    private void Start()
    {
        m_NewArgs = gameObject.GetComponent(typeof(Button)) as Button;
    }

    private void Awake()
    {
    }
}
