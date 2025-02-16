# FPS Game Development Outline

## Timeline (Feb 15 - Feb 21)

### Feb 15-16: Core Player Mechanics
- **Player Movement & Jumping:**
  - Implement walking, jumping, and wall-running (on designated walls)
  - Use `CharacterController` for smooth, frame-rate independent movement.
- **Basic Shooting:**
  - Pistol firing using raycasting
  - Implement one-shot-one-kill logic with quick restart.
- **Katana Attacks:**
  - Melee collision detection for the katana.
  
### Feb 17-18: Enemies & AI
- **Enemy Navigation:**
  - Basic pathfinding using NavMesh or custom logic.
- **Enemy Attacks:**
  - Pistol & grenade firing for enemies.
- **Damage & Death Handling:**
  - Implement `IDamageable` interface.

### Feb 19: Level & Interactions
- **Level Design:**
  - Build the level using Unity primitives.
- **Wall-Running Detection:**
  - Use collision flags or raycasts for valid wall surfaces.
- **Spawn Points:**
  - Set up player and enemy spawn points.

### Feb 20: Polish & UI
- **Visual & Audio Effects:**
  - Muzzle flash, impact effects.
- **UI Feedback:**
  - Ammo count, health, and kill notifications.
- **Quick Restart Optimization:**
  - Ensure fast level restarts without heavy delays.

### Feb 21: Extras & Final Testing
- **Extra Features (if time allows):**
  - Add a second level or a boss.
  - Experiment with custom shaders for a stylized look.
- **Final Playtesting:**
  - Bug fixes, polishing, and final build packaging.

---

## Architecture Overview

### 1. **Core Player Components**

#### **PlayerController**
- **Responsibilities:**
  - Handle input (via Unity's new Input System).
  - Dispatch commands to `CharacterMotor` and `WeaponManager`.
- **Key Methods:**
  - `OnMove(InputAction.CallbackContext context)`
  - `OnJump(InputAction.CallbackContext context)`
  - `OnFire(InputAction.CallbackContext context)`
  - `OnLook(InputAction.CallbackContext context)`

#### **CharacterMotor**
- **Responsibilities:**
  - Process movement using `CharacterController.Move()`.
  - Handle gravity, jumping, and wall-running logic.
  - Utilize `OnControllerColliderHit` for collision detection.
- **Key Points:**
  - Manual collision responses.
  - Use collision flags and supplemental raycasts as needed.

#### **WeaponManager & Weapons**
- **WeaponManager:**
  - Manage active weapon and handle weapon switching.
  - Delegate firing commands to the active weapon.
- **Weapons:**
  - Implement an `IWeapon` interface.
  - **Pistol:** Use raycasting for shooting.
  - **Katana:** Use sphere/box overlap for melee detection.

### 2. **Enemy Components**
- **EnemyAI:**
  - Handle navigation and target detection.
- **EnemyAttack:**
  - Manage enemy firing and grenade throwing.
- **Health Component:**
  - Implement `IDamageable` for both player and enemies.

### 3. **Global Game Management**
- **GameManager:**
  - Maintain game state (InGame, Paused, GameOver).
  - Manage quick restart and level transitions.
- **UIManager:**
  - Update HUD elements and game notifications based on events.

---

## Event System

### Using C# Events
- **Centralized Static Class Example:**
  ```csharp
  public static class GameEvents {
      public static event Action OnPlayerDeath;
      public static event Action<int> OnPlayerDamage;
      public static event Action OnWeaponFired;
      public static event Action<WeaponType> OnWeaponSwitched;
      // ... additional events

      public static void TriggerPlayerDeath() {
          OnPlayerDeath?.Invoke();
      }
      // ... additional trigger methods
  }
