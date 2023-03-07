using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    

    private void Start()
    {
        currentState = GetInitialState(); //берем начальное состояние назначенным методом
        if (currentState != null) //и если начальное состояние действительно есть
            currentState.Enter(); //то входим в его начальный метод
    }

    private void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic(); //логика АИ будет обрабатываться каждый кадр
    }

    private void FixedUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics(); //физика АИ будет обрабатываться каждый фиксированный кадр
    }

    public void ChangeState(BaseState newState) //через метод внутри состояния назначаем сюда новое состояние
    {
        currentState.Exit(); //проигрываем метод выхода в старом состоянии

        currentState = newState; //переходим в новое состояние
        currentState.Enter(); //проигрываем начальный метод его
    }

    protected virtual BaseState GetInitialState() //метод начального состояния - здесь пустой, для каждой стейт машины разный (нужно оверрайдить)
    {
        return null;
    }

    private void OnGUI() //тут рисуем состояние в верхнем левом углу
    {
        string content = currentState != null ? currentState.name : "(no current state)"; //если currentstate не null то присваиваем его значение, а если null то присваиваем no current state
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>"); //размер и цвет стринга контент

    }
}
