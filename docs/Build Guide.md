---
title: Pong — Build Guide
date: 2026-04-21
updated: 2026-04-23
type: guide
---

# Pong — Build Guide

A step-by-step guide to building Pong from a blank Unity project to a complete game. Written for someone who knows software but is new to Unity.

Each phase ends with a **checkpoint** — a testable state you can verify before moving on.

> **Prerequisites:** Unity 6.3 LTS installed, project opened in Unity Hub. See [README](../README.md).

---

## Phase 1: Scene Setup

Get the Game scene built with all visual elements in place. No scripts yet — just GameObjects, components, and layout.

### Step 1 — First-time scene config

When you first open the project, the default scene needs to be configured for 2D:

1. **Save the scene** — `Ctrl+Shift+S` > save to `Assets/Scenes/` > name it `Game`
2. **Switch Scene view to 2D** — click the **2D** button in the Scene view toolbar *(editor viewport only — does not make the game 2D, just changes how you see the scene)*
3. **Set camera to Orthographic** — select `Main Camera` in the Hierarchy > Inspector > **Projection** > change to `Orthographic`. This is what actually makes the game render in 2D — no perspective distortion.
4. **Check camera position** — while `Main Camera` is selected, verify Transform > Position is `(0, 0, -10)`. Unity sometimes defaults the Y to `1`, which offsets the entire play area and puts objects outside the camera view. Z should stay at `-10` so the camera renders objects sitting at Z=0.
5. **Delete the Directional Light** — right-click it in the Hierarchy > Delete *(3D lighting concept, not needed for 2D)*

### Step 2 — Walls

Walls are static collision surfaces — they only need a visual (Sprite Renderer) and a collider (BoxCollider2D). No Rigidbody needed because they never move.

1. Right-click in the Hierarchy > **Create Empty** > rename to `Walls`, position `(0, 0, 0)`
2. Right-click `Walls` > **2D Object > Sprites > Square** > rename to `TopWall`
   - Position: `(0, 4.5, 0)` | Scale: `(20, 0.5, 1)`
   - Add Component > **Box Collider 2D**
3. Right-click `Walls` > **2D Object > Sprites > Square** > rename to `BottomWall`
   - Position: `(0, -4.5, 0)` | Scale: `(20, 0.5, 1)`
   - Add Component > **Box Collider 2D**

### Step 3 — Paddles

Paddles move via script, not physics forces, so they use a **Kinematic** Rigidbody2D. Kinematic means "I'm in the physics world and things collide with me, but I'm not pushed around by forces."

1. Right-click in the Hierarchy > **2D Object > Sprites > Square** > rename to `PlayerPaddle`
   - Position: `(-8, 0, 0)` | Scale: `(0.5, 2, 1)`
   - Add Component > **Rigidbody 2D**
     - Body Type: `Kinematic`
   - Add Component > **Box Collider 2D**
2. Repeat for `AIPaddle`
   - Position: `(8, 0, 0)` | Scale: `(0.5, 2, 1)`
   - Same components, same settings

### Step 4 — Ball

The ball is driven by the physics engine, so it uses a **Dynamic** Rigidbody2D. We disable gravity and freeze rotation so it doesn't spin or fall.

1. Right-click in the Hierarchy > **2D Object > Sprites > Circle** > rename to `Ball`
   - Position: `(0, 0, 0)` | Scale: `(0.5, 0.5, 1)`
   - Add Component > **Rigidbody 2D**
     - Body Type: `Dynamic`
     - Gravity Scale: `0`
     - Collision Detection: `Continuous` *(prevents tunneling at high speeds)*
     - Constraints > Freeze Rotation Z: `checked`
   - Add Component > **Circle Collider 2D**

2. Create a **Physics Material 2D** so the ball bounces without losing energy:
   - In the Project window, right-click > **Create > 2D > Physics Material 2D** > name it `BallBounce`
   - Select it > Inspector > Friction: `0`, Bounciness: `1`
   - Select `Ball` > Circle Collider 2D > **Material** > assign `BallBounce`

3. Tag the Ball so scripts can identify it:
   - Select `Ball` > Inspector > **Tag** dropdown > **Add Tag** > add `Ball` > go back and assign it

### Step 5 — Goals

Goals are invisible trigger zones. When the ball enters one, a point is scored. No sprite needed — just a collider with "Is Trigger" checked.

1. Right-click in the Hierarchy > **Create Empty** > rename to `Goals`, position `(0, 0, 0)`
2. Right-click `Goals` > **Create Empty** > rename to `LeftGoal`
   - Position: `(-10.5, 0, 0)` | Scale: `(1, 10, 1)`
   - Add Component > **Box Collider 2D** > check **Is Trigger**
3. Right-click `Goals` > **Create Empty** > rename to `RightGoal`
   - Position: `(10.5, 0, 0)` | Scale: `(1, 10, 1)`
   - Add Component > **Box Collider 2D** > check **Is Trigger**

### Step 6 — Score UI

> **Unity 6 note:** The Canvas/uGUI system is not included in new Unity 6 projects by default. Add `"com.unity.ugui": "2.0.0"` to the `dependencies` block in `Packages/manifest.json`, save the file, switch back to Unity, and wait for it to reimport. `GameObject > UI > Canvas` will then appear.

1. Right-click in the Hierarchy > **UI > Canvas** — this creates a Canvas and EventSystem automatically
   - Canvas > Inspector > **Render Mode**: `Screen Space - Overlay`
2. Right-click `Canvas` > **UI > Text - TextMeshPro** > rename to `PlayerScore`
   - If prompted to import TMP Essentials, do it
   - Rect Transform: Anchor **top-left**, Pos X: `500`, Pos Y: `-150`, Width: `200`, Height: `100`
   - Text: `0` | Font Size: `72` | Alignment: **Center**
3. Right-click `Canvas` > **UI > Text - TextMeshPro** > rename to `AIScore`
   - Rect Transform: Anchor **top-right**, Pos X: `-500`, Pos Y: `-150`, Width: `200`, Height: `100`
   - Text: `0` | Font Size: `72` | Alignment: **Center**

> **Note on symmetry:** Because PlayerScore anchors to the top-left and AIScore anchors to the top-right, using the same absolute Pos X value guarantees they're equidistant from their respective edges. Same Pos Y ensures the same height. The mirroring is structural, not visual guesswork. X: `500`/`-500` centers the scores toward the middle of the screen (classic Pong style) — use `100`/`-100` to push them toward the edges instead.

> **Note on Pos Y:** The canvas renders at 1920x1080. A value of `-150` places the score text comfortably below the top wall. The default `-50` clips the text — use `-150`.

---

**Checkpoint 1:** Press Play. You should see a dark scene with two white paddles, a white ball centered between them, and two horizontal wall bars. Score text shows `0 0` at the top. Nothing moves yet — that's expected.

---

## Phase 2: Ball Movement

The `BallController` script launches the ball at game start, maintains constant speed across bounces, and scales speed with each paddle hit.

**Why FixedUpdate:** Physics runs on a fixed timestep. Setting velocity in `Update` (variable framerate) causes inconsistent behavior. Always use `FixedUpdate` for physics operations.

**Why normalize velocity:** Each bounce can introduce tiny floating-point drift. Normalizing the velocity vector each frame and re-applying the target speed keeps things crisp.

**Why speed scaling:** Without it, rallies feel flat. `OnCollisionEnter2D` fires when the ball hits a Paddle-tagged object — each hit bumps `speed` up to `maxSpeed`. `baseSpeed` captures the Inspector value at Start so `ResetBall` can restore it without hardcoding.

> **Note:** Speed scaling requires both paddles to be tagged `Paddle`. That tag is assigned in Phase 4 after both paddles exist.

### Step 1 — Create the script

In the Project window, right-click `Assets` > **Create > Folder** > name it `Scripts`. Then right-click `Scripts` > **Create > MonoBehaviour Script** > name it `BallController`.

Open it in Visual Studio and replace the contents:

```csharp
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedIncrement = 0.5f;
    [SerializeField] private float maxSpeed = 20f;

    private Rigidbody2D rb;
    private float baseSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseSpeed = speed;
        Launch();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }

    void Launch()
    {
        float angle = Random.Range(-45f, 45f) * Mathf.Deg2Rad;
        float dirX = Random.value > 0.5f ? 1f : -1f;
        rb.linearVelocity = new Vector2(Mathf.Cos(angle) * dirX, Mathf.Sin(angle)) * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
            speed = Mathf.Min(speed + speedIncrement, maxSpeed);
    }

    public void ResetBall()
    {
        speed = baseSpeed;
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke(nameof(Launch), 1f);
    }
}
```

### Step 2 — Attach the script

Select `Ball` in the Hierarchy > drag `BallController` from the Project window onto the Inspector (or Add Component > search BallController).

---

**Checkpoint 2:** Press Play. The ball launches in a random direction and bounces off the walls continuously. Speed stays constant for now — scaling activates once paddles are tagged in Phase 4.

---

## Phase 3: Player Input

`PlayerPaddleController` reads keyboard input and moves the paddle within the play area bounds.

**Why MovePosition:** Kinematic Rigidbodies ignore forces. `MovePosition` tells the physics engine "I want to be here next frame" — it calculates the move correctly and keeps collision detection intact. Setting `transform.position` directly would bypass physics.

**Why calculate yBound at Start:** The bound derives from actual scene values — wall center, wall half-height, paddle half-height, and a gap. Computing it from components rather than hardcoding means the bound stays correct if you resize the paddle or walls in the Inspector.

### Step 1 — Create the script

Right-click `Scripts` > **Create > MonoBehaviour Script** > name it `PlayerPaddleController`:

```csharp
using UnityEngine;

public class PlayerPaddleController : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float wallY = 4.5f;
    [SerializeField] private float wallHalfHeight = 0.25f;
    [SerializeField] private float padding = 0.1f;

    private Rigidbody2D rb;
    private float yBound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float paddleHalfHeight = transform.localScale.y / 2f;
        yBound = wallY - wallHalfHeight - paddleHalfHeight - padding;
    }

    void FixedUpdate()
    {
        float input = Input.GetAxisRaw("Vertical");
        float newY = Mathf.Clamp(rb.position.y + input * speed * Time.fixedDeltaTime, -yBound, yBound);
        rb.MovePosition(new Vector2(rb.position.x, newY));
    }
}
```

`Input.GetAxisRaw("Vertical")` returns `-1`, `0`, or `1` for W/S and arrow keys.

### Step 2 — Attach the script

Select `PlayerPaddle` > Add Component > `PlayerPaddleController`.

---

**Checkpoint 3:** Press Play. W/S and arrow keys move the left paddle. It stops at the wall bounds.

---

## Phase 4: AI Paddle

The AI tracks the ball's Y position and moves toward it at a capped speed. The speed cap is what makes it beatable.

**Why MoveTowards:** `Mathf.MoveTowards` moves toward a target by at most `speed * delta` per frame — it never overshoots, which prevents paddle jitter at the target position.

**Why calculate yBound the same way:** Both paddles use identical geometry, so they should use the same bound calculation. This keeps them in sync if you ever resize.

### Step 1 — Create the script

Right-click `Scripts` > **Create > MonoBehaviour Script** > name it `AIPaddleController`:

```csharp
using UnityEngine;

public class AIPaddleController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float wallY = 4.5f;
    [SerializeField] private float wallHalfHeight = 0.25f;
    [SerializeField] private float padding = 0.1f;
    [SerializeField] private Transform ball;

    private Rigidbody2D rb;
    private float yBound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float paddleHalfHeight = transform.localScale.y / 2f;
        yBound = wallY - wallHalfHeight - paddleHalfHeight - padding;
    }

    void FixedUpdate()
    {
        if (ball == null) return;
        float targetY = Mathf.Clamp(ball.position.y, -yBound, yBound);
        float newY = Mathf.MoveTowards(rb.position.y, targetY, speed * Time.fixedDeltaTime);
        rb.MovePosition(new Vector2(rb.position.x, newY));
    }
}
```

### Step 2 — Attach and wire up

Select `AIPaddle` > Add Component > `AIPaddleController`.

In the Inspector, drag the `Ball` GameObject from the Hierarchy into the **Ball** field.

### Step 3 — Tag both paddles

`BallController` checks for the `Paddle` tag in `OnCollisionEnter2D` to trigger speed scaling. Without this tag, the ball never speeds up.

1. Select `PlayerPaddle` > Inspector > **Tag** dropdown > **Add Tag** > add `Paddle` > go back and assign it
2. Select `AIPaddle` > assign the same `Paddle` tag

---

**Checkpoint 4:** Press Play. The right paddle tracks the ball. The ball speeds up slightly each time it hits a paddle. If the AI is unbeatable, lower its `Speed` value in the Inspector (4–6 works well).

---

## Phase 5: Debug Overlay

An in-game panel showing real-time debug data, toggled with a key at runtime.

> **Key constraint:** The script must live on an **always-active** GameObject. If it lives on the panel itself, the panel starting inactive means the script never runs — it can never receive input to show itself.

### Step 1 — Create the script

Right-click `Scripts` > **Create > MonoBehaviour Script** > name it `DebugOverlay`:

```csharp
using UnityEngine;
using TMPro;

public class DebugOverlay : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ballRb;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private KeyCode toggleKey = KeyCode.BackQuote;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            panel.SetActive(!panel.activeSelf);

        if (panel.activeSelf && ballRb != null)
            text.text = $"vel   {ballRb.linearVelocity.x:F2}  {ballRb.linearVelocity.y:F2}\nspeed  {ballRb.linearVelocity.magnitude:F2}";
    }
}
```

### Step 2 — Set up the UI

1. Under `Canvas`, right-click > **UI > Panel** > rename to `DebugPanel`
   - Anchor to a corner (bottom-left works well), resize to fit a few lines of text
   - **Set inactive by default** — uncheck the checkbox at the top of the Inspector
2. Right-click `DebugPanel` > **UI > Text - TextMeshPro** > rename to `DebugText`

### Step 3 — Create the host object

1. Right-click the Hierarchy > **Create Empty** > rename to `DebugOverlay`
2. Add Component > `DebugOverlay`
3. Wire up in the Inspector:
   - **Ball Rb** — drag `Ball` from the Hierarchy
   - **Panel** — drag `DebugPanel`
   - **Text** — drag `DebugText`
   - **Toggle Key** — defaults to backtick (`` ` ``)

---

**Checkpoint 5:** Press Play > click the Game view > press backtick. The debug panel appears showing velocity and speed. Press again to hide.

---

## Phase 6: Scoring

Two parts: goal zones that detect when the ball passes a paddle, and a `GameManager` that tracks score and updates the UI.

### Step 1 — GoalZone script

Right-click `Scripts` > **Create > MonoBehaviour Script** > name it `GoalZone`:

```csharp
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public enum Scorer { Player, AI }
    [SerializeField] private Scorer scorer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        if (scorer == Scorer.Player)
            GameManager.Instance.PlayerScored();
        else
            GameManager.Instance.AIScored();
    }
}
```

- Attach `GoalZone` to `LeftGoal` > set **Scorer** to `AI` *(ball passed the player paddle)*
- Attach `GoalZone` to `RightGoal` > set **Scorer** to `Player` *(ball passed the AI paddle)*

### Step 2 — GameManager script

Right-click `Scripts` > **Create > MonoBehaviour Script** > name it `GameManager`:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int winScore = 5;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    [SerializeField] private BallController ball;

    private int playerScore;
    private int aiScore;

    void Awake()
    {
        Instance = this;
    }

    public void PlayerScored()
    {
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        if (playerScore >= winScore)
            EndGame(true);
        else
            ball.ResetBall();
    }

    public void AIScored()
    {
        aiScore++;
        aiScoreText.text = aiScore.ToString();
        if (aiScore >= winScore)
            EndGame(false);
        else
            ball.ResetBall();
    }

    void EndGame(bool playerWon)
    {
        PlayerPrefs.SetInt("PlayerWon", playerWon ? 1 : 0);
        SceneManager.LoadScene("GameOver");
    }
}
```

`PlayerPrefs` is Unity's simple key-value store — persists small values between scenes (like who won).

### Step 3 — Set up GameManager in the scene

1. Right-click in the Hierarchy > **Create Empty** > rename to `GameManager`
2. Attach the `GameManager` script
3. In the Inspector, wire up:
   - **Player Score Text** → drag `PlayerScore` (from Canvas in Hierarchy)
   - **AI Score Text** → drag `AIScore`
   - **Ball** → drag `Ball`

---

**Checkpoint 6:** Press Play. When the ball passes a paddle, the score increments. After 5 points, the game tries to load the GameOver scene (it will error — that's fine, we build it next).

---

## Phase 7: GameOver Scene

### Step 1 — Create the scene

File > **New Scene** > Basic (Built-in) > save as `Assets/Scenes/GameOver`

Repeat the 2D setup from Phase 1 (Orthographic camera, delete Directional Light).

### Step 2 — Build the UI

1. Right-click Hierarchy > **UI > Canvas**
2. Add three TextMeshPro/Button elements under the Canvas:
   - `ResultText` — centered, large font, text: `You Win!` (placeholder)
   - `PlayAgainButton` — below result, text: `Play Again`
   - `MainMenuButton` — below that, text: `Main Menu`

### Step 3 — GameOverController script

Right-click `Scripts` > **Create > MonoBehaviour Script** > name it `GameOverController`:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    void Start()
    {
        bool playerWon = PlayerPrefs.GetInt("PlayerWon", 0) == 1;
        resultText.text = playerWon ? "You Win!" : "You Lose!";
    }

    public void OnPlayAgainPressed()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnMainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
```

### Step 4 — Wire it up

1. Attach `GameOverController` to a GameObject in the scene, assign `ResultText`
2. Select `PlayAgainButton` > Inspector > **On Click ()** > `+` > drag the GameOverController object > select `GameOverController.OnPlayAgainPressed`
3. Same for `MainMenuButton` > `OnMainMenuPressed`

### Step 5 — Register scenes in Build Settings

File > **Build Settings** > drag all three scenes (`Game`, `GameOver`, `MainMenu`) into the **Scenes In Build** list. Order matters: index 0 is the startup scene.

---

**Checkpoint 7:** Play a full game to 5 points. The GameOver screen appears with the correct result. Play Again returns to the game. Main Menu errors (not built yet).

---

## Phase 8: Main Menu Scene

### Step 1 — Create the scene

File > **New Scene** > save as `Assets/Scenes/MainMenu`

Repeat 2D setup.

### Step 2 — Build the UI

1. Right-click Hierarchy > **UI > Canvas**
2. Add:
   - `TitleText` — large, centered, text: `PONG`
   - `PlayButton` — text: `Play`
   - `QuitButton` — text: `Quit`

### Step 3 — MainMenuController script

Right-click `Scripts` > **Create > MonoBehaviour Script** > name it `MainMenuController`:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
```

### Step 4 — Wire it up

Same as GameOver: attach script, wire buttons via **On Click ()** in the Inspector.

---

**Checkpoint 8:** Full game loop works. Main Menu > Play > Game > win/lose > GameOver > Play Again or Main Menu.

---

## Phase 9: Polish

The game is functionally complete. This phase improves feel.

### AI difficulty tuning

Adjust `AIPaddleController.speed` in the Inspector until the AI feels beatable but not trivial. Around `4`–`6` works well at ball speed `10`.

### Background color

The default skybox looks odd in 2D. Set a solid background:

- Select `Main Camera` > Inspector > **Clear Flags**: `Solid Color` > **Background**: pick a color (black is classic Pong)

---

**Checkpoint 9:** Game feels good. AI is beatable. Background is clean.

---

## Done

The game is complete. All milestones from the design doc are implemented.

Next steps (if any): sound effects, difficulty selection on the main menu, or a web build.
