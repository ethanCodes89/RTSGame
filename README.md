This was a fun project I worked on for a couple months mostly just to try some things I had never implemented before. I ended up not sticking with the project purely because I didn't have a ton of interest in the design side of creating an RTS.

The main implementation is the camera system and unit selection. 

Input System:
  I utilized Unity's new Input System, using Input Actions and a loosely coupled architecture to keep input and logic separate.

Camera:
  The camera can be moved with WASD or by moving the mouse to the edge of the screen.
  If you move the mouse to the edge of the screen while also using WASD, the movement speed is compounded. 
  The camera can be zoomed in and out with the mouse wheel.
  The camera can be dragged by clicking and holding the middle mouse button and dragging the mouse. 
  The camera tilts in nicely when zooming really close, giving it a nice feel when zooming in on a particular area.

Unit Selection:
  Units can be selected by left clicking on them.
  By holding the left mouse button and dragging, there is a green ui selection box. When moved over multiple units, all units within the view of the box will be selected.
  You can hold the shift key and left click units to select multiple units one by one, or if you've drag selected multiples, you can use it to unselect specific units.
  Once selected, unit's can be directed to move to new locations using the left mouse button.

Incomplete Implementation:
  I setup Unity's nav mesh system using A* (their implementation, not my own) to get units to move to new locations.
  When moving units, it's necessary to also setup a secondary pathfinding system. This is when I began implementing a flocking system into the project.
  It was towards the end of the flocking system that I moved to UE5 and it never became fully put in place. I believe I have everything coded and all of the components for it ready, but I 
  never set them up. 

  This project was a lot of fun and I learned TONS from it. I moved into UE5 because the Unreal Engine seems more inline with my long term goals and I felt it was time to make the switch. 
  This project was a huge help in me learning UE5, especiall on the c++ side as I used much of my code from this project as a guide for learning C++ as the first thing I did in UE was recreate the camera and selection system from this project.
  
  
