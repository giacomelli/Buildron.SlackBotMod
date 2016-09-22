# Buildron.SlackBotMod
![](docs/images/Buildron.SlackBotMod-2016-09-18.gif)

Mod that adds a bot to your Slack that acts as [Buildron](https://github.com/skahal/buildron) and your team can interact with it, like filter builds, sort builds, move camera, receive build status change notifications and taking screenshots.

### Installation
Download it from our [release page](https://github.com/giacomelli/Buildron.SlackBotMod/releases) and unzip it on your [Buildron mods folder](https://github.com/skahal/Buildron/wiki/Mods-Introduction#mods-folder).

### Configuration
Go to your [Slack team new bot page](http://my.slack.com/services/new/bot) and create a new bot called "buildron" and click in "Add bot integration"
![](docs/images/Buildron-SlackBotMod-bot-setup.png)

In the next page copy the "API token". We need to set it on mod preferences inside Buildron.


Open Buildron and go to Giacomelli.Buildron.SlackBotMod [preferences](https://github.com/skahal/Buildron/wiki/Mods-Introduction#mods-preferences):
![](docs/images/Buildron-SlackBotMod-mod-setup.png)

Paste the previous copied API Token on "Slack key" field.

Go back to Buildron main screen and start it.


### Usage

#### Status changed notifications
Your team can be notified by Buildron about builds status changed. In the mod preferences you can choose what status you want to receive notifications (running|succes|failed).

![](docs/images/Buildron-SlackBotMod-build-status-change-notifications.png)

### Filter builds
Filter buils by status or text.

![](docs/images/Buildron-SlackBotMod-filter-by.png)

### Reset filter
Reset previous builds filter (no filter).

![](docs/images/Buildron-SlackBotMod-reset-filter.png)

### Sort builds
Sort buils by status, text or date.

![](docs/images/Buildron-SlackBotMod-sort-by.png)

### Move camera
Move the camera the amount of pixels define in the x,y,z coordinates.

![](docs/images/Buildron-SlackBotMod-move-camera.png)

### Reset camera
Reset the camera position.

![](docs/images/Buildron-SlackBotMod-reset-camera.png)

### Take a screenshot
Take a screenshot of current Buildron state.

![](docs/images/Buildron-SlackBotMod-take-screenshot.png)


> You can talk with Buildron bot in a specified channel choosed on mod preferences or on its direct message channel.

### FAQ
* Do you want to know more available Buildron bot messages?

type:

```shell
@buildron help
``` 

* Having troubles? 

Ask on Twitter [@ogiacomelli](http://twitter.com/ogiacomelli).
 
 
### How to improve it?

Create a fork of [Buildron.SlackBot](https://github.com/giacomelli/Buildron.SlackBotMod/fork). 

Did you change it? [Submit a pull request](https://github.com/giacomelli/Buildron.SlackBotMod/pull/new/master).


### License
Licensed under the The MIT License (MIT).
In others words, you can use this library for developement any kind of software: open source, commercial, proprietary and alien.
