# FDV-2DTilemaps

*Fundamentos del Desarrollo de Videojuegos*' assignment: 2D tilemaps and 2D physics exercises.

![The assignment's demo scene](Media/demo.gif)

> Reuse the Unity 2D project you've been working on. The objective of this session is to familiarize yourself with the map editor in Unity, as well as implementing mechanics that use the physics engine.
> - **2D Physics**: working with them is quite similar to 3D, even though the engines are independent. Every 2D component has the same name as their 3D variants, but with the "2D" suffix.
> - **Tilemaps system**: A Unity tool that gives us components and utilities to build layers to form our map from graphical resources.
>   - Each part will be an image (or part of one), which we call ***Tile***.
>   - The Tilemaps system uses a Grid to serve as a guide for placing elements in the map.
>   - The component that manages each layer of the map is the ***Tilemap***. It is important to specify layers, order within a layer, etc. for the correct viewing of the resources.
>   - Another tool of the system is the Tilemap palette. We can configure a graphics resources palette, which makes it easier to place resources in the Grid by using a brush analogy.

## Table of contents
1. [Task 1](#task-1)
2. [Task 2](#task-2)
3. [Task 3](#task-3)
4. [Task 4](#task-4)
5. [Task 5](#task-5)
6. [Task 6](#task-6)
7. [Task 7](#task-7)
8. [Task 8](#task-8)

## Task 1
> In this first task, we'll work with the fundamental elements of the game map. You'll have to work with the **Grid**, **Tilemap** and **Tile Palette** elements.
> Starting from the scene implemented in the [Sprites Assignment](https://github.com/lara-ramos31/FDV-2DSprites) we'll build a game map, which will limit the zone in which the action will take place. You can use the resources inside the ZIP file for the assignment, or choose the assets yourself. Adjust the Tile size to the size of each Grid cell, and subdivide those Tiles that span over multiple cells. Create a conventional map that represents the environment and game limits.

I chose to import [Kenney's Pixel Platformer](https://kenney.nl/assets/pixel-platformer) asset pack. It contains the tile sizes in pixels, and in this case these are 18x18, so the PPU of their imported image must be 18.

![A screenshot of the inspector showing the PPU](Media/task1_tilemap_ppu.png)

The tilesheet is conveniently arranged so that it fits in a perfect grid.

![The tilesheet](Assets/Resources/Sprites/Environment/WorldTilemap/pixel_platformer_tilemap.png)

With a palette that uses this tilesheet, the resulting scene looks like this:

![The scene with the designed map](Media/task1_scene.png)

## Task 2

> In this exercise you'll work using various Tilemaps in the same scene. Each of them will have a different rendering order, which can affect their visibility. If you encounter problems you can try changing the order in layer. It is also convenient to check the Z-value, perhaps using the 3D project view.
> - a. Create two additional obstacle Tilemaps. One could be for decorative elements, and the other for obstacles.
> - b. Add the necessary configuration to the obstacles layer so that it is constructed independently and the obstacle acts as such. This, however, could be problematic:
>   - High rendering speed: turns on the continuous collision detection.
>   - Layer order.
>   - Layers allow collisions with each other.
>   - Size and shape of the colliders. This can be fixed by editing the Sprite to adapt to the object's shape, then applying a **Custom Physics Shape**.
>   - If you've modified the scale, it could affect the collision detection.
> - c. Add a collectable objects layer that add points to the player, then another for objects that deal damage and subtract points.

![The editor with task 2's changes](Media/task2_editor.png)

Since the previous task's tilemap served as a map skeleton, I renamed it to `ObstacleTilemap` and added an independent `DecorativeTilemap` for decorative elements, as well as a `BackgroundTilemap` for the background. The `ObstacleTilemap` has a static `Rigidbody2D`, as well as a `CompositeCollider2D`. I also set up custom general and sorting layers, and placed each tilemap were they belong:

![The inspector showing the new layers](Media/task2_layers.png)

## Task 3

> The last block of tasks we'll work with will be about implementing some useful mechanics.
> - **2D Character control**: on each update, using the `Update()` method, we'll advance in the space a certain `speed` allows us in the elapsed time. The time interval between frames is controlled by `Time.deltaTime`. Use the axes to orient the movment. A possible pseudocode could be the following:
> ```c#
> public class PlayerMovement : MonoBehaviour {
>   // Declare the float speed
>   // Declare the SpriteRenderer variable to control the 'flip'
>
>   void Start()
>   {
>     // Obtain the reference to the SpriteRenderer once (for efficiency) 
>   }
>
>   void Update()
>   {
>      // 1. Use the Input class to get the input
>      // 2. Compute the translation as space = speed * time.
>      //    The speed was declared before, the time is fetched with Time.deltaTime.
>      //    The translation can be performed using Transform.Translate.
>      // 3. Change the sprite's orientation based on the left or right movement.
>   }
> }
> ```
> - **2D Character control with rotation and forwards movement**: on each update, using the `Update()` method, we'll always advance forwards in space by a certain speed within the elapsed time. The input's X axis is used for rotation to orient ourselves to the place to walk towards, and the Y axis is used once we already are oriented:
> ```c#
> public class 2DRotationAndLocaLMovement : MonoBehaviour {
>   // Inspector-adjustable variables
>   // Translation forwards speed
>   // Rotation speed (degrees per second) 
>
>   void Update()
>   {
>      // 1. OBTAIN THE INPUTS
>      //    Input for rotation (left/right) with the Horizontal axis.
>      //    Input for movement forwards/backwards with the Vertical axis.
>      // 2. APPLY ROTATION (rotate the object around the Z axis) proportionally to Time.deltaTime to smooth the turn.
>      //    Rotate the object around its Z axis. The Transform component has a Rotate() method.
>      // 3. APPLY LOCAL-SPACE TRANSLATION (forwards), once we have already rotated.
>      //    Create a vector with only the Y AXIS movement, since we have already rotated.
>      //    Use transform.Translate() with Space.Self. In this case, it is necessary to move the object in the direction it is looking towards (its local space), not the global space.
>      //    Test the difference with Space.World
>   }
> }
> ```

The type of project I chose for this assignment is a 2D platformer, so only the first player controller makes sense in this context. Still, I included both. Starting with the rotation-less player controller:

![The player movement without rotations](Media/task3_norotation.gif)

For this, I simply added the previous assignment's [Task 5 script](https://github.com/lara-ramos31/FDV-2DSprites/blob/main/Assets/Resources/Scripts/Task5_Player.cs), but I also added vertical movement for completion's sake.

```c#
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer), typeof(Animator))]
public class Task3_NoRotationMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public float minWalkingDistance = 0.001f;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 restingPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        restingPosition = transform.position;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        movement.Set(horizontalInput, verticalInput);
        transform.Translate(speed * Time.deltaTime * movement);
        if (horizontalInput > 0) { // facing right
            spriteRenderer.flipX = false;
        } else if (horizontalInput < 0) { // facing left
            spriteRenderer.flipX = true;
        }
        float distanceThisFrame = Vector3.Distance(restingPosition, transform.position);
        if (distanceThisFrame >= minWalkingDistance) {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }
        restingPosition = transform.position;
    }
}
```

![The player movement with rotations](Media/task3_rotations.gif)

This script is similar to the previous one, but instead uses the instructed rotation control scheme and forward movement.

```c#
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer), typeof(Animator))]
public class Task3_RotationMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float angularSpeed = 135.0f;
    public float minWalkingDistance = 0.001f;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 restingPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        restingPosition = transform.position;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Rotate(0, 0, -angularSpeed * horizontalInput * Time.deltaTime);
        movement.Set(speed * Time.deltaTime * verticalInput, 0);
        transform.Translate(movement, Space.Self);
        if (verticalInput > 0) { // facing right
            spriteRenderer.flipX = false;
        } else if (verticalInput < 0) { // facing left
            spriteRenderer.flipX = true;
        }
        float distanceThisFrame = Vector3.Distance(restingPosition, transform.position);
        if (distanceThisFrame >= minWalkingDistance) {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }
        restingPosition = transform.position;
    }
}
```

Note that the flipping logic in this case uses the `verticalInput`, since it is what determines the translation.

## Task 4

> Write a player movement script assuming it responds to physics.



## Task 5

> Add an object spanning a certain area that can launch the player towards another zomascone of the map.

## Task 6

> Create an enemy that follows the player, dragging a dangerous pet. If the player is within a certain distance, the rabid pet will make the player lose points.