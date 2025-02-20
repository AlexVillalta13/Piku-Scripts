using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    private const string elementName = "ComboNumberUI";
    
    PlayerInput playerInput;
    InputAction touchPressedAction;
    [SerializeField] GameEvent touchPressedEvent;
    private bool touchEnabled = true;
    
    [SerializeField] UIDocument gameplayUIDocument;
    private VisualElement rootVisualElement;
    private VisualElement rootComboNumber;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressedAction = playerInput.actions.FindAction("TouchPressed");
        
        rootVisualElement = gameplayUIDocument.rootVisualElement;
        rootComboNumber = gameplayUIDocument.rootVisualElement.Query<VisualElement>(name: elementName).First();
    }

    private void OnEnable()
    {
        touchPressedAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        touchPressedAction.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        if(touchEnabled == false) {return;}
        
        Vector2 screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(rootVisualElement.panel, screenPos);

        // Correction because panel has inverted Y
        panelPos.y = rootVisualElement.resolvedStyle.height - panelPos.y;
        
        VisualElement elementUnderPointer = rootVisualElement.panel.Pick(panelPos);
        if (elementUnderPointer is Button button || IsChildOf(elementUnderPointer, rootComboNumber))
        {
            return;
        }
        
        touchPressedEvent.Raise(new Empty(), this);
    }
    
    private bool IsChildOf(VisualElement child, VisualElement potentialParent)
    {
        // Recorremos hacia arriba en la jerarquía hasta que no haya más padres
        while (child != null)
        {
            if (child == potentialParent)
            {
                return true; // Encontramos al padre
            }

            child = child.parent; // Continuamos con el padre del actual
        }

        return false; // No encontramos al padre en la jerarquía
    }

    public void EnableTouch()
    {
        touchEnabled = true;
    }

    public void DisableTouch()
    {
        touchEnabled = false;
    }
}
