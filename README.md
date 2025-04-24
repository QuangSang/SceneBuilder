I am using MVC architecture for this project.

  Controllers:
  
    - SceneBuilderManager : top level manager that is responsible for initializing the UIs, updating the scene data, spawning characters, containing all button click handler. It also has references to all other managers
    
    - ScenePlayerManager: responsible for playing the scene
    
    - ResourceLoader: responsible for loading asset from Addressables
    
    - SceneLoader : responsbible for saving/loading a scene
    
  Views:
  
    - SceneBuilderView: main UI class that is responsible for navigating the UI, hooking up all the buttons
    
    - CharacterSelectionView: UI class that is responsible for displaying the list of available characters
    
    - CharacterAvatarView: UI class that is responsible for displaying an individual character
    
    - AnimationTimelineView : UI class that is responsible for displaying the animation timeline
    
    - AnimationSelectionView: UI class that is responsible for displaying a single animation
    
    - TimelineBarView: UI class that is responsible for populating the timeline visualizaiton
    
  Data:
  
    - CharacterDefinition: a scriptable object that store the defintions of all characters (name, AssetReference, AnimationController that contains all available animations)
    
    - SceneData: a serializable data class that holds all characters in the scene
    
    - CharacterAnimationSequenceData: a serializable data class that holds the animation sequence of a character.
    
    - AnimationData: a serializable data class that holds the animation data.
    
    - RenderData: a list of all RenderData for each views
    
  Components:
  
    - FollowMouseComponent: a component used for placing the character on the scene, handle both rotation and position
    
    - CharacterAnimationController: a component used for playing the animation sequence of a character
    

Demo:
https://youtu.be/OSZW3lwVyR4

Features:

  - Character selection: select a character, add animation sequence with some timeline visualization, place it onto the scene.

  - Play a scene

  - Reset a scene

  - Clear a scene

  - Save/Load a scene
