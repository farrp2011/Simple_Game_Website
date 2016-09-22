//
// Monster Wants Candy demo
// ========================
//
// Monster Wants Candy demo is a simple HTML5 game created with Phaser 2.0.7 and available at: http://candy-demo.enclavegames.com/
//
//    You can read the detailed tutorial about the demo at Tuts+ Game Development: http://gamedevelopment.tutsplus.com/tutorials/getting-started-with-phaser-building-monster-wants-candy--cms-21723
//
//    ![Monster Wants Candy - screen](https://cms-assets.tutsplus.com/uploads/users/22/posts/21723/image/monster-demo-screens.jpg)
//
//    You can also play the full game at: http://enclavegames.com/games/monster-wants-candy/
//
// ## Licensing
//
// This game demo is licensed under the **Creative Commons Attribution-NonCommercial 4.0 International** (CC BY-NC 4.0). 
// See the [human-readable summary](http://creativecommons.org/licenses/by-nc/4.0/) 
// [legalcode](http://creativecommons.org/licenses/by-nc/4.0/legalcode) 
//
// Original work [Andrzej Mazur](http://end3r.com/))
//
// Modifications licensed CC-BY-NC-4.0 (http://creativecommons.org/licenses/by-nc/4.0) Amelia Garripoli http://faculty.olympic.edu/agarripoli
//   compiled into one .js file
//   gameplay tweaked and end-game altered to submit a form with the score
//

//
// Boot
//

var Candy = {};
Candy.Boot = function (game) { };
Candy.Boot.prototype = {
    preload: function () {
        // preload the loading indicator first before anything else
        this.load.image('preloaderBar', '/Content/images/loading-bar.png','gameDiv'); // ARG added gameDiv
    },
    create: function () {
        // set scale options
        this.input.maxPointers = 1;
        this.scale.scaleMode = Phaser.ScaleManager.SHOW_ALL; 
        // centering is in the CSS
        
        // keep it on screen -- lets us not adjust any numbers in there but make it smaller
        this.scale.minWidth = 200;
        this.scale.minHeight = 300;
        this.scale.maxWidth = 480; // Firefox has the biggest px ...
        this.scale.maxHeight = 720;

        this.scale.setScreenSize(true);
        this.scale.refresh();


        // start the Preloader state
        this.state.start('Preloader');
    }
};

//
// Preloader
//

Candy.Preloader = function (game) {
    // define width and height of the game
    Candy.GAME_WIDTH = 640; // 400; .625
    Candy.GAME_HEIGHT = 960; // 600;
};
Candy.Preloader.prototype = {
    preload: function () {
        // set background color and preload image
        this.stage.backgroundColor = '#B4D9E7';
        this.preloadBar = this.add.sprite((Candy.GAME_WIDTH - 311) / 2, (Candy.GAME_HEIGHT - 27) / 2, 'preloaderBar');
        this.load.setPreloadSprite(this.preloadBar);
        // load images
        this.load.image('background', '/Content/images/background.png');
        this.load.image('floor', '/Content/images/floor.png');
        this.load.image('monster-cover', '/Content/images/monster-cover.png');
        this.load.image('title', '/Content/images/title.png');
        this.load.image('game-over', '/Content/images/gameover.png');
        this.load.image('score-bg', '/Content/images/score-bg.png');
        this.load.image('button-pause', '/Content/images/button-pause.png');
        // load spritesheets
        this.load.spritesheet('candy', '/Content/images/candy.png', 82, 98);
        this.load.spritesheet('monster-idle', '/Content/images/monster-idle.png', 103, 131);
        this.load.spritesheet('button-start', '/Content/images/button-start.png', 401, 143);
    },
    create: function () {
        // start the MainMenu state
        this.state.start('MainMenu');
    }
};

//
// MainMenu
//

Candy.MainMenu = function (game) { };
Candy.MainMenu.prototype = {
    create: function () {
        // display images
        this.add.sprite(0, 0, 'background');
        this.add.sprite(-130, Candy.GAME_HEIGHT - 514, 'monster-cover');
        this.add.sprite((Candy.GAME_WIDTH - 395) / 2, 60, 'title');
        // add the button that will start the game
        this.add.button(Candy.GAME_WIDTH - 401 - 10, Candy.GAME_HEIGHT - 143 - 10, 'button-start', this.startGame, this, 1, 0, 2);
    },
    startGame: function () {
        // start the Game state
        this.state.start('Game');
    }
};

//
// Game
//
// Central game play for Monster Wants Candy.
// Aug2015: make candies fall faster and more frequently as the game progresses
//          don't halt play at 15 candies
//

Candy.Game = function (game) {
    // define needed variables for Candy.Game
    this._player = null;
    this._candyGroup = null;
    this._spawnCandyTimer = 0;
    this._fontStyle = null;
    // define Candy variables to reuse them in Candy.item functions
    Candy._scoreText = null;
    Candy._score = 0;
    Candy._health = 0;
    Candy._timeBetween = 1000; // 1 second to start with
    Candy._gravityY = 100; // gravity to apply to candies
};
Candy.Game.prototype = {
    create: function () {
        // start the physics engine
        this.physics.startSystem(Phaser.Physics.ARCADE);
        // set the global gravity
        this.physics.arcade.gravity.y = Candy._gravityY;
        // display images: background, floor and score
        this.add.sprite(0, 0, 'background');
        this.add.sprite(-30, Candy.GAME_HEIGHT - 160, 'floor');
        this.add.sprite(10, 5, 'score-bg');
        // add pause button
        this.add.button(Candy.GAME_WIDTH - 96 - 10, 5, 'button-pause', this.managePause, this);
        // create the player
        this._player = this.add.sprite(5, 760, 'monster-idle');
        // add player animation
        this._player.animations.add('idle', [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], 10, true);
        // play the animation
        this._player.animations.play('idle');
        // set font style
        this._fontStyle = { font: "40px Arial", fill: "#FFCC00", stroke: "#333", strokeThickness: 5, align: "center" };
        // initialize the spawn timer
        this._spawnCandyTimer = 0;
        // initialize the score text with 0
        Candy._scoreText = this.add.text(120, 20, "0", this._fontStyle);
        // set health of the player
        Candy._health = 10; // dies with one candy missed
        // create new group for candy
        this._candyGroup = this.add.group();
        // spawn first candy
        Candy.item.spawnCandy(this);
    },
    managePause: function () {
        // pause the game
        this.game.paused = true;
        // add proper informational text
        var pausedText = this.add.text(62, 156, "Game paused.\nTap anywhere to continue.", this._fontStyle);
        // set event listener for the user's click/tap the screen
        this.input.onDown.add(function () {
            // remove the pause text
            pausedText.destroy();
            // unpause the game
            this.game.paused = false;
        }, this);
    },
    update: function () {
        // update timer every frame
        this._spawnCandyTimer += this.time.elapsed;
        // if spawn timer reaches time between candies, drop another one
        if (this._spawnCandyTimer > Candy._timeBetween) {
            // reset it
            this._spawnCandyTimer = 0;
            // and spawn new candy
            Candy.item.spawnCandy(this);
        }
        // loop through all candy on the screen
        this._candyGroup.forEach(function (candy) {
            // to rotate them accordingly
            candy.angle += candy.rotateMe;
        });
        // if the health of the player drops to 0, the player dies = game over
        if (!Candy._health) {
            this.game.over = false; //not over yet, almost...
            this.manageEnd();
        }
    },
    manageEnd: function () {
        // pause the game
        this.game.paused = true;
        // use text, clearer info...
        // var gameover = this.add.sprite((Candy.GAME_WIDTH-594)/2, (Candy.GAME_HEIGHT-271)/2, 'game-over');
        // add proper informational text
        var pausedText = this.add.text(100, 250, "Game Over.\nTap anywhere to continue.", this._fontStyle);
        var gameEnded = new Date();
        // set event listener for the user's click/tap the screen
        this.input.onDown.add(function () {
            if (!this.game.over) {
                this.game.over = true; // only post it once -- ignores additional clicks
                // post score and submit form
                // NOTE: if you changed the form fields in the view, change the fields set here to match

                document.forms["gameOver"].timeStamp.value = Date();
                document.forms["gameOver"].score.value = Candy._score;
                //document.getElementById('timeStamp').value = today;
                document.gameOver.submit();
            }
            
        }, this);

    }
};

Candy.item = {
    spawnCandy: function (game) {
        // calculate drop position (from 0 to game width) on the x axis
        var dropPos = Math.floor(Math.random() * Candy.GAME_WIDTH);
        // define the offset for every candy
        var dropOffset = [-27, -36, -36, -38, -48];
        // randomize candy type
        var candyType = Math.floor(Math.random() * 5);
        // create new candy
        var candy = game.add.sprite(dropPos, dropOffset[candyType], 'candy');
        // add new animation frame
        candy.animations.add('anim', [candyType], 10, true);
        // play the newly created animation
        candy.animations.play('anim');
        // enable candy body for physic engine
        game.physics.enable(candy, Phaser.Physics.ARCADE);
        // set the candy gravity speed
        candy.body.gravity.y = Candy._gravityY;
        // enable candy to be clicked/tapped
        candy.inputEnabled = true;
        // add event listener to click/tap
        candy.events.onInputDown.add(this.clickCandy, this);
        // be sure that the candy will fire an event when it goes out of the screen
        candy.checkWorldBounds = true;
        // reset candy when it goes out of screen
        candy.events.onOutOfBounds.add(this.removeCandy, this);
        // set the anchor (for rotation, position etc) to the middle of the candy
        candy.anchor.setTo(0.5, 0.5);
        // set the random rotation value
        candy.rotateMe = (Math.random() * 4) - 2;
        // add candy to the group
        game._candyGroup.add(candy);
    },
    clickCandy: function (candy) {
        // kill the candy when it's clicked
        candy.kill();
        // add points to the score
        Candy._score += 1;
        // update score text
        Candy._scoreText.setText(Candy._score);
        // take off .01 second every 7 candies
        if (Candy._score % 7 == 0) {
            Candy._timeBetween -= 10;
        }
        // increase the gravity for candies every 11 candies (speed up fall)
        if (Candy._score % 11 == 0) {
            Candy._gravityY += 5;
        }
    },
    removeCandy: function (candy) {
        // kill the candy
        candy.kill();
        // decrease player's health
        Candy._health -= 10;
    }
};