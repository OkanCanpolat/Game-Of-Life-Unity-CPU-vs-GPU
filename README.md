# Game-Of-Life-Unity-CPU-vs-GPU

This repo is the version of Game of Life, created by the famous mathematician John Horton Conway, ported to Unity. There are two different version of the game. One of them (**GameOfLifeCPU.cs**) is runs on the CPU. Other one (**GameOfLife.cs, GameOfLifeCS.compute**) runs on the GPU via compute shader. 

There is huge FPS difference between these two methods. For example on my PC if i set width to 1920 and height to 1080; CPU version will run at approximately 5 FPS and GPU version will run at 220 FPS.
