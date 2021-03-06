# MatterVRX
"Matter VR Explorer" is a VR prototype to display a voxel model to visualize matter data.

## Launching the application
Download the zip file of the application from the releases section of this github repository. Uncompress it. You obtain a .exe file in the unzipped root folder. Launch it with your VR equipment set up, and it should automatically launch both the application and Steam VR. Because of the size of the data used, the application can take a while to load, so don't worry if you are stuck for a few minutes in a black void. Also, because of a know bug, the controllers are for now invisible.

## Controls
This is the features that are currently implemented, along with their controls :  
- **Move voxels horizontally** : touch the left track pad in the wanted direction
- **Move voxels vertically** : touch the right track pad in the wanted direction
- **Zoom** : hold both triggers and change distance between the controllers to zoom in or out
- **Select a voxel** : point a voxel with one of the controller and grab the grip of said controller

## Control Panel
The control panel is an interactive user interface : user can press buttons by pointing it with the **right** controller and pressing the **trigger**.  
These are the currently implemented buttons on this panel :  
- **Reset** : resets the voxels positions and scales
- **Filters** : a dropdown menu which allows user to choose a filter in order to disable certain voxels according to some criteria. Until they are enabled again, disabled voxels are treated as non-existant and cannot be seen or interacted with.
  - *No Filter* : no filter, all voxels are enabled
  - *Selected* : all unselected voxels are disabled
  - *Unselected* : all selected voxels are disabled
  - *Greater Than* : this filter uses the slider beneath the button : all voxels which have a value less than or equal to the slider's are disabled
  - *Less Than* : this filter uses the slider beneath the button : all voxels which have a value greater than or equal to the slider's are disabled
- **Colormaps** : a dropdown menu which allows user to change the colormap used for coloring voxels. There currently are 4 colormaps : *Grey, Hot, Cool, Jet*.
- **Record annotation** : starts a voice recognition engine which converts voice to text and saves the annotation to all selected voxels. Please note that, at least on Windows, using the dictation engine first requires enabling voice recognition in Windows settings.

## Using your own data
In order to use your own data files, you first need to add them to the project. To do so, you must go to the folder located at *ECS MatterVRX_Data/StreamingAssets/Resources/*. You can then add any file and folder you want.  
Please note that for the configuration files requiring file names, **you must give paths starting in the Resources folder**. For example, the file "foo.txt" located at *ECS MatterVRX_Data/StreamingAssets/Resources/bar/* must be named "bar/foo.txt"

### Configuration File
Located at *ECS MatterVRX_Data/StreamingAssets/Configutation/BaseConfig.xml*, this file contains editable options for loading and navigating :
- **zoom factor** : the speed at which the voxel cluster is zoomed in or out
- **zoom center offset** : the distance between the user head and the center point of the zoom 
- **render distance** : the distance, expressed in *voxels*, beyond which voxels do not appear
- **selection distance** : the distance, expressed in *voxels*, beyond which voxels cannot be pointed nor selected
- **vertical speed** : the speed at which voxels move vertically
- **horizontal speed** : the speed at which voxels move horizontally
- **file name** : the name of the file to be loaded. It must be a *.nii* or a *.nii.gz*, and must be located in ECS MatterVRX/Assets/Resources/
- **load mode** : the loading mode for the file. It can have the following values :
    - *whole* : load the whole file *(NB : the app gets extremely slow in this mode)*
    - *region* : loads the cuboid configured below
    - *dummyTumor* : creates a dummy tumor which parameters are explained later
- **load region**: if the file is not fully loaded, the loaded cuboid within the file
- **load streamlines** : whether the software should parse and display streamlines or not
- **streamline files** : file names and information needed to parse streamlines, if loaded. Please refer to the default ones already present in the project to better understand what is needed where.
  - *adjacency matrix* : the csv file defining the strength of the streamline between two nodes
  - *size* : the number of nodes used for streamlines (the number of lines in the adjacency matrix)
  - *assignments* : the csv file allowing the software to determine which output streamline file corresponds to which entry in the adjacency matrix
  - *ignore assignment first line* : whether to ignore or not the first line of the assignment file (if it contains a history of commands for example)
  - *nodes* : the txt file used to determine the color of a streamline, by mixing the colors of its two extremities
  - *streamline template* : the template name used for the txt individual streamline files
  - *digits number* : the number of digits used in the txt individual streamline files names. For example, if your streamline files are located in the "streamlines" folder and are named "line-0000", "line-0001", "line-"0002", streamline template would be "streamlines/line-" and digits number would be 4
- **dummy tumor pos X/Y/Z** : the coordinates of the center of the spherical dummy tumor, given in the matrix referential (they must thus be integers)
- **dummy tumor radius** : the radius of the dummy tumor in millimeters
- **dummy tumor periphery** : the additional radius in which voxels only have a probability of being tumorous
- **dummy tumor healthy** : the additional radius in which voxels are all healthy

### CSS File
Located at *ECS MatterVRX_Data/StreamingAssets/Configutation/stylesheet.css*, this file contains editable display options. It is written exactly as a standard CSS file, but uses custom classes and attributes.  
The classes are as follows:
- **value_lt:x** : applies on voxels which have a value less than x
- **annotated:b** : applies on voxels that are either annotated (b = true) or not (b = false)
- **selected** : applies on selected voxels
- **map:m** : applies on all voxels, but only when the color map corresponds. Possible values for m are [grey, hot, cool, jet]
The attributes are as follows:
- **color**: a hexadecimal color (#000000 for example) replacing the model color of the voxels
- **outline-color** : a hexadecimal colors replacing the outline color of the voxels


## Development : git clone
When cloning the project you may encounter the following issues:
- **Loading blocked on SteamVR_Settings.asset** : there are 2 known methods regarding this issue:
  - *method 1* : wait a bit, then interrupt the loading (alt f4 or task manager). Then restart Unity.
  - *method 2* : interrupt the loading, delete the 3 folders Assets/SteamVR*, and start the Unity project again by ignoring the warning. When in the project, use the package manager to remove the SteamVr SDk, then reinstall it. If the loading blocks again on the SteamVR_Settings.asset, force restart Unity.
- **missing mesh** :
  - *default mesh for the minimap* : the .obj is too big to be pushed on git, you need to ask the relevant people to give it to you. Import the .obj in the Assets/Obj folder. Once the .obj obtained, put the mesh on the MiniMap gameobject (the mesh is called "default")
  - *various meshes for the voxels* : the meshes for the voxels are in Assets/Obj. You need to give them to the Voxels ECS Spawner -> Ecs Spawner (Script) -> Meshes (Array). Currently, the distribution of shapes is random, with a density defined by the proportions in the array (the distribution used is [tetrahedron, diamond, cube, cube], in this order) 
