using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StateMachine : MonoBehaviour
{
    BaseState currentState;

    private GameObject agent;

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

    public void GetGameObject(GameObject agent)
    {
        this.agent = agent;
    }

    private void OnGUI()
    {
        if (agent != null)
        {
            Vector3 agentPosition = agent.transform.position; // получаем позицию агента
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(agentPosition); // переводим позицию агента в позицию на экране
            float yOffset = 50f; // смещение по оси Y относительно агента
            Vector2 labelPosition = new Vector2(screenPosition.x, Screen.height - screenPosition.y - yOffset); // позиция текста на экране

            string content = currentState != null ? currentState.name : "(no current state)";

            GUI.Label(new Rect(labelPosition.x - 50, labelPosition.y, 200, 50), content); // отображаем текст над агентом
        }
        
    }

    /*private void OnGUI() //тут рисуем состояние в верхнем левом углу
    {
        string content = currentState != null ? currentState.name : "(no current state)"; //если currentstate не null то присваиваем его значение, а если null то присваиваем no current state
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>"); //размер и цвет стринга контент

    }*/
}
