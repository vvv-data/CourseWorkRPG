using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))] // вставляем скрипт Health, чтобы он не удалялся из компонентов противника
    public class CombatTarget : MonoBehaviour
    {

    }
}
