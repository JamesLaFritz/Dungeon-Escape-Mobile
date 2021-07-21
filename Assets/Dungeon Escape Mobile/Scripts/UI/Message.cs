using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

[RequireComponent(typeof(Animator))]
public class Message : MonoBehaviour
{
    private Text m_text;
    private bool m_hasText;
    private Animator m_animator;
    private static readonly int Display1 = Animator.StringToHash("Display");

    private void Start()
    {
        m_text = GetComponentInChildren<Text>();
        m_hasText = m_text != null;
    }

    public void Display(string message)
    {
        if (m_hasText)
        {
            Debug.Assert(m_text != null, nameof(m_text) + " != null");
            m_text.text = message;
        }

        Debug.Assert(m_animator != null, nameof(m_animator) + " != null");
        m_animator.SetTrigger(Display1);
    }
}
