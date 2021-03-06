![MonoGame.Extended Logo](https://raw.githubusercontent.com/craftworkgames/MonoGame.Extended/master/Logos/logo-banner-800.png)

# Notice
This is a fork, check out the original repository; https://github.com/craftworkgames/MonoGame.Extended
(This "README.md" has been edited.)

# MonoGame.Extended
It makes MonoGame more awesome.

[![Join the chat at https://gitter.im/craftworkgames/MonoGame.Extended](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/craftworkgames/MonoGame.Extended?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) [![Build Status](http://build.craftworkgames.com/app/rest/builds/buildType:(id:MonoGameExtended_Build)/statusIcon)](http://build.craftworkgames.com/viewType.html?buildTypeId=MonoGameExtended_CI&guest=1) [![Docs](https://img.shields.io/badge/docs-latest-brightgreen.svg?style=flat)](http://craftworkgames.github.io/MonoGame.Extended/)

MonoGame.Extended is an open source collection of NuGet packages for [MonoGame](http://www.monogame.net/). A collection of classes and extensions to make it easier to make games with MonoGame. 

## Things are changing around here

We're in the process of migrating MonoGame.Extended to .NET Standard among other things.
Please be patient, things will break.

## Packages

 - **MonoGame.Extended** - the core package creates a solid foundation with sprites, [bitmap fonts](http://craftworkgames.github.io/MonoGame.Extended/MonoGame.Extended/BitmapFonts/), [collections](http://craftworkgames.github.io/MonoGame.Extended/MonoGame.Extended/Collections/), [serialization](http://craftworkgames.github.io/MonoGame.Extended/MonoGame.Extended/Serialization/), shapes, texture atlases, viewport adapters, [cameras](http://craftworkgames.github.io/MonoGame.Extended/MonoGame.Extended/Camera2D/), timers, math, [object pooling](http://craftworkgames.github.io/MonoGame.Extended/MonoGame.Extended/Object-Pooling/), [screens](http://craftworkgames.github.io/MonoGame.Extended/MonoGame.Extended/Screens/), and diagnostics.
 - **MonoGame.Extended.Animations** - animated sprites and sprite sheets.
 - **MonoGame.Extended.Collisions (experimental)** - collision detection and response.
 - **MonoGame.Extended.Content.Pipeline** - a collection of importers for the [MonoGame Pipeline Tool](http://www.monogame.net/documentation/?page=Using_The_Pipeline_Tool).
 - **MonoGame.Extended.Entities (experimental)** - An entity component system.
 - **MonoGame.Extended.Graphics** - high performance geometry and sprite rendering.
 - **MonoGame.Extended.Gui (wip)** - a gui system built from the ground up for desktop and mobile games.
 - **MonoGame.Extended.Input** - event based input listeners with mouse, keyboard, touch and game pad support.
 - **MonoGame.Extended.NuclexGui** - a port of the [Nuclex GUI framework](https://nuclexframework.codeplex.com/wikipage?title=Nuclex.UserInterface).
 - **MonoGame.Extended.Particles** - high performance particle engine ported from the [Mercury Particle Engine](matthew-davey.github.io/mercury-particle-engine/).
 - **MonoGame.Extended.SceneGraphs** - scene graphs and trees.
 - **MonoGame.Extended.Tiled** - load and render maps created with the popular [Tiled Map Editor](http://www.mapeditor.org/).
 - **MonoGame.Extended.Tweening (experimental)** - tween based animations.

## Patreon Supporters (Patreon for original MonoGame.Extended by craftworkgames)

Thanks to all those that support the project on Patreon! Running an open source project can be done on a shoe string budget, but it's certainly not free. A little funding goes a long way. It keeps the build server up and running and let's me devote more of my time to the project. Even just a few supporters really helps.

**What happens to MonoGame.Extended if we don't get the funding?** Never fear. The project won't die. The code will always be safely open sourced on github.

[![image](https://cloud.githubusercontent.com/assets/3201643/17462536/f5608898-5cf3-11e6-8e81-47d6594a8d9c.png)](https://www.patreon.com/craftworkgames)

### Special thanks to the top supporters

We're in the process of developing MonoGame.Extended 3.6! 

There may be some confusion, pain and disruption for a while. Here's what you need to know:
 
 - Everything that used to be in the `develop` branch is now in `master`
 - NuGet packages built from `master` have been [published to nuget.org as version 1.1](https://www.nuget.org/packages?q=monogame.extended)
 - There's lots of breaking changes happening to create a cleaner more useful API
 - From now on we're going to (attempt) to use [Git Flow](https://gitversion.readthedocs.io/en/latest/git-branching-strategies/gitflow/)
 - We're now using [cake builds](https://cakebuild.net/) so that you can build everything (including the NuGet packages) locally
 - We're migrating everything to [.NET Standard!](https://www.patreon.com/posts/one-library-to-18916187)

## Patreon Supporters

If you're not on the list and you should be please let me know! Managing Patreon is a job in itself.

## Getting Started

If you're using the [NuGet packages](https://www.nuget.org/packages?q=monogame.extended) please read the [install guide](http://craftworkgames.github.io/MonoGame.Extended/installation/) to setup the Pipeline tool.

As a reward to some of my patrons I've linked thier websites here:
 - [PRT Studios](http://prt-studios.com/)
 - [optimuspi](http://www.optimuspi.com/)
 
If you're not on the list and you should be please let me know!

## Forums

Our forum is part of the [MonoGame community](http://community.monogame.net/category/extended). Please ask any questions or post about problems or bugs that you have found there. Let us know if you're making a game with MonoGame.Extended!

## Design goals
 - The primary goal is to make it easier to *make games*.
 - Choose the features you like and the rest stays out of your way.
 - A clean and consistent API familiar to MonoGame developers.
 - It's *not* a game engine, but extends the framework.

## License


## Special Thanks

 - Matthew-Davey for letting us use the [Mercury Particle Engine](https://github.com/Matthew-Davey/mercury-particle-engine).
 - John McDonald for [2D XNA Primitives](https://bitbucket.org/C3/2d-xna-primitives/wiki/Home)
 - [LibGDX](https://libgdx.badlogicgames.com) for a whole lot of inspiration.
 - @prime31 for [Nez](https://github.com/prime31/Nez), which ideas and code bounce back and forth.
 - All of our contributors!