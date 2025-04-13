//Handles the debug console

using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class DebugController : MonoBehaviour
{
    private bool _showConsole;
    private bool _showHelp;

    private string _input;

    private float _lastTimeScale;

    public static DebugCommand HELP;

    public static DebugCommand KILL_ALL;
    public static DebugCommand GODMODE;
    public static DebugCommand FREECAM;

    public static DebugCommand<float> SET_PLAYER_MOVESPEED;
    public static DebugCommand<float> SET_PLAYER_JUMPFORCE;
    public static DebugCommand<float> SET_PLAYER_HEALTH;
    public static DebugCommand<float> SET_PLAYER_LIGHTATTACK;
    public static DebugCommand<float> SET_PLAYER_HEAVYATTACK;
    public static DebugCommand<float> SET_PLAYER_DODGESPEED;
    public static DebugCommand<float> SET_PLAYER_DODGEDURATION;
    public static DebugCommand<float> SET_PLAYER_DODGECOOLDOWN;

    public static DebugCommand<float> SET_BASE_ENEMY_MOVESPEED;
    public static DebugCommand<float> SET_BASE_ENEMY_HEALTH;
    public static DebugCommand<float> SET_BASE_ENEMY_ATTACK;

    public static DebugCommand<float> SET_SUMMONER_HEALTH;

    public static DebugCommand<float> SET_BRUTE_MOVESPEED;
    public static DebugCommand<float> SET_BRUTE_HEALTH;
    public static DebugCommand<float> SET_BRUTE_ATTACK;

    public static DebugCommand<float> SET_MAGE_MOVESPEED;
    public static DebugCommand<float> SET_MAGE_HEALTH;
    public static DebugCommand<float> SET_MAGE_PROJECTILE_SPEED;
    public static DebugCommand<float> SET_MAGE_PROJECTILE_ATTACK;

    public List<object> CommandList;

    private HealthSystem _healthSystem;

    private Vector2 _scroll;

    void Awake()
    {
        _healthSystem = GameObject.FindGameObjectWithTag("healthSystem").GetComponent<HealthSystem>();
    }

    void Start()
    {
        HELP = new DebugCommand("help", "Shows full list of commands", "help", () =>
        {
            _showHelp = true;
        });

        KILL_ALL = new DebugCommand("kill_all", "Removes all enemies from the level", "kill_all", () =>
        {
            foreach (GameObject enemy in GameManager.Instance.AmountOfEnemies)
            {
                enemy.GetComponent<BaseEnemyHealth>().CurrentHealth = 0;
            }
        });

        GODMODE = new DebugCommand("godmode", "Toggle Godmode", "godmode", () =>
        {
            _healthSystem.CanTakeDamage = !_healthSystem.CanTakeDamage;
        });

        FREECAM = new DebugCommand("freecam", "Toggle Freecam", "freecam", () =>
        {
            GameManager.Instance.FreeCam = !GameManager.Instance.FreeCam;
        });

        SET_PLAYER_MOVESPEED = new DebugCommand<float>("set_player_movespeed", "Sets the Player's movement speed (Default = 5)", "set_player_movespeed <float>", (x) =>
        {
            GameManager.Instance.PlayerMoveSpeed = x;
        });

        SET_PLAYER_HEALTH = new DebugCommand<float>("set_player_health", "Sets the Player's health (Default = 100)", "set_player_health <float>", (x) =>
        {
            GameManager.Instance.PlayerHealth = x;
        });

        SET_PLAYER_JUMPFORCE = new DebugCommand<float>("set_player_jumpforce", "Sets the Player's jump force (Default = 5)", "set_player_jumpforce <float>", (x) =>
        {
            GameManager.Instance.PlayerJump = x;
        });

        SET_PLAYER_LIGHTATTACK = new DebugCommand<float>("set_player_lightattack", "Sets the Player's light attack (Default = 20)", "set_player_lightattack <float>", (x) =>
        {
            GameManager.Instance.PlayerLightAttack = x;
        });

        SET_PLAYER_HEAVYATTACK = new DebugCommand<float>("set_player_heavyattack", "Sets the Player's heavy attack (Default = 30)", "set_player_heavyattack <float>", (x) =>
        {
            GameManager.Instance.PlayerHeavyAttack = x;
        });

        SET_PLAYER_DODGESPEED = new DebugCommand<float>("set_player_dodgespeed", "Sets the Player's dodge speed (Default = 10)", "set_player_dodgespeed <float>", (x) =>
        {
            GameManager.Instance.PlayerDodgeSpeed = x;
        });

        SET_PLAYER_DODGECOOLDOWN = new DebugCommand<float>("set_player_dodgecooldown", "Sets the Player's dodge speed (Default = 1)", "set_player_dodgecooldown <float>", (x) =>
        {
            GameManager.Instance.PlayerDodgeCooldown = x;
            GameManager.Instance.Player.GetComponent<PlayerMovement>().DodgeCooldownTimer = x;
        });

        SET_PLAYER_DODGEDURATION = new DebugCommand<float>("set_player_dodgeduration", "Sets the Player's dodge speed (Default = 0.5)", "set_player_dodgeduration <float>", (x) =>
        {
            GameManager.Instance.PlayerDodgeDuration = x;
        });

        SET_BASE_ENEMY_MOVESPEED = new DebugCommand<float>("set_base_enemy_movespeed", "Sets every Base Enemy's movespeed (Default = 2.5)", "set_base_enemy_movespeed <float>", (x) =>
        {
            GameManager.Instance.BaseEnemyMoveSpeed = x;
            foreach (GameObject enemy in GameManager.Instance.ListOfBaseEnemies)
            {
                enemy.GetComponent<BaseEnemyMovement>().DebugMoveSpeed();
            }
        });

        SET_BASE_ENEMY_HEALTH = new DebugCommand<float>("set_base_enemy_health", "Sets every Base Enemy's health (Default = 100)", "set_base_enemy_health <float>", (x) =>
        {
            GameManager.Instance.BaseEnemyHealth = x;
            foreach(GameObject enemy in GameManager.Instance.ListOfBaseEnemies)
            {
                enemy.GetComponent<BaseEnemyHealth>().DebugChangeHealth();
            }
        });

        SET_BASE_ENEMY_ATTACK = new DebugCommand<float>("set_base_enemy_attack", "Sets every Base Enemys attack (Default = 10)", "set_base_enemy_attack <float>", (x) =>
        {
            GameManager.Instance.BaseEnemyAttack = x;
        });


        CommandList = new List<object>()
        {
            HELP,
            KILL_ALL,
            GODMODE,
            FREECAM,
            SET_PLAYER_MOVESPEED,
            SET_PLAYER_HEALTH,
            SET_PLAYER_JUMPFORCE,
            SET_PLAYER_DODGECOOLDOWN,
            SET_PLAYER_DODGEDURATION,
            SET_PLAYER_DODGESPEED,
            SET_PLAYER_LIGHTATTACK,
            SET_PLAYER_HEAVYATTACK,
            SET_BASE_ENEMY_MOVESPEED,
            SET_BASE_ENEMY_HEALTH,
            SET_BASE_ENEMY_ATTACK
        };

    }

    public void OnToggleConsole(CallbackContext context)
    {
        _showConsole = !_showConsole;

        if (_showConsole)
        {
            GameManager.Instance.IsPaused = true;
        }
        else
        {
            GameManager.Instance.IsPaused = false;
        }
    }

    public void OnReturn(CallbackContext value)
    {
        if (value.performed)
        {
            if (_showConsole)
            {
                HandleInput();
                _input = "";
            }
        }
    }

    void OnGUI()
    {
        if (!_showConsole)
        {
            return;
        }

        float y = 0f;

        if (_showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");
            Rect viewport = new Rect(0, 0, Screen.width - 30f, 20 * CommandList.Count);
            _scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), _scroll, viewport);
            for (int i = 0; i < CommandList.Count; i++)
            {
                DebugCommandBase command = CommandList[i] as DebugCommandBase;
                string label = $"{command.commandFormat} - {command.commandDescription}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }
            GUI.EndScrollView();
            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30f), "");
        GUI.backgroundColor = Color.black;
        _input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), _input);
    }

    void HandleInput()
    {
        string[] properties = _input.Split(" ");

        for(int i = 0; i < CommandList.Count; i++)
        {
            DebugCommandBase commandBase = CommandList[i] as DebugCommandBase;

            if (_input.Contains(commandBase.commandId))
            {
                if (CommandList[i] as DebugCommand != null)
                {
                    (CommandList[i] as DebugCommand).Invoke();
                }
                else if (CommandList[i] as DebugCommand<float> != null)
                {
                    (CommandList[i] as DebugCommand<float>).Invoke(float.Parse(properties[1]));
                }
                else if (CommandList[i] as DebugCommand<int> != null)
                {
                    (CommandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
            }
        }
    }
}
