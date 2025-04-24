Architecture: I am using MVC architecture for this project. Note: I am using the Name to reference each character for simplicity but in real project it should be an a unique Id

  - Controllers:
    - SceneBuilderManager : top level manager that is responsible for initializing the UIs, updating the scene data, spawning characters, containing all button click handler. It also has references to all other managers
    - ScenePlayerManager: responsible for playing the scene
    - ResourceLoader: responsible for loading asset from Addressables
    - SceneLoader : responsbible for saving/loading a scene
    
  - Views:
    - SceneBuilderView: main UI class that is responsible for navigating the UI, hooking up all the buttons
    - CharacterSelectionView: UI class that is responsible for displaying the list of available characters
    - CharacterAvatarView: UI class that is responsible for displaying an individual character
    - AnimationTimelineView : UI class that is responsible for displaying the animation timeline
    - AnimationSelectionView: UI class that is responsible for displaying a single animation
    - TimelineBarView: UI class that is responsible for populating the timeline visualizaiton
    - 
  - Data:
    - CharacterDefinition: a scriptable object that store the defintions of all characters (name, AssetReference, AnimationController that contains all available animations)
    - SceneData: a serializable runtime data class that holds all characters in the scene
    - CharacterAnimationSequenceData: a serializable runtime data class that holds the animation sequence of a character.
    - AnimationData: a serializable runtime data class that holds the animation data.
    - RenderData: a list of all RenderData for each views
      
  - Components:
    - FollowMouseComponent: a component used for placing the character on the scene, handle both rotation and position
    - CharacterAnimationController: a component used for playing the animation sequence of a character
    

Demo:
https://youtu.be/OSZW3lwVyR4

Features:
  - Character selection: select a character, add animation sequence with some timeline visualization, place it onto the scene with rotation (you can right click or ESC while you're on a placing state to cancel)
  - Play/Reset/Clear a scene
  - Save/Load a scene

Extensibility:
  - We can create different Definition ScriptableObjects to store the definition of other type of objects that we want to put into the scene and we should have different manager classes for each type of objects.( Maybe I should rename SceneBuilderManager into CharacterBuilderManager because it is specific to characters only).
  - SceneData is a runtime data class that represents the entire scene. Right now it only has a list of all the characters animation sequence but we can add some data into it for other objects and load them using its respective manager
  - CharacterAnimationController is a monobehaviour component that is responsible for playing the animation sequence of a character. We could probably create other components to handle different objects

Things I would improve if I had more time:
  - Use DI frame (VContainer) to manager all dependencies and organize the codebase cleaner.
  - Create a unique ID for each characters and reference them by that Id instead of name
  - Improve the timeline UI, make the block draggable for better UX
  - Use DOTween to tween UI
  - Add elapsed time onto the scene
  - Use Cinemachine package to handle camera controls
  - Use Animancer package for playing/sequencing animation instead of built-in Animator
  - Add visual improvement (lightning, ...)
  - Rename SceneBuilderManager -> CharacterBuilderManager
  - Make naming convention more consistent (Character vs Actor)
