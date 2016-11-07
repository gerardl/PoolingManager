# PoolingManager
A Simple Unity3D Pooling System.  Create Pools of objects from the Editor or through code.

Create a new Object Pool and get an object from it
```c#
// create a new pool with 100 objects
PoolingManager.CreatePoolingList(fireballGameObject, "FireBallPool", 100);
// get a fireball and use it
var fireball = PoolingManager.GetPooledObjectByName("FireBallPool");
if (fireball != null) fireball.GetComponent<Fireball>().Fire();
```

You can also create and configure an object pool through the Editor and then use the object pool through code

![Image of Editor Creation](http://i.imgur.com/FW1eyVD.jpg)

More examples can be found in a demo scene in the project.
