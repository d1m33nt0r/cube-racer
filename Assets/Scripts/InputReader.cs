using System;
using System.Collections;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public event Action LeftButtonPressed;
    public event Action RightButtonPressed;
    public event Action UpButtonPressed;
    
    public event Action LeftButtonReleased;
    public event Action RightButtonReleased;
    public event Action UpButtonReleased;

    private bool _previousLeftButtonStatusIsPressed;
    private bool _previousRightButtonStatusIsPressed;
    private bool _previousUpButtonStatusIsPressed;

    public bool PreviousLeftButtonStatusIsPressed => 
        _previousLeftButtonStatusIsPressed;
    public bool PreviousRightButtonStatusIsPressed => 
        _previousRightButtonStatusIsPressed;
    public bool PreviousUpButtonStatusIsPressed => 
        _previousUpButtonStatusIsPressed;
    
#if UNITY_EDITOR
    private IEnumerator _inputReadCoroutine;
#endif
    
    private void Construct(GameController gameController)
    {
        gameController.StartedGame += StartReadInput;
        gameController.ContinuedGame += StartReadInput;
        
        gameController.FinishedGame += StopReadInput;
        gameController.FailedGame += StopReadInput;
        gameController.PausedGame += StopReadInput;
        
#if UNITY_EDITOR
        _inputReadCoroutine = ReadInput();
#endif
    }
    
    public void LeftButtonPress()
    {
        _previousLeftButtonStatusIsPressed = true;
        LeftButtonPressed?.Invoke();
        Debug.Log("Left Button Pressed");
    }
    
    public void RightButtonPress()
    {
        _previousRightButtonStatusIsPressed = true;
        RightButtonPressed?.Invoke();
        Debug.Log("Right Button Pressed");
    }
    
    public void UpButtonPress()
    {
        _previousUpButtonStatusIsPressed = true;
        UpButtonPressed?.Invoke();
        Debug.Log("Up Button Pressed");
    }
    
    public void LeftButtonRelease()
    {
        _previousLeftButtonStatusIsPressed = false;
        LeftButtonReleased?.Invoke();
        Debug.Log("Left Button Released");
    }
    
    public void RightButtonRelease()
    {
        _previousRightButtonStatusIsPressed = false;
        RightButtonReleased?.Invoke();
        Debug.Log("Right Button Released");
    }
    
    public void UpButtonRelease()
    {
        _previousUpButtonStatusIsPressed = false;
        UpButtonReleased?.Invoke();
        Debug.Log("Up Button Released");
    }
    
    private void StartReadInput()
    {
#if UNITY_EDITOR
        StartCoroutine(_inputReadCoroutine);
#endif
    }

    private void StopReadInput()
    {
#if UNITY_EDITOR
        StopCoroutine(_inputReadCoroutine);
#endif
    }
    
#if UNITY_EDITOR
    private IEnumerator ReadInput()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                LeftButtonPress();
            }
            else
            {
                if (_previousLeftButtonStatusIsPressed)
                {
                    LeftButtonRelease();
                }
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                RightButtonPress();
            }
            else
            {
                if (_previousRightButtonStatusIsPressed)
                {
                    RightButtonRelease();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UpButtonPress();
            }
            else
            {
                if (_previousUpButtonStatusIsPressed)
                {
                    UpButtonRelease();
                }
            }

            yield return null;
        }
    }
#endif
}
