using System.Collections.Generic;
using UnityEngine;

namespace Framework.GUI
{
    [CreateAssetMenu(menuName = "Configs/Game Windows")]
    public class GameWindows : ScriptableObject
    {
        [SerializeField] public List<WindowBase> Windows;
    }
}